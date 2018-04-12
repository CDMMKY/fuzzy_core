using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSMMethods
{
    public class RWLSMTakagiSugeno : AbstractNotSafeLearnAlgorithm
    {



        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }

        public IFuzzySystem TuneUpFuzzySystem(IFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            TSAFuzzySystem toRunSystem = Approximate as TSAFuzzySystem;
            return TuneUpFuzzySystem(toRunSystem, conf);
        }


      

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {


            if (Approximate.RulesDatabaseSet.Count == 0)
            {
                throw new InvalidOperationException("Нечеткая система не была корректно инициализированна");
            }
            KnowlegeBaseTSARules newBase = new KnowlegeBaseTSARules(Approximate.RulesDatabaseSet[0]);


            double result_before = Approximate.approxLearnSamples(newBase);
       
            foreach (TSARule Rule in newBase.RulesDatabase)
            {double [] coefficient  = null;
            double Value = LSMWeghtReqursiveSimple.EvaluteConsiquent(Approximate, Rule.ListTermsInRule, out coefficient);
            Rule.IndependentConstantConsequent = Value;
            Rule.IndependentConstantConsequent = Value;
            Rule.RegressionConstantConsequent = coefficient;

            }
            
            double result_after = Approximate.approxLearnSamples(newBase);
            if (result_before > result_after)
            {
                Approximate.RulesDatabaseSet[0] = newBase;
            }
            GC.Collect();
            Approximate.RulesDatabaseSet[0].TermsSet.Trim();
            return Approximate;

        }
        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "Рекурсивный взвешенный МНК {";
                result += "}";
                return result;
            }
            return "Рекурсивный взвешенный МНК";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new NullConfForAll();
            result.Init(CountFeatures);
            return result;
        }





















    }
}
