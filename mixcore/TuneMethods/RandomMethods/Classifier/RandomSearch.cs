using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzyCoreUtils;
using System.Linq;
using System.IO;
using System.Text;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class RandomSearch : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        Random rand;
        protected RandomSearchConf Config;
        protected int MaxIter, numberOfAllParts, numberOfFeatures;
        protected int iter = 0;
        protected List<bool[]> Population;
        protected bool[] HeadLeader;

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
            SetPopulation();
            Population[0].CopyTo(HeadLeader, 0);
            result.AcceptedFeatures = HeadLeader;
            double HLAcc = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
            iter = 0;
            while (iter < MaxIter)
            {
                ChangePositions();
                SortPopulation();
                result.AcceptedFeatures = Population[0];
                if (result.ClassifyLearnSamples(result.RulesDatabaseSet[0]) > HLAcc)
                {
                    HLAcc = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                    Population[0].CopyTo(HeadLeader, 0);
                }
                iter++;
            }
            int count_ones = 0;
            result.AcceptedFeatures = HeadLeader;
            for (int j = 0; j < HeadLeader.Length; j++)
            {
                if (HeadLeader[j])
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
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/RS" + folder_name + ".txt", "Признаки: " + count_ones + Environment.NewLine);
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/RS" + folder_name + ".txt", "Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2) + Environment.NewLine);
            File.AppendAllText("E:/TUSUR/GPO/Эксперименты/Behavior/RS" + folder_name + ".txt", "Время: " + Environment.NewLine);
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

        private void ChangePositions()
        {
            for (int i = 0; i < Population.Count; i++)
            {
                ChangePosition(Population[i]);
            }
        }

        private void ChangePosition(bool[] Particle)
        {
            for (int i = 0; i < Particle.Length; i++)
            {
                Particle[i] = BoolRand(0.5);
            }
        }

        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as RandomSearchConf;
            MaxIter = ((RandomSearchConf)Conf).TRSCCountIteration;
            numberOfAllParts = ((RandomSearchConf)Conf).TRSCCountparticles;
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
            RandomSearchConf conf = new RandomSearchConf();
            conf.Init(CountFeatures);
            return conf;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Random Search{";
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
