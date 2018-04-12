using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;

using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Config_Random_Search : Abstract_learn_algorithm
    {
        Random rand = new Random();
        int count_iteration = 0;
        int count_Rules = 0;
        private Term randomize_term(Term source)
        {
            Term result = source;
            for (int k = 0; k < Member_Function.Count_Params_For_Term(source.Term_Func_Type); k++)
            {
                
                result.Parametrs[k] = GaussRandom.Random_gaussian(rand,result.Parametrs[k],result.Parametrs[k]/10);
            }

            return result;
        }




        public override Fuzzy_system.Approx_Singletone.a_Fuzzy_System TuneUpFuzzySystem(a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf conf)
        {



            a_Fuzzy_System result = Approximate;
             count_iteration = ((Term_Config_Random_Search_conf)conf).Количество_итераций;
            count_Rules = ((Term_Config_Random_Search_conf)conf).Количество_генерируемых_баз_правил_за_итерацию;
         
            for (int i = 0; i < count_iteration; i++)
            {
                int temp_prev_count_c_Rule = result.Rulles_Database_Set.Count;
                double temp_best_result = result.approx_Learn_Samples();
                int temp_best_index = 0;

                for (int j = 0; j < count_Rules; j++)
                {


                    Knowlege_base_ARules temp_a_Rule = new Knowlege_base_ARules(result.Rulles_Database_Set[0]);
                    result.Rulles_Database_Set.Add(temp_a_Rule);
                    int temp_index = result.Rulles_Database_Set.Count - 1;
                    for (int k = 0; k < result.Rulles_Database_Set[temp_index].Terms_Set.Count; k++)
                    {
                        result.Rulles_Database_Set[temp_index].Terms_Set[k] =
                            randomize_term(result.Rulles_Database_Set[temp_index].Terms_Set[k]);
                    }
                    double[] kons = result.Rulles_Database_Set[temp_index].all_conq_of_rules;
                    for (int k = 0; k < kons.Count(); k++)
                    {
                        kons[k] =GaussRandom.Random_gaussian(rand, kons[k], kons[k] / 10); 
                    }
                    result.Rulles_Database_Set[temp_index].all_conq_of_rules = kons;
                    bool success = true;
                    double current_score = 0;
                    try
                    {
                        current_score = result.approx_Learn_Samples(temp_index);
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

                result.Rulles_Database_Set[0] = result.Rulles_Database_Set[temp_best_index];
                result.Rulles_Database_Set.RemoveRange(temp_prev_count_c_Rule, result.Rulles_Database_Set.Count - temp_prev_count_c_Rule);
            }



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
    }
}
