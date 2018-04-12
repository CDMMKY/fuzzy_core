using System.Linq;

namespace FuzzySystem.SingletoneApproximate.Mesure
{
    public static class Complexitycs
    {
        public static double getComplexit(this SAFuzzySystem source, int indexDataBase = 0)
        {
            double result = 0;
            if (source != null)
            {
                result += source.RulesDatabaseSet[indexDataBase].TermsSet.Count();
                result += source.RulesDatabaseSet[indexDataBase].RulesDatabase.Count();
            }
            return result;
        }

        public static double getRulesCount(this SAFuzzySystem source, int indexDataBase = 0)
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
