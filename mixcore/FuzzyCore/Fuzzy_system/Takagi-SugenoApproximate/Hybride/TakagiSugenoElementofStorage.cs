using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.TakagiSugenoApproximate.Hybride
{
     public class TakagiSugenoElementofStorage:ElementofStorage
     {
         protected KnowlegeBaseTSARules element;

         public KnowlegeBaseTSARules Element { get {return element; } }

         public TakagiSugenoElementofStorage(TSAFuzzySystem Checker, KnowlegeBaseTSARules SourceElem, string algName):base(algName)
        {
            element =new KnowlegeBaseTSARules(SourceElem);
           LearnError=  Checker.approxLearnSamples(SourceElem);
           TestError = Checker.approxTestSamples(SourceElem);
        }

     }
}
