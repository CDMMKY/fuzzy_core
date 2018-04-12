
using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;
using BeesMethods.Base.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.BeeDis
{
    public class BeeStructureAlgorithm : AbstractNotSafeLearnAlgorithm
    {
        protected int countScouts;
        protected int countWorkers;
        protected int countIters;
        protected int countBestBase;
        static int seed = Environment.TickCount * Thread.CurrentThread.ManagedThreadId;

        protected readonly ThreadLocal<Random> rand =
     new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        protected PCFuzzySystem theFuzzySystem;
        protected Scout[] theScouts;
        protected Worker[] theWorkers;
        protected double baseLine = 0;
        protected BeeComparer toBeeSort = new BeeComparer();
        protected BeeComparerAccuracy toSolutions = new BeeComparerAccuracy();
        protected BeeComparerEqual toDistinct = new BeeComparerEqual();
        protected int iterate = 0;

        protected List<Bee> solutionInfo = new List<Bee>();

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
            for (int r = 0; r < countIters; r++)
            {
                iterate = r;
                oneIterate(theFuzzySystem);
                Console.WriteLine($"Accuracy {solutionInfo.Last().accuracy} on {r + 1}");

            }
            Final();
            return Classifier;

        }

        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелинной колонии дискретный {";
                result += "Количество разведчиков= " + countScouts.ToString() + " ;" + Environment.NewLine;
                result += "Количество рабочих пчел= " + countWorkers.ToString() + " ;" + Environment.NewLine;
                for (int z = solutionInfo.Count - countBestBase; z < solutionInfo.Count; z++)
                {
                    int saved = solutionInfo[z].PositionOfBee.Where(x => x == true).Count();
                    int lost = solutionInfo[z].PositionOfBee.Length - saved;
                    theFuzzySystem.AcceptedFeatures = solutionInfo[z].PositionOfBee;
                    double accuracyLearn = theFuzzySystem.ClassifyLearnSamples(theFuzzySystem.RulesDatabaseSet[0]);
                    double accuracyTest = theFuzzySystem.ClassifyTestSamples(theFuzzySystem.RulesDatabaseSet[0]);
                    result += $"Оставшиеся признаки {saved}:{accuracyLearn}||{accuracyTest} [ ";
                    for (int i = 0; i < solutionInfo[z].PositionOfBee.Length; i++)
                    {
                        if (solutionInfo[z].PositionOfBee[i] == true) result += (i + 1).ToString() + ", ";
                    }
                    result += "]" + Environment.NewLine;
                    result += $"Усеченные признаки {saved}:{accuracyLearn}||{accuracyTest} [ ";
                    for (int i = 0; i < solutionInfo[z].PositionOfBee.Length; i++)
                    {
                        if (solutionInfo[z].PositionOfBee[i] == false) result += (i + 1).ToString() + ", ";
                    }

                    result += "]" + Environment.NewLine;
                }

                result += "}";
               return result;
            
            }
            return "Алгоритм пчелинной колонии дискретный";
        }




        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new BeeDisStructureConf();
            result.Init(CountFeatures);
            return result;
        }





        public virtual void oneIterate(PCFuzzySystem result)
        {
            baseLine = theFuzzySystem.ClassifyLearnSamples(theFuzzySystem.RulesDatabaseSet[0]);
           
            Console.WriteLine("Parallel scouts");
            Parallel.For(0, countScouts, new ParallelOptions { MaxDegreeOfParallelism = countScouts, TaskScheduler = null }, s =>
            //        for (int s = 0; s < countScouts; s++)
            {
                double goodsLine = -1;
                int ztryes = 0;
                while ((goodsLine <= 0) && (ztryes < 100))
                {
                    theScouts[s] = new Scout(theFuzzySystem.AcceptedFeatures);
                    theScouts[s].generateNewVector(theFuzzySystem, rand.Value);
                    goodsLine = theScouts[s].getGoodsImproove(theFuzzySystem, baseLine);
                    ztryes++;
                }
            }
              );
            Array.Sort(theScouts, toSolutions);
            Console.WriteLine("Slow 1");
            Console.WriteLine("Parallel workers");

            for (int a =0; a< 3;a++)
            { 
            Parallel.For(0, countWorkers, new ParallelOptions { MaxDegreeOfParallelism = countWorkers, TaskScheduler = null }, w =>
            //   for (int w = 0; w < countWorkers; w++)
            {
            theWorkers[countWorkers * a + w] = new Worker(theScouts[theScouts.Length -1-a].PositionOfBee);
                theWorkers[countWorkers * a + w].WorkerFly(theFuzzySystem, rand.Value,iterate, countIters);
                theWorkers[countWorkers * a + w].getGoodsImproove(theFuzzySystem, baseLine);
            }
               );
            }

            Console.WriteLine("Slow 2");


            Array.Sort(theWorkers, toSolutions);



            solutionInfo.AddRange(theWorkers);
            solutionInfo.AddRange(theScouts);
            solutionInfo.Distinct(toDistinct);
         
            solutionInfo.Sort(toSolutions);

            solutionInfo = solutionInfo.GetRange(solutionInfo.Count - countBestBase, countBestBase);


            theFuzzySystem.AcceptedFeatures = (bool[])solutionInfo.Last().PositionOfBee.Clone();

        }

        public virtual void Init(ILearnAlgorithmConf Config)
        {

            BeeDisStructureConf config = Config as BeeDisStructureConf;
            countScouts = config.ABCDSCountScout;
            countWorkers = config.ABCDSCountWorkers;
            theScouts = new Scout[countScouts];
            theWorkers = new Worker[countWorkers*3];
            countIters = config.ABCDSCountIter;
            countBestBase = config.ABCDS_CountOfBestBase;
            solutionInfo.Clear();


            if (theFuzzySystem.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }


        }

        public virtual void Final()
        {
          //  solutionInfo.Clear();
            theScouts = null;
            theWorkers = null;
            GC.Collect();
        }
    }
}

