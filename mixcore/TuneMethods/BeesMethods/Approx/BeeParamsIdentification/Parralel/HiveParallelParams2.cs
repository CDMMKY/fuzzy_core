using System;
using System.Threading;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class HiveParallelParams2 : ParallelHiveParams
    {

        ParallelSelectorOnBee Selector = new ParallelSelectorOnBee();


        public HiveParallelParams2(BeeParamsAlgorithm Parrent, KnowlegeBaseSARules Best)
            : base(Parrent,Best)
      {
          
      }

        public override void flyWorkers(BeeParams best, Random rand)
        {
            Thread[] threads = new Thread[2];

            threads[0] = new Thread(() => { theWorkers = ParralelWorkerParams.WorkersFly(best, hostArchive, rand, theParrent); });
            threads[1] = new Thread(() => { theOutlookers = ParallelOultLookersBeeParams.OutLookerFly(best, hostArchive, rand, theParrent); });

            try
            {
                threads[0].Start();
                threads[1].Start();

            }
            catch (Exception)
            {
            }
            while (threads[0].IsAlive || threads[1].IsAlive)
            {
         //       Thread.Sleep(10);
            }

        }


        public override void flyOutLookers(BeeParams best, Random rand)
        {
        

        }





    }
}
