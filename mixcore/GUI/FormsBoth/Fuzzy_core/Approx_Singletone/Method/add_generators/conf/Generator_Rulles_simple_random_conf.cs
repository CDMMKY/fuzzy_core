using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;

namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    internal class Generator_Rulles_simple_random_conf : Abstract_generator_conf
    {
        public enum Stable_Term_Type
        {
            Указанный = 0,
            Случайный = 1
        }

        
        [Description("Использовать термы одинакого типа ?"), Category("Термы")]
        public Stable_Term_Type Тип_Термов { get { return (Stable_Term_Type) Settings.Default.Generator_Rulles_simples_random_stable; }
             set { Settings.Default.Generator_Rulles_simples_random_stable = (int) value; 
                Settings.Default.Save();}  }

        [Description("Вид функции принадлежности"), Category("Термы")]
        public Type_Term_Func_Enum Функция_принадлежности { get { return (Type_Term_Func_Enum) Settings.Default.Generator_Rulles_simples_random_func; } 
            set { Settings.Default.Generator_Rulles_simples_random_func = (int) value; Settings.Default.Save(); } }
        [Description("Количество генерируемых правил "), Category("Правила")]
        public int Количество_правил { get { return Settings.Default.Generator_Rulles_simples_random_сount_rules; } 
            set {Settings.Default.Generator_Rulles_simples_random_сount_rules=value ;
            Settings.Default.Save();
            }
        }
        
    }
}