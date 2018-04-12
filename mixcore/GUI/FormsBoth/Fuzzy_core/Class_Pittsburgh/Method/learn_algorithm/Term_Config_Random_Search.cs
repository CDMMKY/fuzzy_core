using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Class_Pittsburgh;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh.learn_algorithm
{
    class Term_Config_Random_Search : Abstract_term_config
    {
        Random rand = new Random();

        private Term randomize_term(Term source)
        {
            Term result = source;
            for (int k = 0; k < c_Fuzzy_System.Count_Params_For_Term(source.Term_Func_Type); k++)
            {
               
                result.Parametrs[k] =GaussRandom.Random_gaussian(rand, source.Parametrs[k], source.Parametrs[k]/ 10);
            }

            return result;
        }


        public override Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System Classifier, Abstract_learn_algorithm_conf conf)
        {
            int count_iteration = ((Term_Config_Random_Search_conf)conf).Количество_итераций;
            int count_c_Rules = ((Term_Config_Random_Search_conf)conf).Количество_генерируемых_баз_правил_за_итерацию;
            c_Fuzzy_System result = Classifier;
            for (int i = 0; i < count_iteration; i++)
            {
                int temp_prev_count_c_Rule = result.Rulles_Database_Set.Count;
                double temp_best_result = result.Classify_Learn_Samples();
                int temp_best_index = 0;

                for (int j = 0; j < count_c_Rules; j++)
                {


                    Knowlege_base_CRules temp_c_Rule = new Knowlege_base_CRules(result.Rulles_Database_Set[0]);
                    result.Rulles_Database_Set.Add(temp_c_Rule);
                    int temp_index = result.Rulles_Database_Set.Count - 1;
                    for (int k = 0; k < result.Rulles_Database_Set[temp_index].Terms_Set.Count; k++)
                    {
                        result.Rulles_Database_Set[temp_index].Terms_Set[k] =
                            randomize_term(result.Rulles_Database_Set[temp_index].Terms_Set[k]);
                    }


                    bool success = true;
                    double current_score = 0;
                    try
                    {
                        current_score = result.Classify_Learn_Samples(temp_index);
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

                result.Rulles_Database_Set[0] = result.Rulles_Database_Set[temp_best_index];
                result.Rulles_Database_Set.RemoveRange(temp_prev_count_c_Rule, result.Rulles_Database_Set.Count - temp_prev_count_c_Rule);
            }

            return result;
        }
    }
}
