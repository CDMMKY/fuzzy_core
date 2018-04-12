/*using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.SingletoneApproximate;

namespace FuzzySystem.SingletoneApproximate.add_generators
{
    class SimpleChooseSinpletone : AbstractNotSafeGenerator
    {
        string ChooseFeature;
        
        public override SAFuzzySystem Generate(SAFuzzySystem Classifier, IGeneratorConf config)
        {
            ChooseFeature= ((SimpleChooseFeatureConf)config).ChooseFeatures;
            string[] shouldShrink = ChooseFeature.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < Classifier.CountVars; i++)
            {
                Classifier.AcceptedFeatures[i] = false;
            }
            for (int i = 0; i < shouldShrink.Length; i++)
            { int a;
                int.TryParse(shouldShrink[i], out a);

                Classifier.AcceptedFeatures[a - 1] = true;
                    }
            return Classifier;

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
                return new List<FuzzySystemRelisedList.TypeSystem> { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }
    }
}
*/