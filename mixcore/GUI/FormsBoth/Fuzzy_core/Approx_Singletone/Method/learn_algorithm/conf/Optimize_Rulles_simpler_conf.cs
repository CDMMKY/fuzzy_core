using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Mix_core.Forms;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm.conf
{
    internal class Optimize_Rulles_shrink_conf : Abstract_learn_algorithm_conf
    {

        static long Factorial(int n)
        {
            if (n == 0)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        private int calc_min()
        {
            long variants = 1;
            int result = max_count_rules;
            for (int i = max_count_rules; (variants / Factorial(max_count_rules - i) < 350000) && (i > 0); i--)
            {
                variants *= i;
                result = i - 1;
            }
            return result;

        }

        readonly int max_count_rules;
        int min_count_rules;







        public Optimize_Rulles_shrink_conf(int count_Rulles)
        {
            max_count_rules = count_Rulles;
            min_count_rules = calc_min();
        }


        [Description("Нужное количество правил "), Category("Параметры НС")]
        public int Нужно_Правил
        {
            get { return Settings.Default.Pareto_simpler_Request_count_rules = Settings.Default.Pareto_simpler_Request_count_rules < max_count_rules ? Settings.Default.Pareto_simpler_Request_count_rules > min_count_rules ? Settings.Default.Pareto_simpler_Request_count_rules : min_count_rules : max_count_rules; }
            set
            {
                Settings.Default.Pareto_simpler_Request_count_rules = value < max_count_rules ? value : max_count_rules;
                Settings.Default.Pareto_simpler_Request_count_rules = Settings.Default.Pareto_simpler_Request_count_rules < min_count_rules ? min_count_rules : Settings.Default.Pareto_simpler_Request_count_rules;
                Settings.Default.Save();
            }


        }

        [Description("Минимальное количество правил "), Category("Параметры НС")]
        public int Минимально_Правил
        {
            get { return min_count_rules; }



        }
        [Description("Максимальная сложность которую вы можете указать "), Category("Параметры НС")]

        public int Максимально_Правил
        {
            get { return max_count_rules; }



        }




    }
}