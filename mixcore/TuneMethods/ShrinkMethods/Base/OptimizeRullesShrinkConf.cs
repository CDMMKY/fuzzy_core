using System.ComponentModel;
using Settings = ShrinkMethods.Properties.SettingsBase;


namespace FuzzySystem.FuzzyAbstract.conf
{
    public class OptimizeRullesShrinkConf : ILearnAlgorithmConf
    {
        [DisplayName("Удалить правил")]
        [Description("Удалить правил "), Category("Параметры НС")]
        public int ORSCCountShrinkRules
        {
            get { return Settings.Default.Pareto_simpler_count_shrink_rules; }
            set
            {
                Settings.Default.Pareto_simpler_count_shrink_rules = value;
                Settings.Default.Save();
            }


        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            ORSCCountShrinkRules = Extention.getParamValueInt(temp, "ORSCCountShrinkRules");
        }

        public void Init(int countVars)
        {
      
        }


    }
}