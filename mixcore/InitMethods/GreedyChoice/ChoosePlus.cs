using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier;
using Linglib;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.FuzzyAbstract.Utils;

namespace GreedyChoice
{
    public class ChoosePlus : AbstractNotSafeLearnAlgorithm
    {
        List<bool[]> test;
        List<double> Errors;
        List<FeatureSelectionModel> Storage;
        bool[] BestSolute;
        int max_Features;
        bool isClass = false;
        SortType SortWay;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            isClass = true;
            return UniversalMethod(FSystem, conf) as PCFuzzySystem;
        }
        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            return UniversalMethod(FSystem, conf) as SAFuzzySystem;
        }
        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            return UniversalMethod(FSystem, conf) as TSAFuzzySystem;
        }

        public void init(IFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            GreedyChoiceConfigPlus Config = conf as GreedyChoiceConfigPlus;
            max_Features = Config.GCCMaxVars>FSystem.AcceptedFeatures.Length? FSystem.AcceptedFeatures.Length: Config.GCCMaxVars;
            SortWay = Config.GCCSortWay;
            test = new List<bool[]>();
            Errors = new List<double>();
           Storage = new List<FeatureSelectionModel>();
            BestSolute = new bool[FSystem.AcceptedFeatures.Count()];
            for (int i = 0; i < FSystem.CountFeatures; i++)
            {
                BestSolute[i] = false;
            }
        }

        public IFuzzySystem UniversalMethod(IFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            init(FSystem, conf);
            for (int m = 0; m < max_Features; m++)
            {
                for (int i = 0; i < FSystem.CountFeatures; i++)
                {
                    if (BestSolute[i] == true) { continue; }
                    test.Add(BestSolute.Clone() as bool[]);
                    test[test.Count - 1][i] = true;
                    FSystem.AcceptedFeatures = test[test.Count - 1];
                    Errors.Add(FSystem.ErrorLearnSamples(FSystem.AbstractRulesBase()[0]));
                }
                int best = Errors.IndexOf(Errors.Min());
                BestSolute = test[best].Clone() as bool[];
                FSystem.AcceptedFeatures = BestSolute;
                Storage.Add(new FeatureSelectionModel(FSystem, BestSolute));
                test.Clear();
                Errors.Clear();
            }
            Storage = FeatureSelectionModel.Distinct(Storage);
            FeatureSelectionModel.Sort(Storage, SortWay);
            return FSystem;
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier, FuzzySystemRelisedList.TypeSystem.Singletone, FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }


        string makeNameFeatures(IFuzzySystem FSystem, bool[] Source)
        {
            string temp = String.Empty;
            string temp2 = String.Empty;
            for (int i = 0; i < Source.Count(); i++)
            {
                if (Source[i])
                {
                    temp += FSystem.LearnSamplesSet.InputAttributes[i].Name + ", ";
                    temp2 += $"{i + 1}, ";
                }
            }
            return temp + Environment.NewLine + temp2;
        }




        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Жадный алгоритм +";
                result += " ; " + Environment.NewLine;
                result += FeatureSelectionModel.getFullInfo(Storage, isClass);

                result += "}";
                return result;
            }
            return "Жадный алгоритм +";
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new GreedyChoiceConfigPlus();
            result.Init(CountFeatures);
            return result;
        }

    }
}
