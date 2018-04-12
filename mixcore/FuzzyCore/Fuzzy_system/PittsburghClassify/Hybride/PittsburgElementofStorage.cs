using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.PittsburghClassifier.Hybride
{
    public class PittsburgElementofStorage : ElementofStorage
    {

        public KnowlegeBasePCRules Element { get; protected set; }


        public PittsburgElementofStorage(PCFuzzySystem Checker, KnowlegeBasePCRules SourceElem, string algName) : base(algName)
        {
            Element = new KnowlegeBasePCRules(SourceElem);
            LearnError = Checker.ErrorLearnSamples(SourceElem);
            TestError = Checker.ErrorTestSamples(SourceElem);
        }

    }
}
