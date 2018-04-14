using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class BaggingBSOClassifier : AbstractNotSafeLearnAlgorithm
    {
        List<int[]> groups;
        protected Random rand = new Random();
        protected PCFuzzySystem result;
        protected BSBConfig config;
        protected List<List<KnowlegeBasePCRules>> Populations;
        protected KnowlegeBasePCRules[] NewPopulation;
        protected int N, m, D, iter, cur_iter, numberOfPopulations;
        protected double p_one, p_replace, p_one_center, p_two_center, F, CR, p;
        protected int[] NS;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            result = Classifier;
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
            groups = new List<int[]>();
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
            Populations = new List<List<KnowlegeBasePCRules>>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                Populations.Add(SetPopulation(new List<KnowlegeBasePCRules>()));
                Populations[i] = ListPittsburgClassifierTool.SortRules(Populations[i], result);
            }
            NS = new int[m];
            for (int i = 0; i < m; i++)
            {
                NS[i] = (N - 1) / m;
            }
            cur_iter = 0;
            while (cur_iter < iter)
            {
                for (int p_i = 0; p_i < Populations.Count; p_i++)
                {
                    groups = GroupStream(Populations[p_i]);
                    if (p_one > rand.NextDouble())
                    {
                        ChooseOneCluster(Populations[p_i]);
                    }
                    else
                    {
                        ChooseTwoClusters(Populations[p_i]);
                    }
                    Populations[p_i] = ListPittsburgClassifierTool.SortRules(Populations[p_i], results[p_i]);
                    //Console.WriteLine(cur_iter + " - Итерация");
                    //Console.WriteLine("Обуч. выборка = " + result.ErrorLearnSamples(Populations[p_i][0]));
                    //Console.WriteLine("Тест. выборка = " + result.ErrorTestSamples(Populations[p_i][0]));
                }
                cur_iter++;
            }
            //Выводим точность классификации для лучшей частицы из каждой популяции
            for (int j = 0; j < Populations.Count; j++)
            {
                Populations[j] = ListPittsburgClassifierTool.SortRules(Populations[j], results[j]);
                Console.WriteLine("Популяция №" + j + ":");
                Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(Populations[j][0]), 2));
                Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(Populations[j][0]), 2));
            }
            //Допобавляем в базу правил лучшие решения
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

        public virtual void Init(ILearnAlgorithmConf conf)
        {
            config = conf as BSBConfig;
            iter = ((BSBConfig)conf).iter;
            N = ((BSBConfig)conf).N;
            m = ((BSBConfig)conf).m;

            F = ((BSBConfig)conf).F;

            p_one = ((BSBConfig)conf).p_one;
            p_one_center = ((BSBConfig)conf).p_one_center;
            p_two_center = ((BSBConfig)conf).p_two_center;
            p = ((BSBConfig)conf).p;
            numberOfPopulations = ((BSBConfig)conf).numberOfPopulations;

        }

        private List<KnowlegeBasePCRules> SetPopulation(List<KnowlegeBasePCRules> Population)
        {
            KnowlegeBasePCRules TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
            Population.Add(TempRule);
            for (int i = 1; i < N; i++)
            {
                TempRule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                Population.Add(TempRule);
                for (int j = 0; j < Population[i].TermsSet.Count; j++)
                {
                    for (int k = 0; k < Population[i].TermsSet[j].Parametrs.Length; k++)
                    {
                        Population[i].TermsSet[j].Parametrs[k] = GaussRandom.Random_gaussian(rand, Population[i].TermsSet[j].Parametrs[k], 0.1 * Population[i].TermsSet[j].Parametrs[k]);
                    }
                }
            }
            return Population;
        }

        private List<int[]> GroupStream(List<KnowlegeBasePCRules> Population)
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

        private void ChooseOneCluster(List<KnowlegeBasePCRules> Population)
        {
            int cluster_index = rand.Next(0, m);
            if (p_one_center > rand.NextDouble())
            {
                //Console.WriteLine("!");
                OriginalOperator(Population, cluster_index);
            }
            else
            {
                //Console.WriteLine("!!");
                OneDEOperator(Population, cluster_index);
            }
        }

        private void OriginalOperator(List<KnowlegeBasePCRules> Population, int cluster_index)
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

        private void OneDEOperator(List<KnowlegeBasePCRules> Population, int cluster_index)
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

        private void ChooseTwoClusters(List<KnowlegeBasePCRules> Population)
        {
            if (p_two_center > rand.NextDouble())
            {
                //Console.WriteLine("!!!");
                OriginalTwoClusters(Population);
            }
            else
            {
                //Console.WriteLine("!!!!");
                TwoDEOperator(Population);
            }
        }

        private void OriginalTwoClusters(List<KnowlegeBasePCRules> Population)
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

        private void TwoDEOperator(List<KnowlegeBasePCRules> Population)
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
            BSBConfig conf = new BSBConfig();
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
            return "Brain Storm Algorithm (Bagging)";
        }
    }
}
