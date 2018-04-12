using System;
using System.Linq;
using System.Collections.Generic;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;
namespace Fuzzy_system.Class_Pittsburgh
{
  



    public class Knowlege_base_CRules : Knowlege_base_Rules
    {
        #region public Visible methods

        public void constuct__and_add_the_Rule(List<Term> terms, Fuzzy_System FS)
        {
            throw new NotImplementedException();
        }

        public override void trim_not_used_Terms()
        {
            {
                bool need_to_cut_this_term;
                for (int j = terms_set.Count - 1; j >= 0; j--)
                {
                    need_to_cut_this_term = true;
                    for (int i = 0; i < crules_database.Count; i++)
                    {
                        if (crules_database[i].Term_of_Rule_Set.Contains(terms_set[j])) { need_to_cut_this_term = false; break; }
                    }
                    if (need_to_cut_this_term) { terms_set.RemoveAt(j); }
                }
            }

        }


        public Knowlege_base_CRules(Knowlege_base_CRules source, List<bool> used_rules = null)
        {
            for (int i =0; i< source.Terms_Set.Count;i++)
            {
                
                Term temp_term = new Term(source.terms_set[i]);
                terms_set.Add(temp_term);
            }


            for (int j =0 ;j<source.Rules_Database.Count;j++)
            { if ( (used_rules==null) || (used_rules[j]) )
                { int [] order = new int[source.crules_database[j].Term_of_Rule_Set.Count];
                for (int k = 0; k < source.crules_database[j].Term_of_Rule_Set.Count; k++)
                {
                   Term temp_term = source.crules_database[j].Term_of_Rule_Set[k];
                    order[k] = source.Terms_Set.FindIndex(x=>x==temp_term);
                }
                string temp_class_label = source.crules_database[j].Label_of_Class;
                double temp_cf = source.crules_database[j].CF;
                CRule temp_rule = new CRule(terms_set,order,temp_class_label,temp_cf);
                crules_database.Add(temp_rule);
                }
            }
        }





        public Knowlege_base_CRules( )
        {
        }


        public List<CRule> Rules_Database
        {
            get { return crules_database; 
            
           
            }
        }

        public double[] Weigth {get { return Rules_Database.Select(rule => rule.CF).ToArray(); }

            set
            {
                for (int i=0; i<value.Count();i++)
                {
                    crules_database[i].CF = value[i];
                }

            }
        }



        #endregion

        #region private invisible structure

       

       

        protected List<CRule> crules_database = new List<CRule>();
       
   

        #endregion

    }
}
