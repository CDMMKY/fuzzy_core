using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzyCoreUtils;
using System.Linq;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{ 
    class MixBagging : AbstractNotSafeLearnAlgorithm
    {
        protected PCFuzzySystem result;
        protected MBConf Config;
        protected int voteType, populInFirstAlg, populInSecondAlg, populInThirdAlg;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            Init(conf);
            switch (voteType)
            {
                case 0:
                    { 
                        int i = 0;
                        List<KnowlegeBasePCRules> populsOfOneAlgorithm = new List<KnowlegeBasePCRules>();
                        for (int p_i = 0; p_i < populInFirstAlg; p_i++)
                        {
                            populsOfOneAlgorithm.Add(result.RulesDatabaseSet[i]);
                            i++;
                        }
                        Console.WriteLine("Bagging First: ");
                        Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(populsOfOneAlgorithm), 2));
                        Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(populsOfOneAlgorithm), 2));
                        populsOfOneAlgorithm.Clear();
                        if (populInSecondAlg > 0)
                        {
                            for (int p_i = 0; p_i < populInSecondAlg; p_i++)
                            {
                                populsOfOneAlgorithm.Add(result.RulesDatabaseSet[i]);
                                i++;
                            }
                            Console.WriteLine("Bagging Second: ");
                            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(populsOfOneAlgorithm), 2));
                            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(populsOfOneAlgorithm), 2));
                            populsOfOneAlgorithm.Clear();
                            if (populInThirdAlg > 0)
                            {
                                for (int p_i = 0; p_i < populInThirdAlg; p_i++)
                                {
                                    populsOfOneAlgorithm.Add(result.RulesDatabaseSet[i]);
                                    i++;
                                }
                                Console.WriteLine("Bagging Third: ");
                                Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(populsOfOneAlgorithm), 2));
                                Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(populsOfOneAlgorithm), 2));
                                populsOfOneAlgorithm.Clear();
                            }
                            Console.WriteLine("Mixed Bagging: ");
                            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(result.RulesDatabaseSet), 2));
                            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(result.RulesDatabaseSet), 2));
                        }
                        break;
                    }
            }
            return result;
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
            MBConf conf = new MBConf();
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
            return "Mixed Bagging";
        }
        public virtual void Init(ILearnAlgorithmConf Conf)
        {
            Config = Conf as MBConf;
            voteType = (int)Config.Тип_голосования;
            populInFirstAlg = Config.Популяций_от_первого_алгоритма;
            populInSecondAlg = Config.Популяций_от_второго_алгоритма;
            populInThirdAlg = Config.Популяций_от_третьего_алгоритма;
        }
    }
}