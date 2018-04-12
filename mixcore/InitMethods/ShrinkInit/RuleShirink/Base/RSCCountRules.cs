using System.ComponentModel;
using System.Linq;
using Settings = ShrinkInit.Properties.SettingsBase;


namespace FuzzySystem.FuzzyAbstract.conf
{
    public class RullesShrinkConf : InitEveryoneWithEveryone
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

        int max_count_rules;
        int min_count_rules;

        public RullesShrinkConf()
            : base()
        {

        }
        public override void Init(int countVars)
        {
            base.Init(countVars);
            int count_Rulles = count_terms.Aggregate((x1, x2) => x1 * x2);
            max_count_rules = count_Rulles;
            min_count_rules = calc_min();
        }


        [DisplayName("Нужное количество правил")]
        [Description("Нужное количество правил "), Category("Параметры НС")]
        public int RSCCountRules
        {
            get {

                int count_Rulles = count_terms.Aggregate((x1, x2) => x1 * x2);
                max_count_rules = count_Rulles;
                min_count_rules = calc_min();

                
                return Settings.Default.Pareto_simpler_Request_count_rules = Settings.Default.Pareto_simpler_Request_count_rules < max_count_rules ? Settings.Default.Pareto_simpler_Request_count_rules > min_count_rules ? Settings.Default.Pareto_simpler_Request_count_rules : min_count_rules : max_count_rules; }
            set
            {
                int count_Rulles = count_terms.Aggregate((x1, x2) => x1 * x2);
                max_count_rules = count_Rulles;
                min_count_rules = calc_min();


                Settings.Default.Pareto_simpler_Request_count_rules = value < max_count_rules ? value : max_count_rules;
                Settings.Default.Pareto_simpler_Request_count_rules = Settings.Default.Pareto_simpler_Request_count_rules < min_count_rules ? min_count_rules : Settings.Default.Pareto_simpler_Request_count_rules;
                Settings.Default.Save();
            }


        }

        [DisplayName("Минимальное количество правил")]
        [Description("Минимальное количество правил "), Category("Параметры НС")]
        public int RSCMinRules
        {
            get { return min_count_rules; }

        }

        [DisplayName("Максимальное количество правил")]
        [Description("Максимальная сложность которую вы можете указать "), Category("Параметры НС")]

        public int RSCMaxRules
        {
            get { return max_count_rules; }



        }

        public override void loadParams(string param)
{
 	 base.loadParams(param);
             string[] temp = param.Split('}');
            RSCCountRules = Extention.getParamValueInt(temp, "RSCCountRules");
            
}



    }
}