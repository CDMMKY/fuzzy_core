using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Fuzzy_system.Fuzzy_Abstract;
using Fuzzy_system.Approx_Singletone.add_generators.I_k_mean;
namespace Fuzzy_system.Approx_Singletone.add_generators
{
    class k_mean_rules_generator:Abstract_generator
    {
        Type_k_mean_algorithm type_alg;
        int count_rules = 0;
        Type_Term_Func_Enum type_func ;
        double nebulisation_factor =0;
        int Max_iteration =0;
        double need_precision =0;



        private double Calc_distance_for_member_ship_function_for_Clust(int number_cluster,int number_var,k_mean_base Alg)
        {double nominator =0;
            double denominator=0;
            for (int e=0;e<Alg.Learn_table.Count_Samples;e++)
            {nominator+=Math.Pow (Alg.U_matrix[number_cluster][e],2)*Math.Pow(Alg.Centroid_cordinate_S[number_cluster][number_var]-Alg.Learn_table.Data_Rows[e].Input_Attribute_Value[number_var],2);
            denominator+=Math.Pow (Alg.U_matrix[number_cluster][e],2);
            
            }
            return nominator/denominator;
        
            }
        


        
     
        public override a_Fuzzy_System Generate(a_Fuzzy_System Approximate, Abstract_generator_conf config)
        {
            type_alg = ((k_mean_rules_generator_conf)config).Алгоритм;
            count_rules = ((k_mean_rules_generator_conf)config).Количество_правил;
            type_func = ((k_mean_rules_generator_conf)config).Функция_принадлежности;
            nebulisation_factor = ((k_mean_rules_generator_conf)config).Экспоненциальный_вес_алгоритма;
            Max_iteration = ((k_mean_rules_generator_conf)config).Итераций;
             need_precision = ((k_mean_rules_generator_conf)config).Точность;


                k_mean_base K_Agl= null;

                switch (type_alg)
                {
                    case Type_k_mean_algorithm.Gath_geva: K_Agl = new k_mean_Gath_Geva(Approximate.Learn_Samples_set, Max_iteration,need_precision, count_rules,nebulisation_factor); break;
                    case Type_k_mean_algorithm.Gustafson_Kessel: K_Agl = new k_mean_Gustafson_kessel(Approximate.Learn_Samples_set, Max_iteration, need_precision, count_rules,nebulisation_factor); break;
                    case Type_k_mean_algorithm.FCM: K_Agl = new k_mean_base(Approximate.Learn_Samples_set, Max_iteration, need_precision, count_rules,nebulisation_factor); break;

                }
                K_Agl.Calc();
          
            Knowlege_base_ARules New_Rules= new Knowlege_base_ARules();
            for(int i=0;i<count_rules;i++)
            { int [] order_terms = new int [Approximate.Learn_Samples_set.Count_Vars];
            List<Term> term_set = new List<Term>();
                for (int j=0;j<Approximate.Learn_Samples_set.Count_Vars;j++)
            {
               Term temp_term= Term.Make_Term(K_Agl.Centroid_cordinate_S[i][j], Math.Sqrt( Calc_distance_for_member_ship_function_for_Clust(i, j, K_Agl))*3, type_func,j);
               term_set.Add(temp_term);
            }
                New_Rules.constuct__and_add_the_Rule(term_set,Approximate);
            }
          
            a_Fuzzy_System Result = Approximate;
            if (Result.Rulles_Database_Set.Count > 0)
            {
                Result.Rulles_Database_Set[0] = New_Rules;
            }
            else { Result.Rulles_Database_Set.Add (New_Rules); }
            Result.unlaid_protection_fix();
            GC.Collect();
            return Result;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "модификация k-средних {";
               result += "Тип модификции = ";
                switch(this.type_alg)
                {
                    case Type_k_mean_algorithm.FCM: result += "FCM"; break;
                    case Type_k_mean_algorithm.Gath_geva: result += "Gath-Geva"; break;
                    case Type_k_mean_algorithm.Gustafson_Kessel: result += "Guthstafson Kessel"; break; 
           
                }
                result += " ; " + Environment.NewLine;

                result += "Функции принадлежности= " + Member_Function.ToString(type_func) + " ;" + Environment.NewLine;
               
                result += "Генерируется правил= "+this.count_rules.ToString()+ " ;" + Environment.NewLine;
                 result += "Итераций = "+this.Max_iteration.ToString()+ " ;" + Environment.NewLine;
                 result += "Экспоненциальный вес = "+this.nebulisation_factor.ToString()+ " ;" + Environment.NewLine;
                   
                            
                result += "}";
                return result;
            }
            return "модификация k-средних";
        }

    }
}
