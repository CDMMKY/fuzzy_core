using System;
using System.Collections.Generic;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using LSMMethods;

namespace FuzzySystem.TakagiSugenoApproximate.LearnAlgorithm.Bee
{
   public class BeeParamsAlgorithm : AbstractNotSafeLearnAlgorithm
    {
     protected   TSAFuzzySystem theFuzzySystem;
     protected   int hiveSize;
     protected   double persentScouts;
     protected   int countScouts;
     protected   int countWorkers;
     protected   int countOutLookers;
     protected double initTemp;
     protected double coefTemp;
     protected int iterration;
     protected double border1;
     protected  double border2;
     protected double border3;
     protected int repeat1;
     protected int repeat2;
     protected int repeat3;
     protected BeeParamsConf.Type_Selection TypeSelection;
     protected List<KnowlegeBaseTSARules> Tempory = new List<KnowlegeBaseTSARules>();
     protected object Cleaner = new object();
     protected Random rand = new Random(DateTime.Now.Millisecond);
     protected HiveParams theHive;
     protected BeeParams theBest;

     public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
     {
         get
         {
             return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
         }
     }
    

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            TSAFuzzySystem result = Approx;
            BeeParamsConf Config = conf as BeeParamsConf;
            Init(Config);
            theFuzzySystem = result;
          
            if (result.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }
             theHive = new HiveParams(this,result.RulesDatabaseSet[0]);
         //  HiveParams theHive = new ParallelHiveParams(this, result.RulesDatabaseSet[0]);
           // HiveParams theHive = new HiveParallelParams2(this, result.RulesDatabaseSet[0]);

            theBest= new BeeParams(result.RulesDatabaseSet[0],this);
            double temperature = initTemp;
            for (int r = 0; r < iterration; r++)
            {
                temperature = oneIterate(temperature);
                GC.Collect();
            }
            theBest = lastStep(theBest);
            Approx.RulesDatabaseSet[0] = theBest.PositionOfBee;
            Approx.RulesDatabaseSet[0].TermsSet.Trim();
            return Approx;

        }

        public double oneIterate(double temperature)
        {
            theHive.flyScouts(countScouts, rand, theBest.PositionOfBee);
          double   newtemperature = theHive.LimitScoutBeeSolution(temperature, coefTemp, rand);
            theBest = theHive.findBestinHive();
            theHive.flyWorkers(theBest, rand);
            theHive.flyOutLookers(theBest, rand);
            countScouts = theHive.makeNewPopulation(TypeSelection, hiveSize, persentScouts, rand, border1, border2, border3, repeat1, repeat2, repeat3);
            theBest = theHive.findBestinHive();
            return newtemperature;
        
        }


        public double CalcNewProfit(KnowlegeBaseTSARules Solution)
        {
            Tempory.Add(Solution);
            theFuzzySystem.UnlaidProtectionFix(Solution);
            return theFuzzySystem.approxLearnSamples(Solution);
          
        }


        public void Init(BeeParamsConf Config)
        {
            hiveSize = Config.ABCWHiveSize;
            persentScouts = Config.ABCWPercentScoutInHive;
            countScouts = (int)(hiveSize * persentScouts / 100);
            countWorkers = (int)((hiveSize - countScouts) / 2);
            countOutLookers = hiveSize - countScouts - countWorkers;
            initTemp = Config.ABCWInitTemperature;
            coefTemp = Config.ABCWColdCoeff;
            iterration = Config.ABCWCountIteration;
            border1 = Config.ABCWFirstBorder;
            repeat1 = Config.ABCWFirstRepeate;
            border2 = Config.ABCWSecondBorder;
            repeat2 = Config.ABCWSecondRepeate;
            border3 = Config.ABCWFirdBorder;
            repeat3 = Config.ABCWFirdRepeate;
            TypeSelection = Config.ABCWTypeSelection;
           
        
        }

        public void CleanTempory()
        { 
            lock (Cleaner)
            {
                for (int i = 0; i < Tempory.Count; i++)
                {
                    theFuzzySystem.RulesDatabaseSet.Remove(Tempory[i]);
                }
                Tempory.Clear();
            }

        }

        public TSAFuzzySystem getCurrentNs()
        {
            return theFuzzySystem;

        }


       protected BeeParams lastStep(BeeParams theBest)
        {  double tempgood = theBest.getGoodsImproove();
           KnowlegeBaseTSARules tempPositionofBee = theBest.PositionOfBee;
          TSAFuzzySystem ToOpintNS =getCurrentNs();
           KnowlegeBaseTSARules zeroSolution = ToOpintNS.RulesDatabaseSet[0];
           ToOpintNS.RulesDatabaseSet[0]= theBest.PositionOfBee;

           RWLSMTakagiSugeno tryOpt = new RWLSMTakagiSugeno();

           tryOpt.TuneUpFuzzySystem (ToOpintNS,new NullConfForAll());
        
           theBest.PositionOfBee = ToOpintNS.RulesDatabaseSet[0];
           double newgood =theBest.getGoodsImproove(); 

           if (newgood>tempgood)
           {theBest.PositionOfBee = tempPositionofBee;
           }
           return theBest;
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелиной колонии для параметрической оптимизации {";
                result += "Количество итераций= " + iterration.ToString() + " ;" + Environment.NewLine;
                result += "Размер улья= " + hiveSize.ToString() + " ;" + Environment.NewLine;
                result += "Начальная температура= " + initTemp.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент отхлаждения= " + coefTemp.ToString() + " ;" + Environment.NewLine;
                result += "Граница 1= " + border1.ToString() + " ;" + Environment.NewLine;
                result += "Повторов 1= " + repeat1.ToString() + " ;" + Environment.NewLine;

                result += "Граница 2= " + border2.ToString() + " ;" + Environment.NewLine;
                result += "Повторов 2= " + repeat2.ToString() + " ;" + Environment.NewLine;

                result += "Граница 3= " + border3.ToString() + " ;" + Environment.NewLine;
                result += "Повторов 3= " + repeat3.ToString() + " ;" + Environment.NewLine;
          
                result += "Алгоритм селекции= " + typeSelectionToString(TypeSelection) + " ;" + Environment.NewLine;
                result += "}";
                return result;
                }
            return "Алгоритм пчелиной колонии для параметрической оптимизации";
        }

        public string typeSelectionToString(BeeParamsConf.Type_Selection Source)
        { switch(Source)
        {
            case BeeParamsConf.Type_Selection.Бинарный_турнир: { return "Бинарныйтурнир"; }
            case BeeParamsConf.Type_Selection.Рулетка: { return "Рулетка"; }
            case BeeParamsConf.Type_Selection.Случайный_отбор: { return "Случайный отбор"; }
            case BeeParamsConf.Type_Selection.Элитный_отбор: { return "Элитный отбор"; }
        }
        return "";
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new BeeParamsConf();
            result.Init(CountFeatures);
            return result;
        }


    }
}
