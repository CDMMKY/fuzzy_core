using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Mix_core.Forms;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    class init_everyone_with_everyone:Abstract_generator_conf
    {   protected Type_Term_Func_Enum type_term_func;
        protected int[] count_terms;
   


         [Description("Вид функции принадлежности"), Category("Термы")]
        public Type_Term_Func_Enum Функция_принадлежности { get { return type_term_func; } 
            set { type_term_func = value;
           
                Settings.Default.init_everyone_with_everyone_func = (int) value;
                Settings.Default.Save();
               
            }  
        }
        public init_everyone_with_everyone(int count_vars)
        {
            type_term_func = (Type_Term_Func_Enum)Settings.Default.init_everyone_with_everyone_func;
            count_terms = new int[count_vars];
            for (int i = 0; i < count_vars;i++ )
            {
                count_terms[i] = Settings.Default.init_everyone_with_everyone_agv_count_terms;
            }
            Количество_термов_для_каждого_признака = count_terms;
        }
         [Description("Количество термов на каждом для каждого признака"), Category("Термы")]
        public int[] Количество_термов_для_каждого_признака
        {
            get { return count_terms; }
            set
            {
                
                count_terms = value;
                Settings.Default.init_everyone_with_everyone_agv_count_terms = value.Sum() / value.Count<int>();
                Settings.Default.Save();
            }
    }
    }
}
