using System;
using System.Linq;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Threading;
using FuzzySystem.SingletoneApproximate;
using FuzzyCoreUtils;



namespace MonkeyOptimization.Approx
{
    public class MonkeyST : AbstractNotSafeLearnAlgorithm
    {
        Random rand = new Random();
        int population_count;
        int crawl_iter;
        int jump_iter;
        int somersault_iter;
        double step;    // crawl step
        double watch_jump_parameter;
        double somersault_interval_left;
        double somersault_interval_right;
        KnowlegeBaseSARules monkeysum;
        KnowlegeBaseSARules monkeysub;
        protected KnowlegeBaseSARules[] monkey;
        protected KnowlegeBaseSARules WJVector; // watch-jump vector
        protected KnowlegeBaseSARules SSVector; // somersault vector
        protected KnowlegeBaseSARules IndividualSSVector;
        KnowlegeBaseSARules bestsolution;
        double bestsolutionnumber;
        int deltaLength = 0;
        int iter = 0;
        int iter_amount;

        //delete
        double[] testvals;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            Init(conf);
            KnowlegeBaseSARules temp_c_Rule = new KnowlegeBaseSARules(Classifier.RulesDatabaseSet[0]);
            SAFuzzySystem result = Classifier;
            for (int t = 0; t < population_count; t++)
            {
                monkey[t] = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
                for (int k = 0; k < result.RulesDatabaseSet[0].TermsSet.Count; k++)
                {
                    for (int q = 0; q < result.RulesDatabaseSet[0].TermsSet[k].CountParams; q++)
                    {
                        //monkey[t].TermsSet[k].Parametrs[q] = StaticRandom.NextDouble() * (result.RulesDatabaseSet[0].TermsSet[k].Max - result.RulesDatabaseSet[0].TermsSet[k].Min);
                        monkey[t].TermsSet[k].Parametrs[q] = GaussRandom.Random_gaussian(rand, monkey[t].TermsSet[k].Parametrs[q], monkey[t].TermsSet[k].Parametrs[q] * 0.05);
                    }
                }

                result.UnlaidProtectionFix(monkey[t]);

                // delete
                testvals[t] = result.ErrorLearnSamples(monkey[t]);
                Console.WriteLine("Begin: " + t.ToString() + " " + iter.ToString() + " " + testvals[t].ToString());
            }
            bestsolution = new KnowlegeBaseSARules(monkey.SelectBest(result, 1)[0]);
            bestsolutionnumber = result.ErrorLearnSamples(bestsolution);

            iter_amount = somersault_iter * (1 + jump_iter * (1 + crawl_iter));
            deltaLength = result.RulesDatabaseSet[0].TermsSet.Sum(x => x.Parametrs.Length);
            for (int r = 0; r < somersault_iter; r++)
            {
                for (int t = 0; t < jump_iter; t++)
                {
                    for (int e = 0; e < crawl_iter; e++)
                    {
                        iter++;
                        CheckForBest(result);
                        oneClimb(result, deltaLength, step);
                        //Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
                    }
                    iter++;
                    oneWatchJump(result);
                    //Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
                }
                iter++;
                oneGlobalJump(result);
                Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
            }
            /*  for (int t = 0; t < population_count; t++)
                  if (result.ErrorLearnSamples(monkey[best]) < result.ErrorLearnSamples(monkey[t]))
                      best = t; */
            CheckForBest(result);
            if (bestsolutionnumber < result.ErrorLearnSamples(result.RulesDatabaseSet[0]))
                result.RulesDatabaseSet[0] = bestsolution;
            iter = 0;
            return result;
        }

        public virtual void CheckForBest(SAFuzzySystem result)
        {
            KnowlegeBaseSARules temp = monkey.SelectBest(result, 1)[0];
            double tempnumber = result.ErrorLearnSamples(temp);
            if (bestsolutionnumber > tempnumber)
            {
                bestsolution = new KnowlegeBaseSARules(temp);
                bestsolutionnumber = tempnumber;
                // delete
                // Console.WriteLine("NEWBEST " + bestsolutionnumber);
            }
        }

