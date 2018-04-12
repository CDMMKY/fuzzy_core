using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Linglib;

namespace Fuzzy_system.Approx_Singletone.add_generators
{
    class Generator_Rulles_shrink : Abstract_generator
    {
        List< List<bool>> Pull_of_systems = new List< List<bool>>();
        List<double> errors_of_systems = new List<double>();
        int start_add_rules;
        int Request_count_rules = 0;
        int max_count_rules = 0;
        int min_count_rules = 0;
        int[] count_slices = null;
        Type_Term_Func_Enum type_term;

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


        public override a_Fuzzy_System Generate(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate,Abstract_generator_conf config)
        {
            start_add_rules = Approximate.Count_Rulles_Databases;
            a_Fuzzy_System result = Approximate;
            if (result.Count_Rulles_Databases == 0)
            {

                result.Init_Rules_everyone_with_everyone(config);
            }



            

            Request_count_rules = ((Rulles_shrink_conf)config).Нужно_Правил;
            max_count_rules = ((Rulles_shrink_conf)config).Максимально_Правил;
            count_slices = ((Rulles_shrink_conf)config).Количество_термов_для_каждого_признака;
            min_count_rules= ((Rulles_shrink_conf)config).Минимально_Правил;
            type_term = ((Rulles_shrink_conf)config).Функция_принадлежности;

            int count_of_swith_off = ((Rulles_shrink_conf)config).Максимально_Правил - Request_count_rules;
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



                GC.Collect();
//            result.unlaid_protection_fix();
             return result;
            
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "отсечение правил {";
                result += "Функции принадлежности= " + Member_Function.ToString(type_term) + " ;" + Environment.NewLine;
                result += "Требуется правил =" + Request_count_rules.ToString() + " ; " + Environment.NewLine;
                for (int i = 0; i < count_slices.Count(); i++)
                {
                    result += " " + count_slices[i].ToString() + pluralform.nobot(count_slices[i], new string[3] { "терм", "терма", "термов" }) + " по " + (i + 1).ToString() + " " + " параметру ;" + Environment.NewLine;

                }
                result += "}";
                return result;
            }
            return " отсечение правил";
        }


    }
}
