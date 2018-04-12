//#define CONTRACTS_FULL
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.TakagiSugenoApproximate
{
    public partial class TSAFuzzySystem : SAFuzzySystem
    {
        #region Visible public methods

        public new List<KnowlegeBaseTSARules> RulesDatabaseSet
        {
            get;
            protected set;
        } = new List<KnowlegeBaseTSARules>();

        public override List<KnowlegeBaseRules> AbstractRulesBase()
        {
            return RulesDatabaseSet.Cast<KnowlegeBaseRules>().ToList();
        }


        public override int ValueComplexity(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBaseTSARules)
                return ((KnowlegeBaseTSARules)Source).ValueComplexity;
            return 0;
        }
        public override int ValueRuleCount(KnowlegeBaseRules Source)
        {
            if (Source is KnowlegeBaseTSARules)
                return ((KnowlegeBaseTSARules)Source).RulesDatabase.Count;
            return 0;
        }


        #region constructor


        public TSAFuzzySystem(SampleSet learnSet, SampleSet testSet)
            : base(learnSet, testSet)
        {


        }


        public TSAFuzzySystem(TSAFuzzySystem Source)
            : base(Source)
        {

            RulesDatabaseSet = new List<KnowlegeBaseTSARules>();
            for (int i = 0; i < Source.RulesDatabaseSet.Count; i++)
            {
                RulesDatabaseSet.Add(new KnowlegeBaseTSARules(Source.RulesDatabaseSet[i]));
            }
        }

        #endregion

        public override double ErrorTestSamples(KnowlegeBaseRules Source)
        {
            KnowlegeBaseTSARules temp = Source as KnowlegeBaseTSARules;
            return approxTestSamples(temp);
        }

        public override double ErrorLearnSamples(KnowlegeBaseRules Source)
        {
            KnowlegeBaseTSARules temp = Source as KnowlegeBaseTSARules;
            return approxLearnSamples(temp);
        }



        public double approxTestSamples(KnowlegeBaseTSARules forTest)
        {
            Requires(forTest != null);
            double sum = LearnSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => Math.Pow(x.DoubleOutput - approx_base(x.InputAttributeValue, forTest), 2)).Sum();
            return (double)Math.Sqrt(sum) / (double)TestSamplesSet.CountSamples;

        }



        public double approxLearnSamples(KnowlegeBaseTSARules Solution) //attention only for multicore processor optimized 
        {
            Requires(Solution != null);
            double sum = LearnSamplesSet.DataRows.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => Math.Pow(x.DoubleOutput - approx_base(x.InputAttributeValue, Solution), 2)).Sum();
            return (double)Math.Sqrt(sum) / (double)LearnSamplesSet.CountSamples;

        }


        public double approx_base(double[] object_c, KnowlegeBaseTSARules Solution)
        {
            Requires(Solution != null);
            Requires(object_c != null);

            double sum = 0;
            double sum2 = 0;
            foreach (var Rule in Solution.RulesDatabase)
            {
                //Calc Anticedient
                double mul = Rule.ListTermsInRule.Where(x => AcceptedFeatures[x.NumVar]).Select(y => y.LevelOfMembership(object_c)).Aggregate(1.0, (p, n) => p * n);
                sum2 += mul;

                //Calc Consiquent
                double value = Rule.IndependentConstantConsequent;
                for (int i = 0; i < Rule.RegressionConstantConsequent.Length; i++)
                {
                    if (AcceptedFeatures[i] == false) { continue; }
                    value += Rule.RegressionConstantConsequent[i] * object_c[i];
                }
                //End

                sum += mul * value;
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
