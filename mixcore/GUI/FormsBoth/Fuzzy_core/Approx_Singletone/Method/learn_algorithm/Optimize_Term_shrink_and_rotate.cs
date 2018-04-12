using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Optimize_Term_shrink_and_rotate : Abstract_learn_algorithm
    {
        List< List<int>> Pull_of_systems = new List< List<int>>();
        List <Knowlege_base_ARules> Systems_ready_to_test = new List<Knowlege_base_ARules>();
        List<double> errors_of_systems = new List<double>();
        int count_shrink = 0;
        int size_shrink = 0;
  
       

     
        private void Generate_all_variant_in_pool(List<int> Bool_struct)
        {
                        int pos =0;
            do
            {Pull_of_systems.Add( new  List<int> (Bool_struct) );
                
                pos = Bool_struct.Count - 2;
                for (int i = Bool_struct.Count - 1; pos >= 0 && Bool_struct[pos] >= Bool_struct[i]; i--) pos--; 
                int j = Bool_struct.Count - 1; 
                while (pos >= 0 && Bool_struct[pos] >= Bool_struct[j]) j--;
                //j++;
                if (pos >= 0)
                {
                    int temp = Bool_struct[pos];
                    Bool_struct[pos] = Bool_struct[j];

                    Bool_struct[j] = temp;
                 
                }
                int l = pos + 1, r = Bool_struct.Count - 1;
                while (l < r)
                {
                    int temp = Bool_struct[l];
                  
                    Bool_struct[l]=Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }


        public override a_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf config)
        {
            a_Fuzzy_System result = Approximate;
            if (result.Count_Rulles_Databases == 0)
            {
                throw new System.FormatException("Что то не то с входными данными");
            }
            Optimize_Term_shrink_and_rotate_conf Config = config as Optimize_Term_shrink_and_rotate_conf;
            count_shrink = Config.Число_параметров_для_уменьшения_термов;
            size_shrink = Config.Значение_уменьшения_термов;



            

            List <int> Varians_of_run_system = new List<int>();
            for (int i=0; i<Approximate.Count_Vars;i++)
             {
                 int count_terms_for_var = Approximate.Rulles_Database_Set[0].Terms_Set.FindAll(x => x.Number_of_Input_Var == i).Count;
                 if (i < count_shrink)
                 {
                     Varians_of_run_system.Add(count_terms_for_var-size_shrink);
                 }
                 else { Varians_of_run_system.Add(count_terms_for_var); }
                }

            Varians_of_run_system.Sort();
           Type_Term_Func_Enum type_of_term =  Approximate.Rulles_Database_Set[0].Terms_Set[0].Term_Func_Type;
            Generate_all_variant_in_pool(Varians_of_run_system);
            
            for (int i = 0; i < Pull_of_systems.Count;i++ )
            {
                Approximate.Rulles_Database_Set.Clear();
                
                Approximate.Init_Rules_everyone_with_everyone(type_of_term,Pull_of_systems[i].ToArray());
                Systems_ready_to_test.Add(Approximate.Rulles_Database_Set[0]);
                errors_of_systems.Add(result.approx_Learn_Samples(0));


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
           result.Rulles_Database_Set.Clear();
           result.Rulles_Database_Set.Add(Systems_ready_to_test[best_index]);
                Console.WriteLine(Pull_of_systems.Count());
         
          


//            result.unlaid_protection_fix();
             return result;
            
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение правил {";
                result += "По скольким параметрам уменшать  =" + this.count_shrink.ToString() + " ; " + Environment.NewLine;
                result += "На сколько уменьшать термов =" + this.size_shrink.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "отсечение правил";
        }

    }
}
