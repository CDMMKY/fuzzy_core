using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate
{
    public class SARule : Rule
    {
        public SARule(TermSetGlobal<Term> terms_set, int[] number_of_terms, double DoubleOutput)
            : base(terms_set, number_of_terms)
        {
            IndependentConstantConsequent = DoubleOutput;
        }

        public double IndependentConstantConsequent
        {
            get; set;
        }
    }
}
