using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Adaptive_LSM : Abstract_learn_algorithm
    {
        private double[, ,] extract_Rules(Knowlege_base_ARules rules_Database)
        { 
            int count_rules= rules_Database.Rules_Database.Count;
            int count_vars = rules_Database.Rules_Database[0].Term_of_Rule_Set.Count;
            int count_term_params =Member_Function.Count_Params_For_Term(rules_Database.Terms_Set[0].Term_Func_Type);

            double [,,] Result = new double[count_rules,count_vars,count_term_params];

            for (int i = 0; i < count_rules; i++)
            {
                for (int j = 0; j < count_vars; j++)
                {

                    Term temp_term = rules_Database.Rules_Database[i].Term_of_Rule_Set.First(x => x.Number_of_Input_Var == j);
                    for (int k = 0; k < count_term_params; k++)
                    {
                        Result[i, j, k] = temp_term.Parametrs[k];
                    }
                }
            }

            return Result;
        }
        private double[,] extract_Sample_table(a_samples_set learn_Set)
        {
            double[,] Result = new double[learn_Set.Count_Samples, learn_Set.Count_Vars];
            for (int i = 0; i < learn_Set.Count_Samples; i++)
            {
                for (int j = 0; j < learn_Set.Count_Vars; j++)
                {
                    Result[i, j] = learn_Set.Data_Rows[i].Input_Attribute_Value[j]; 
                }
            }
            return Result;

        }

       

        private double[] extract_Sample_table_Out(a_samples_set learn_Set)
        {
            double[] Result = new double[learn_Set.Count_Samples];
            for (int i = 0; i < learn_Set.Count_Samples; i++)
            {
                Result[i] = learn_Set.Data_Rows[i].Approx_Value; 
            }
            return Result;
        }

        public override Fuzzy_system.Approx_Singletone.a_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf conf)
        {

            Mnk_lib.Mnk_class Mnk_me = new Mnk_lib.Mnk_class();
           
            double [,,] Extracted_rules= extract_Rules(Approximate.Rulles_Database_Set[0]);
            double [,] Extracted_Samples = extract_Sample_table(Approximate.Learn_Samples_set);
            double [] Extracted_Samples_out = extract_Sample_table_Out(Approximate.Learn_Samples_set);
            int count_rules= Approximate.Count_Rules();
            int count_samples = Approximate.Learn_Samples_set.Count_Samples;
            int count_Vars = Approximate.Learn_Samples_set.Count_Vars;
            double [] New_consq = new double[count_rules];
            Type_Term_Func_Enum type_Func= Approximate.Rulles_Database_Set[0].Terms_Set[0].Term_Func_Type;
            int type_func= (int) type_Func;
            
            Mnk_me.mnk_R(Extracted_rules,count_rules,type_func,Extracted_Samples,Extracted_Samples_out,count_samples,count_Vars,out New_consq);
           
            a_Fuzzy_System Result = Approximate;
  
            double result_before = Result.approx_Learn_Samples(0);
           double [] Back_consq  = Result.Rulles_Database_Set[0].all_conq_of_rules;
            Result.Rulles_Database_Set[0].all_conq_of_rules = New_consq;
            double result_after =  Result.approx_Learn_Samples(0);
            if (result_before<result_after)
            { Result.Rulles_Database_Set[0].all_conq_of_rules = Back_consq;
            }
            GC.Collect();
            return Result;
          
                 }
        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "адаптивный МНК {";
                result += "}";
                return result;
            }
            return "адаптивный МНК";
        }


    }
}
