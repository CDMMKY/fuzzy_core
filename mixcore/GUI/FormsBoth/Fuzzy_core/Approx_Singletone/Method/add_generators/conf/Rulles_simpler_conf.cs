using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Mix_core.Forms;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;

namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    internal class Rulles_shrink_conf : init_everyone_with_everyone
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







        public Rulles_shrink_conf(int count_vars):base(count_vars)
        {
            int count_Rulles = count_terms.Aggregate((x1, x2) => x1 * x2);
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