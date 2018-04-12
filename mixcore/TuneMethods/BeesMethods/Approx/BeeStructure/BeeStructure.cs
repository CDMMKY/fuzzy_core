#define PClass
#if PClass
using KnowlegeBaseRules = FuzzySystem.PittsburghClassifier.KnowlegeBasePCRules;
using Rule = FuzzySystem.PittsburghClassifier.PCRule;
using FS = FuzzySystem.PittsburghClassifier.PCFuzzySystem;
#elif SApprox
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#elif TSApprox
using KnowlegeBaseRules = FuzzySystem.TakagiSugenoApproximate.KnowlegeBaseTSARules;
using FS = FuzzySystem.TakagiSugenoApproximate.TSAFuzzySystem;
#else
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#endif

using BeesMethods.Base.Common;


namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    public class Bee:AbstractBeeStructure
    {
       protected KnowlegeBaseRules thePositionOfBee;
       public KnowlegeBaseRules PositionOfBee { get { return thePositionOfBee; } set { thePositionOfBee = value; } }
       protected FS Parrent;
        protected double goods ;


        override public  double Goods { get {return goods; } }
      
        public Bee(KnowlegeBaseRules theSource, FS parrent)
        {
            thePositionOfBee = new KnowlegeBaseRules(theSource);
            
            Parrent = parrent;
        }

        public double getGoodsImproove(double baseValue)
        {
            goods = baseValue - Parrent.ErrorLearnSamples(thePositionOfBee);
          return goods  ;
        }

 

     


    }
}
