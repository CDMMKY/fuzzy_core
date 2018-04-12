//#define CONTRACTS_FULL
using System;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.PittsburghClassifier
{
    public class PCFuzzySystem : IFuzzySystem
    {
        #region Visible public methods




        public List<KnowlegeBasePCRules> RulesDatabaseSet
        {
            get;
            protected set;
        } = new List<KnowlegeBasePCRules>();

        public override List<KnowlegeBaseRules> AbstractRulesBase()
        {
            return RulesDatabaseSet.Cast<KnowlegeBaseRules>().ToList();
        }
        public override int ValueComplexity(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBasePCRules)
                return ((KnowlegeBasePCRules)Source).ValueComplexity;
            return 0;

        }

        public override int ValueRuleCount(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBasePCRules)
                return ((KnowlegeBasePCRules)Source).RulesDatabase.Count;
            return 0;
        }

        public int CountClass
        {
            get
            {
                Requires(LearnSamplesSet != null);
                return LearnSamplesSet.CountClass;
            }
        }

        #region constructor

        public PCFuzzySystem(SampleSet learnSet, SampleSet testSet)
            : base(learnSet, testSet)
        {
            Requires(learnSet != null);
            LearnSamplesSet = learnSet;
            if (testSet != null)
            {
                TestSamplesSet = testSet;
                for (int i = 0; i < CountFeatures; i++)
                {
                    if (
                     !LearnSamplesSet.InputAttributes[i].Name.Equals(TestSamplesSet.InputAttributes[i].Name,
                                                                        StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }
                }
            }
        }

        #endregion



        public override double ErrorTestSamples(KnowlegeBaseRules Source)
        {
            return 100.0 - ClassifyTestSamples(Source as KnowlegeBasePCRules);
        }
        public override double ErrorLearnSamples(KnowlegeBaseRules Source)
        {
            return 100.0 - ClassifyLearnSamples(Source as KnowlegeBasePCRules);
        }
        public virtual double ClassifyTestSamples(KnowlegeBasePCRules Source)
        {
            Requires(Source != null && TestSamplesSet.DataRows!=null);

            int sum = TestSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Count(x => x.StringOutput.Equals(classifyBase(x.InputAttributeValue, Source), StringComparison.OrdinalIgnoreCase));
            return (double)sum / (double)TestSamplesSet.CountSamples * 100.0;
        }

        public virtual double ClassifyLearnSamples(KnowlegeBasePCRules Source) //attention only for multicore processor optimized 
        {
            Requires(Source != null && LearnSamplesSet.DataRows != null);
            int sum = LearnSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Count(x => x.StringOutput.Equals(classifyBase(x.InputAttributeValue, Source), StringComparison.OrdinalIgnoreCase));
           
            return (double)sum / (double)LearnSamplesSet.CountSamples * 100.0;
        }

        public virtual double ClassifyLearnSamplesBagging(List<KnowlegeBasePCRules> Sources) //attention only for multicore processor optimized 
        {
            Requires(Sources != null && LearnSamplesSet.DataRows != null);
            int sum = LearnSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Count(x => x.StringOutput.Equals(classifyBaseBagging(x.InputAttributeValue, Sources), StringComparison.OrdinalIgnoreCase));

            return (double)sum / (double)LearnSamplesSet.CountSamples * 100.0;
        }

        public virtual double ClassifyTestSamplesBagging(List<KnowlegeBasePCRules> Sources)
        {
            Requires(Sources != null && TestSamplesSet.DataRows != null);

            int sum = TestSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Count(x => x.StringOutput.Equals(classifyBaseBagging(x.InputAttributeValue, Sources), StringComparison.OrdinalIgnoreCase));
            return (double)sum / (double)TestSamplesSet.CountSamples * 100.0;
        }



        public string classifyBase(double[] object_c, KnowlegeBasePCRules Source)
        {
            Requires(Source != null);
            Requires(object_c != null);

            double max_sum = 0;
            bool correct_classificate = false;
            string result = "";
            var RulesOfClass = Source.RulesDatabase.GroupBy(x => x.LabelOfClass);
            foreach (var RulesOfOneClass in RulesOfClass)
            {
                double sum = 0;
                foreach (var Rule in RulesOfOneClass)
                {
                    double mul = Rule.ListTermsInRule.Where(x => AcceptedFeatures[x.NumVar]).Select(y => y.LevelOfMembership(object_c)).Aggregate(1.0, (p, n) => p * n);
                    sum += mul * Rule.CF;

                }
                if (max_sum < sum)
                {
                    correct_classificate = true;
                    max_sum = sum;
                    result = RulesOfOneClass.Key;
                }
            }
            if (!correct_classificate)
            {
                return "nonResult";
                //throw (new ArgumentException(
                //  "Невозможно классифицировать объект скорее всего существуют не накрытые участи на признаках"));
            }
            return result;
        }

        public string classifyBaseBagging(double[] object_c, List<KnowlegeBasePCRules> Sources)
        {
            Requires(Sources != null);
            Requires(object_c != null);
            Dictionary<string, int> results = new Dictionary<string, int>();
            for (int i = 0; i < Sources.Count; i++)
            {
                foreach (var RulesOfOneClass in Sources[i].RulesDatabase.GroupBy(x => x.LabelOfClass))
                {
                    if (!results.ContainsKey(RulesOfOneClass.Key))
                        results.Add(RulesOfOneClass.Key, 0);
                }
            }
            foreach (var Source in Sources)
            {
                double max_sum = 0;
                string key = "";
                bool correct_classificate = false;
                var RulesOfClass = Source.RulesDatabase.GroupBy(x => x.LabelOfClass);
                foreach (var RulesOfOneClass in RulesOfClass)
                {
                    double sum = 0;
                    foreach (var Rule in RulesOfOneClass)
                    {
                        double mul = Rule.ListTermsInRule.Where(x => AcceptedFeatures[x.NumVar]).Select(y => y.LevelOfMembership(object_c)).Aggregate(1.0, (p, n) => p * n);
                        sum += mul * Rule.CF;

                    }
                    if (max_sum < sum)
                    {
                        correct_classificate = true;
                        max_sum = sum;
                        key = RulesOfOneClass.Key;
                    }
                }
                if (!correct_classificate)
                {
                    return "nonResult";
                    //throw (new ArgumentException(
                    //  "Невозможно классифицировать объект скорее всего существуют не накрытые участи на признаках"));
                }
                results[key] += 1;
            }
            string best_class = "";
            int max_class = 0;
            foreach (var Key in results.Keys)
            {
                if (results[Key] > max_class)
                {
                    best_class = Key;
                    max_class = results[Key];
                }
            }
            return best_class;
        }

        #endregion
    }
}
