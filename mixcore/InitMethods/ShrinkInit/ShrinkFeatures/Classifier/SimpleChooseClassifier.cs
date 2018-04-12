using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using System.Linq;
using System.Linq.Expressions;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;

namespace FuzzySystem.PittsburghClassifier.add_generators
{
    class SimpleChooseClassifier : AbstractNotSafeGenerator
    {
        string ChooseFeature;

        public IFuzzySystem Universal(IFuzzySystem FS, IGeneratorConf config)
        {
            ChooseFeature = ((SimpleChooseFeatureConf)config).ChooseFeatures;
            string[] shouldShrink = ChooseFeature.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < FS.CountFeatures; i++)
            {
                FS.AcceptedFeatures[i] = false;
            }
            for (int i = 0; i < shouldShrink.Length; i++)
            {
                int a;
                int.TryParse(shouldShrink[i], out a);

                FS.AcceptedFeatures[a - 1] = true;
            }

            
            return FS;

        }



        public override PCFuzzySystem Generate(PCFuzzySystem Classifier, IGeneratorConf config)
        {
            Classifier = Universal(Classifier, config) as  PCFuzzySystem;
            Classifier.RulesDatabaseSet.AsParallel().ForAll(x => x.ShrinkNotAcceptedFeatures(Classifier.AcceptedFeatures));
            return Classifier;
        }
        public override SAFuzzySystem Generate(SAFuzzySystem Approximate, IGeneratorConf config)
        {
            Approximate = Universal(Approximate, config) as SAFuzzySystem;
            Approximate.RulesDatabaseSet.AsParallel().ForAll(x => x.ShrinkNotAcceptedFeatures(Approximate.AcceptedFeatures));
            return Approximate;
        }
        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            Approximate = Universal(Approximate, config) as TSAFuzzySystem;
            Approximate.RulesDatabaseSet.AsParallel().ForAll(x => x.ShrinkNotAcceptedFeatures(Approximate.AcceptedFeatures));
            return Approximate;
        }
        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf conf = new SimpleChooseFeatureConf();
            conf.Init(CountFeatures);
            return conf;
        }
        public override string ToString(bool withParam = false)
        {
            if (withParam)
            {
                string result = "Задать выбранные признаки {";
                result += "Выбраны признаки= [" + ChooseFeature + "] ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Задать выбранные признаки";
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem> { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier, FuzzySystemRelisedList.TypeSystem.Singletone,  FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }
    }
}
