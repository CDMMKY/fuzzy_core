using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system;

namespace Fuzzy_system.Fuzzy_Abstract
{
   abstract public class  Knowlege_base_Rules:Object
    {
            #region public Visible methods





       public Knowlege_base_Rules(Knowlege_base_Rules source, List<bool> used_rules = null)
       { 
       }
            


            
   public Knowlege_base_Rules()
        {

        }
       
    
        public List<Term> Terms_Set { get { return terms_set; } }

      

    public abstract void trim_not_used_Terms();
       /* {
            bool need_to_cut_this_term;
            for (int j = terms_set.Count-1; j >=0 ; j--)
            {
                need_to_cut_this_term = true;
                for (int i = 0; i < rules_database.Count; i++)
                {
                    if (rules_database[i].Term_of_Rule_Set.Contains(terms_set[j])) { need_to_cut_this_term = false; break; }
                }
                if (need_to_cut_this_term) { terms_set.RemoveAt(j); }
            }
        }

            */

      // !!!!!
    

   

        #endregion

        #region private invisible structure
        protected List<Term> terms_set = new List<Term>();

        #endregion

    }
}
