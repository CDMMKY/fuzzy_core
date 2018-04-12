using System.Linq;

namespace FuzzySystem.PittsburghClassifier.Mesure
{
    public static class Complexitycs
    {
        public static double getComplexit(this PCFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0.0;
            if (source != null)
            {
                result += source.RulesDatabaseSet[indexDataBase].TermsSet.Count();
                result += source.RulesDatabaseSet[indexDataBase].RulesDatabase.Count();
            }
            return result;
        }

        public static double getRulesCount(this PCFuzzySystem source, int indexDataBase = 0)
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
