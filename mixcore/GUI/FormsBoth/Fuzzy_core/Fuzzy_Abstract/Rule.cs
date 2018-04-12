using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuzzy_system.Fuzzy_Abstract
{
    public class Rule
    {
        protected List<Term> term_of_rule_set = new List<Term>();

        public List<Term> Term_of_Rule_Set
        {
            get { return term_of_rule_set; }
        }

       
        public Rule(List<Term> terms_set, int[] number_of_terms)
        {
            foreach (int i in number_of_terms)
            {
                term_of_rule_set.Add(terms_set[i]);
            }



        }

    }
}
