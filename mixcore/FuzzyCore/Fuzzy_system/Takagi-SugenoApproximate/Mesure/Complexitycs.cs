using System.Linq;

namespace FuzzySystem.TakagiSugenoApproximate.Mesure
{
    public static class Complexitycs
    {
        public static double getComplexit(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0;
            if (source != null)
            {
                result += source.RulesDatabaseSet[indexDataBase].TermsSet.Count();
                result += source.RulesDatabaseSet[indexDataBase].RulesDatabase.Count();
            }
            return result;
        }

        public static double getRulesCount(this TSAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0;
            if (source != null)
            {
                result += source.RulesDatabaseSet[indexDataBase].RulesDatabase.Count();
            }
            return result;
        }
    }
}
