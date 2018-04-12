using System;
using System.Linq;
using System.Collections.Generic;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone
{
  


    public class Knowlege_base_ARules : Knowlege_base_Rules
    {
        #region public Visible methods


     
        public override void trim_not_used_Terms()
        {
            {
                bool need_to_cut_this_term;
                for (int j = terms_set.Count - 1; j >= 0; j--)
                {
                    need_to_cut_this_term = true;
                    for (int i = 0; i < arules_database.Count; i++)
                    {
                        if (arules_database[i].Term_of_Rule_Set.Contains(terms_set[j])) { need_to_cut_this_term = false; break; }
                    }
                    if (need_to_cut_this_term) { terms_set.RemoveAt(j); }
                }
            }

        }


        public Knowlege_base_ARules(Knowlege_base_ARules source, List<bool> used_rules = null)
        {
            for (int i = 0; i < source.Terms_Set.Count; i++)
            {

                Term temp_term = new Term(source.terms_set[i]);
                terms_set.Add(temp_term);
            }


            for (int j = 0; j < source.Rules_Database.Count; j++)
            {
                if ((used_rules == null) || (used_rules[j]))
                {
                    int[] order = new int[source.arules_database[j].Term_of_Rule_Set.Count];
                    for (int k = 0; k < source.arules_database[j].Term_of_Rule_Set.Count; k++)
                    {
                        Term temp_term = source.arules_database[j].Term_of_Rule_Set[k];
                        order[k] = source.Terms_Set.FindIndex(x => x == temp_term);
                    }
                    double temp_approx_Values = source.arules_database[j].Kons_approx_Value;
                    ARule temp_rule = new ARule(terms_set, order, temp_approx_Values);
                    arules_database.Add(temp_rule);
                }
            }
        }





        public Knowlege_base_ARules()
        {

        }



        public List<ARule> Rules_Database
        {
            get
            {
                return arules_database;

            }
        }


     

        public void constuct__and_add_the_Rule(List<Term>  terms,a_Fuzzy_System FS)
        {
            ARule Result;

            int[] order_of_terms = new int[terms.Count()];
            for (int i=0; i<terms.Count();i++)
            {
                order_of_terms[i] = terms_set.Count;
                terms_set.Add(terms[i]);
            }
            double kons_Value = FS.Nearest_Approx(terms_set);
            Result = new ARule(terms_set, order_of_terms, kons_Value);
            arules_database.Add(Result);

        }


        public double[] all_conq_of_rules
        {
            get
            {
                double[] result = new double[arules_database.Count];
                for (int i = 0; i < arules_database.Count; i++)
                {
                    result[i] = arules_database[i].Kons_approx_Value; 
                }
                return result;
            }
            set
            {
                for (int i = 0; i < value.Count(); i++)
                {
                    arules_database[i].Kons_approx_Value = value[i]; 
                }
            }
        }


        #endregion

        #region private invisible structure

       
        
        protected List<ARule> arules_database = new List<ARule>();
      


        #endregion

    }
}
