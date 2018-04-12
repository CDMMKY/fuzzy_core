using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier
{
    public class PCRule : Rule
    {
        public PCRule(TermSetGlobal<Term> terms_set, int[] number_of_terms, string class_label, double CF_parms=1.0)
            : base(terms_set, number_of_terms)
        {
            cF = CF_parms;
            LabelOfClass = class_label;
        }

        public string LabelOfClass
        {
            get; set;
        }

        private double cF;
        public double CF
        {
            get { return cF; }
            set { cF = value; }
        }
    }

}
