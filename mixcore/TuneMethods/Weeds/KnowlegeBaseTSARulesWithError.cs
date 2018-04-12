using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzySystem.TSAApproximate.Weeds
{
    class KnowlegeBaseTSARulesWithError: KnowlegeBaseTSARules
    {
        public KnowlegeBaseTSARulesWithError()
        {
        }


       

        public KnowlegeBaseTSARulesWithError(KnowlegeBaseTSARules source,  List<bool> used_rules = null) : base(source, used_rules)
        {
            if (source is KnowlegeBaseTSARulesWithError)
            error = ((KnowlegeBaseTSARulesWithError)source).error;
        }

        public double error { get; set; }
    }
}
