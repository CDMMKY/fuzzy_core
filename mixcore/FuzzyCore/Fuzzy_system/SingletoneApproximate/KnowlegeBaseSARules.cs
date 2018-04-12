using System.Linq;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.SingletoneApproximate
{
    public class KnowlegeBaseSARules : KnowlegeBaseRules
    {
        #region public Visible methods

        public virtual int ValueComplexity{
            get {
                Requires(RulesDatabase != null);
                Requires(TermsSet != null);
                return RulesDatabase.Count + TermsSet.Count; }
        }
   
     
        public override void TrimTerms()
        {
            {
                bool need_to_cut_this_term;
                for (int j = TermsSet .Count - 1; j >= 0; j--)
                {
                    need_to_cut_this_term = true;
                    for (int i = 0; i <  RulesDatabase.Count; i++)
                    {
                        if (RulesDatabase[i].ListTermsInRule.Contains(TermsSet[j])) { need_to_cut_this_term = false; break; }
                    }
                    if (need_to_cut_this_term) { TermsSet.RemoveAt(j); }
                }
            }
        }


        public KnowlegeBaseSARules(KnowlegeBaseSARules source, List<bool> used_rules = null)
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
                    SARule temp_rule = new SARule(TermsSet, order, temp_DoubleOutputs);
                    RulesDatabase.Add(temp_rule);
                }
            }
        }
        public KnowlegeBaseSARules()
        {
        }

        public List<SARule> RulesDatabase
        {
            get;
            protected set;        } = new List<SARule>();


        public void constuct__and_add_the_Rule(List<Term>  terms,SAFuzzySystem FS)
        {
            SARule Result;
            int[] order_of_terms = new int[terms.Count()];
            for (int i=0; i<terms.Count();i++)
            {
                order_of_terms[i] = TermsSet.Count;
                TermsSet.Add(terms[i]);
            }
            double kons_Value = KNNConsequent.NearestApprox(FS,TermsSet );
            Result = new SARule(TermsSet, order_of_terms, kons_Value);
            RulesDatabase.Add(Result);
        }


        public virtual double[] all_conq_of_rules
        {
            get
            {
                double[] result = new double[RulesDatabase.Count];
                for (int i = 0; i < RulesDatabase.Count; i++)
                {
                    result[i] = RulesDatabase[i].IndependentConstantConsequent; 
                }
                return result;
            }
            set
            {
                for (int i = 0; i < value.Count(); i++)
                {
                    RulesDatabase[i].IndependentConstantConsequent = value[i]; 
                }
            }
        }

        #endregion

    }
}
