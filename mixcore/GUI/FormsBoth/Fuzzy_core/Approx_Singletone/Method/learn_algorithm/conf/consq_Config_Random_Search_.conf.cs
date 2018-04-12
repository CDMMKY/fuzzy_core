using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm.conf
{
    class consq_Config_Random_Search_conf : Abstract_learn_algorithm_conf
    {

        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.consq_Config_Random_Search_count_iteration; }
            set
            {
                Settings.Default.consq_Config_Random_Search_count_iteration = value;
                Settings.Default.Save();
            }
        }

        [Description("Сколько сгенерируется векторов весов за такт "), Category("Итерации")]
        public int Количество_генерируемых_векторов_веса_за_итерацию
        {
            get
            {
                return Settings.Default.consq_Config_Random_Search_count_generate_by_iteration;
            }
            set
            {
                Settings.Default.consq_Config_Random_Search_count_generate_by_iteration = value;
                Settings.Default.Save();
            }
        }


    }
}
