#define PClass
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
#else
using KnowlegeBaseRules = FuzzySystem.SingletoneApproximate.KnowlegeBaseSARules;
using Rule = FuzzySystem.SingletoneApproximate.SARule;
using FS = FuzzySystem.SingletoneApproximate.SAFuzzySystem;
#endif

using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using BeesMethods.Base.Common;
using System.Threading.Tasks;
using System.Threading;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    public class BeeStructureAlgorithm : AbstractNotSafeLearnAlgorithm
    {
       
     protected   FS theFuzzySystem;
     protected int countScouts;
     protected int countWorkers;
     protected int countRules;
     protected double baseLine;
     protected TypeTermFuncEnum typeTerm;
     static int seed = Environment.TickCount * Thread.CurrentThread.ManagedThreadId;
     protected readonly ThreadLocal<Random> rand =
     new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        protected BeeStructureConf Config;
     protected Scout [] theScouts;
     protected Worker [] theWorkers;
     protected KnowlegeBaseRules Best;
     public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
     {
         get
         {
                
                var res = new List<FuzzySystemRelisedList.TypeSystem>();

#if PClass
                res.Add(FuzzySystemRelisedList.TypeSystem.PittsburghClassifier);
#elif SApprox
                res.Add (FuzzySystemRelisedList.TypeSystem.Singletone) ;

#elif TSApprox
                res.Add(FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate);
#else
      res.Add (FuzzySystemRelisedList.TypeSystem.Singletone) ;
#endif
                return res;

            }
        }

        public override FS TuneUpFuzzySystem(FS Approx, ILearnAlgorithmConf conf)
        {
             theFuzzySystem = Approx;
            Init(conf);
            for (int r = 0; r < countRules; r++)
            {
                oneIterate(theFuzzySystem);
            }

            Final();
            Approx.RulesDatabaseSet[0].TermsSet.Trim();
            return Approx;

        }

        public virtual void oneIterate(FS result)
        {
        //    theScouts.Clear();
          //  theWorkers.Clear();
                baseLine = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
                Best = result.RulesDatabaseSet[0];
            /*     for (int s = 0; s < countScouts; s++)
                 {
                     double goodsLine = -1;
                     int ztryes = 0;
                     while ((goodsLine <= 0) && (ztryes < 100))
                     {
                         theScout = new Scout(Best, theFuzzySystem);
                         theScout.generateNewRule(typeTerm, rand);
                         goodsLine = theScout.getGoodsImproove(baseLine);
                     ztryes++;
                     }
                     theScouts.Add(theScout);
                 }*/
            Parallel.For(0, countScouts, new ParallelOptions { MaxDegreeOfParallelism = countScouts, TaskScheduler = null }, s =>
            //        for (int s = 0; s < countScouts; s++)
            {
                double goodsLine = -1;
                int ztryes = 0;
                while ((goodsLine <= 0) && (ztryes < 100))
                {
                    theScouts[s] = new Scout(Best,theFuzzySystem);
                    theScouts[s].generateNewRule(typeTerm, rand.Value);
                    goodsLine = theScouts[s].getGoodsImproove( baseLine);
                    ztryes++;
                }
            }
);



            BeeComparer toBeeSort = new BeeComparer();
               Array.Sort( theScouts,toBeeSort);
                KnowlegeBaseRules ScoutBest = theScouts[countScouts-1].PositionOfBee;
                int ScoutBestNumRule = theScouts[countScouts - 1].NumOFRule;
            /*for (int w = 0; w < countWorkers; w++)
            {
                theWorker = new Worker(ScoutBest, theFuzzySystem);
                theWorkers.Add(theWorker);
                theWorkers[theWorkers.Count - 1].WorkerFly( ScoutBestNumRule, rand);
                theWorkers[countWorkers*3 - 1].getGoodsImproove(baseLine);
            }*/

            for (int a = 0; a < 3; a++)
            {
                Parallel.For(0, countWorkers, new ParallelOptions { MaxDegreeOfParallelism = countWorkers, TaskScheduler = null }, w =>
                //   for (int w = 0; w < countWorkers; w++)
                {
                    theWorkers[countWorkers * a + w] = new Worker(theScouts[theScouts.Length - 1 - a].PositionOfBee,theFuzzySystem);
                    theWorkers[countWorkers * a + w].WorkerFly(ScoutBestNumRule, rand.Value);
                    theWorkers[countWorkers * a + w].getGoodsImproove(baseLine);
                }
                   );
            }

            Array.Sort( theWorkers,toBeeSort);

                if (theScouts[countScouts-1].Goods > theWorkers[countWorkers*3-1].Goods)
                {
                    Best = theScouts[countScouts-1].PositionOfBee;
                }
                else
                {
                    Best = theWorkers[countWorkers*3-1].PositionOfBee;
                }
                theFuzzySystem.RulesDatabaseSet[0] = Best;
            }
      
    

        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелиной колонии для структурной оптимизации {";
                result += "Количество разведчиков= " + countScouts.ToString() + " ;" + Environment.NewLine;
                result += "Количество рабочих пчел= " + countWorkers.ToString() + " ;" + Environment.NewLine;
                result += "Количество генерируемых правил= " + countRules.ToString() + " ;" + Environment.NewLine;
                result += "Вид функции принадлежности= " + Term.ToStringTypeTerm(typeTerm) + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм пчелиной колонии для структурной оптимизации";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new BeeStructureConf();
            result.Init(CountFeatures);
            return result;
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {
            this.Config = Config as BeeStructureConf;
            countScouts = this.Config.ABCSCountScout;
            countWorkers = this.Config.ABCSCountWorkers;
            countRules = this.Config.ABCSCountRules;
            typeTerm = this.Config.ABCSTypeFunc;
            theScouts = new Scout[countScouts];
            theWorkers = new Worker[countWorkers * 3];


            if (theFuzzySystem.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }

        }

        public virtual void Final()
        {
            theFuzzySystem.RulesDatabaseSet[0] = Best;
            baseLine = theFuzzySystem.ErrorLearnSamples(theFuzzySystem.RulesDatabaseSet[0]);
       //     theScouts = null;
       //     theWorkers = null;
       //     theScout = null;
       //     theWorker = null;
            GC.Collect();
        }
    }
}
