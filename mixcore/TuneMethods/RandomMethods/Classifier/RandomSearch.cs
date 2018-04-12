using System;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class RandomSearch : AbstractNotSafeLearnAlgorithm
    {
        Random rand = new Random();
        int count_iteration;
        int count_c_Rules;
        private Term randomize_term(Term source)
        {
            Term result = source;
            for (int k = 0; k < source.CountParams; k++)
            {
               
                result.Parametrs[k] =GaussRandom.Random_gaussian(rand, source.Parametrs[k], source.Parametrs[k]/ 10);
            }

            return result;
        }

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public override  PCFuzzySystem TuneUpFuzzySystem(FuzzySystem.PittsburghClassifier.PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            RandomSearchConf Config = conf as RandomSearchConf;
             count_iteration = Config.TRSCCountIteration;
            count_c_Rules = Config.TRSCCountRules;
            PCFuzzySystem result = Classifier;
            for (int i = 0; i < count_iteration; i++)
            {
                int temp_prev_count_c_Rule = result.RulesDatabaseSet.Count;
                double temp_best_result = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                int temp_best_index = 0;

                for (int j = 0; j < count_c_Rules; j++)
                {


                    KnowlegeBasePCRules temp_c_Rule = new KnowlegeBasePCRules(result.RulesDatabaseSet[0]);
                    result.RulesDatabaseSet.Add(temp_c_Rule);
                    int temp_index = result.RulesDatabaseSet.Count - 1;
                    for (int k = 0; k < result.RulesDatabaseSet[temp_index].TermsSet.Count; k++)
                    {
                        result.RulesDatabaseSet[temp_index].TermsSet[k] =
                            randomize_term(result.RulesDatabaseSet[temp_index].TermsSet[k]);
                    }


                    bool success = true;
                    double current_score = 0;
                    try
                    {
                        current_score = result.ClassifyLearnSamples(result.RulesDatabaseSet[ temp_index]);
                    }
                    catch (Exception )
                    {
                        success = false;
                    }
                    if (success && (current_score >= temp_best_result))
                    {
                        temp_best_result = current_score;
                        temp_best_index = temp_index;
                    }


                }

                result.RulesDatabaseSet[0] = result.RulesDatabaseSet[temp_best_index];
                result.RulesDatabaseSet.RemoveRange(temp_prev_count_c_Rule, result.RulesDatabaseSet.Count - temp_prev_count_c_Rule);
            }
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "случайная оптимизация {";
                result += "Итераций =" + count_iteration.ToString() + " ; " + Environment.NewLine;

                result += "Вариантов баз правил за итерацию =" + count_c_Rules.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "случайная оптимизация";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new RandomSearchConf();
            result.Init(CountFeatures);
            return result;
        }

    }
}
