using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate;

namespace FuzzySystem.TakagiSugenoApproximate
{
    public class TSARule : SARule
    {
        public TSARule(TermSetGlobal<Term> terms_set, int[] number_of_terms, double independentConstantConsequent, double[] regressionConstantConsequent)
            : base(terms_set, number_of_terms, independentConstantConsequent)
        {
            RegressionConstantConsequent = regressionConstantConsequent;
        }



      

        double[] regressionConstantConsequent;
        public double[] RegressionConstantConsequent
        {
            get { return regressionConstantConsequent; }
            set { regressionConstantConsequent = value; }
        }




    }
}
