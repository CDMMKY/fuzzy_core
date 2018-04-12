using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Optimize_Rulles_shrink : Abstract_learn_algorithm
    {
        List< List<bool>> Pull_of_systems = new List< List<bool>>();
        List<double> errors_of_systems = new List<double>();
        int start_add_rules;
        int Request_count_rules = 0;
        int max_count_rules = 0;
        int min_count_rules = 0;
     
    
        public static bool BytetoBool(byte value)
        {
            return !value.Equals(0); 
        }

       

        private void Generate_all_variant_in_pool(List<byte> Bool_struct)
        {
                        int pos =0;
            do
            {Pull_of_systems.Add( Bool_struct.ConvertAll<bool>(BytetoBool) );
                
                pos = Bool_struct.Count - 2;
                for (int i = Bool_struct.Count - 1; pos >= 0 && Bool_struct[pos] >= Bool_struct[i]; i--) pos--; 
                int j = Bool_struct.Count - 1; 
                while (pos >= 0 && Bool_struct[pos] >= Bool_struct[j]) j--;
                //j++;
                if (pos >= 0)
                {
                    byte temp = Bool_struct[pos];
                    Bool_struct[pos] = Bool_struct[j];

                    Bool_struct[j] = temp;
                 
                }
                int l = pos + 1, r = Bool_struct.Count - 1;
                while (l < r)
                {
                    byte temp = Bool_struct[l];
                  
                    Bool_struct[l]=Bool_struct[r];
                    Bool_struct[r] = temp;
                    l++;
                    r--;
                }
            } while (pos >= 0);




        }


        public override a_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf config)
        {
            start_add_rules = Approximate.Count_Rulles_Databases;
            a_Fuzzy_System result = Approximate;
            if (result.Count_Rulles_Databases == 0)
            {

                throw new System.FormatException("Что то не то с входными данными");
            }




            Optimize_Rulles_shrink_conf Config = config as Optimize_Rulles_shrink_conf;
            Request_count_rules = ((Rulles_shrink_conf)config).Нужно_Правил;
            max_count_rules = ((Rulles_shrink_conf)config).Максимально_Правил;
            min_count_rules = ((Rulles_shrink_conf)config).Минимально_Правил;
       
            int count_of_swith_off = Config.Максимально_Правил - Request_count_rules;
            List <byte> Varians_of_run_system = new List<byte>();
            for (int i=0; i<Approximate.Rulles_Database_Set[0].Rules_Database.Count;i++)
            {
                Varians_of_run_system.Add(1);
            }
              for (int i=0; i<count_of_swith_off;i++)
            {
                Varians_of_run_system[i]=0;
            }
            Generate_all_variant_in_pool(Varians_of_run_system);
            for (int i = 0; i < Pull_of_systems.Count;i++ )
            {
                Knowlege_base_ARules temp_rules = new  Knowlege_base_ARules(result.Rulles_Database_Set[0],Pull_of_systems[i]);
                temp_rules.trim_not_used_Terms();
                
                result.Rulles_Database_Set.Add(temp_rules);
                result.unlaid_protection_fix(start_add_rules + i);
                errors_of_systems.Add(result.approx_Learn_Samples(start_add_rules+i));


            }

            int best_index = errors_of_systems.IndexOf(errors_of_systems.Min());
           Knowlege_base_ARules best = result.Rulles_Database_Set[start_add_rules + best_index];
           result.Rulles_Database_Set.Clear();
           result.Rulles_Database_Set.Add(best);
                Console.WriteLine(Pull_of_systems.Count());
         
          


//            result.unlaid_protection_fix();
             return result;
            
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение правил {";
                result += "Требуется правил =" + Request_count_rules.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "отсечение правил";
        }

    }
}
