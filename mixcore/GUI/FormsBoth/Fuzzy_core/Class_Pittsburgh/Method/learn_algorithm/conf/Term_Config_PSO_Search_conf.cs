using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
namespace Fuzzy_system.Approx_Singletone.learn_algorithm.conf
{
    class CP_Term_Config_PSO_Search_conf:Abstract_learn_algorithm_conf
    {
        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.PSO_iter; }
            set { Settings.Default.PSO_iter = value; Settings.Default.Save(); }
        }

        [Description("Сколько баз правил сгенерируется за такт"), Category("Итерации")]
        public int Количество_генерируемых_баз_правил_за_итерацию
        {
            get
            {
                return Settings.Default.Term_Config_Random_Search_count_generate_by_iteration;
            }
            set
            {
                Settings.Default.Term_Config_Random_Search_count_generate_by_iteration = value;
                Settings.Default.Save();
            }
        }
        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int Особей_в_популяции
        {
            get { return Settings.Default.PSO_population; }
            set { Settings.Default.PSO_population = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент c1"), Category("Параметры алгоритма")]
        public double Коэффициент_c1
        {
            get { return Settings.Default.PSO_c1; }
            set { Settings.Default.PSO_c1 = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент c2"), Category("Параметры алгоритма")]
        public double Коэффициент_c2
        {
            get { return Settings.Default.PSO_c2; }
            set { Settings.Default.PSO_c2 = value; Settings.Default.Save(); }
        }
    }
}
