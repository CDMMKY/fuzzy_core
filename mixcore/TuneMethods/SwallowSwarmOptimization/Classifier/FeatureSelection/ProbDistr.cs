using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;
using System.Linq;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class ProbDistr : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand = new Random();
        protected ProbDistrConf Config;
        protected int MaxIter, numberOfFeatures, numberOfAllParts;
        protected int iter = 0, countOnes = 0, countZeros = 0, l = 0;
        protected double u, MathExpect, Deviation;
        protected List<int[]> Population;
        protected int[] HeadLeader;
        protected List<int[]> BestParticles;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classif, ILearnAlgorithmConf conf)
        {
            result = Classif;
            numberOfFeatures = result.CountFeatures;
            Init(conf);

            HeadLeader = new int[numberOfFeatures];
            BestParticles = new List<int[]>();
            for (int i = 0; i < numberOfFeatures; i++)
            {
                BestParticles.Add(new int[2]);
                BestParticles[i][0] = 0;
                BestParticles[i][1] = 0;
            }

            SetPopulation();

            iter = 0;
            while (iter < MaxIter)
            {
                SortPopulation();

                for (int i = 0; i < Population[0].Length; i++)
                {
                    BestParticles[i][Population[0][i]] += 1;
                }

                HeadLeader = Population[0];

                ChangeParticles();

                iter++;
            }

            SortPopulation();

            for (int j = 0; j < Population[0].Length; j++)
            {
                Console.Write(Convert.ToString(Population[0][j]) + ' ');
                if (Population[0][j] == 0)
                {
                    result.AcceptedFeatures[j] = false;
                }
                else
                {
                    result.AcceptedFeatures[j] = true;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(result.RulesDatabaseSet[0]), 2));
            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2));
            return result;
        }

        private void SetPopulation()
        {
            Population = new List<int[]>();
            for (int i = 0; i < numberOfAllParts; i++)
            {
                Population.Add(new int[numberOfFeatures]);
                for (int j = 0; j < Population[i].Length; j++)
                {
                    Population[i][j] = rand.Next(0, 2);
                }
            }
        }

        private void ChangeParticles()
        {
            foreach (var Particle in Population)
            {
                countOnes = 0;
                countZeros = 0;
                for (int i = 0; i < Particle.Length; i++)
                {
                    if (Particle[i] == 0)
                        countZeros += 1;
                    else
                        countOnes += 1;
                }
                u = GaussRandom.Random_gaussian(rand, MathExpect, Deviation);
                if (u > 0)
                {
                    l = Convert.ToInt32(Math.Round(countZeros * hypTang(u)));
                    List<int> Zeroes = new List<int>();
                    for (int i = 0; i < Particle.Length; i++)
                    {
                        if (Particle[i] == 0)
                            Zeroes.Add(i);
                    }
                    for(int i = l; i > 0; i--)
                    {
                        int randindex = rand.Next(0, Zeroes.Count);
                        int index = Zeroes[randindex];
                        Zeroes.RemoveAt(randindex);
                        if(iter < 200)
                        {
                            if (rand.NextDouble() > 0.5)
                                Particle[index] = 1;
                        }
                        else
                        {
                            if (rand.NextDouble() > (BestParticles[index][0] / (BestParticles[index][0] + BestParticles[index][1])))
                                Particle[index] = 1;
                        }
                    }
                }
                else
                {
                    l = Math.Abs(Convert.ToInt32(Math.Round(countOnes * hypTang(u))));
                    List<int> Ones = new List<int>();
                    for (int i = 0; i < Particle.Length; i++)
                    {
                        if (Particle[i] == 1)
                            Ones.Add(i);
                    }
                    for (int i = l; i > 0; i--)
                    {
                        int randindex = rand.Next(0, Ones.Count);
                        int index = Ones[randindex];
                        Ones.RemoveAt(randindex);
                        if (iter < 200)
                        {
                            if (rand.NextDouble() > 0.5)
                                Particle[index] = 0;
                        }
                        else
                        {
                            if (rand.NextDouble() > (BestParticles[index][1] / (BestParticles[index][0] + BestParticles[index][1])))
                                Particle[index] = 0;
                        }
                    }
                }
            }
        }

        private double hypTang(double u)
        {
            double tang;
            tang = (Math.Exp(u) - Math.Exp(-u)) / (Math.Exp(u) + Math.Exp(-u));
            return tang;
        }

        private void SortPopulation()
        {
            Dictionary<int[], double> PopulationWithAccuracy = new Dictionary<int[], double>();
            double accuracy = 0;
            for (int i = 0; i < Population.Count; i++)
            {
                for (int j = 0; j < Population[i].Length; j++)
                {
                    if (Population[i][j] == 0)
                    {
                        result.AcceptedFeatures[j] = false;
                    }
                    else
                    {
                        result.AcceptedFeatures[j] = true;
                    }
                }
                accuracy = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                PopulationWithAccuracy.Add(Population[i], accuracy);
            }
            Population.Clear();
            foreach (var pair in PopulationWithAccuracy.OrderByDescending(pair => pair.Value))
            {
                Population.Add(pair.Key);
            }
            PopulationWithAccuracy.Clear();
        }

      
        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as ProbDistrConf;
            MaxIter = ((ProbDistrConf)Conf).Количество_итераций;
            numberOfAllParts = ((ProbDistrConf)Conf).Количество_всех_частиц;
            MathExpect = ((ProbDistrConf)Conf).Мат_ожидание;
            Deviation = ((ProbDistrConf)Conf).Отклонение;
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
            ProbDistrConf conf = new ProbDistrConf();
            conf.Init(CountFeatures);
            return conf;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Swallow Swarm Optimization{";
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                result += "}";
                return result;
            }
            return "Метод Вероятностных Распределений";
        }
    }
}
