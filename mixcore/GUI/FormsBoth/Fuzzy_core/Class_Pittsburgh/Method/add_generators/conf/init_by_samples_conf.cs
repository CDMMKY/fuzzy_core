using Fuzzy_system;
using System.ComponentModel;
using Mix_core.Properties;


namespace Fuzzy_system.Approx_Singletone.init.conf
{
    class init_by_samples_conf
    {
        private Type_Term_Func_Enum type_term_func;





         [Description("Вид функции принадлежности"), Category("Правила")]
        public Type_Term_Func_Enum Функция_принадлежности { get { return type_term_func; } 
            set { type_term_func = value;
           
                Settings.Default.init_by_samples_conf_Type_func = (int) value; 
               
            }  
        }
        public init_by_samples_conf()
        {
            type_term_func = (Type_Term_Func_Enum)Settings.Default.init_by_samples_conf_Type_func;
        }
    }
}
