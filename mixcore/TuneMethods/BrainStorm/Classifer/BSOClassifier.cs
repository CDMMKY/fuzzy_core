using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class BSOClassifier : AbstractNotSafeLearnAlgorithm
    {
        List<int[]> groups;
        protected Random rand = new Random();
        protected PCFuzzySystem result;
        protected BSConfig config;
        protected KnowlegeBasePCRules[] Population;
        protected KnowlegeBasePCRules[] NewPopulation;
        protected int N, m, D, iter, cur_iter;
        protected double p_one, p_replace, p_one_center, p_two_center, F, CR, p;
        protected int[] NS;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            result = Classifier;
            groups = new List<int[]>();
            Init(conf);
            SetPopulation();
            Population = SortRules(Population);
            NS = new int[m];
            for (int i = 0; i < m; i++)
            {
                NS[i] = (N - 1) / m;
            }
            cur_iter = 0;
            while (cur_iter < iter)
            {
                groups = GroupStream();
                if (p_one > rand.NextDouble())
                {
                    ChooseOneCluster();
                }
                else
                {
                    ChooseTwoClusters();
                }
                Population = ListPittsburgClassifierTool.SortRules(Population, result);
                Console.WriteLine(cur_iter + " - Итерация");
                Console.WriteLine("Обуч. выборка = " + result.ErrorLearnSamples(Population[0]));
                Console.WriteLine("Тест. выборка = " + result.ErrorTestSamples(Population[0]));
                cur_iter++;
            }
            Population = ListPittsburgClassifierTool.SortRules(Population, result);
            result.RulesDatabaseSet[0] = Population[0];
            return result;
        }

        public virtual void Init(ILearnAlgorithmConf conf)
        {
            config = conf as BSConfig;
            iter = ((BSConfig)conf).iter;
            N = ((BSConfig)conf).N;
            m = ((BSConfig)conf).m;

            F = ((BSConfig)conf).F;

            p_one = ((BSConfig)conf).p_one;
            p_one_center = ((BSConfig)conf).p_one_center;
            p_two_center = ((BSConfig)conf).p_two_center;
            p = ((BSConfig)conf).p;

        }

        private void SetPopulation()
        {
            Population = new KnowlegeBasePCRules[N];
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population[0] = TempRule;
            for (int i = 1; i < N; i++)
            {
                TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                Population[i] = TempRule;
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                    }
                }
            }
        }

        private KnowlegeBasePCRules[] SortRules(KnowlegeBasePCRules[] Source)
        {
            double[] keys = new double[Source.Count()];
            KnowlegeBasePCRules[] tempSol = Source.Clone() as KnowlegeBasePCRules[];
            for (int i = 0; i < Source.Count(); i++)
            {
                keys[i] = result.ErrorLearnSamples(Source[i]);

            }
            Array.Sort(keys, tempSol);
            return Source;
        }

        private List<int[]> GroupStream()
        {
            List<int[]> GroupsCalc = new List<int[]>();
            Dictionary<int, double> distances = new Dictionary<int, double>();
            for (int j = 1; j < N; j++)
            {
                distances.Add(j, 1);
            }
            for (int i = 0; i < m; i++)
            {
                int[] group = new int[NS[i]];
                foreach (int j in distances.Keys.ToArray())
                {
                    distances[j] = Distance(Population[0], Population[j]);

                }
                for (int j = 0; j < NS[i]; j++)
                {
                    var KeyMinValue = GetKeyByValue(distances, distances.Values.Min());
                    group[j] = KeyMinValue;
                    distances.Remove(KeyMinValue);

                }

                GroupsCalc.Add(group);
            }
            return GroupsCalc;
        }

        private static int GetKeyByValue(Dictionary<int, double> myDictionary, double value)
        {
            foreach (var recordOfDictionary in myDictionary)
            {
                if (recordOfDictionary.Value.Equals(value))
                    return recordOfDictionary.Key;
            }
            return -1;
        }

        private double Distance(KnowlegeBasePCRules x, KnowlegeBasePCRules y)
        {
            double dist, sum = 0;
            for (int i = 0; i < x.TermsSet.Count; i++)
            {
                for (int j = 0; j < x.TermsSet[j].Parametrs.Length; j++)
                {
                    sum += Math.Pow(x.TermsSet[i].Parametrs[j] - y.TermsSet[i].Parametrs[j], 2);
                }
            }

            dist = Math.Sqrt(sum);
            return dist;
        }

        private void ChooseOneCluster()
        {
            int cluster_index = rand.Next(0, m);
            if (p_one_center > rand.NextDouble())
            {
                Console.WriteLine("!");
                OriginalOperator(cluster_index);
            }
            else
            {
                Console.WriteLine("!!");
                OneDEOperator(cluster_index);
            }
        }

        private void OriginalOperator(int cluster_index)
        {
            NewPopulation = new KnowlegeBasePCRules[groups[cluster_index].Length];
            double epsi_newstep = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
            for (int i = 0; i < groups[cluster_index].Length; i++)
            {
                NewPopulation[i] = Population[groups[cluster_index][i]];
            }
            for (int i = 0; i < groups[cluster_index].Length; i++)
            {
                int number = groups[cluster_index][i];

                for (int j = 0; j < NewPopulation[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < NewPopulation[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] += epsi_newstep * rand.NextDouble();
                    }
                }
                double errornew = result.ErrorLearnSamples(NewPopulation[i]);
                double errorold = result.ErrorLearnSamples(Population[number]);
                if (errornew < errorold)
                {
                    Population[number] = NewPopulation[i];
                }
            }
        }

        private void OneDEOperator(int cluster_index)
        {
            NewPopulation = new KnowlegeBasePCRules[groups[cluster_index].Length];
            double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
            for (int i = 0; i < groups[cluster_index].Length; i++)
            {
                NewPopulation[i] = Population[groups[cluster_index][i]];
            }
            for (int i = 1; i < groups[cluster_index].Length; i++)
            {
                int rand1 = rand.Next(0, groups[cluster_index].Length);
                int rand2 = rand.Next(0, groups[cluster_index].Length);
                int rand3 = rand.Next(0, groups[cluster_index].Length);
                while (rand2 == rand1)
                    rand2 = rand.Next(0, groups[cluster_index].Length);
                while ((rand3 == rand1) || (rand3 == rand2))
                    rand3 = rand.Next(0, groups[cluster_index].Length);

                int number1 = groups[cluster_index][rand1];
                int number2 = groups[cluster_index][rand2];
                int number3 = groups[cluster_index][rand3];
                for (int j = 0; j < Population[groups[cluster_index][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] = Population[number1].TermsSet[j].Parametrs[k] + F * (Population[number2].TermsSet[j].Parametrs[k] - Population[number3].TermsSet[j].Parametrs[k]);
                    }
                }
                double errornew = result.ErrorLearnSamples(NewPopulation[i]);
                double errorold1 = result.ErrorLearnSamples(Population[number1]);
                double errorold2 = result.ErrorLearnSamples(Population[number2]);
                double errorold3 = result.ErrorLearnSamples(Population[number3]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else
                {
                    if (errorold2 > errornew)
                    {
                        Population[number2] = NewPopulation[i];
                    }
                    else if (errorold3 > errornew)
                    {
                        Population[number3] = NewPopulation[i];

                    }
                }
                //Console.WriteLine("новый этап");
                for (int j = 0; j < Population[groups[cluster_index][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();

                        // Console.WriteLine(NewPopulation[i].TermsSet[j].Parametrs[k]);
                    }
                }
                errornew = result.ErrorLearnSamples(NewPopulation[i]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else
                {
                    if (errorold2 > errornew)
                    {
                        Population[number2] = NewPopulation[i];
                    }
                    else if (errorold3 > errornew)
                    {
                        Population[number3] = NewPopulation[i];
                    }
                }

            }
        }

        private void ChooseTwoClusters()
        {
            if (p_two_center > rand.NextDouble())
            {
                Console.WriteLine("!!!");
                OriginalTwoClusters();
            }
            else
            {
                Console.WriteLine("!!!!");
                TwoDEOperator();
            }
        }

        private void OriginalTwoClusters()
        {
            int cluster_index_1 = rand.Next(0, m);
            int cluster_index_2 = rand.Next(0, m);
            while (cluster_index_1 == cluster_index_2)
                cluster_index_2 = rand.Next(0, m);

            NewPopulation = new KnowlegeBasePCRules[groups[cluster_index_1].Length];
            double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));

            for (int i = 0; i < groups[cluster_index_1].Length; i++)
            {
                NewPopulation[i] = Population[groups[cluster_index_1][i]];
            }
            for (int i = 0; i < groups[cluster_index_1].Length; i++)
            {
                int number1 = groups[cluster_index_1][i];
                int number2 = groups[cluster_index_2][i];
                for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        double rand1 = rand.NextDouble();
                        NewPopulation[i].TermsSet[j].Parametrs[k] = rand1 * Population[number1].TermsSet[j].Parametrs[k] + (1 - rand1) * Population[number2].TermsSet[j].Parametrs[k];
                    }
                }
                double errornew = result.ErrorLearnSamples(NewPopulation[i]);
                double errorold1 = result.ErrorLearnSamples(Population[number1]);
                double errorold2 = result.ErrorLearnSamples(Population[number2]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else if (errorold2 > errornew)
                {
                    Population[number2] = NewPopulation[i];
                }
                for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();
                    }
                }
                errornew = result.ErrorLearnSamples(NewPopulation[i]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else if (errorold2 > errornew)
                    {
                        Population[number2] = NewPopulation[i];
                    }
            }
        }

        private void TwoDEOperator()
        {
            int cluster_index_1 = rand.Next(0, m);
            int cluster_index_2 = rand.Next(0, m);
            while (cluster_index_1 == cluster_index_2)
                cluster_index_2 = rand.Next(0, m);

            NewPopulation = new KnowlegeBasePCRules[groups[cluster_index_1].Length];
            double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
            for (int i = 0; i < groups[cluster_index_1].Length; i++)
            {
                NewPopulation[i] = Population[groups[cluster_index_1][i]];
            }
            for (int i = 1; i < groups[cluster_index_1].Length; i++)
            {
                int rand1 = rand.Next(0, groups[cluster_index_1].Length);
                int rand2 = rand.Next(0, groups[cluster_index_2].Length);
                while (rand2 == rand1)
                    rand2 = rand.Next(0, groups[cluster_index_2].Length);
                int number1 = groups[cluster_index_1][rand1];
                int number2 = groups[cluster_index_2][rand2];
                for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] = Population[0].TermsSet[j].Parametrs[k] + F * (Population[number1].TermsSet[j].Parametrs[k] - Population[number2].TermsSet[j].Parametrs[k]);
                    }
                }

                double errornew = result.ErrorLearnSamples(NewPopulation[i]);
                double errorold1 = result.ErrorLearnSamples(Population[number1]);
                double errorold2 = result.ErrorLearnSamples(Population[number2]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else if (errorold2 > errornew)
                {
                    Population[number2] = NewPopulation[i];
                }
                //      Console.WriteLine( "новый этап /n");
                for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
                    {
                        NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();
                    }
                }
                errornew = result.ErrorLearnSamples(NewPopulation[i]);
                if (errorold1 > errornew)
                {
                    Population[number1] = NewPopulation[i];
                }
                else if (errorold2 > errornew)
                {
                    Population[number2] = NewPopulation[i];
                }
            }
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
            BSConfig conf = new BSConfig();
            conf.Init(CountFeatures);
            return conf;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Brain Storm Algorithm" + "{" + Environment.NewLine;
                result = "Итераций = " + iter + ";" + Environment.NewLine;
                result = "Идей = " + N + ";" + Environment.NewLine;
                return result;
            }
            return "Brain Storm Algorithm";
        }
    }
}
