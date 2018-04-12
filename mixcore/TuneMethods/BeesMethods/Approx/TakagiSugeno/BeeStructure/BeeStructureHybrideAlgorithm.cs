/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.Bee
{
    class BeeStructureHybrideAlgorithm : BeeStructureAlgorithm//, ILearnHybride
    {
        public SAFuzzySystem TuneUpFuzzySystem(SingletonHybride Ocean, SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            SAFuzzySystem result = Approximate;


            BeeStructureConf Config = conf as BeeStructureConf;
            countScouts = Config.ABCSCountScout;
            countWorkers = Config.ABCSCountWorkers;
            countRules = Config.ABCSCountRules;
            theFuzzySystem = result;
            typeTerm = Config.ABCSTypeFunc;

            List<KnowlegeBaseSARules> Solution = new List<KnowlegeBaseSARules>();
            if (result.RulesDatabaseSet.Count < 1)
            {
                throw (new Exception("Что то не так с базой правил"));
            }

            for (int r = 0; r < countRules; r++)
            {
                
                oneIterate(result);
                Solution.Add(result.RulesDatabaseSet[0]);

                Ocean.Store(Solution, this.ToString());
                Solution.RemoveAt(1);
          Solution=      Ocean.Get(1, FuzzyAbstract.Hybride.FuzzyHybrideBase.goodness.best, FuzzyAbstract.Hybride.FuzzyHybrideBase.islandStrategy.All);
          if (Solution.Count > 0)
          {
              Solution.Add(result.RulesDatabaseSet[0]);
              result.RulesDatabaseSet.Add( Solution[0]);
              double tempRes = result.approxLearnSamples(1);

              if (tempRes > baseLine)
              {
                  result.RulesDatabaseSet.RemoveAt(0);
              }
              else { result.RulesDatabaseSet.RemoveAt(1); }
          }
            }

            return result;
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Алгоритм пчелиной колонии для структурной оптимизации (острова) {";
                result += "Количество разведчиков= " + countScouts.ToString() + " ;" + Environment.NewLine;
                result += "Количество рабочих пчел= " + countWorkers.ToString() + " ;" + Environment.NewLine;
                result += "Количество генерируемых правил= " + countRules.ToString() + " ;" + Environment.NewLine;
                result += "Вид функции принадлежности= " + Term.ToStringTypeTerm(typeTerm) + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Алгоритм пчелиной колонии для структурной оптимизации (острова)";
        }

    }
}
*/