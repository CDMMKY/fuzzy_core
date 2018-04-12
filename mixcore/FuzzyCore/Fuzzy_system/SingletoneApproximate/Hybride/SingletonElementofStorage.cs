using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.SingletoneApproximate.Hybride
{
    public class SingletonElementofStorage:ElementofStorage
    {
        protected KnowlegeBaseSARules element;

        public KnowlegeBaseSARules Element { get {return element; } }
        public SingletonElementofStorage (SAFuzzySystem Checker, KnowlegeBaseSARules SourceElem, string algName):base(algName)
       {
           element =new KnowlegeBaseSARules(SourceElem);
          LearnError=  Checker.approxLearnSamples(SourceElem);
          TestError = Checker.approxTestSamples(SourceElem);
       }
    }
}
