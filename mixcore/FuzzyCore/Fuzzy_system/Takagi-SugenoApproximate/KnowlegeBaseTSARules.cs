using System.Linq;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;
namespace FuzzySystem.TakagiSugenoApproximate
{

    public class KnowlegeBaseTSARules : KnowlegeBaseSARules
    {
        #region public Visible methods

        public override int ValueComplexity{

            get {
                return RulesDatabase.Count + TermsSet.Count; }
        }
   
     
        public override void TrimTerms()
        {
            {
                bool need_to_cut_this_term;
                for (int j = TermsSet.Count - 1; j >= 0; j--)
                {
                    need_to_cut_this_term = true;
                    for (int i = 0; i < RulesDatabase.Count; i++)
                    {
                        if (RulesDatabase[i].ListTermsInRule.Contains(TermsSet[j])) { need_to_cut_this_term = false; break; }
                    }
                    if (need_to_cut_this_term) { TermsSet.RemoveAt(j); }
                }
            }
        }

        public KnowlegeBaseTSARules(KnowlegeBaseTSARules source, List<bool> used_rules = null)
        {
            for (int i = 0; i < source.TermsSet.Count; i++)
            {
                Term temp_term = new Term(source.TermsSet[i]);
                TermsSet.Add(temp_term);
            }

            for (int j = 0; j < source.RulesDatabase.Count; j++)
            {
                if ((used_rules == null) || (used_rules[j]))
                {
                    int[] order = new int[source.RulesDatabase[j].ListTermsInRule.Count];
                    for (int k = 0; k < source.RulesDatabase[j].ListTermsInRule.Count; k++)
                    {
                        Term temp_term = source.RulesDatabase[j].ListTermsInRule[k];
                        order[k] = source.TermsSet.FindIndex(x => x == temp_term);
                    }
                    double temp_DoubleOutputs = source.RulesDatabase[j].IndependentConstantConsequent;
                    double[] temp_approx_RegressionConstantConsequent =  source.RulesDatabase[j].RegressionConstantConsequent.Clone() as double [];
                    TSARule temp_rule = new TSARule(TermsSet, order, temp_DoubleOutputs,temp_approx_RegressionConstantConsequent);
                    RulesDatabase.Add(temp_rule);
                }
            }
        }

        public KnowlegeBaseTSARules()
        {        }

        public new List<TSARule> RulesDatabase
        {
            get;
            protected set;
        } = new List<TSARule>();


        public void ConstructNewRule(List<Term>  terms,TSAFuzzySystem FS)
        {
            TSARule Result;
            int[] order_of_terms = new int[terms.Count()];
            for (int i=0; i<terms.Count();i++)
            {
                order_of_terms[i] = TermsSet.Count;
                TermsSet.Add(terms[i]);
            }
            double [] temp_regressionCoefficent;
            double kons_Value = LSMWeghtReqursiveSimple.EvaluteConsiquent(FS,TermsSet, out temp_regressionCoefficent);
            Result = new TSARule(TermsSet, order_of_terms, kons_Value, temp_regressionCoefficent);
            RulesDatabase.Add(Result);
        }


        public override double[] all_conq_of_rules
        {
            get
            {
                int sizeofConsient=0;
                for (int i =0; i< RulesDatabase.Count;i++)
                {sizeofConsient+= RulesDatabase[i].RegressionConstantConsequent.Length+1;
                }

                double[] result = new double[sizeofConsient];
                int currentIndex = 0;
                for (int i = 0; i < RulesDatabase.Count; i++)
                {
                    result[currentIndex] = RulesDatabase[i].IndependentConstantConsequent;
                    currentIndex++;
                    for (int j = 0; j < RulesDatabase[i].RegressionConstantConsequent.Length; i++)
                    {
                        result[currentIndex] = RulesDatabase[i].RegressionConstantConsequent[j];
                        currentIndex++;
                    }
                }
                return result;
            }
            set
            {
                int currentIndex = 0;
                for (int i = 0; i < RulesDatabase.Count; i++)
                {
                    RulesDatabase[i].IndependentConstantConsequent = value[currentIndex];
                    currentIndex++;
                    for (int j = 0; j < RulesDatabase[i].RegressionConstantConsequent.Length;j++ )
                    {
                        RulesDatabase[i].RegressionConstantConsequent[j] = value[currentIndex];
                        currentIndex++;
                    }
                }
            }
        }

        #endregion

    }
}
