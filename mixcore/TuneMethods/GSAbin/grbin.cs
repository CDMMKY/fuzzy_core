using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem.PittsburghClassifier.add_generators;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem;
using Linglib;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.FuzzyAbstract.Utils;

namespace GSAbin
{
    public class grbin : AbstractNotSafeLearnAlgorithm
    {
        double G, G0, alpha, epsilon, Ebest, Ebest1;
        Random rand = new Random();
        List<bool[]> test;
        List<double> Errors;
        double[] mass;
        double[][,] R;
        double[,] RR;
        double[,] a, speed;
        List<FeatureSelectionModel> Storage;
        SortType SortWay;
        bool[] BestSolute, Solute, GoodSolute;

        int max_Features, open_Features, MCount, iterMax;
        IFuzzySystem theFuzzySystem;
        bool isClass = false;





        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier, FuzzySystemRelisedList.TypeSystem.Singletone, FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            isClass = true;
            return UniversalMethod(FSystem, conf) as PCFuzzySystem;
        }
        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            return UniversalMethod(FSystem, conf) as SAFuzzySystem;
        }
        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            return UniversalMethod(FSystem, conf) as TSAFuzzySystem;
        }



        public IFuzzySystem UniversalMethod(IFuzzySystem FSystem, ILearnAlgorithmConf config)
        {
            init(FSystem, config);


            ////////////////////////////////////////////////////////////////////////////////////Первоначальная генерация
            for (int u = 0; u < MCount; u++)
            {
                ///Создаем частицу
                for (int i = 0; i < max_Features; i++)
                {
                    if (rand.NextDouble() > 0.5) Solute[i] = true;
                    else Solute[i] = false;
                }
                ///Проверяем, есть ли признаки. Если частица пуста, рандомный признак становится единицей
                if (Solute.Count(x => x == true) == 0) BestSolute[rand.Next(BestSolute.Count())] = true; ;

                //Заносим частицу в популяцию
                test.Add(Solute.Clone() as bool[]);
                //Заносим признаки в классификатор
                FSystem.AcceptedFeatures = test[test.Count - 1];
                Errors.Add(FSystem.ErrorLearnSamples(FSystem.AbstractRulesBase()[0]));

                if (Errors[u] < Ebest)
                {
                    Ebest = Errors[u];

                    BestSolute = test[u].Clone() as bool[];
                    Console.WriteLine("Найдена частица с ошибкой E=" + Errors[u]);
                    Storage.Add(new FeatureSelectionModel(FSystem, BestSolute));
                }
                Console.WriteLine(Errors[u]);
            }
            /////////////////////////////////////////// Сгенерировали первоначальные частицы, ура!

            algoritm();
            Storage = FeatureSelectionModel.Distinct(Storage);
            FeatureSelectionModel.Sort(Storage, SortWay);
            FSystem.AcceptedFeatures = BestSolute;
            return FSystem;
        }

        public void newclass()
        {
            for (int q = 0; q < MCount; q++)
            {

                if (test[q].Count(x => x == true) == 0) test[q][rand.Next(BestSolute.Count())] = true; ;

                theFuzzySystem.AcceptedFeatures = test[q];
                Errors[q] = theFuzzySystem.ErrorLearnSamples(theFuzzySystem.AbstractRulesBase()[0]);
                if (Errors[q] < Ebest)
                {
                    Ebest = Errors[q];
                    BestSolute = test[q].Clone() as bool[];
                    // Best_remebed = rules;
                    Console.WriteLine("Найдена частица с ошибкой E=" + Errors[q]);

                    Storage.Add(new FeatureSelectionModel(theFuzzySystem, BestSolute));
                }
                int true_count = test[q].Count(x => x == true);
                if ((true_count <= open_Features) && (Errors[q] != Ebest) && (Errors[q] < Ebest1))
                {
                    GoodSolute = test[q].Clone() as bool[];
                    Storage.Add(new FeatureSelectionModel(theFuzzySystem, GoodSolute));
                    Console.WriteLine("Найдена дубль-частица с ошибкой E=" + Errors[q]);
                }
                Console.WriteLine(Errors[q]);
            }

        }

        //Итерационный алгоритм
        public void algoritm()
        {
            mass = new double[MCount];
            R = new double[MCount][,];
            for (int i = 0; i < MCount; i++)
            {
                R[i] = new double[MCount, max_Features];
            }
            RR = new double[MCount, MCount];
            a = new double[MCount, max_Features];
            speed = new double[MCount, max_Features];

            //Понеслись итерации
            for (int iter = 0; iter < iterMax; iter++)
            {
                Console.WriteLine("-------------- Итерация " + iter);
                G = G0 * Math.Pow(Math.E, ((-1) * alpha * iter / iterMax));
                Errors.CopyTo(mass);
                weight();
                distance();
                acceleration();
                velocity();
                sigmoid();
                newclass();
            }

        }

        //Массы - ведем расчет по ошибкам
        private void weight()
        {
            double sum = 0;
            double best = mass[0];
            double worst = mass[0];
            for (int i = 1; i < MCount; i++)
            {
                // mass[i] = Errors[i];

                if (mass[i] > worst) worst = mass[i];
                if (mass[i] < best) best = mass[i];
            }
            for (int i = 0; i < MCount; i++)
            {
                mass[i] = (mass[i] - worst) / (best - worst);
                sum = sum + mass[i];
            }
            for (int i = 0; i < MCount; i++)
            {
                mass[i] = mass[i] / sum;
            }
        }

        //Расчет дистанции между каждым признаком и суммирующее расстояние между каждой частицей
        private void distance()
        {
            for (int j = 0; j < MCount; j++)
            {

                for (int k = 0; k < MCount; k++)
                {
                    for (int l = 0; l < max_Features; l++)
                        if (test[j][l] != test[k][l])
                        {
                            R[j][k, l] = 1;
                            RR[j, k] += 1;
                        }
                        else R[j][k, l] = 0;
                    RR[j, k] = Math.Sqrt(RR[j, k]);
                }
            }
        }

        //Расчет ускорения для каждого признака в каждой частице
        public void acceleration()
        {
            for (int q = 0; q < MCount; q++)
            {
                for (int w = 0; w < max_Features; w++)
                {
                    a[q, w] = 0;
                    for (int t = 0; t < MCount; t++)
                    {
                        a[q, w] += rand.NextDouble() * (mass[t] * R[q][t, w]) / (RR[q, t] + epsilon);
                    }
                    a[q, w] *= G;
                }
            }
        }

        //Расчет скорости для каждого признака в каждой частице
        public void velocity()
        {
            for (int q = 0; q < MCount; q++)
            {
                for (int w = 0; w < max_Features; w++)
                {
                    speed[q, w] = rand.NextDouble() * speed[q, w] + a[q, w];
                }
            }

        }
        // По формуле сигмоида изменяем вектор признаков
        public void sigmoid()
        {
            for (int q = 0; q < MCount; q++)
            {
                for (int w = 0; w < max_Features; w++)
                {
                    if (rand.NextDouble() < (1 / (1 + Math.Pow(Math.E, (-1) * speed[q, w])))) test[q][w] = true;
                    else test[q][w] = false;
                }
            }
        }



        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Дискретная гравитация +";
                result += " ; " + Environment.NewLine;
                if (Storage != null) result += FeatureSelectionModel.getFullInfo(Storage,isClass);
             
                result += "}";
                return result;
            }
            return "Дискретная гравитация";
        }


        public void init(IFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            theFuzzySystem = FSystem;
            grbin_conf Config = conf as grbin_conf;
            Ebest = Config.GSAErrorBest;
            Ebest1 = Ebest;
            iterMax = Config.GSAMInter;
            MCount = Config.GSAMCount;
            G0 = Config.GSAG0;
            alpha = Config.GSAAlpha;
            epsilon = Config.GSAEpsilon;
            open_Features = Config.GSAMaxVars;
            max_Features = FSystem.CountFeatures;
            test = new List<bool[]>();
            Errors = new List<double>();
            BestSolute = new bool[FSystem.AcceptedFeatures.Count()];
            GoodSolute = new bool[FSystem.AcceptedFeatures.Count()];
            Solute = new bool[FSystem.AcceptedFeatures.Count()];
            Storage = new List<FeatureSelectionModel>();
            SortWay = Config.GSASortWay;
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new grbin_conf();
            result.Init(CountFeatures);
            return result;
        }

    }
}
