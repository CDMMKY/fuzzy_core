namespace FuzzySystem.FuzzyAbstract
{
    public class Rule
    {
        protected TermSetInRule<Term> term_of_rule_set = new TermSetInRule<Term>();

        public TermSetInRule<Term> Term_of_Rule_Set
        {
            get { return term_of_rule_set; }
        }



        public Rule(TermSetGlobal<Term> terms_set, int[] number_of_terms)
        {
            foreach (int i in number_of_terms)
            { if (i!=-1)
                term_of_rule_set.Add(terms_set[i]);
            }
            
            terms_set.AddDependencyRule(term_of_rule_set);
            term_of_rule_set.AddTermSetGlobal(terms_set);


        }

        public Rule()
        { }


       

    }
}