        public virtual void oneClimb(SAFuzzySystem result, int length, double st)
        {
            int i;
            double step = GaussRandom.Random_gaussian(rand, st, st * 0.05);
            KnowlegeBaseSARules temp;
            double[] delta = new double[length];
            bool sign_f;
            int sign_num;
            for (int j = 0; j < population_count; j++)
            {
                i = 0;
                if (j % 4 < 2)
                    delta = ClimbVector(length, step);
                else
                    delta = ClimbVectorR(length, step);
                monkeysum = new KnowlegeBaseSARules(monkey[j]);
                monkeysub = new KnowlegeBaseSARules(monkey[j]);
                temp = new KnowlegeBaseSARules(monkey[j]);
                for (int k = 0; k < result.RulesDatabaseSet[0].TermsSet.Count; k++)
                {
                    for (int q = 0; q < result.RulesDatabaseSet[0].TermsSet[k].CountParams; q++, i++)
                    {
                        monkeysum.TermsSet[k].Parametrs[q] += delta[i];
                        monkeysub.TermsSet[k].Parametrs[q] -= delta[i];
                    }
                }
                if (result.ErrorLearnSamples(monkeysum) < result.ErrorLearnSamples(monkeysub))
                    sign_num = 1;
                else
                    sign_num = -1;
                i = 0;
                if (j % 2 == 0)
                {
                    for (int k = 0; k < result.RulesDatabaseSet[0].TermsSet.Count; k++)
                    {
                        for (int q = 0; q < result.RulesDatabaseSet[0].TermsSet[k].CountParams; q++, i++)
                        {
                            // old
                            //temp.TermsSet[k].Parametrs[q] = monkey[j].TermsSet[k].Parametrs[q] + step * sign(monkeysum, monkeysub, delta[i], result);

                            // Двойной шаг
                            // temp.TermsSet[k].Parametrs[q] = monkey[j].TermsSet[k].Parametrs[q] + step * sign_num * delta[i];

                            temp.TermsSet[k].Parametrs[q] = monkey[j].TermsSet[k].Parametrs[q] + sign_num * delta[i];
                        }
                    }

                }
                // Идея ГИВ
                else
                {
                    sign_f = (result.ErrorLearnSamples(monkeysum) < result.ErrorLearnSamples(monkeysub));
                    if (sign_f) temp = monkeysum;
                    else temp = monkeysub;
                }

                //if ((result.ErrorLearnSamples(temp) < result.ErrorLearnSamples(monkey[j])))
                monkey[j] = temp;
                //delete
                double testval = result.ErrorLearnSamples(monkey[j]);
                if (testval < testvals[j])
                {
                    Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Climb");
                    testvals[j] = testval;
                }
            }
        }

        public virtual void oneWatchJump(SAFuzzySystem result)
        {
            for (int j = 0; j < population_count; j++)
            {
                WJVector_gen(j);
                if (result.ErrorLearnSamples(WJVector) <= result.ErrorLearnSamples(monkey[j]))
                {
                    monkey[j] = WJVector;
                    // delete
                    double testval = result.ErrorLearnSamples(monkey[j]);
                    Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Success at WatchJump");
                }
            }
        }

        public virtual void oneGlobalJump(SAFuzzySystem result)
        {
            for (int j = 0; j < population_count; j++)
            {
                SSVector_gen();
                IndividualSSVector = new KnowlegeBaseSARules(monkey[j]);
                for (int k = 0; k < monkey[j].TermsSet.Count; k++)
                {
                    for (int q = 0; q < monkey[j].TermsSet[k].CountParams; q++)
                    {
                        IndividualSSVector.TermsSet[k].Parametrs[q] += (somersault_interval_left + (somersault_interval_right - somersault_interval_left) * StaticRandom.NextDouble()) * (SSVector.TermsSet[k].Parametrs[q] - monkey[j].TermsSet[k].Parametrs[q]);
                    }
                }
                if (result.ErrorLearnSamples(IndividualSSVector) <= result.ErrorLearnSamples(monkey[j]))
                {
                    monkey[j] = IndividualSSVector;
                    // delete
                    double testval = result.ErrorLearnSamples(monkey[j]);
                    Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Success at Global Jump");
                }
            }
        }

