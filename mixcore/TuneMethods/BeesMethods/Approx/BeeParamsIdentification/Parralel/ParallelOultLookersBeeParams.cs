using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ParallelOultLookersBeeParams : OutLookersBeeParams
    {

        public ParallelOultLookersBeeParams(KnowlegeBaseSARules theSource, KnowlegeBaseSARules Best, BeeParamsAlgorithm parrent, Random rand)
            : base(theSource, Best, parrent, rand)
        { 
        }


        public new static List<BeeParams> OutLookerFly(BeeParams Best, List<BeeParams> Source, Random rand, BeeParamsAlgorithm parrent)
        {

            List<BeeParams> Result = new List<BeeParams>();
            Parallel.For(0, Source.Count, (int i) =>
                {
                    Result.Add(new OutLookersBeeParams(Source[i].PositionOfBee, Best.PositionOfBee, parrent, rand));
                });

            return Result;
        }


    }
}
