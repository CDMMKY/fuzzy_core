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
    public class ChooseMinus : AbstractNotSafeLearnAlgorithm
    {
        List<bool[]> test;
        List<double> Errors;
        List<FeatureSelectionModel> Storage;
        bool[] BestSolute;
        SortType SortWay;
        bool isClass = false;


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
            GreedyChoiceConfigMinus Config = conf as GreedyChoiceConfigMinus;
            SortWay = Config.GCCSortWay;
            Storage = new List<FeatureSelectionModel>(FSystem.CountFeatures);
            test = new List<bool[]>();
            Errors = new List<double>();
            BestSolute = new bool[FSystem.AcceptedFeatures.Count()];
            for (int i = 0; i < FSystem.CountFeatures; i++)
            {
                BestSolute[i] = true;
            }
        }

        public IFuzzySystem UniversalMethod(IFuzzySystem FSystem, ILearnAlgorithmConf conf)
        {
            init(FSystem, conf);
            Storage.Add(new FeatureSelectionModel(FSystem, BestSolute));
            for (int m = FSystem.CountFeatures-1; m > 0; m--)
            {
                for (int i = 0; i < FSystem.CountFeatures; i++)
                {
                    if (BestSolute[i] == false) { continue; }
                    test.Add(BestSolute.Clone() as bool[]);
                    test[test.Count - 1][i] = false;
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

            Storage= FeatureSelectionModel.Distinct(Storage);
            FeatureSelectionModel.Sort(Storage,SortWay);
            return FSystem;
        }
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier, FuzzySystemRelisedList.TypeSystem.Singletone, FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }


       




        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Жадный алгоритм -";
                result += " ; " + Environment.NewLine;

                result += FeatureSelectionModel.getFullInfo(Storage,isClass);

                result += "}";
                return result;
            }
            return "Жадный алгоритм -";
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new GreedyChoiceConfigMinus();
            result.Init(CountFeatures);
            return result;
        }

    }
}
