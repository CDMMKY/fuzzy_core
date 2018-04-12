using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class ConfigRandomSearch : AbstractNotSafeLearnAlgorithm
    {
        Random rand = new Random();
        int count_iteration = 0;
        int count_Rules = 0;
        private Term randomize_term(Term source)
        {
            Term result = source;
            for (int k = 0; k < source.CountParams; k++)
            {
                
                result.Parametrs[k] = GaussRandom.Random_gaussian(rand,result.Parametrs[k],result.Parametrs[k]/10);
            }

            return result;
        }


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }


        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {



            SAFuzzySystem result = Approximate;
             count_iteration = ((RandomSearchConf)conf).TRSCCountIteration;
            count_Rules = ((RandomSearchConf)conf).TRSCCountRules;
         
            for (int i = 0; i < count_iteration; i++)
            {
                int temp_prev_count_c_Rule = result.RulesDatabaseSet.Count;
                double temp_best_result = result.approxLearnSamples(result.RulesDatabaseSet[0]);
                int temp_best_index = 0;

                for (int j = 0; j < count_Rules; j++)
                {


                    KnowlegeBaseSARules temp_a_Rule = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);
                    result.RulesDatabaseSet.Add(temp_a_Rule);
                    int temp_index = result.RulesDatabaseSet.Count - 1;
                    for (int k = 0; k < result.RulesDatabaseSet[temp_index].TermsSet.Count; k++)
                    {
                        result.RulesDatabaseSet[temp_index].TermsSet[k] =
                            randomize_term(result.RulesDatabaseSet[temp_index].TermsSet[k]);
                    }
                    double[] kons = result.RulesDatabaseSet[temp_index].all_conq_of_rules;
                    for (int k = 0; k < kons.Count(); k++)
                    {
                        kons[k] =GaussRandom.Random_gaussian(rand, kons[k], kons[k] / 10); 
                    }
                    result.RulesDatabaseSet[temp_index].all_conq_of_rules = kons;
                    bool success = true;
                    double current_score = 0;
                    try
                    {
                        current_score = result.approxLearnSamples(result.RulesDatabaseSet[ temp_index]);
                    }
                    catch (Exception)
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
            GC.Collect();
            return result; 
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "случайная оптимизация {";
                result += "Итераций =" + count_iteration.ToString() + " ; " + Environment.NewLine;

                result += "Вариантов баз правил за итерацию =" + count_Rules.ToString() + " ; " + Environment.NewLine;
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
