using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
namespace Fuzzy_system.Approx_Singletone.learn_algorithm.conf
{
    class Term_Config_Random_Search_conf:Abstract_learn_algorithm_conf
    {
         [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.Term_Config_Random_Search_count_iteration; }
            set { Settings.Default.Term_Config_Random_Search_count_iteration = value; Settings.Default.Save(); }
        }
      
        [Description("Сколько баз правил сгенерируется за такт"), Category("Итерации")]
         public int Количество_генерируемых_баз_правил_за_итерацию
         {
             get { return Settings.Default.Term_Config_Random_Search_count_generate_by_iteration;
             }
             set { Settings.Default.Term_Config_Random_Search_count_generate_by_iteration = value;
             Settings.Default.Save();
             }
         }
    }
}