        public virtual void WJVector_gen(int j)
        {
            WJVector = new KnowlegeBaseSARules(monkey[j]);
            for (int k = 0; k < monkey[j].TermsSet.Count; k++)
            {
                for (int q = 0; q < monkey[j].TermsSet[k].CountParams; q++)
                {
                    WJVector.TermsSet[k].Parametrs[q] += 2 * (StaticRandom.NextDouble() - 0.5) * watch_jump_parameter;
                }
            }
        }

        public virtual void SSVector_gen()
        {
            SSVector = new KnowlegeBaseSARules(monkey[0]);
            for (int j = 1; j < population_count; j++)
            {
                for (int k = 0; k < monkey[j].TermsSet.Count; k++)
                {
                    for (int q = 0; q < monkey[j].TermsSet[k].CountParams; q++)
                    {
                        SSVector.TermsSet[k].Parametrs[q] += monkey[j].TermsSet[k].Parametrs[q];
                    }
                }
            }
            for (int k = 0; k < SSVector.TermsSet.Count; k++)
            {
                for (int q = 0; q < SSVector.TermsSet[k].CountParams; q++)
                {
                    SSVector.TermsSet[k].Parametrs[q] /= population_count;
                }
            }
        }

        // дописываю
        public static int sign(KnowlegeBaseSARules monkeysum, KnowlegeBaseSARules monkeysub, double num, SAFuzzySystem result)
        {
            double f1 = ((result.ErrorLearnSamples(monkeysum) - result.ErrorLearnSamples(monkeysub))) / 2 * num;
            if (f1 < 0)
                return 1;
            else
                return -1;
        }

        public static double[] ClimbVector(int length, double step)
        {
            double[] V = new double[length];
            for (int i = 0; i < length; i++)
            {
                if (StaticRandom.Next(2) == 0)
                    V[i] = step;
                else
                    V[i] = -step;
            }
            return V;
        }

        public static double[] ClimbVectorR(int length, double step)
        {
            double[] V = new double[length];
            for (int i = 0; i < length; i++)
            {
                if (StaticRandom.Next(2) == 0)
                    V[i] = 2 * step * StaticRandom.NextDouble();
                else
                    V[i] = 2 * (-step) * StaticRandom.NextDouble();
            }
            return V;
        }

        public override string ToString(bool with_param = false)// без параметров возвращает имя алгоритма, с параметров true возвращает имя алгоритма и значения его параметров
        {
            if (with_param)
            {
                string result = "Обезьяний алгоритм {";
                result += "Количеств обезьян=" + population_count.ToString() + " ;" + Environment.NewLine;
                result += "Шаг=" + step.ToString() + " ;" + Environment.NewLine;
                result += "Интервал локального прыжка=" + watch_jump_parameter.ToString() + " ;" + Environment.NewLine;
                result += "Интервал кувырка= [" + somersault_interval_left.ToString() + " ;" + somersault_interval_right.ToString() + "];" + Environment.NewLine;
                result += "Итераций движения=" + crawl_iter.ToString() + " ;" + Environment.NewLine;
                result += "Итераций прыжка=" + jump_iter.ToString() + " ;" + Environment.NewLine;
                result += "Итераций кувырка=" + somersault_iter.ToString() + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Обезьяний алгоритм";
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
        /*
        public ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигуратора для вашего метода
        {
            throw (new NotImplementedException());
        }*/

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new Param();
            result.Init(CountFeatures);
            return result;
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            Param conf = Config as Param;

            population_count = conf.Количество_особей;
            monkey = new KnowlegeBaseSARules[population_count];

            // delete
            testvals = new double[population_count];

            crawl_iter = conf.Итераций_движения;
            jump_iter = conf.Итераций_прыжка;
            somersault_iter = conf.Итераций_кувырка;
            step = conf.Шаг;
            watch_jump_parameter = conf.Интервал_локального_прыжка;
            somersault_interval_left = conf.Левая_граница_кувырка;
            somersault_interval_right = conf.Правая_граница_кувырка;
        }
    }
}
