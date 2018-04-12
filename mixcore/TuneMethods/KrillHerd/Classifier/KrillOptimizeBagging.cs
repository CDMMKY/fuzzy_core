#define debug
#undef debug
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
    public class KrillOptimizeBagging : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand = new Random();
        protected KrillBConfig Config;
        protected int Nkrill, iter, numberOfPopulations;
        protected double wn, wf, ct, dmax, nmax, e, Vf, neighbor_coeff;
        double Kworst;
        double Kbest;
        protected List<List<KnowlegeBasePCRules>> Populations;

        //основные вычисления
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            //Узнаем название папки с данными
            string path_name = "../../OLD/Data/Keel/Classifier/KEEL-10/";
            string folder_name = "";
            foreach (var letter in result.LearnSamplesSet.FileName)
            {
                if (letter != '-')
                    folder_name += letter;
                else
                    break;
            }
            Init(conf);
            //Создаем новые обучающую и тестовую выбоки и удаляем из них некоторое количество случайных элементов
            List<PCFuzzySystem> results = new List<PCFuzzySystem>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                SampleSet new_learn = new SampleSet(path_name + folder_name + "/" + result.LearnSamplesSet.FileName);
                SampleSet new_test = new SampleSet(path_name + folder_name + "/" + result.TestSamplesSet.FileName);
                results.Add(new PCFuzzySystem(new_learn, new_test));
                int ground = (int)Math.Round(results[i].LearnSamplesSet.DataRows.Count * 0.25);
                for (int j = 0; j < ground; j++)
                {
                    results[i].LearnSamplesSet.DataRows.RemoveAt(rand.Next(0, results[i].LearnSamplesSet.DataRows.Count));
                }
            }
            //Создаем популяции и архив лучших положений каждой частицы
            Populations = new List<List<KnowlegeBasePCRules>>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                Populations.Add(SetPopulation(new List<KnowlegeBasePCRules>()));
            }
            KnowlegeBasePCRules BEST = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            double bestError = result.ErrorLearnSamples(BEST);
            if (double.IsNaN(bestError) || double.IsInfinity(bestError) || bestError == 100)
            {
                result.UnlaidProtectionFix(BEST);
                bestError = result.ErrorLearnSamples(BEST);
            }
                //отчистка консоли

#if debug
            Console.Clear();
