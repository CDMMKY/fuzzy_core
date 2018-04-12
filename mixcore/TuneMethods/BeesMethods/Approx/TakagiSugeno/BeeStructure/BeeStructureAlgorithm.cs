#define TSApprox
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

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
    public class BeeStructureAlgorithm : AbstractNotSafeLearnAlgorithm
    {
       
     protected   FS theFuzzySystem;
     protected int countScouts;
     protected int countWorkers;
     protected int countRules;
     protected double baseLine;
     protected TypeTermFuncEnum typeTerm;
     protected Random rand = new Random(DateTime.Now.Millisecond);
     protected BeeStructureConf Config;
     protected List<Scout> theScouts;
     protected List<Worker> theWorkers;
     Scout theScout;
     Worker theWorker;
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
            theScouts.Clear();
            theWorkers.Clear();
                baseLine = result.ErrorLearnSamples(result.RulesDatabaseSet[0]);
                Best = result.RulesDatabaseSet[0];
                theScout = null;
                theWorker = null;
                for (int s = 0; s < countScouts; s++)
                {
                    double goodsLine = -1;
                    int ztryes = 0;
                    while ((goodsLine <= 0) && (ztryes < 100))
                    {
                        theScout = new Scout(Best, theFuzzySystem);
                        theScout.generateNewRule(typeTerm, rand);
                        goodsLine = theScout.getGoodsImproove(baseLine);
                    }
                    theScouts.Add(theScout);
                }
            BeeComparer toBeeSort = new BeeComparer();
                theScouts.Sort(toBeeSort);
                KnowlegeBaseRules ScoutBest = theScouts[theScouts.Count-1].PositionOfBee;
                int ScoutBestNumRule = theScouts[theScouts.Count - 1].NumOFRule;
                for (int w = 0; w < countWorkers; w++)
                {
                    theWorker = new Worker(ScoutBest, theFuzzySystem);
                    theWorkers.Add(theWorker);
                    theWorkers[theWorkers.Count - 1].WorkerFly( ScoutBestNumRule, rand);
                    theWorkers[theWorkers.Count - 1].getGoodsImproove(baseLine);
                }

                theWorkers.Sort(toBeeSort);

                if (theScouts[theScouts.Count-1].Goods > theWorkers[theWorkers.Count-1].Goods)
                {
                    Best = theScouts[theScouts.Count-1].PositionOfBee;
                }
                else
                {
                    Best = theWorkers[theWorkers.Count-1].PositionOfBee;
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

            if (theFuzzySystem.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }

            theScouts = new List<Scout>();
           theWorkers = new List<Worker>();
        }

        public virtual void Final()
        {
            theFuzzySystem.RulesDatabaseSet[0] = Best;
            baseLine = theFuzzySystem.ErrorLearnSamples(theFuzzySystem.RulesDatabaseSet[0]);
            theScouts = null;
            theWorkers = null;
            theScout = null;
            theWorker = null;
            GC.Collect();
        }
    }
}
