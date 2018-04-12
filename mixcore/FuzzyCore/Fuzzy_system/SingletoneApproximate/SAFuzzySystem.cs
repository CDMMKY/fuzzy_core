//#define CONTRACTS_FULL
using System;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.SingletoneApproximate
{
    public class SAFuzzySystem : IFuzzySystem
    {
        #region Visible public methods

        public  List<KnowlegeBaseSARules> RulesDatabaseSet
        {
            get;
            protected set;
        } = new List<KnowlegeBaseSARules>();

        public override List<KnowlegeBaseRules> AbstractRulesBase()
        {
            return RulesDatabaseSet.Cast<KnowlegeBaseRules>().ToList();
        }
        public override int ValueComplexity(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBaseSARules)
                return ((KnowlegeBaseSARules)Source).ValueComplexity;
            return 0;
        }
        public override int ValueRuleCount(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBaseSARules)
                return ((KnowlegeBaseSARules)Source).RulesDatabase.Count;
            return 0;
        }

        #region constructor

        public SAFuzzySystem(SampleSet learnSet, SampleSet testSet)
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

        public SAFuzzySystem(SAFuzzySystem Source)
            : base(Source)
        {
            RulesDatabaseSet = new List<KnowlegeBaseSARules>();
            for (int i = 0; i < Source.RulesDatabaseSet.Count; i++)
            {
                RulesDatabaseSet.Add(new KnowlegeBaseSARules(Source.RulesDatabaseSet[i]));
            }
        }

        #endregion

        public override double ErrorTestSamples(KnowlegeBaseRules Source)
        {
            KnowlegeBaseSARules temp = Source as KnowlegeBaseSARules;
            return approxTestSamples(temp);
        }

        public override double ErrorLearnSamples(KnowlegeBaseRules Source)
        {
            KnowlegeBaseSARules temp = Source as KnowlegeBaseSARules;
            return approxLearnSamples(temp);
        }

        public virtual double approxTestSamples(KnowlegeBaseSARules forTest)
        {
            Requires(forTest != null);

            double sum = TestSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => Math.Pow(x.DoubleOutput - approx_base(x.InputAttributeValue, forTest), 2)).Sum();
            return (double)Math.Sqrt(sum) / (double)TestSamplesSet.CountSamples;
        }

        public virtual double approxLearnSamples(KnowlegeBaseSARules Solution) //attention only for multicore processor optimized 
        {
            Requires(Solution != null);
            double sum = LearnSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => Math.Pow(x.DoubleOutput - approx_base(x.InputAttributeValue, Solution), 2)).Sum();
            return (double)Math.Sqrt(sum) / (double)LearnSamplesSet.CountSamples;
        }

        public virtual double approx_base(double[] object_c, KnowlegeBaseSARules Solution)
        {
            Requires(object_c != null);
            Requires(Solution != null);
            double sum = 0;
            double sum2 = 0;
          foreach(var rule in Solution.RulesDatabase)
            {
                double mul = rule.ListTermsInRule.Where(x => AcceptedFeatures[x.NumVar]).Select(x=>x.LevelOfMembership(object_c)).Aggregate(1.0, (g, h) =>  g* h );
                sum2 += mul;
                sum += mul * rule.IndependentConstantConsequent;
            }
            if (sum2 != 0)
            {
                return sum / (double)sum2;
            }

            return double.MaxValue;
        }

        #endregion


      

    }
}
