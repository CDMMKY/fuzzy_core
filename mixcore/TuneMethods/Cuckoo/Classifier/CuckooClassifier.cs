using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class CuckooClassifier: AbstractNotSafeLearnAlgorithm
    {
        double m;
        double p;
        double beta;
        int count_particle;

        int count_iteration;


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }


        public override FuzzySystem.PittsburghClassifier.PCFuzzySystem TuneUpFuzzySystem(FuzzySystem.PittsburghClassifier.PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            PCFuzzySystem result = Classifier;

            count_iteration = ((CuckooConf)conf).CuckooCountIterate;
            count_particle = ((CuckooConf)conf).CuckooPopulationSize;
            m = ((CuckooConf)conf).CuckooWorse;
            p = ((CuckooConf)conf).CuckooLifeChance;
            beta = ((CuckooConf)conf).CuckooBeta;
      

            KnowlegeBasePCRules[] X = new KnowlegeBasePCRules[count_particle + 1];
            double[] Errors = new double[count_particle + 1];
            double[] Er = new double[count_particle + 1];
                        
            Random rnd = new Random();
            int best = 0;

            for (int i = 0; i < count_particle + 1; i++)
            {
                KnowlegeBasePCRules temp_c_Rule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                X[i] = temp_c_Rule;
                Errors[i] = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
            }
            ///////////
            for (int i = 0; i < count_iteration; i++)
            {
                X[0] = new  KnowlegeBasePCRules(X[0]);
                for (int k = 0; k < X[0].TermsSet.Count; k++)
                {
                    for (int q = 0; q < X[0].TermsSet[k].CountParams; q++)
                    {
                        double b = (rnd.Next(1000, 2000) / Convert.ToDouble(1000));
                        X[0].TermsSet[k].Parametrs[q] = X[0].TermsSet[k].Parametrs[q] + Levi(BM(sigu(beta)), BM(1.0), beta);
                    }
                }

                for (int k = 0; k < X[0].Weigths.Length; k++)
                {
                    X[0].Weigths[k] = rnd.NextDouble() / 200;
                }

                result.RulesDatabaseSet.Add(X[0]);
                int temp_index = result.RulesDatabaseSet.Count - 1;
                Errors[0] = result.ClassifyLearnSamples(result.RulesDatabaseSet[ temp_index]);
                result.RulesDatabaseSet.RemoveAt(temp_index);

                int s = rnd.Next(1, count_particle + 1);

                if (Errors[0] > Errors[s])
                {
                    X[s] = X[0];
                    Errors[s] = Errors[0];
                }
                else
                {
                    X[0] = X[s];
                    Errors[0] = Errors[s];
                }

                for (int v = 0; v < m; v++)
                {
                    double max = Errors[1];
                    int ind = 1;
                    for (int r = 2; r < count_particle + 1; r++)
                    {
                        if (Errors[r] < max)
                        {
                            max = Errors[r];
                            ind = r;
                        }
                        else { };
                    }
                    double h = (rnd.Next(1, 1000) / Convert.ToDouble(1000));
                    if (h > p)
                    {
                        X[ind] = new KnowlegeBasePCRules(X[ind]);
                        for (int j = 0; j < X[ind].TermsSet.Count; j++)
                        {
                            for (int k = 0; k < X[ind].TermsSet[j].CountParams; k++)
                            {
                                X[ind].TermsSet[j].Parametrs[k] = X[0].TermsSet[j].Parametrs[k] + (rnd.Next(-1000, 1000) / Convert.ToDouble(1000));

                            }
                            for (int k = 0; k < X[ind].Weigths.Length; k++)
                            {                                
                                X[ind].Weigths[k] = X[0].Weigths[k] + (rnd.Next(1, 1000) / Convert.ToDouble(10000));
                            }
                        }
                        result.RulesDatabaseSet.Add(X[ind]);
                        temp_index = result.RulesDatabaseSet.Count - 1;
                        Errors[ind] = result.ClassifyLearnSamples(result.RulesDatabaseSet[ temp_index]);
                        result.RulesDatabaseSet.RemoveAt(temp_index);
                    }
                }
            }

            double min = Errors[0];
            best = 0;
            for (int g = 1; g < count_particle+1; g++)
            {
                if (Errors[g] > min)
                {
                    min = Errors[g];
                    best = g;
                }
            }
            
            X[0] = X[best];            
           
            result.RulesDatabaseSet.Add(X[0]);
            int t_index = result.RulesDatabaseSet.Count - 1;
            Errors[0] = result.ClassifyLearnSamples(result.RulesDatabaseSet[ t_index]);
            result.RulesDatabaseSet.RemoveAt(t_index);           


            result.RulesDatabaseSet[0] = X[0];
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }
        public override string ToString(bool with_param = false)// без параметров возвращает имя алгоритма, с параметров true возвращает имя алгоритма и значения его параметров
        {
            if (with_param)
            {
                string result = "Кукушкин поиск{";
                result += "Итераций= " + count_iteration.ToString() + " ;" + Environment.NewLine;
                result += "Заданное количество худших= " + m.ToString() + " ;" + Environment.NewLine;
                result += "Вероятность выжить для худших= " + p.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_particle.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Кукушкин поиск";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигураатора для вашего метода
        {
            ILearnAlgorithmConf result = new CuckooConf();
            result.Init(CountFeatures);
            return result;
        }

        private double Levi(double u, double v, double b)
        {
            double koef = 0.1;
            Random randnew = new Random();
            double Le = (koef) * u / (Math.Pow(Math.Abs(v), 1 / b));
            return Le;
        }

        private double BM(double sig)
        {
            Random randnew2 = new Random();
            double N = sig * Math.Sqrt(-2 * Math.Log(randnew2.Next(1, 1000) / Convert.ToDouble(1000))) * Math.Cos(2 * Math.PI * randnew2.Next(1, 1000) / Convert.ToDouble(1000));
            return N;
        }

        private double sigu(double b)
        {
            Random randnew = new Random();
            double s = (gamma(1 + b) * Math.Sin(Math.PI * (b) / 2)) / (gamma((1 + (b)) / 2) * (b) * Math.Pow(2, (b - 1) / 2));
            return s;
        }

        //*************************************************************************************************************
        // аппроксимация гамма-функции в интервале от 1 до 2
        // отношением полиномов 8 степени
        double gammaapprox(double x)
        {
            double[] p;
            p = new double[8] {-1.71618513886549492533811e+1, 
                                2.47656508055759199108314e+1, 
                               -3.79804256470945635097577e+2, 
                                6.29331155312818442661052e+2,
                                8.66966202790413211295064e+2, 
                               -3.14512729688483675254357e+4, 
                               -3.61444134186911729807069e+4,
                                6.64561438202405440627855e+4};
            double[] q;
            q = new double[8]{-3.08402300119738975254353e+1,
                               3.15350626979604161529144e+2,
                              -1.01515636749021914166146e+3, 
                              -3.10777167157231109440444e+3,
                               2.25381184209801510330112e+4,
                               4.75584627752788110767815e+3,
                              -1.34659959864969306392456e+5,
                              -1.15132259675553483497211e+5};
            double z = x - 1.0;
            double a = 0.0;
            double b = 1.0;
            for (int i = 0; i < 8; i++)
            {
                a = (a + p[i]) * z;
                b = b * z + q[i];
            }
            return (a / b + 1.0);
        }

        //*************************************************************************************************************
        // Гамма-функция вещественного агрумента
        // возвращает значение гамма-функции аргумента z
        double gamma(double z)
        {
            if ((z > 0.0) && (z < 1.0)) return gamma(z + 1.0) / z;     // рекурентное соотношение для 0
            if (z > 2) return (z - 1) * gamma(z - 1);   // рекурентное соотношение для z>2 
            if (z <= 0) return Math.PI / (Math.Sin(Math.PI * z) * gamma(1 - z)); // рекурентное соотношение для z<=0 
            return gammaapprox(z); // 1<=z<=2 использовать аппроксимацию
        }


    }
}



