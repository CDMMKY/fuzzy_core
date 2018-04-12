/*using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;

namespace FuzzySystem.TakagiSugenoApproximate.add_generators
{
    class SimpleShrinkSignletone: AbstractNotSafeGenerator
    {
        string ShrinkFeatures;
        
        public override TSAFuzzySystem Generate(TSAFuzzySystem Classifier, IGeneratorConf config)
        {
            ShrinkFeatures= ((SimpleShrinkFeatureConf)config).ShrinkFeature;
            string[] shouldShrink = ShrinkFeatures.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < shouldShrink.Length; i++)
            { int a;
                int.TryParse(shouldShrink[i], out a);

                Classifier.AcceptedFeatures[a - 1] = false;
                    }
            return Classifier;

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
                return new List<FuzzySystemRelisedList.TypeSystem> { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }
    }
}
*/