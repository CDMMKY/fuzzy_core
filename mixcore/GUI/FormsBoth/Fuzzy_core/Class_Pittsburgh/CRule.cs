using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh
{
    public class CRule : Rule
    {
        /*   private List< Term> term_of_rule_set = new List<Term>();
           public List<Term> Term_of_Rule_Set
           {
               get { return term_of_rule_set; }
           } */
        public CRule(List<Term> terms_set, int[] number_of_terms, string class_label, double CF_parms)
            : base(terms_set, number_of_terms)
        {
            cF = CF_parms;
            label_of_class = class_label;

        }

        private string label_of_class;
        public string Label_of_Class
        {
            get { return label_of_class; }
        }

        private double cF;
        public double CF
        {
            get { return cF; }
            set { cF = value; }
        }


    }

}
