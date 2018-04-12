using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone
{
    public class ARule : Rule
    {
        public ARule(List<Term> terms_set, int[] number_of_terms, double approx_value)
            : base(terms_set, number_of_terms)
        {

            approx_value_kons = approx_value;

        }

        private double approx_value_kons;
        public double Kons_approx_Value
        {
            get { return approx_value_kons; }
            set { approx_value_kons = value; }
        }




    }
}