#endif
                //запуск итераций
                for (int it = 1; it <= iter; it++)
            {
                for (int p_index = 0; p_index < Populations.Count; p_index++)
                {

#if debug
                //вывод номера итерации
                Console.Write("Итерация __№__ = ");
                Console.WriteLine(it);

#endif
                    //расчитыавем значение фитнес-функции
                    Populations[p_index] = ListPittsburgClassifierTool.SortRules(Populations[p_index], results[p_index]);
                    double[] K = new double[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        K[i] = results[p_index].ErrorLearnSamples(Populations[p_index][i]);
#if debug
                    Console.Write("Значние  K[i1] = ");
                    Console.WriteLine(K[i]);
#endif
                        if (double.IsNaN(K[i]) || double.IsInfinity(K[i]) || K[i] == 100)
                        {
                            results[p_index].UnlaidProtectionFix(Populations[p_index][i]);
                            K[i] = results[p_index].ErrorLearnSamples(Populations[p_index][i]);

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
                    var Xfood = new KnowlegeBasePCRules(Populations[p_index][0]);
                    for (int t = 0; t < Xfood.TermsSet.Count; t++)
                    {
                        for (int p = 0; p < Xfood.TermsSet[t].CountParams; p++)
                        {
                            Xfood.TermsSet[t].Parametrs[p] = 0;

                            for (int i = 0; i < Populations[p_index].Count; i++)
                            {
                                Xfood.TermsSet[t].Parametrs[p] += Populations[p_index][i].TermsSet[t].Parametrs[p] / K[i];
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
                    double Kfood = results[p_index].ErrorLearnSamples(Xfood);
                    if (double.IsNaN(Kfood) || double.IsInfinity(Kfood) || Kfood == 100)
                    {
                        results[p_index].UnlaidProtectionFix(Xfood);
                        Kfood = results[p_index].ErrorLearnSamples(Xfood);
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
                    KnowlegeBasePCRules[] Bfood = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        Bfood[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
                        double KRoofifood = CalcKroof(K[i], Kfood);
                        KnowlegeBasePCRules Xroofifood = new KnowlegeBasePCRules(CalcXroof(Populations[p_index][i], Xfood));
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
                    KnowlegeBasePCRules[] Bbest = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        Bbest[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
                        double KRoofifood;
                        if (i != ibest)
                        {
                            KRoofifood = CalcKroof(K[i], K[ibest]);
                        }
                        else
                        {
                            KRoofifood = 1.0;
                        }
                        KnowlegeBasePCRules Xroofifood = new KnowlegeBasePCRules(CalcXroof(Populations[p_index][i], Populations[p_index][ibest]));
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
                    KnowlegeBasePCRules[] B = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        B[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                    KnowlegeBasePCRules[] F = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        if (i == 0)
                        {
                            F[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                            F[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                    List<int>[] neihbors = new List<int>[Populations[p_index].Count];
                    //расчитываем значение alocal
                    KnowlegeBasePCRules[] alocal = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        alocal[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
                        neihbors[i] = countneihbors(Populations[p_index][i], Populations[p_index]);
                        /*
                        #if debug
                                            //вывод значений количества соседей
                                            for (int g = 0; g < Populations[p_index].Count; g++)
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
                                    KnowlegeBasePCRules XRoofij = new KnowlegeBasePCRules(CalcXroof(Populations[p_index][i], Populations[p_index][neihbors[i][j]]));

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
                    KnowlegeBasePCRules[] atarget = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        atarget[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
                        double KRoofibest = CalcKroof(K[i], K[ibest]);
                        KnowlegeBasePCRules XRoofibest = new KnowlegeBasePCRules(CalcXroof(Populations[p_index][i], Populations[p_index][ibest]));
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
                    KnowlegeBasePCRules[] a = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        a[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                    KnowlegeBasePCRules[] N = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        if (i == 0)
                        {
                            N[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                            N[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                    KnowlegeBasePCRules[] dX = new KnowlegeBasePCRules[Populations[p_index].Count];
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        dX[i] = new KnowlegeBasePCRules(Populations[p_index][i]);
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
                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        Populations[p_index][i] = new KnowlegeBasePCRules(Populations[p_index][i]);
                        for (int t = 0; t < Populations[p_index][i].TermsSet.Count; t++)
                        {
                            for (int p = 0; p < F[i].TermsSet[t].CountParams; p++)
                            {
                                Populations[p_index][i].TermsSet[t].Parametrs[p] = Populations[p_index][i].TermsSet[t].Parametrs[p] + calcdeltat(ct) * dX[i].TermsSet[t].Parametrs[p];
#if debug
                            //выводим значение Xnew
                            Console.Write("Знание X(t+dt) = ");
                            Console.WriteLine(Populations[p_index][i].TermsSet[t].Parametrs[p]);
#endif
                            }
                        }
                    }




                    for (int i = 0; i < Populations[p_index].Count; i++)
                    {
                        double temp = results[p_index].ErrorLearnSamples(Populations[p_index][i]);
                        if (double.IsNaN(temp) || double.IsInfinity(temp) || temp == 100)
                        {
                            results[p_index].UnlaidProtectionFix(Xfood);
                            temp = results[p_index].ErrorLearnSamples(Populations[p_index][i]);
                        }

                        if (temp < bestError)
                        {
                            BEST = new KnowlegeBasePCRules(Populations[p_index][i]);
                            bestError = temp;
                        }
                    }


                    double y = it;
                    //if (y % 10 == 0 & y != 0)
                    //{
                    //    Console.WriteLine(it);
                    //    Console.WriteLine(bestError);
                    //}
#if debug
                // выводим значение лучшей ошибки Kbest
                Console.Write("Значние BestError = ");
                Console.WriteLine(bestError);

                Console.WriteLine(".");
#endif



                }
            }
            for (int j = 0; j < Populations.Count; j++)
            {
                Populations[j] = ListPittsburgClassifierTool.SortRules(Populations[j], results[j]);
                Console.WriteLine("Популяция №" + j + ":");
                Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(Populations[j][0]), 2));
                Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(Populations[j][0]), 2));
            }
            //Создаем ансамбль лучших решений
            List<KnowlegeBasePCRules> BestPopulation = new List<KnowlegeBasePCRules>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                BestPopulation.Add(Populations[i][0]);
            }
            //Выводим точность классификации ансамбля лучших решений
            Console.WriteLine("Bagging: ");
            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(BestPopulation), 2));
            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(BestPopulation), 2));
            //Добавляем в базу правил лучшие решения
            if (result.RulesDatabaseSet.Count == 1)
            {
                result.RulesDatabaseSet.Clear();
            }
            for (int i = 0; i < Populations.Count; i++)
            {
                result.RulesDatabaseSet.Add(Populations[i][0]);
            }
            //Возвращаем результат
            return result;
        }


        //расчитыавем значение X с крышкой
        public KnowlegeBasePCRules CalcXroof(KnowlegeBasePCRules xi, KnowlegeBasePCRules xj)
        {
            var Xroof = new KnowlegeBasePCRules(xi);
            for (int t = 0; t < Xroof.TermsSet.Count; t++)
            {
                for (int p = 0; p < Xroof.TermsSet[t].CountParams; p++)
                {   
                    Xroof.TermsSet[t].Parametrs[p] = (xj.TermsSet[t].Parametrs[p] - xi.TermsSet[t].Parametrs[p]) / (Math.Sqrt(Math.Pow((xj.TermsSet[t].Parametrs[p] - xi.TermsSet[t].Parametrs[p]), 2) + Math.Pow((xi.TermsSet[t].Parametrs[p] - xj.TermsSet[t].Parametrs[p]), 2)) + e);
                }
            }

            return Xroof;
        }

        //расчитыавем значение K с крышкой
        public double CalcKroof(double ki, double kj)
        {
            //double Kroof = (ki - kj) / (Kworst - Kbest);
            double Kroof = 1.0;

            return Kroof;
        }


        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as KrillBConfig;
            Nkrill = ((KrillBConfig)Conf).Количество_особей;
            iter = ((KrillBConfig)Conf).Количеств_итераций;
            wn = ((KrillBConfig)Conf).Вес_инерции_вызванного_движения;
            wf = ((KrillBConfig)Conf).Вес_инерции_движения_добывающего_продовольствие;
            ct = ((KrillBConfig)Conf).Коэффициент_Ct;
            dmax = ((KrillBConfig)Conf).Скорость_распространения_Dmax;
            nmax = ((KrillBConfig)Conf).Вызванная_скорость_Nmax;
            e = ((KrillBConfig)Conf).Положительное_число_е;
            Vf = ((KrillBConfig)Conf).Добывающая_продовольствие_скорость_Vf;
            numberOfPopulations = ((KrillBConfig)Conf).Количеств_популяций;
            neighbor_coeff = ((KrillBConfig)Conf).Коэффициент_соседей;
        }
        private List<KnowlegeBasePCRules> SetPopulation(List<KnowlegeBasePCRules> Population)
        {
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population.Add(TempRule);
            for (int i = 1; i < Nkrill; i++)
            {
                TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                Population.Add(TempRule);
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
            return Population;
        }

        //нахождение расстояния между крилями
        private double Distance(KnowlegeBasePCRules x, KnowlegeBasePCRules y)
        {
            double dist, sum = 0;
            for (int i = 0; i < x.TermsSet.Count; i++)
            {
                for (int j = 0; j < x.TermsSet[i].Parametrs.Length; j++)
                {
                    sum += Math.Pow(x.TermsSet[i].Parametrs[j] - y.TermsSet[i].Parametrs[j], 2);
                }
            }
            //for (int i = 0; i < x.RulesDatabase.Count; i++)
            //{
            //    sum += Math.Pow(x.RulesDatabase[i].Cons_DoubleOutput - y.RulesDatabase[i].Cons_DoubleOutput, 2);
            //}
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
        private double distneihbor(KnowlegeBasePCRules x, List<KnowlegeBasePCRules> Population)
        {
            double sum = 0, dneihbor;
            for (int j = 0; j < Population.Count; j++)
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
            dneihbor = sum * (1.0 / (neighbor_coeff * Population.Count));
            /*
            //вывод значение dneihbor соседей
            Console.Write("Значение dneihbor для расчета расстояния ощущения = ");
            Console.WriteLine(dneihbor);
            */

            return dneihbor;
        }

        //нахождение расстояния ощущения (соседства)
        private List<int> countneihbors(KnowlegeBasePCRules x, List<KnowlegeBasePCRules> Population)
        {
            double border = distneihbor(x, Population);
            List<int> neihbors = new List<int>();

            /*
            //вывод значение дистанции соседа
            for (int g = 0; g < Populations[p_index].Count; g++)
            {
                Console.Write("Знаение distneihbor = ");
                Console.WriteLine(distneihbor(x));
            }
            */

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
            KrillBConfig conf = new KrillBConfig();
            conf.Init(CountFeatures);

            return conf;
        }
        public override string ToString(bool with_param = false)
        {
            return "Krill Herd (Bagging)";
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