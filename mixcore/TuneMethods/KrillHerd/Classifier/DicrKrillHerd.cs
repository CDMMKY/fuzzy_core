using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using FuzzyCoreUtils;
using FuzzySystem.PittsburghClassifier;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class DisKrillHerd : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand;
        protected KrillConfig Config;
        protected int Nkrill, iter;
        protected double wn, wf, ct, dmax, nmax, e, Vf;
        bool Kroofifood;
        protected List<bool[]> Population;

        //основные вычисления
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            rand = new Random();
            Init(conf);
            SetPopulation();
            bool[] BEST = result.AcceptedFeatures;
            double bestError = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
            //отчистка консоли
            Dictionary<bool[], double> PopulationWithAccuracy = new Dictionary<bool[], double>();
            double accuracy = 0;
            //запуск итераций
            for (int it = 0; it < iter; it++)
            {
                //расчитыавем значение фитнес-функции
                for (int i = 0; i < Population.Count; i++)
                {
                    result.AcceptedFeatures = Population[i];
                    accuracy = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
                    PopulationWithAccuracy.Add(Population[i], accuracy);
                }
                Population.Clear();
                foreach (var pair in PopulationWithAccuracy.OrderByDescending(pair => pair.Value))
                {
                    Population.Add(pair.Key);
                }
                PopulationWithAccuracy.Clear();
                double[] K = new double[Population.Count];
                double sumK = 0;
                double avK = 0;
                for (int i = 0; i < Population.Count; i++)
                {
                    result.AcceptedFeatures = Population[i];
                    K[i] = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
                    sumK += K[i];
                }
                avK = sumK / K.Length;
                bool[] KDis = new bool[result.CountFeatures];
                for (int i = 0; i < KDis.Length; i++)
                {
                    if (rand.Next(0, 2) == 0)
                        KDis[i] = false;
                    else
                        KDis[i] = true;
                }

                //перебрать значения фитнес функции
                //расчитыавем значение D
                double dit;
                dit = it;
                double diter;
                diter = iter;

                bool D = true;

                //расчитыавем значение Xfood
                var Xfood = new bool[result.CountFeatures];
                for (int t = 0; t < Xfood.Length; t++)
                {
                    Xfood[t] = false;

                    for (int i = 0; i < Population.Count; i++)
                    {
                        Xfood[t] = merge(Population[i][t], KDis[t]);
                    }
                }
                //расчитываем значение Cfood
                double Cfood = 2 * (1 - (dit / diter));
                bool CDisfood;
                if (Cfood <= 1)
                    CDisfood = false;
                else
                    CDisfood = true;

                //расчитываем значение Bfood
                List<bool[]> Bfood = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    Bfood.Add(new bool[Population[i].Length]);
                    if (rand.Next(0, 2) == 0)
                        Kroofifood = false;
                    else
                        Kroofifood = true;
                    for (int t = 0; t < Bfood[i].Length; t++)
                    {
                        Bfood[i][t] = merge(merge(CDisfood, Kroofifood), Xfood[t]);
                    }
                }

                //расчитываем значение Bbest
                List<bool[]> Bbest = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    Bbest.Add(new bool[Population[i].Length]);
                    if (rand.Next(0, 2) == 0)
                        Kroofifood = false;
                    else
                        Kroofifood = true;
                    for (int t = 0; t < Bbest[i].Length; t++)
                    {
                        Bbest[i][t] = merge(Kroofifood, Xfood[t]);
                    }
                }

                //расчитываем значение B
                List<bool[]> B = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    B.Add(new bool[Population[i].Length]);
                    for (int t = 0; t < B[i].Length; t++)
                    {
                        B[i][t] = merge(Bfood[i][t], Bbest[i][t]);
                    }
                }

                //расчитываем значение F
                List<bool[]> F = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    if (i == 0)
                    {
                        F.Add(new bool[Population[i].Length]);
                        for (int t = 0; t < F[i].Length; t++)
                        {
                                F[i][t] = merge(true, B[i][t]);
                        }
                    }
                    else
                    {
                        F.Add(new bool[Population[i].Length]);
                        for (int t = 0; t < F[i].Length; t++)
                        {
                                F[i][t] = merge(merge(true, B[i][t]), merge(false, F[i - 1][t]));
                        }
                    }
                }

                List<int>[] neihbors = new List<int>[Population.Count];
                //расчитываем значение alocal
                List<bool[]> alocal = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    alocal.Add(new bool[Population[i].Length]);
                    neihbors[i] = countneihbors(Population[i]);
                   
                    for (int t = 0; t < alocal[i].Length; t++)
                    {
                        alocal[i][t] = false;
                        for (int j = 0; j < neihbors[i].Count; j++)
                        {
                            bool KRoofij = merge(KDis[t], Population[neihbors[i][j]][t]);
                            bool[] XRoofij = new bool[Population.Count];
                            XRoofij = CalcXroof(Population[i], Population[neihbors[i][j]]);

                            alocal[i][t] = merge(KRoofij, XRoofij[t]);
                        }
                    }
                }

                //расчитываем значение Cbest
                bool Cbest;
                if (rand.Next(0, 2) == 0)
                    Cbest = false;
                else
                    Cbest = true;
                

                //расчитываем значение atarget
                List<bool[]> atarget = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    atarget.Add(new bool[Population[i].Length]);
                    bool[] XRoofibest = new bool[Population.Count];
                    XRoofibest = CalcXroof(Population[i], Population[0]);
                    for (int t = 0; t < alocal[i].Length; t++)
                    {
                        bool KRoofibest = KDis[t];
                        atarget[i][t] = merge(merge(Cbest, KRoofibest), XRoofibest[t]);
                    }
                }

                //расчитываем значение a
                List<bool[]> a = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    a.Add(new bool[Population[i].Length]);
                    for (int t = 0; t < a[i].Length; t++)
                    {
                        a[i][t] = merge(atarget[i][t], alocal[i][t]);
                    }
                }

                //расчитываем значение N
                List<bool[]> N = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    if (i == 0)
                    {
                        N.Add(new bool[Population[i].Length]);
                        for (int t = 0; t < N[i].Length; t++)
                        {
                            N[i][t] = merge(true, a[i][t]);
                        }
                    }
                    else
                    {
                        N.Add(new bool[Population[i].Length]);
                        for (int t = 0; t < F[i].Length; t++)
                        {
                            N[i][t] = merge(a[i][t], N[i - 1][t]);
                        }
                    }
                }
                //расчитываем значение dX
                List<bool[]> dX = new List<bool[]>(Population.Count);
                for (int i = 0; i < Population.Count; i++)
                {
                    dX.Add(new bool[Population[i].Length]);
                    for (int t = 0; t < a[i].Length; t++)
                    {
                        dX[i][t] = merge(merge(F[i][t], N[i][t]), D);
                    }
                }

                //выводим значение BEST
                //   Console.Write("Значение BEST_ = ");
                //  Console.WriteLine(BEST);


                //расчитываем значение X(t+dt)
                for (int i = 0; i < Population.Count; i++)
                {
                    Population[i] = new bool[Population[i].Length];
                    for (int t = 0; t < Population[i].Length; t++)
                    {
                        Population[i][t] = merge(Population[i][t], dX[i][t]);
                    }
                }

                for (int i = 0; i < Population.Count; i++)
                {
                    result.AcceptedFeatures = Population[i];
                    double temp = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
                    
                    if (temp < bestError)
                    {
                        BEST = Population[i];
                        bestError = temp;
                    }
                }

                double y = it;
                if (y % 10 == 0 & y != 0)
                {
                    Console.WriteLine(it);
                    Console.WriteLine(bestError);
                }
            }
            result.AcceptedFeatures = BEST;
            for (int i = 0; i < result.AcceptedFeatures.Length; i++)
            {
                if (result.AcceptedFeatures[i] == false)
                    Console.Write("0 ");
                else
                    Console.Write("1 ");
            }
            Console.WriteLine();
            Console.WriteLine(result.ErrorLearnSamples(result.RulesDatabaseSet[0]));
            Console.WriteLine(result.ErrorTestSamples(result.RulesDatabaseSet[0]));
            return result;
        }

        public bool merge(bool a, bool b)
        {
            if (a == b)
            {
                return a;
            }
            else
            {
                if (rand.Next(0, 2) == 0)
                    return false;
                else
                    return true;
            }
        }

        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as KrillConfig;
            Nkrill = ((KrillConfig)Conf).Количество_особей;
            iter = ((KrillConfig)Conf).Количеств_итераций;
            wn = ((KrillConfig)Conf).Вес_инерции_вызванного_движения;
            wf = ((KrillConfig)Conf).Вес_инерции_движения_добывающего_продовольствие;
            ct = ((KrillConfig)Conf).Коэффициент_Ct;
            dmax = ((KrillConfig)Conf).Скорость_распространения_Dmax;
            nmax = ((KrillConfig)Conf).Вызванная_скорость_Nmax;
            e = ((KrillConfig)Conf).Положительное_число_е;
            Vf = ((KrillConfig)Conf).Добывающая_продовольствие_скорость_Vf;
        }
        private void SetPopulation()
        {
            Population = new List<bool[]>();
            for (int i = 0; i < Nkrill; i++)
            {
                Population.Add(new bool[result.CountFeatures]);
                for (int j = 0; j < Population[i].Length; j++)
                {
                    if (rand.Next(0, 2) == 0)
                        Population[i][j] = false;
                    else
                        Population[i][j] = true;
                }
            }
        }

        //нахождение расстояния между крилями
        private double Distance(bool[] x, bool[] y)
        {
            double dist = 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    dist += 1;
            }
            return dist;
        }

        //нахождение расстояния ощущения (соседства)
        private double distneihbor(bool[] x)
        {
            double sum = 0, dneihbor;
            for (int j = 0; j < Population.Count; j++)
            {
                if (x != Population[j]) sum += Distance(x, Population[j]);
            }
            dneihbor = sum * (1.0 / (0.0001 * Population.Count));
            return dneihbor;
        }

        //нахождение расстояния ощущения (соседства)
        private List<int> countneihbors(bool[] x)
        {
            double border = distneihbor(x);
            List<int> neihbors = new List<int>();

            for (int j = 0; j < Population.Count; j++)
            {
                if (x != Population[j])
                {
                    if (Distance(x, Population[j]) < border)
                        neihbors.Add(j);
                }
            }
            return neihbors;
        }

        //расчитываем значение deltat
        private double calcdeltat(double ct)
        {
            double deltat = 0;
            for (int i = 0; i < result.CountFeatures; i++)
                deltat += result.LearnSamplesSet.InputAttributes[i].Scatter;

            return ct * deltat;
        }

        //расчитыавем значение X с крышкой
        public bool[] CalcXroof(bool[] xi, bool[] xj)
        {
            var Xroof = new bool[xi.Length];
            for (int t = 0; t < Xroof.Length; t++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    Xroof[t] = merge(merge(xj[t], xi[t]), true);
                }
            }
            return Xroof;
        }


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>()
                {
                    FuzzySystemRelisedList.TypeSystem.PittsburghClassifier
                };
            }
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            KrillConfig conf = new KrillConfig();
            conf.Init(CountFeatures);

            return conf;
        }
        public override string ToString(bool with_param = false)
        {
            return "Discret Krill Herd";
        }
        public static class StaticRandom // random
        {
            static int seed = Environment.TickCount;

            static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

            public static int Next(int x)
            {
                return random.Value.Next(x);
            }
            public static double NextDouble()
            {
                return random.Value.NextDouble();
            }
        }
    }
}
