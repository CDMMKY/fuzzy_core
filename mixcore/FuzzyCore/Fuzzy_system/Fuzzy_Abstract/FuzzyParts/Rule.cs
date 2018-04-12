namespace FuzzySystem.FuzzyAbstract
{
    /// <summary>
    /// Абстрактный класс описывающий объекта "правило"
    /// </summary>
    public class Rule
    {/// <summary>
     /// Колекция термов в правиле. 
     /// </summary>
        public TermSetInRule<Term> ListTermsInRule
        {
            get;
            protected set;
        } = new TermSetInRule<Term>();


        /// <summary>
        /// Конструктор правила
        /// </summary>
        /// <param name="TermsSet">Колекция всех термов в базе правил</param>
        /// <param name="NumOfTerms">Номера термов используемых в этом правиле из TermsSet</param>
        public Rule(TermSetGlobal<Term> TermsSet, int[] NumOfTerms)
        {
            foreach (int i in NumOfTerms)
            {
                if (i != -1)
                    ListTermsInRule.Add(TermsSet[i]);
            }

            TermsSet.AddDependencyRule(ListTermsInRule);
            ListTermsInRule.AddTermSetGlobal(TermsSet);
        }

    }
}
