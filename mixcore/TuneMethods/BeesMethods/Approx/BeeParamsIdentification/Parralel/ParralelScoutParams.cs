using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ParralelScoutParams : ScoutParams
    {
        public ParralelScoutParams(KnowlegeBaseSARules theSource, BeeParamsAlgorithm parrent, Random rand)
            : base(theSource, parrent, rand)
        { 
        }
        public new static List <BeeParams> FlyScout(int countofBee, Random rand, KnowlegeBaseSARules Source,BeeParamsAlgorithm parrent)
        {
            List <BeeParams> result = new List<BeeParams>();
            Parallel.For(0, countofBee, (int i) =>
              {
                  result.Add(new ScoutParams(Source, parrent, rand));
              });

                return result;
               
        }

    }
}
