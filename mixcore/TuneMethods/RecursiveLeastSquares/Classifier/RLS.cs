using FuzzySystem.PittsburghClassifier;
using FuzzySystem.FuzzyAbstract.conf;

namespace RecursiveLeastSquares.Classifier
{
    using System.Collections.Generic;

    using FuzzySystem.FuzzyAbstract;

    public class RLS: AbstractNotSafeLearnAlgorithm
    {
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Recursive Least Squares{";

                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                // result+= param1+Environment.NewLine;
                result += "}";
                return result;
            }

            return "Recursive Least Squares";
        }

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            PCFuzzySystem result = (PCFuzzySystem)Approximate;

            
            return result;
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            throw new System.NotImplementedException();
        }
    }
}
