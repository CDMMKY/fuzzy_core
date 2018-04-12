using System;
using System.Linq;
using FuzzyCoreUtils;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System.Collections.Generic;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class DBSOClassifier : AbstractNotSafeLearnAlgorithm
    {
        List<int[][]> groups;
        protected Random rand = new Random();
        protected PCFuzzySystem result;
        protected DBSConfig config;
        protected List<bool[]> Population;
        protected List<bool[]> NewPopulation;
        protected int N, m, D, iter, cur_iter;
        protected double p_one, p_replace, p_one_center, p_two_center, F, CR, p;
        protected int[] NS;
        protected List<int[]> NP;
        protected int numberOfParametrs, numberOfFeatures;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            result = Classifier;
            numberOfFeatures = result.CountFeatures;
            groups = new List<int[][]>();
            Init(conf);
            SetPopulation();
            NS = new int[m];
            for (int i = 0; i < m; i++)
            {
                NS[i] = (N-1) / m;
            }
            cur_iter = 0;
            double featurecount = 0;

            while (cur_iter < iter)
            {
                //Console.WriteLine("\nИТЕРАЦИЯ " + cur_iter);

                SortPopulation();

                groups = GroupStream();
                if (p_one > rand.NextDouble())
                {
                    ChooseOneCluster();
                }
                else
                {
                    ChooseTwoClusters();

                }

                SortPopulation();
                cur_iter++;

            }
            for (int j = 0; j < Population[0].Length; j++)
            {

                if (Population[0][j] == true)
                    featurecount += 1;
            }
            result.AcceptedFeatures = Population[0];
            Console.WriteLine();
            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(result.RulesDatabaseSet[0]), 2));
            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2));
            Console.WriteLine("Признаки:" + featurecount);
            featurecount = 0;
            return result;
        }



        private void SetPopulation()
        {
            Population = new List<bool[]>();
            for (int i = 0; i < N; i++)
            {
                Population.Add(new bool[numberOfFeatures]);

                for (int j = 0; j < Population[i].Length; j++)

                {

                    Population[i][j] = BoolRand();

                }

            }

        }
        public virtual void Init(ILearnAlgorithmConf conf)
        {
            config = conf as DBSConfig;
            iter = ((DBSConfig)conf).iter;
            N = ((DBSConfig)conf).N;
            m = ((DBSConfig)conf).m;

            F = ((DBSConfig)conf).F;

            p_one = ((DBSConfig)conf).p_one;
            p_one_center = ((DBSConfig)conf).p_one_center;
            p_two_center = ((DBSConfig)conf).p_two_center;
            p = ((DBSConfig)conf).p;

        }


        private void SortPopulation()

        {

            Dictionary<int, Tuple<bool[],double>> PopulationWithAccuracy = new Dictionary<int, Tuple<bool[], double>>();

            double accuracy = 0;
            

            for (int i = 0; i < Population.Count; i++)

            {
                
                result.AcceptedFeatures = Population[i];

                accuracy = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                Tuple<bool[], double> tuple = new Tuple<bool[], double>(Population[i], accuracy);
                try
                {
                    PopulationWithAccuracy.Add(i,  tuple);
                }
                catch(ArgumentException)
                {
                    //Console.WriteLine(i);
                }


            }
            double[] accuracylist = new double[Population.Count];

            Population.Clear();

            foreach (var pair in PopulationWithAccuracy.OrderByDescending(pair => pair.Value.Item2))

            {
                accuracylist[pair.Key] = pair.Value.Item2;
                Population.Add(pair.Value.Item1);

            }

            PopulationWithAccuracy.Clear();

        }

        private bool BoolRand()

        {
            if (rand.Next(0, 2) == 0)
                return false;
            else
                return true;
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
        
        private List<int[][]> GroupStream()
        {
            List<int[][]> GroupsCalc = new List<int[][]>();
            NP = new List<int[]>();


            for (int i = 0; i < Population.Count; i++)

            {
                NP.Add(new int [Population[i].Length]);
                for (int j = 0; j < Population[i].Length; j++)
                {
                    NP[i][j] = ToInt(Population[i][j]);
                }
            }
            List <int[]> NewP = new List<int[]> (NP);
            List<int[][]> group = new List<int[][]>();
            for (int i = 0; i < m; i++)
            {
                group.Add(new int[NS[i]][]);
                for (int j = 0; j < NS[i]; j++)
                { 
                     group[i][j] = NewP[j];
                     
                }
                NewP.RemoveRange(0,NS[i]);
                GroupsCalc.Add(group[i]);
            }
            
            return GroupsCalc;
       }



    //foreach (int j in distances.Keys.ToArray())
    //{
    //    distances[j] = Distance(Population[0], Population[j]);

    //}
    //for (int j = 0; j < NS[i]; j++)
    //{
    //    var KeyMinValue = GetKeyByValue(distances, distances.Values.Min());
    //    group[j] = KeyMinValue;
    //    distances.Remove(KeyMinValue);

    //}




        private int ToInt(bool x)
        {
            int c = 0;
            if (x == true)
                c = 1;
            return c;
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

        private void ChooseOneCluster()
        {
            int cluster_index = rand.Next(0, m);
            if (p_one_center > 0)//rand.NextDouble())
            {
                //Console.WriteLine("!");
                OriginalOperator(cluster_index);
            }
            else
            {
                Console.WriteLine("sas");
                return;
                //    Console.WriteLine("!!");
                //    OneDEOperator(cluster_index);
                //
            }
        }

        private void OriginalOperator(int cluster_index)
        {
            //Console.WriteLine(cluster_index);
            NewPopulation = new List<bool[]>();
            double fu = 0;
            int number = cluster_index * groups[cluster_index].Length;
            double epsi_newstep = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
            for (int i = 0; i < groups[cluster_index].Length; i++)
            {
                NewPopulation.Add(new bool[numberOfFeatures]);
                Population[number].CopyTo(NewPopulation[i], 0);
                number++;
            }
            number = cluster_index * groups[cluster_index].Length;
            for (int i = 0; i < groups[cluster_index].Length; i++)
            {
                for (int j = 0; j < Population[i].Length; j++)
                {
                        double c = groups[cluster_index][i][j];
                        c = c + epsi_newstep * rand.NextDouble();
                        fu = Convert(c);
                        if (fu == 1)
                            NewPopulation[i][j] = true;
                        else
                            NewPopulation[i][j] = false;                    
                }
                result.AcceptedFeatures = NewPopulation[i];
                double accuracy = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                result.AcceptedFeatures = Population[number];
                double accuracyold = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                if (accuracy > accuracyold)
                {
                    Population[number] = NewPopulation[i];
                }
                else
                    Population[number] = Population[number];
                number++;

            }
            NewPopulation.Clear();

        }
        private double Convert(double x)
        {
            double f = 0;
            int c = 0;
            double rnd = rand.NextDouble();
            x = -x;
            f = 1 / (1 + Math.Exp(x));
            if (rnd > f)
                c = 1;
            return c;
        }

        //private void OneDEOperator(int cluster_index)
        //{
        //    NewPopulation = new List<bool[]>();
        //    NewPopulation[0] = Population[0];
        //    double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
        //    for (int i = 1; i < groups[cluster_index].Length; i++)
        //    {
        //        NewPopulation[i] = Population[groups[cluster_index][i]];
        //    }
        //    for (int i = 1; i < groups[cluster_index].Length; i++)
        //    {
        //        int rand1 = rand.Next(0, groups[cluster_index].Length);
        //        int rand2 = rand.Next(0, groups[cluster_index].Length);
        //        int rand3 = rand.Next(0, groups[cluster_index].Length);
        //        while (rand2 == rand1)
        //            rand2 = rand.Next(0, groups[cluster_index].Length);
        //        while ((rand3 == rand1) || (rand3 == rand2))
        //            rand3 = rand.Next(0, groups[cluster_index].Length);

        //        int number1 = groups[cluster_index][rand1];
        //        int number2 = groups[cluster_index][rand2];
        //        int number3 = groups[cluster_index][rand3];
        //        for (int j = 0; j < Population[groups[cluster_index][i]].TermsSet.Count; j++)
        //        {
        //            for (int k = 0; k < Population[groups[cluster_index][i]].TermsSet[j].Parametrs.Length; k++)
        //            {
        //                NewPopulation[i].TermsSet[j].Parametrs[k] = Population[number1].TermsSet[j].Parametrs[k] + F * (Population[number2].TermsSet[j].Parametrs[k] - Population[number3].TermsSet[j].Parametrs[k]);
        //                // Console.WriteLine(NewPopulation[i].TermsSet[j].Parametrs[k]);
        //            }
        //        }
        //        if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number1]))
        //        {
        //            Population[number1] = NewPopulation[i];
        //        }
        //        else
        //        {
        //            if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number2]))
        //            {
        //                Population[number2] = NewPopulation[i];
        //            }
        //            else if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number3]))
        //            {
        //                Population[number3] = NewPopulation[i];

        //            }
        //        }
        //        //Console.WriteLine("новый этап");
        //        for (int j = 0; j < Population[groups[cluster_index][i]].TermsSet.Count; j++)
        //        {
        //            for (int k = 0; k < Population[groups[cluster_index][i]].TermsSet[j].Parametrs.Length; k++)
        //            {
        //                NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();

        //                // Console.WriteLine(NewPopulation[i].TermsSet[j].Parametrs[k]);
        //            }
        //        }
        //        if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number1]))
        //        {
        //            Population[number1] = NewPopulation[i];
        //        }
        //        else
        //        {
        //            if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number2]))
        //            {
        //                Population[number2] = NewPopulation[i];
        //            }
        //            else if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number3]))
        //            {
        //                Population[number3] = NewPopulation[i];
        //            }
        //        }

        //    }
        //}

        private void ChooseTwoClusters()
        {
            //if (p_two_center > rand.NextDouble())
            //{
                //Console.WriteLine("!!!");
                OriginalTwoClusters();
            //}
            //else
            //{
            //    Console.WriteLine("!!!!");
            //    TwoDEOperator();
            //}
        }

        private void OriginalTwoClusters()
        {
            NewPopulation = new List<bool[]>();
            int cluster_index_1 = rand.Next(0, m);
            int cluster_index_2 = rand.Next(0, m);
            while (cluster_index_1 == cluster_index_2)
                cluster_index_2 = rand.Next(0, m);
            int number1 = cluster_index_1 * groups[cluster_index_1].Length;
            double epsi_newstep = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
            for (int i = 0; i < groups[cluster_index_1].Length; i++)
            {
                NewPopulation.Add(new bool[numberOfFeatures]);
                Population[number1].CopyTo(NewPopulation[i],0);
                number1++;
            }
            //Console.WriteLine(cluster_index_1+" "+cluster_index_2);


            number1 = cluster_index_1 * groups[cluster_index_1].Length;
            int number2 = cluster_index_2 * groups[cluster_index_2].Length;

            double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));

            for (int i = 0; i < groups[cluster_index_1].Length; i++)
            {
                for (int j = 0; j < Population[i].Length; j++)
                {
                    double rand1 = rand.NextDouble();
                    double c = groups[cluster_index_1][i][j];
                    c = rand1 * NP[number1][j] + (1 - rand1) * NP[number2][j];
                    double fu = Convert(c);
                    if (fu == 1)
                        NewPopulation[i][j] = true;
                    else
                        NewPopulation[i][j] = false;

                }
                result.AcceptedFeatures = NewPopulation[i];
                double accuracy = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                result.AcceptedFeatures = Population[number1];
                double accuracyold1 = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                result.AcceptedFeatures = Population[number2];
                double accuracyold2 = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);

                if (accuracy > accuracyold1)
                {
                    Population[number1] = NewPopulation[i];
                }
                else if (accuracy > accuracyold2)
                {
                    Population[number2] = NewPopulation[i];
                }
                
                number2++;
                number1++;
                //number2++;
                //for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
                //{
                //    for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
                //    {
                //        NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();
                //    }
                //}
                //if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number1]))
                //{
                //    Population[number1] = NewPopulation[i];
                //}
                //else if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number2]))
                //{
                //    Population[number2] = NewPopulation[i];
                //}

            }
            NewPopulation.Clear();

        }

        //private void TwoDEOperator()
        //{
        //    int cluster_index_1 = rand.Next(0, m);
        //    int cluster_index_2 = rand.Next(0, m);
        //    while (cluster_index_1 == cluster_index_2)
        //        cluster_index_2 = rand.Next(0, m);

        //    NewPopulation = new KnowlegeBasePCRules[groups[cluster_index_1].Length];
        //    NewPopulation[0] = Population[0];
        //    double epsi = rand.NextDouble() * Math.Exp(1 - (iter / (iter - cur_iter + 1)));
        //    for (int i = 1; i < groups[cluster_index_1].Length; i++)
        //    {
        //        NewPopulation[i] = Population[groups[cluster_index_1][i]];
        //    }
        //    for (int i = 1; i < groups[cluster_index_1].Length; i++)
        //    {
        //        int rand1 = rand.Next(0, groups[cluster_index_1].Length);
        //        int rand2 = rand.Next(0, groups[cluster_index_2].Length);
        //        while (rand2 == rand1)
        //            rand2 = rand.Next(0, groups[cluster_index_2].Length);
        //        int number1 = groups[cluster_index_1][rand1];
        //        int number2 = groups[cluster_index_2][rand2];
        //        for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
        //        {
        //            for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
        //            {
        //                NewPopulation[i].TermsSet[j].Parametrs[k] = Population[0].TermsSet[j].Parametrs[k] + F * (Population[number1].TermsSet[j].Parametrs[k] - Population[number2].TermsSet[j].Parametrs[k]);
        //                //                  Console.WriteLine(NewPopulation[i].TermsSet[j].Parametrs[k]);

        //            }
        //        }



        //        if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number1]))
        //        {
        //            Population[number1] = NewPopulation[i];
        //        }
        //        else if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number2]))
        //        {
        //            Population[number2] = NewPopulation[i];
        //        }
        //        //      Console.WriteLine( "новый этап /n");
        //        for (int j = 0; j < Population[groups[cluster_index_1][i]].TermsSet.Count; j++)
        //        {
        //            for (int k = 0; k < Population[groups[cluster_index_1][i]].TermsSet[j].Parametrs.Length; k++)
        //            {
        //                NewPopulation[i].TermsSet[j].Parametrs[k] += epsi * rand.NextDouble();


        //                //          Console.WriteLine(NewPopulation[i].TermsSet[j].Parametrs[k]);

        //            }
        //        }
        //        if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number1]))
        //        {
        //            Population[number1] = NewPopulation[i];
        //        }
        //        else if (result.ErrorLearnSamples(NewPopulation[i]) < result.ErrorLearnSamples(Population[number2]))
        //        {
        //            Population[number2] = NewPopulation[i];
        //        }
        //    }
        //}

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
            DBSConfig conf = new DBSConfig();
            conf.Init(CountFeatures);
            return conf;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Discret Brain Storm Algorithm" + "{" + Environment.NewLine;
                result = "Итераций = " + iter + ";" + Environment.NewLine;
                result = "Идей = " + N + ";" + Environment.NewLine;
                return result;
            }
            return "Discret Brain Storm Algorithm";
        }
    }
}
