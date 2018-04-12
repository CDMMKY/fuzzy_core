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
        protected int voteType;
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classify, ILearnAlgorithmConf conf)
        {
            result = Classify;
            Init(conf);
            Console.WriteLine("Mixed Bagging: ");
            Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamplesBagging(result.RulesDatabaseSet), 2));
            Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamplesBagging(result.RulesDatabaseSet), 2));
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
            if(Config.Тип_голосования == MBConf.voteType.Обычное)
            {
                voteType = 0;
            }
        }
    }
}