using System;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class ParallelHiveParams:HiveParams
    {
        ParallelSelectorOnBee Selector = new ParallelSelectorOnBee();

        public ParallelHiveParams(BeeParamsAlgorithm Parrent, KnowlegeBaseSARules Best)
            : base(Parrent,Best)
      {
          
      }


        public override void flyScouts(int Scouts, Random rand, KnowlegeBaseSARules Base)
        {
            theScouts = ParralelScoutParams.FlyScout(Scouts, rand, Base, theParrent);
            
        }



        public override void flyWorkers(BeeParams best, Random rand)
        {
            theWorkers = ParralelWorkerParams.WorkersFly(best, hostArchive, rand, theParrent);
           

        }


        public override void flyOutLookers(BeeParams best, Random rand)
        {
            theOutlookers = ParallelOultLookersBeeParams.OutLookerFly(best, hostArchive, rand, theParrent);
           

        }

        public override int makeNewPopulation(BeeParamsConf.Type_Selection selectType, int hiveSize, double percentScout, Random rand, double Border1, double Border2, double Border3, int Repeat1, int Repeat2, int Repeat3)
        {

            hostArchive.AddRange(theWorkers);

            hostArchive.AddRange(theOutlookers);
            int countafterselect = (int)(hiveSize * (1 - percentScout / 100));
            hostArchive.RemoveAll(x => double.IsInfinity(x.Goods));


            switch (selectType)
            {
                case BeeParamsConf.Type_Selection.Бинарный_турнир: { hostArchive = Selector.selectBinaryTornament(hostArchive, countafterselect, rand); } break;
                case BeeParamsConf.Type_Selection.Рулетка: { hostArchive = Selector.selectRoulet(hostArchive, countafterselect, rand); } break;
                case BeeParamsConf.Type_Selection.Случайный_отбор: { hostArchive = Selector.selectRandom(hostArchive, countafterselect, rand); } break;
                case BeeParamsConf.Type_Selection.Элитный_отбор: { hostArchive = Selector.selectElite(hostArchive, countafterselect); } break;
            }



            hostArchive = Selector.Regeration(hostArchive, Border1, Repeat1, Border2, Repeat2, Border3, Repeat3, countafterselect);


            theWorkers.Clear();
            theOutlookers.Clear();
            theParrent.CleanTempory();
            return hiveSize - hostArchive.Count;
        }





    }
}
