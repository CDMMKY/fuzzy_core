﻿//#define debug
#undef debug
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using FuzzyCoreUtils;
using FuzzySystem.TakagiSugenoApproximate;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class KrillTS : AbstractNotSafeLearnAlgorithm
    {
        protected TSAFuzzySystem result;
        Random rand = new Random();
        protected KrillConfig Config;
        protected int Nkrill, iter;
        protected double wn, wf, ct, dmax, nmax, e, Vf;
        double Kworst;
        double Kbest;
        protected KnowlegeBaseTSARules[] Population;

        //основные вычисления
        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            result = Approx;
            Init(conf);
            SetPopulation();
            KnowlegeBaseTSARules BEST = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
            double bestError = result.ErrorLearnSamples(BEST);
            //отчистка консоли

#if debug 
            Console.Clear();
#endif
            //запуск итераций
            for (int it = 0; it < iter; it++)
            {

#if debug 
                //вывод номера итерации
                Console.Write("Итерация __№__ = ");
                Console.WriteLine(it);

#endif
                //расчитыавем значение фитнес-функции
                Population =ListTakagiSugenoApproximateTool.SortRules(Population, result);
                double[] K = new double[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    K[i] = result.ErrorLearnSamples(Population[i]);
#if debug
                    Console.Write("Значние  K[i1] = ");
                    Console.WriteLine(K[i]);
#endif
                    if (double.IsNaN(K[i]) || double.IsInfinity(K[i]))
                    {
                        result.UnlaidProtectionFix( Population[i]);
                        K[i] = result.ErrorLearnSamples(Population[i]);

#if debug
                        Console.Write("Значние  K[i2] = ");
                        Console.WriteLine(K[i]);
#endif
                    }
                }
                Kworst = K.Max();
                if (double.IsNaN(Kworst) || double.IsInfinity(Kworst))
                {
                    int iworst = K.ToList().IndexOf(Kworst);
 #if debug  
                    Console.Write("Значние iworst = ");
                    Console.WriteLine(iworst);
#endif
                }

#if debug
                //вывод Kworst
                Console.Write("Значние KWorst = ");
                Console.WriteLine(Kworst);
#endif
                Kbest = K.Min();
#if debug
                //вывод Kbest
                Console.Write("Значние Kbest = ");
                Console.WriteLine(Kbest);
#endif
                int ibest = K.ToList().IndexOf(Kbest);
#if debug
                //вывод ibest
                Console.Write("Значние ibest = ");
                Console.WriteLine(ibest);
#endif
                //перебрать значения фитнес функции
                //расчитыавем значение D
                double dit;
                dit = it;
                double diter;
                diter = iter;

                double D = (dmax * (rand.NextDouble() * 2 - 1) * (dit / diter));

                //расчитываем значение rand1 для D
                double rand1;
                rand1 = D / (dmax * (it) / iter);
#if debug
                //выводим значение rand1 для D
                Console.Write("Значение Drand = ");
                Console.WriteLine(rand1);

                //выводим значение D
                Console.Write("Значение __D__ = ");
                Console.WriteLine(D);
#endif
                //расчитыавем значение Xfood
                double divide = K.Select(x => 1 / x).ToList().Sum();
                var Xfood = new KnowlegeBaseTSARules(Population[0]);
                for (int t = 0; t < Xfood.TermsSet.Count; t++)
                {
                    for (int p = 0; p < Xfood.TermsSet[t].CountParams; p++)
                    {
                        Xfood.TermsSet[t].Parametrs[p] = 0;

                        for (int i = 0; i < Population.Length; i++)
                        {
                            Xfood.TermsSet[t].Parametrs[p] += Population[i].TermsSet[t].Parametrs[p] / K[i];
#if debug
                            //выводим значение Xfood
                            Console.Write("Значение Xfood = ");
                            Console.WriteLine(Xfood.TermsSet[t].Parametrs[p]);
#endif
                        }
                        Xfood.TermsSet[t].Parametrs[p] /= divide;
                    }
                }
#if debug
                //вывод divide
                Console.Write("Значние divide = ");
                Console.WriteLine(divide);
#endif
                //расчитываем значение Kfood
                double Kfood = result.ErrorLearnSamples(Xfood);
                if (double.IsNaN(Kfood) || double.IsInfinity(Kfood))
                {
                    result.UnlaidProtectionFix(Xfood);
                    Kfood = result.ErrorLearnSamples(Xfood);
                }
#if debug
                //выводим значение Kfood
                Console.Write("Значение Kfood = ");
                Console.WriteLine(Kfood);
#endif
                //расчитываем значение Cfood
                double Cfood = 2 * (1 - (dit / diter));
#if debug
                //выводим значение Cfood
                Console.Write("Значение Cfood = ");
                Console.WriteLine(Cfood);
#endif
                //расчитываем значение Bfood
                KnowlegeBaseTSARules[] Bfood = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    Bfood[i] = new KnowlegeBaseTSARules(Population[i]);
                    double KRoofifood = CalcKroof(K[i], Kfood);
                    KnowlegeBaseTSARules Xroofifood = new KnowlegeBaseTSARules(CalcXroof(Population[i], Xfood));
                    for (int t = 0; t < Bfood[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < Bfood[i].TermsSet[t].CountParams; p++)
                        {
                            Bfood[i].TermsSet[t].Parametrs[p] = Cfood * KRoofifood * Xroofifood.TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение Bfood
                            Console.Write("Значение Bfood = ");
                            Console.WriteLine(Bfood[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }

                //расчитываем значение Bbest
                KnowlegeBaseTSARules[] Bbest = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    Bbest[i] = new KnowlegeBaseTSARules(Population[i]);
                    double KRoofifood = CalcKroof(K[i], K[ibest]);
                    KnowlegeBaseTSARules Xroofifood = new KnowlegeBaseTSARules(CalcXroof(Population[i], Population[ibest]));
                    for (int t = 0; t < Bbest[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < Bbest[i].TermsSet[t].CountParams; p++)
                        {
                            Bbest[i].TermsSet[t].Parametrs[p] = KRoofifood * Xroofifood.TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение Bbest
                            Console.Write("Значение Bbest = ");
                            Console.WriteLine(Bbest[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }

                //расчитываем значение B
                KnowlegeBaseTSARules[] B = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    B[i] = new KnowlegeBaseTSARules(Population[i]);
                    for (int t = 0; t < B[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < B[i].TermsSet[t].CountParams; p++)
                        {
                            B[i].TermsSet[t].Parametrs[p] = Bfood[i].TermsSet[t].Parametrs[p] + Bbest[i].TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение B
                            Console.Write("Значение __B__ = ");
                            Console.WriteLine(B[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }

                //расчитываем значение F
                KnowlegeBaseTSARules[] F = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    if (i == 0)
                    {
                        F[i] = new KnowlegeBaseTSARules(Population[i]);
                        for (int t = 0; t < F[i].TermsSet.Count; t++)
                        {
                            for (int p = 0; p < F[i].TermsSet[t].CountParams; p++)
                            {
                                F[i].TermsSet[t].Parametrs[p] = Vf * B[i].TermsSet[t].Parametrs[p];
#if debug
                                //выводим значение F
                                Console.Write("Значение __F__ = ");
                                Console.WriteLine(F[i].TermsSet[t].Parametrs[p]);
#endif
                            }
                        }
                    }
                    else
                    {
                        F[i] = new KnowlegeBaseTSARules(Population[i]);
                        for (int t = 0; t < F[i].TermsSet.Count; t++)
                        {
                            for (int p = 0; p < F[i].TermsSet[t].CountParams; p++)
                            {
                                F[i].TermsSet[t].Parametrs[p] = Vf * B[i].TermsSet[t].Parametrs[p] + wf * F[i - 1].TermsSet[t].Parametrs[p];
#if debug
                                //выводим значение F
                                Console.Write("Значение __F__ = ");
                                Console.WriteLine(F[i].TermsSet[t].Parametrs[p]);
#endif
                            }
                        }
                    }
                }
                List<int> [] neihbors = new List<int>[Population.Length];
                //расчитываем значение alocal
                KnowlegeBaseTSARules[] alocal = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    alocal[i] = new KnowlegeBaseTSARules(Population[i]);
                    neihbors[i] = countneihbors(Population[i]);
/*
#if debug
                    //вывод значений количества соседей
                    for (int g = 0; g < Population.Length; g++)
                    {
                        Console.Write("Знаение countneihbors = ");
                        Console.WriteLine(countneihbors(Population[g]));
                    }
#endif
 */

                    for (int t = 0; t < alocal[i].TermsSet.Count; t++)
                    {

                        for (int p = 0; p < alocal[i].TermsSet[t].CountParams; p++)
                        {
                            alocal[i].TermsSet[t].Parametrs[p] = 0;
                            for (int j = 0; j < neihbors[i].Count; j++)
                            {
                                double KRoofij = CalcKroof(K[i], K[neihbors[i][j]]);
                                KnowlegeBaseTSARules XRoofij = new KnowlegeBaseTSARules(CalcXroof(Population[i], Population[neihbors[i][j]]));

                                alocal[i].TermsSet[t].Parametrs[p] += KRoofij * XRoofij.TermsSet[t].Parametrs[p];

#if debug
                                //выводим значение alocal
                                Console.Write("Знаение alocal = ");
                                Console.WriteLine(alocal[i].TermsSet[t].Parametrs[p]);
#endif

                            }
                        }
                    }
                }

                //расчитываем значение Cbest
                double Cbest = 2 * (rand.NextDouble() - (dit / diter));
#if debug
                //выводим значение Cbest
                Console.Write("Значение Сbest = ");
                Console.WriteLine(Cbest);
#endif
                //расчитываем значение rand для Cbest
                double rand2;
                rand2 = it / iter - Cbest / 2;
#if debug
                //выводим значение rand2 для Cbest
                Console.Write("Значение Crand = ");
                Console.WriteLine(rand2);
#endif
                //расчитываем значение atarget
                KnowlegeBaseTSARules[] atarget = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    atarget[i] = new KnowlegeBaseTSARules(Population[i]);
                    double KRoofibest = CalcKroof(K[i], K[ibest]);
                    KnowlegeBaseTSARules XRoofibest = new KnowlegeBaseTSARules(CalcXroof(Population[i], Population[ibest]));
                    for (int t = 0; t < alocal[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < atarget[i].TermsSet[t].CountParams; p++)
                        {
                            atarget[i].TermsSet[t].Parametrs[p] = Cbest * KRoofibest * XRoofibest.TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение atarget
                            Console.Write("Знание atarget = ");
                            Console.WriteLine(atarget[i].TermsSet[t].Parametrs[p]);
#endif


                        }
                    }
                }

                //расчитываем значение a
                KnowlegeBaseTSARules[] a = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    a[i] = new KnowlegeBaseTSARules(Population[i]);
                    for (int t = 0; t < a[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < a[i].TermsSet[t].CountParams; p++)
                        {
                            a[i].TermsSet[t].Parametrs[p] = atarget[i].TermsSet[t].Parametrs[p] + alocal[i].TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение a
                            Console.Write("Значение __a__ = ");
                            Console.WriteLine(a[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }

                //расчитываем значение N
                KnowlegeBaseTSARules[] N = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    if (i == 0)
                    {
                        N[i] = new KnowlegeBaseTSARules(Population[i]);
                        for (int t = 0; t < N[i].TermsSet.Count; t++)
                        {
                            for (int p = 0; p < F[i].TermsSet[t].CountParams; p++)
                            {
                                N[i].TermsSet[t].Parametrs[p] = Vf * a[i].TermsSet[t].Parametrs[p];
#if debug
                                //выводим значение N
                                Console.Write("Значение __N__ = ");
                                Console.WriteLine(N[i].TermsSet[t].Parametrs[p]);
#endif
                            }
                        }
                    }
                    else
                    {
                        N[i] = new KnowlegeBaseTSARules(Population[i]);
                        for (int t = 0; t < F[i].TermsSet.Count; t++)
                        {
                            for (int p = 0; p < N[i].TermsSet[t].CountParams; p++)
                            {
                                N[i].TermsSet[t].Parametrs[p] = nmax * a[i].TermsSet[t].Parametrs[p] + wn * N[i - 1].TermsSet[t].Parametrs[p];
#if debug
                                //выводим значение N
                                Console.Write("Значение __N__ = ");
                                Console.WriteLine(N[i].TermsSet[t].Parametrs[p]);
#endif
                            }
                        }
                    }
                }

                //расчитываем значение dX
                KnowlegeBaseTSARules[] dX = new KnowlegeBaseTSARules[Population.Length];
                for (int i = 0; i < Population.Length; i++)
                {
                    dX[i] = new KnowlegeBaseTSARules(Population[i]);
                    for (int t = 0; t < a[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < a[i].TermsSet[t].CountParams; p++)
                        {
                            dX[i].TermsSet[t].Parametrs[p] = F[i].TermsSet[t].Parametrs[p] + N[i].TermsSet[t].Parametrs[p] + D;
#if debug
                            //выводим значение dX
                            Console.Write("Значение _dX__ = ");
                            Console.WriteLine(dX[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }






                //выводим значение BEST
                //   Console.Write("Значение BEST_ = ");
                //  Console.WriteLine(BEST);


                //расчитываем значение X(t+dt)
                for (int i = 0; i < Population.Length; i++)
                {
                    Population[i] = new KnowlegeBaseTSARules(Population[i]);
                    for (int t = 0; t < Population[i].TermsSet.Count; t++)
                    {
                        for (int p = 0; p < F[i].TermsSet[t].CountParams; p++)
                        {
                            Population[i].TermsSet[t].Parametrs[p] = Population[i].TermsSet[t].Parametrs[p] + calcdeltat(ct) * dX[i].TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение Xnew
                            Console.Write("Знание X(t+dt) = ");
                            Console.WriteLine(Population[i].TermsSet[t].Parametrs[p]);
#endif
                        }
                    }
                }


              
               
                for (int i = 0; i < Population.Length; i++)
                {
                    double temp = result.ErrorLearnSamples(Population[i]);
                    if (double.IsNaN(temp) || double.IsInfinity(temp))
                    {
                        result.UnlaidProtectionFix(Xfood);
                        temp = result.ErrorLearnSamples(Population[i]);
                    }

                    if (temp < bestError)
                    {
                        BEST = new KnowlegeBaseTSARules(Population[i]);
                        bestError = temp;
                    }
                }


                double y=it;
                if (y % 10 == 0 & y != 0)
                {
                    Console.WriteLine(it);
                    Console.WriteLine(bestError);
                }
#if debug
                // выводим значение лучшей ошибки Kbest
                Console.Write("Значние BestError = ");
                Console.WriteLine(bestError);

                Console.WriteLine(".");
#endif



            }
            result.RulesDatabaseSet[0] = BEST;
            return result;
        }


        //расчитыавем значение X с крышкой
        public KnowlegeBaseTSARules CalcXroof(KnowlegeBaseTSARules xi, KnowlegeBaseTSARules xj)
        {
            var Xroof = new KnowlegeBaseTSARules(xi);
            for (int t = 0; t < Xroof.TermsSet.Count; t++)
            {
                for (int p = 0; p < Xroof.TermsSet[t].CountParams; p++)
                {
                    for (int i = 0; i < Population.Length; i++)
                    {
                        Xroof.TermsSet[t].Parametrs[p] = (xj.TermsSet[t].Parametrs[p] - xi.TermsSet[t].Parametrs[p]) / (Math.Sqrt(Math.Pow((xj.TermsSet[t].Parametrs[p] - xi.TermsSet[t].Parametrs[p]), 2) + Math.Pow((xi.TermsSet[t].Parametrs[p] - xj.TermsSet[t].Parametrs[p]), 2)) + e);
                    }

                }
            }

            return Xroof;
        }

        //расчитыавем значение K с крышкой
        public double CalcKroof(double ki, double kj)
        {
            double Kroof = (ki - kj) / (Kworst - Kbest);

            return Kroof;
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
            Population = new KnowlegeBaseTSARules[Nkrill];
            KnowlegeBaseTSARules TempRule = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
            Population[0] = TempRule;
            for (int i = 1; i < Nkrill; i++)
            {
                TempRule = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
                Population[i] = TempRule;
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {

                        // если что поменять параметр 0,1
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                        //перебрать значения параметров
                    }
                }
            }
        }

        //нахождение расстояния между крилями
        private double Distance(KnowlegeBaseTSARules x, KnowlegeBaseTSARules y)
        {
            double dist, sum = 0;
            for (int i = 0; i < x.TermsSet.Count; i++)
            {
                for (int j = 0; j < x.TermsSet[i].Parametrs.Length; j++)
                {
                    sum += Math.Pow(x.TermsSet[i].Parametrs[j] - y.TermsSet[i].Parametrs[j], 2);
                }
            }
            for (int i = 0; i < x.RulesDatabase.Count; i++)
            {
                sum += Math.Pow(x.RulesDatabase[i].IndependentConstantConsequent - y.RulesDatabase[i].IndependentConstantConsequent, 2);
            }
            dist = Math.Sqrt(sum);
        /*
#if debug
            //вывод значение dist соседа
            Console.Write("Значение dist = ");
            Console.WriteLine(dist);
#endif
         */
            return dist;
        }

        //нахождение расстояния ощущения (соседства)
        private double distneihbor(KnowlegeBaseTSARules x)
        {
            double sum = 0, dneihbor;
            for (int j = 0; j < Population.Length; j++)
            {
                if (x != Population[j]) sum += Distance(x, Population[j]);
                /*
#if debug               
                //вывод значение суммы расстояний между переменными
                Console.Write("Значение суммы для расчета расстояния ощущения = ");
                Console.WriteLine(sum);
#endif                
                 */

            }
            dneihbor = sum * (1.0 / (1.5 * Population.Length));
            /*
            //вывод значение dneihbor соседей
            Console.Write("Значение dneihbor для расчета расстояния ощущения = ");
            Console.WriteLine(dneihbor);
            */

            return dneihbor;
        }

        //нахождение расстояния ощущения (соседства)
        private List<int> countneihbors(KnowlegeBaseTSARules x)
        {
            double border = distneihbor(x);
            List<int> neihbors = new List<int>();

            /*
            //вывод значение дистанции соседа
            for (int g = 0; g < Population.Length; g++)
            {
                Console.Write("Знаение distneihbor = ");
                Console.WriteLine(distneihbor(x));
            }
            */

            for (int j = 0; j < Population.Length; j++)
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

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>()
                {
                    FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate
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
            return "Krill Herd";
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