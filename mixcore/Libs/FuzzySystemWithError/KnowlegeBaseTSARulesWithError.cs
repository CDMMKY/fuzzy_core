using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzySystem.TSAApproximate.Weeds
{
  public  class KnowlegeBaseTSARulesWithError: KnowlegeBaseTSARules
    {
        public KnowlegeBaseTSARulesWithError()
        {
        }


       

        public KnowlegeBaseTSARulesWithError(KnowlegeBaseTSARules source,  List<bool> used_rules = null) : base(source, used_rules)
        {
            if (source is KnowlegeBaseTSARulesWithError) { 
            error = ((KnowlegeBaseTSARulesWithError)source).error;
                if (((KnowlegeBaseTSARulesWithError)source).RulesAcceptedFeatures!=null)
                RulesAcceptedFeatures = ((KnowlegeBaseTSARulesWithError)source).RulesAcceptedFeatures.ToArray().ToList();
            }
        }

        public double error { get; set; }
        public List<bool> RulesAcceptedFeatures { get; set; }

    }
}
