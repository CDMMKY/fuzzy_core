using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ParralelWorkerParams : WorkerParams
    {

        public ParralelWorkerParams(KnowlegeBaseSARules theSource, KnowlegeBaseSARules Best, BeeParamsAlgorithm parrent, Random rand)
            : base(theSource,Best, parrent, rand)
        { }

        public new  static List<BeeParams> WorkersFly(BeeParams Best, List<BeeParams> Source, Random rand, BeeParamsAlgorithm parrent)
        {

            List<BeeParams> Result = new List<BeeParams>();
            Parallel.For(0, Source.Count, (int i) =>
               {
                   Result.Add(new WorkerParams(Source[i].PositionOfBee, Best.PositionOfBee, parrent, rand));
               });

            return Result;
        }
     
    }
}
