using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;
using System.Linq;
using System.IO;
using System.Text;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class RandomSearch : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand;
        protected ConfigSSO Config;
        protected bool shrink_features;
        protected int MaxIter, numberOfLocalLeaders, numberOfAimlessParts, numberOfAllParts, numberOfParametrs, numberOfFeatures;
        protected int iter = 0;
        protected double ALocal, BLocal, AGlobal, BGlobal, unlaidtest;
        protected List<bool[]> Population;
        protected bool[] HeadLeader, Universal;
        protected List<bool[]> LocalLeaders, ExplorerParticles, AimlessParticles;
        protected bool[] VelocityVector, VelocityVectorLL, VelocityVectorHL;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            string folder_name = "";
            foreach (var letter in result.LearnSamplesSet.FileName)
            {
                if (letter != '-')
                    folder_name += letter;
                else
                    break;
            }
            numberOfFeatures = result.CountFeatures;
            Init(conf);
            rand = new Random();
            HeadLeader = new bool[numberOfFeatures];
            VelocityVector = new bool[numberOfFeatures];
            VelocityVectorLL = new bool[numberOfFeatures];
            VelocityVectorHL = new bool[numberOfFeatures];

            SetPopulation();

            LocalLeaders = new List<bool[]>();
            ExplorerParticles = new List<bool[]>();
            AimlessParticles = new List<bool[]>();

            iter = 0;
            while (iter < MaxIter)
            {
                SortPopulation();

                SetRoles();

                ChangeExplorersPositions();
                ChangeAimlessPositions();

                DiscardRoles();

                iter++;
            }

            SortPopulation();

            int count_ones = 0;
            result.AcceptedFeatures = Population[0];
            for (int j = 0; j < Population[0].Length; j++)
            {
                if (Population[0][j])
                {
                    Console.Write("1 ");
                    count_ones++;
                }
                else
                    Console.Write("0 ");
            }
            Console.WriteLine();
            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(result.RulesDatabaseSet[0]), 2));
            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2));
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/SSODiscret" + folder_name + ".txt", "Признаки: " + count_ones + Environment.NewLine);
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/SSODiscret" + folder_name + ".txt", "Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2) + Environment.NewLine);
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/SSODiscret" + folder_name + ".txt", "Время: " + Environment.NewLine);
            return result;
        }

        private void SetPopulation()
        {
            Population = new List<bool[]>();
            for (int i = 0; i < numberOfAllParts; i++)
            {
                Population.Add(new bool[numberOfFeatures]);
                for (int j = 0; j < Population[i].Length; j++)
                {
                    Population[i][j] = BoolRand(0.5);
                }
            }
        }

        private void SortPopulation()
        {
            Dictionary<bool[], double> PopulationWithAccuracy = new Dictionary<bool[], double>();
            double accuracy = 0;
            for (int i = 0; i < Population.Count; i++)
            {
                result.AcceptedFeatures = Population[i];
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

        private bool BoolRand(double border)
        {
            if (rand.NextDouble() < border)
                return false;
            else
                return true;
        }

        private bool BoolRand(bool first, bool second, double border)
        {
            if (rand.NextDouble() < border)
                return first;
            else
                return second;
        }

        private bool[] Merge(bool[] first, bool[] second, double border)
        {
            bool[] tmp = new bool[first.Length];
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i])
                    tmp[i] = BoolRand(first[i], second[i], border);
                else
                    tmp[i] = first[i];
            }
            return tmp;
        }

        private void SetRoles()
        {
            HeadLeader = Population[0];
            for (int i = 1; i <= numberOfLocalLeaders; i++)
            {
                LocalLeaders.Add(Population[i]);
            }
            for (int i = numberOfAllParts - numberOfAimlessParts; i < numberOfAllParts; i++)
            {
                AimlessParticles.Add(Population[i]);
            }
            for (int i = numberOfLocalLeaders + 1; i < numberOfAllParts - numberOfAimlessParts; i++)
            {
                ExplorerParticles.Add(Population[i]);
            }
        }

        private void ChangeExplorersPositions()
        {
            int index;
            for (int i = 0; i < ExplorerParticles.Count; i++)
            {
                index = findNearestLocalLeader(ExplorerParticles[i]);
                calculateVHL(ExplorerParticles[i]);
                calculateVLL(ExplorerParticles[i], index);
                calculateV();
                ExplorerParticles[i] = Merge(VelocityVector, ExplorerParticles[i], 0.7);
            }
        }

        private int findNearestLocalLeader(bool[] Explorer)
        {
            int index = 0;
            int minimum = 100000;
            int hammingDistance = 0;
            for (int i = 0; i < numberOfLocalLeaders; i++)
            {
                for (int j = 0; j < Explorer.Length; j++)
                {
                    if (Explorer[j] != LocalLeaders[i][j])
                    {
                        hammingDistance += 1;
                    }
                }
                if (hammingDistance < minimum)
                {
                    minimum = hammingDistance;
                    index = i;
                }
            }
            return index;
        }

        private void calculateVHL(bool[] Explorer)
        {
            bool[] part1 = new bool[numberOfFeatures];
            bool[] part2 = new bool[numberOfFeatures];

            for (int i = 0; i < part1.Length; i++)
            {
                part1[i] = BoolRand(0.5);
            }

            part2 = Merge(HeadLeader, Explorer, 0.9);

            VelocityVectorHL = Merge(part2, part1, 0.9);
        }

        private void calculateVLL(bool[] Explorer, int index)
        {
            bool[] part1 = new bool[numberOfFeatures];
            bool[] part2 = new bool[numberOfFeatures];

            for (int i = 0; i < part1.Length; i++)
            {
                part1[i] = BoolRand(0.5);
            }

            part2 = Merge(LocalLeaders[index], Explorer, 0.8);

            VelocityVectorLL = Merge(part2, part1, 0.8);
        }

        private void calculateV()
        {
            VelocityVector = Merge(VelocityVectorHL, VelocityVectorLL, 0.7);
        }

        private void ChangeAimlessPositions()
        {
            for (int i = 0; i < AimlessParticles.Count; i++)
            {
                ChangeAimlessPosition(AimlessParticles[i]);
            }
        }

        private void ChangeAimlessPosition(bool[] Aimless)
        {
            for (int i = 0; i < Aimless.Length; i++)
            {
                Aimless[i] = BoolRand(0.5);
            }
        }

        private void DiscardRoles()
        {
            int k = 1;
            Population[0] = HeadLeader;
            for (int i = 0; i < LocalLeaders.Count; i++)
            {
                Population[k] = LocalLeaders[i];
                k++;
            }
            LocalLeaders.Clear();
            for (int i = 0; i < ExplorerParticles.Count; i++)
            {
                Population[k] = ExplorerParticles[i];
                k++;
            }
            ExplorerParticles.Clear();
            for (int i = 0; i < AimlessParticles.Count; i++)
            {
                Population[k] = AimlessParticles[i];
                k++;
            }
            AimlessParticles.Clear();
        }
        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as ConfigSSO;
            MaxIter = ((ConfigSSO)Conf).Количество_итераций;
            numberOfLocalLeaders = ((ConfigSSO)Conf).Количество_лок_лидеров;
            numberOfAimlessParts = ((ConfigSSO)Conf).Количество_бесц_част;
            numberOfAllParts = ((ConfigSSO)Conf).Количество_всех_частиц;
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
            ConfigSSO conf = new ConfigSSO();
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
            return "Random Search";
        }
    }
}
