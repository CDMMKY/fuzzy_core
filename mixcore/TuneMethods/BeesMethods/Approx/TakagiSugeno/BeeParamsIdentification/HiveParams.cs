using System;
using System.Collections.Generic;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    public class HiveParams
    {
      public List<BeeParams> theScouts;
      public List<BeeParams> theWorkers;
      public List<BeeParams> theOutlookers;
      protected BeeParamsAlgorithm theParrent;
      public List<BeeParams> HostArchive { get { return hostArchive; } }
        protected List<BeeParams> hostArchive = new List<BeeParams>();
        protected BeeParams.BeeParamsComparer sorterBee = new BeeParams.BeeParamsComparer();
        
        private SelectionOnBee Selector = new SelectionOnBee();
 
      public HiveParams(BeeParamsAlgorithm Parrent, KnowlegeBaseTSARules Best)
      {
          theParrent = Parrent;
          hostArchive = new List<BeeParams>();
           hostArchive.Add(new BeeParams( Best,Parrent));
           hostArchive[0].getGoodsImproove();

      }

        public virtual void flyScouts(int Scouts, Random rand,KnowlegeBaseTSARules Base)
      {
          theScouts = ScoutParams.FlyScout(Scouts,rand,Base,theParrent);
        }

        public virtual double LimitScoutBeeSolution(double currentTemperature, double coefCold,Random rand)
        {
            theScouts.Sort(sorterBee);
   
            double best_error = theScouts[0].Goods;
               for (int i = theScouts.Count-1; i >=0; i--)
               {
                   if (best_error != theScouts[i].Goods)
                   {
                       if ((Math.Exp(-1 * Math.Abs(best_error - theScouts[i].Goods)) / currentTemperature) > rand.NextDouble())
                       {
                           theScouts.RemoveAt(i);
                       }
                   }
               }

            hostArchive.AddRange(theScouts);
            double result = currentTemperature * coefCold;
            return result;
        }

        public BeeParams findBestinHive()
        {
            hostArchive.Sort(sorterBee);
            return hostArchive[0];
        }

        public virtual void flyWorkers(BeeParams best ,Random rand)
        {
            theWorkers = WorkerParams.WorkersFly ( best,hostArchive,rand,theParrent);
        
        
        }


        public virtual void flyOutLookers( BeeParams best, Random rand)
        {
            theOutlookers = OutLookersBeeParams.OutLookerFly(best, hostArchive, rand, theParrent);
            

        }

        public virtual int makeNewPopulation(BeeParamsConf.Type_Selection selectType, int hiveSize, double percentScout, Random rand, double Border1, double Border2, double Border3, int Repeat1, int Repeat2, int Repeat3)
        {
            
            hostArchive.AddRange(theWorkers);
            
            hostArchive.AddRange(theOutlookers);
            int countafterselect = (int) (hiveSize * (1-percentScout/100));
            hostArchive.RemoveAll(x => double.IsInfinity(x.Goods));
          
   
            switch (selectType)
            { case BeeParamsConf.Type_Selection.Бинарный_турнир: {hostArchive= Selector.selectBinaryTornament(hostArchive,countafterselect,rand);} break;
                case BeeParamsConf.Type_Selection.Рулетка: {hostArchive= Selector.selectRoulet(hostArchive,countafterselect,rand);} break;
                case BeeParamsConf.Type_Selection.Случайный_отбор: {hostArchive= Selector.selectRandom(hostArchive,countafterselect,rand);} break;
                case BeeParamsConf.Type_Selection.Элитный_отбор: {hostArchive =Selector.selectElite(hostArchive,countafterselect);} break;           
            }



            hostArchive = Selector.Regeration(hostArchive, Border1, Repeat1, Border2, Repeat2, Border3, Repeat3, countafterselect);


            theWorkers.Clear();
            theOutlookers.Clear();
            theParrent.CleanTempory();
            GC.Collect();
            return hiveSize - hostArchive.Count;
        }

    }
}
