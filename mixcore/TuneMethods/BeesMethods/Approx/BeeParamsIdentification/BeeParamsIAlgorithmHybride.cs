using System;
using System.Collections.Generic;
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class BeeParamsIAlgorithmHybride:BeeParamsAlgorithm//, ILearnHybride
    {


       public SAFuzzySystem TuneUpFuzzySystem(SingletonHybride Ocean, SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            SAFuzzySystem result = Approximate;
            BeeParamsConf Config = conf as BeeParamsConf;
            Init(Config);
            theFuzzySystem = result;

            if (result.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }
            theHive = new HiveParams(this, result.RulesDatabaseSet[0]);
            //  HiveParams theHive = new ParallelHiveParams(this, result.RulesDatabaseSet[0]);
            // HiveParams theHive = new HiveParallelParams2(this, result.RulesDatabaseSet[0]);

            theBest = new BeeParams(result.RulesDatabaseSet[0], this);
            double temperature = initTemp;
            for (int r = 0; r < iterration; r++)
            {


                temperature = oneIterate(temperature);

                int CountSend = theHive.HostArchive.Count / 2;
                List<KnowlegeBaseSARules> toStore = new List<KnowlegeBaseSARules>();
                for (int i = 0; i <CountSend ; i++)
                {
                    toStore.Add(theHive.HostArchive[i].PositionOfBee);
                }

                Ocean.Store(toStore, this.ToString());

                int toGet = theHive.HostArchive.Count - CountSend;

                List<KnowlegeBaseSARules> Getted = Ocean.Get(toGet, FuzzyAbstract.Hybride.FuzzyHybrideBase.goodness.best, FuzzyAbstract.Hybride.FuzzyHybrideBase.islandStrategy.All);

                toGet = Getted.Count < toGet ? Getted.Count : toGet;

                for (int i = 0; i < toGet; i++)
                {
                    theHive.HostArchive[i + CountSend].PositionOfBee = Getted[i];
                }


                    GC.Collect();
            }
            theBest = lastStep(theBest);
            Approximate.RulesDatabaseSet[0] = theBest.PositionOfBee;

            return result;

        }


       public override string ToString(bool with_param = false)
       {

           if (with_param)
           {
               string result = "Алгоритм пчелиной колонии для параметрической оптимизации (острова) {";
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
           return "Алгоритм пчелиной колонии для параметрической оптимизации (острова)";
       }
    
    }
}
