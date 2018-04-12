#define TSApprox
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
using Rule = FuzzySystem.TakagiSugenoApproximate.TSARule;
#else
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#endif

using System;
using System.Linq;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    public class Worker : Bee
    {


        public Worker(KnowlegeBaseRules theSource, FS parrent)
             : base(theSource, parrent)
        {
        }

        public void WorkerFly(int numOfRule, Random rand)
        {
            Rule optimised = thePositionOfBee.RulesDatabase[numOfRule];
            double coust = 0;



            for (int i = 0; i < optimised.ListTermsInRule.Count; i++)
            {
                int numOfFreature = optimised.ListTermsInRule[i].NumVar;
                for (int j = 0; j < optimised.ListTermsInRule[i].Parametrs.Count(); j++)
                {
                    coust = rand.Next(100) / 500.0;
                    if (rand.Next(2) > 0)
                    {
                        optimised.ListTermsInRule[i].Parametrs[j] *= (1 - coust);
                    }
                    else {
                        optimised.ListTermsInRule[i].Parametrs[j] *= (1 + coust);

                    }
                }
            }
        }
    }
}
