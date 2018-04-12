/*
using System;
using System.Collections.Generic;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;
using BeesMethods.Base.Common;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.Bee
{
    public class BeeStructureAlgorithm : AbstractNotSafeLearnAlgorithm
    {
        protected int countScouts;
        protected int countWorkers;
        protected int countRules;
        protected int countClass;
        protected TypeTermFuncEnum typeTerm;
        protected Random rand = new Random(DateTime.Now.Millisecond);
        protected PCFuzzySystem theFuzzySystem;
        protected List<Scout> theScouts = new List<Scout>();
        protected List<Worker> theWorkers = new List<Worker>();
        protected double baseLine = 0;
        protected KnowlegeBasePCRules Best = null;
        protected BeeComparer toBeeSort = new BeeComparer();

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            theFuzzySystem = Classifier;
            Init(conf);
            for (int r = 0; r < countRules; r++)
            {
                oneIterate(theFuzzySystem);
            }
            Final();
            Classifier.RulesDatabaseSet[0].TermsSet.Trim();
            return Classifier;

        }



        public double CalcNewProfit(KnowlegeBasePCRules Solution)
        {
            theFuzzySystem.RulesDatabaseSet.Add(Solution);
            double result = theFuzzySystem.ClassifyLearnSamples(theFuzzySystem.RulesDatabaseSet.Count - 1);
            theFuzzySystem.RulesDatabaseSet.Remove(Solution);
            return result;
        }

        public PCFuzzySystem getCurrentNs()
        {
            return theFuzzySystem;

        }



        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелинной колонии для структурной оптимизации {";
                result += "Количество разведчиков= " + countScouts.ToString() + " ;" + Environment.NewLine;
                result += "Количество рабочих пчел= " + countWorkers.ToString() + " ;" + Environment.NewLine;
                result += "Количество генерируемых правил= " + countRules.ToString() + " ;" + Environment.NewLine;
                result += "Вид функции принадлежности= " + Term.ToStringTypeTerm(typeTerm) + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм пчелинной колонии для структурной оптимизации";
        }




        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new BeeStructureConf();
            result.Init(CountFeatures);
            return result;
        }





        public virtual void oneIterate(PCFuzzySystem result)
        {
            theScouts.Clear();
            theWorkers.Clear();
            baseLine = theFuzzySystem.ClassifyLearnSamples();
            Best = theFuzzySystem.RulesDatabaseSet[0];
            Scout theScout = null;
            Worker theWorker = null;
            for (int s = 0; s < countScouts; s++)
            {
                double goodsLine = -1;
                int ztryes = 0;
                while ((goodsLine <= 0) && (ztryes < 100))
                {
                    theScout = new Scout(Best, this);
                    theScout.generateNewRule(typeTerm, rand);
                    goodsLine = theScout.getGoodsImproove(baseLine);
                }
                theScouts.Add(theScout);
            }

            theScouts.Sort(toBeeSort);
            KnowlegeBasePCRules ScoutBest = theScouts[theScouts.Count - 1].PositionOfBee;
            int ScoutBestNumRule = theScouts[theScouts.Count - 1].NumOFRule;
            for (int w = 0; w < countWorkers; w++)
            {
                theWorker = new Worker(ScoutBest, this);
                theWorkers.Add(theWorker);
                theWorkers[theWorkers.Count - 1].WorkerFly(ScoutBestNumRule, rand);
                theWorkers[theWorkers.Count - 1].getGoodsImproove(baseLine);
            }

            theWorkers.Sort(toBeeSort);

            if (theScouts[theScouts.Count - 1].Goods > theWorkers[theWorkers.Count - 1].Goods)
            {
                Best = theScouts[theScouts.Count - 1].PositionOfBee;
            }
            else
            {
                Best = theWorkers[theWorkers.Count - 1].PositionOfBee;
            }

            theFuzzySystem.RulesDatabaseSet[0] = Best;
            baseLine = theFuzzySystem.ClassifyLearnSamples();
       
        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {

            BeeStructureConf config = Config as BeeStructureConf;
            countScouts = config.ABCSCountScout;
            countWorkers = config.ABCSCountWorkers;
            countRules = config.ABCSCountRules;
            countClass = theFuzzySystem.CountClass;
            typeTerm = config.ABCSTypeFunc;


            if (theFuzzySystem.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }


        }

        public virtual void Final()
        {
             

          /*  if (theScouts[theScouts.Count - 1].Goods > theWorkers[theWorkers.Count - 1].Goods)
            {
                Best = theScouts[theScouts.Count - 1].PositionOfBee;
            }
            else
            {
                Best = theWorkers[theWorkers.Count - 1].PositionOfBee;
            }

          */ /*  theFuzzySystem.RulesDatabaseSet[0] = Best;
              theScouts = null;
              theWorkers = null;
           
              GC.Collect();
        }
    }
}

*/