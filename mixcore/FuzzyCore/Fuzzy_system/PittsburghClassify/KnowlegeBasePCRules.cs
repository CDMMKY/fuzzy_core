using System.Linq;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.PittsburghClassifier
{
    public class KnowlegeBasePCRules : KnowlegeBaseRules
    {
        #region public Visible methods

        public void ConstructNewRule(List<Term> terms, PCFuzzySystem FS)
        {
            PCRule Result;
            int[] order_of_terms = new int[terms.Count()];
            for (int i = 0; i < terms.Count(); i++)
            {
                order_of_terms[i] =TermsSet.Count;
                TermsSet.Add(terms[i]);
            }
            string kons_Value = KNNClassName.NearestClass (FS,TermsSet);
            Result = new PCRule(TermsSet, order_of_terms, kons_Value,1.0);
            RulesDatabase.Add(Result);
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
                        if (RulesDatabase[i].ListTermsInRule.Contains(TermsSet [j])) { need_to_cut_this_term = false; break; }
                    }
                    if (need_to_cut_this_term) { TermsSet.RemoveAt(j); }
                }
            }
        }

        public KnowlegeBasePCRules(KnowlegeBasePCRules source, List<bool> used_rules = null)
        {
            for (int i =0; i< source.TermsSet.Count;i++)
            {
                Term temp_term = new Term(source.TermsSet[i]);
                TermsSet.Add(temp_term);
            }
            for (int j =0 ;j<source.RulesDatabase.Count;j++)
            { if ( (used_rules==null) || (used_rules[j]) )
                { int [] order = new int[source.RulesDatabase[j].ListTermsInRule.Count];
                for (int k = 0; k < source.RulesDatabase[j].ListTermsInRule.Count; k++)
                {
                   Term temp_term = source.RulesDatabase[j].ListTermsInRule[k];
                    order[k] = source.TermsSet.FindIndex(x=>x==temp_term);
                }
                string temp_class_label = source.RulesDatabase[j].LabelOfClass;
                double temp_cf = source.RulesDatabase[j].CF;
                PCRule temp_rule = new PCRule(TermsSet,order,temp_class_label,temp_cf);
                RulesDatabase.Add(temp_rule);
                }
            }
        }

        public KnowlegeBasePCRules( )
        {
        }


        public List<PCRule> RulesDatabase
        {
            get;
            protected set;
        } = new List<PCRule>();

        public double[] Weigths {get { return RulesDatabase.Select(rule => rule.CF).ToArray(); }

            set
            {
                for (int i=0; i<value.Count();i++)
                {
                    RulesDatabase[i].CF = value[i];
                }

            }
        }

        public  int ValueComplexity{

            get {
                Requires(RulesDatabase != null);
                Requires(TermsSet != null);
                return RulesDatabase.Count + TermsSet.Count; }
        }


        #endregion


    }
}
