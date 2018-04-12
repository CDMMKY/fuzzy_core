using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using System.Linq;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.SingletoneApproximate;

namespace FuzzySystem.PittsburghClassifier.add_generators
{
    class SimpleShrinkClassifier: AbstractNotSafeGenerator
    {
        string ShrinkFeatures;

        public IFuzzySystem Universal(IFuzzySystem FS, IGeneratorConf config)

        {
            ShrinkFeatures = ((SimpleShrinkFeatureConf)config).ShrinkFeature;
            string[] shouldShrink = ShrinkFeatures.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < shouldShrink.Length; i++)
            {
                int a;
                int.TryParse(shouldShrink[i], out a);

                FS.AcceptedFeatures[a - 1] = false;
            }
            return FS;

        }
        public override PCFuzzySystem Generate(PCFuzzySystem Classifier, IGeneratorConf config)
        {
            Classifier = Universal(Classifier, config) as PCFuzzySystem;
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
            IGeneratorConf conf = new SimpleShrinkFeatureConf();
            conf.Init(CountFeatures);
            return conf;
        }
        public override string ToString(bool withParam = false)
        {
            if (withParam)
            {
                string result = "Задать усеченные признаки {";
                result += "Усечены признаки= [" + ShrinkFeatures + "] ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Задать усеченные признаки";
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem> { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier, FuzzySystemRelisedList.TypeSystem.Singletone, FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }
    }
}
