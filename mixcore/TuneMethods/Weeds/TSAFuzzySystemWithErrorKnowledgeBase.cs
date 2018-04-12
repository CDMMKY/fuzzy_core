using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.TSAApproximate.Weeds
{
    class TSAFuzzySystemWithErrorKnowledgeBase : TSAFuzzySystem
    {
        public TSAFuzzySystemWithErrorKnowledgeBase(TSAFuzzySystem Source) : base(Source)
        {


            rulesdatabaseset = new List<KnowlegeBaseTSARules>();
            for (int i = 0; i < Source.RulesDatabaseSet.Count; i++)
            {
                rulesdatabaseset.Add(new KnowlegeBaseTSARulesWithError(Source.RulesDatabaseSet[i]));
            }
        }

        public TSAFuzzySystemWithErrorKnowledgeBase(SampleSet learnSet, SampleSet testSet) : base(learnSet, testSet)
        {
        }



        public void reinit ()
        {
           
            for (int i = 0; i  < RulesDatabaseSet.Count; i++)
            {
                KnowlegeBaseTSARulesWithError temp = new KnowlegeBaseTSARulesWithError(RulesDatabaseSet[i]) ;
                RulesDatabaseSet[i] = temp;
            }

           
        }
    }
}
