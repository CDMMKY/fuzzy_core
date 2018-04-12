using System;
using System.Linq;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Threading;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzyCoreUtils;



namespace MonkeyOptimization.Approx
{
    public class MonkeyTS : AbstractNotSafeLearnAlgorithm
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
        KnowlegeBaseTSARules monkeysum;
        KnowlegeBaseTSARules monkeysub;
        protected KnowlegeBaseTSARules[] monkey;
        protected KnowlegeBaseTSARules WJVector; // watch-jump vector
        protected KnowlegeBaseTSARules SSVector; // somersault vector
        protected KnowlegeBaseTSARules IndividualSSVector;
        KnowlegeBaseTSARules bestsolution;
        double bestsolutionnumber;
        int deltaLength = 0;
        int iter = 0;
        int iter_amount;
        bool debug = false;
        bool totxt = true;
        int final_iter = 0;
        int final_counter = 50;
        bool last = false;
        // string debug_string;

        //delete
        double[] testvals;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            Init(conf);
            KnowlegeBaseTSARules temp_c_Rule = new KnowlegeBaseTSARules(Classifier.RulesDatabaseSet[0]);
            TSAFuzzySystem result = Classifier;
            string file_string = @"..\logs_" + result.TestSamplesSet.FileName + ".txt";
            string file_string_to_txt = @"..\result_" + result.TestSamplesSet.FileName + ".txt";
            for (int t = 0; t < population_count; t++)
            {
                monkey[t] = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
                if (t > 3)
                    for (int k = 0; k < result.RulesDatabaseSet[0].TermsSet.Count; k++)
                    {
                        for (int q = 0; q < result.RulesDatabaseSet[0].TermsSet[k].CountParams; q++)
                        {
                            //monkey[t].TermsSet[k].Parametrs[q] = StaticRandom.NextDouble() * (result.RulesDatabaseSet[0].TermsSet[k].Max - result.RulesDatabaseSet[0].TermsSet[k].Min);
                            monkey[t].TermsSet[k].Parametrs[q] = GaussRandom.Random_gaussian(rand, monkey[t].TermsSet[k].Parametrs[q], monkey[t].TermsSet[k].Parametrs[q] * 0.05);
                        }
                    }

                double unlaidtest = result.ErrorLearnSamples(monkey[t]);
                //result.UnlaidProtectionFix(monkey[t]);
                //Console.WriteLine("Unlaid: " + result.ErrorLearnSamples(monkey[0]).ToString());
                if (double.IsNaN(unlaidtest) || double.IsInfinity(unlaidtest))
                    result.UnlaidProtectionFix(monkey[t]);
                // delete
                testvals[t] = result.ErrorLearnSamples(monkey[t]);
                Console.WriteLine("Begin: " + t.ToString() + " " + iter.ToString() + " " + testvals[t].ToString());
            }
            bestsolution = new KnowlegeBaseTSARules(monkey.SelectBest(result, 1)[0]);
            bestsolutionnumber = result.ErrorLearnSamples(bestsolution);
            deltaLength = result.RulesDatabaseSet[0].TermsSet.Sum(x => x.Parametrs.Length);
            if (debug)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_string, true))
                {
                    file.WriteLine(DateTime.Today.ToString() + "\t" + result.LearnSamplesSet.FileName);
                    file.WriteLine("Parameters:");
                    file.WriteLine("Population\t" + population_count.ToString());
                    file.WriteLine("Iteration count\t" + iter_amount.ToString());
                    file.WriteLine("Crawl count\t" + crawl_iter.ToString());
                    file.WriteLine("Jump count\t" + jump_iter.ToString());
                    file.WriteLine("Somersault count\t" + somersault_iter.ToString());
                    file.WriteLine("Crawl step\t" + step.ToString());    // crawl step
                    file.WriteLine("Jump step\t" + watch_jump_parameter.ToString());
                    file.WriteLine("Somersault left border\t" + somersault_interval_left.ToString());
                    file.WriteLine("Somersault right border\t" + somersault_interval_right.ToString());
                    file.WriteLine("\t\tMonkeys");
                    file.Write("Iterations\t");
                    for (int t = 0; t < population_count; t++)
                        file.Write("\t" + t);
                    file.WriteLine();
                    file.Write("0\tbegin");
                    for (int t = 0; t < population_count; t++)
                    {
                        file.Write("\t" + testvals[t].ToString());
                    }

                    // excel вставки
                    // наибольший в таблице
                    file.WriteLine();
                }
            }


            //iter_amount = somersault_iter * (1 + jump_iter * (1 + crawl_iter));
            iter_amount = (((crawl_iter + jump_iter) * jump_iter) + somersault_iter) * somersault_iter;
            for (int r = 0; r < somersault_iter; r++)
            {
                for (int t = 0; t < jump_iter; t++)
                {
                    for (int e = 0; e < crawl_iter; e++)
                    {
                        iter++;
                        oneClimb(result, deltaLength, step);
                        CheckForBest(result);
                        //Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString

                        // дебаг
                        if (debug)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_string, true))
                            {
                                file.Write(iter.ToString() + "\tcrawl");
                                for (int p = 0; p < population_count; p++)
                                {
                                    file.Write("\t" + result.ErrorLearnSamples(monkey[p]).ToString());
                                }
                                file.WriteLine();
                            }
                        }
                    }
                    for (int e = 0; e < jump_iter; e++)
                    {
                        iter++;
                        oneWatchJump(result);
                        //Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
                        CheckForBest(result);
                        // дебаг
                        if (debug)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_string, true))
                            {
                                file.Write(iter.ToString() + "\tlocaljump");
                                for (int p = 0; p < population_count; p++)
                                {
                                    file.Write("\t" + result.ErrorLearnSamples(monkey[p]).ToString());
                                }
                                file.WriteLine();
                            }
                        }
                    }
                }
                for (int e = 0; e < somersault_iter; e++)
                {
                    iter++;
                    oneGlobalJump(result);
                    CheckForBest(result);
                    // дебаг
                    if (debug)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_string, true))
                        {
                            file.Write(iter.ToString() + "\tglobaljump");
                            for (int p = 0; p < population_count; p++)
                            {
                                file.Write("\t" + result.ErrorLearnSamples(monkey[p]).ToString());
                            }
                            file.WriteLine();
                        }
                    }
                    Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
                }
            }
            //Console.WriteLine(final_iter.ToString() + "/" + final_counter.ToString());
            //FOR VICTORY!!!
            while ((final_iter < final_counter) && (last == true))
            {
                step *= 0.9;
                watch_jump_parameter *= 0.9;
                somersault_interval_left *= 0.9;
                somersault_interval_right *= 0.9;
                for (int r = 0; r < somersault_iter; r++)
                {
                    oneClimb(result, deltaLength, step);
                    CheckForBest(result);
                    iter++;
                }
                for (int t = 0; t < jump_iter; t++)
                {
                    oneWatchJump(result);
                    CheckForBest(result);
                    iter++;
                }
                for (int e = 0; e < crawl_iter; e++)
                {
                    oneGlobalJump(result);
                    CheckForBest(result);
                    iter++;
                }
                Console.WriteLine(iter_amount.ToString() + "/" + iter.ToString());
            }

            /*  for (int t = 0; t < population_count; t++)
                  if (result.ErrorLearnSamples(monkey[best]) < result.ErrorLearnSamples(monkey[t]))
                      best = t; */
            CheckForBest(result);
            if (bestsolutionnumber <= result.ErrorLearnSamples(result.RulesDatabaseSet[0]))
                result.RulesDatabaseSet[0] = bestsolution;
            iter = 0;
            if (debug)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_string, true))
                {
                    file.WriteLine("Results\t" + result.ErrorLearnSamples(bestsolution).ToString() + "\t" + result.ErrorTestSamples(bestsolution).ToString());
                }
            }
            if (totxt)
            {
                using (System.IO.StreamWriter file_result = new System.IO.StreamWriter(file_string_to_txt, true))
                {
                    file_result.WriteLine(result.ErrorLearnSamples(bestsolution).ToString() + "\t" + result.ErrorTestSamples(bestsolution).ToString());
                }
            }
            return result;
        }

        public virtual void CheckForBest(TSAFuzzySystem result)
        {
            KnowlegeBaseTSARules temp = monkey.SelectBest(result, 1)[0];
            double tempnumber = result.ErrorLearnSamples(temp);
            if (bestsolutionnumber > tempnumber)
            {
                bestsolution = new KnowlegeBaseTSARules(temp);
                bestsolutionnumber = tempnumber;
                // delete
                Console.WriteLine("NEWBEST " + bestsolutionnumber);
                final_iter = 0;
            }
            else
                final_iter++;
        }

        public virtual void oneClimb(TSAFuzzySystem result, int length, double st)
        {
            int i;
            double step = GaussRandom.Random_gaussian(rand, st, st * 0.05);
            KnowlegeBaseTSARules temp;
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
                monkeysum = new KnowlegeBaseTSARules(monkey[j]);
                monkeysub = new KnowlegeBaseTSARules(monkey[j]);
                temp = new KnowlegeBaseTSARules(monkey[j]);
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

                if ((result.ErrorLearnSamples(temp) < result.ErrorLearnSamples(monkey[j])))
                    monkey[j] = temp;
                //delete
                double testval = result.ErrorLearnSamples(monkey[j]);
                if (testval < testvals[j])
                {
                    //Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Climb");
                    testvals[j] = testval;
                }
            }
        }

        public virtual void oneWatchJump(TSAFuzzySystem result)
        {
            for (int j = 0; j < population_count; j++)
            {
                WJVector_gen(j);
                if (result.ErrorLearnSamples(WJVector) <= result.ErrorLearnSamples(monkey[j]))
                {
                    monkey[j] = WJVector;
                    // delete
                    //double testval = result.ErrorLearnSamples(monkey[j]);
                    //Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Success at WatchJump");
                }
            }
        }

        public virtual void oneGlobalJump(TSAFuzzySystem result)
        {
            for (int j = 0; j < population_count; j++)
            {
                // SSVector_gen();
                SSVector = bestsolution;
                IndividualSSVector = new KnowlegeBaseTSARules(monkey[j]);
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
                    //double testval = result.ErrorLearnSamples(monkey[j]);
                    //Console.WriteLine(j.ToString() + " " + iter.ToString() + " " + testval.ToString() + " - Success at Global Jump");
                }
            }
        }

        public virtual void WJVector_gen(int j)
        {
            WJVector = new KnowlegeBaseTSARules(monkey[j]);
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
            SSVector = new KnowlegeBaseTSARules(monkey[0]);
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

        public static double[] ClimbVector(int length, double step)
        {
            double[] V = new double[length];
            double val;
            for (int i = 0; i < length; i++)
            {
                val = StaticRandom.NextDouble();
                if (val < 0.4)
                    V[i] = step;
                else
                    if (val < 0.8)
                    V[i] = -step;
                else
                    V[i] = 0;
            }
            return V;
        }

        public static double[] ClimbVectorR(int length, double step)
        {
            double[] V = new double[length];
            double val;
            for (int i = 0; i < length; i++)
            {
                val = StaticRandom.NextDouble();
                if (val < 0.4)
                    V[i] = 2 * step * StaticRandom.NextDouble();
                else
                    if (val < 0.8)
                    V[i] = 2 * (-step) * StaticRandom.NextDouble();
                else
                    V[i] = 0;
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
            monkey = new KnowlegeBaseTSARules[population_count];

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
