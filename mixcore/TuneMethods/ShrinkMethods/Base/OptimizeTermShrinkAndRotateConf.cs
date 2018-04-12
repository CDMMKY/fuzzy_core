using System.ComponentModel;
using Settings = ShrinkMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class OptimizeTermShrinkAndRotateConf : ILearnAlgorithmConf
    {

     
        int max_count_shrink_vars;
        readonly int min_count_shrink_vars=1;
     
        
        public OptimizeTermShrinkAndRotateConf()
    {
    }

    [DisplayName("Число параметров для уменьшения термов")]
        [Description("По скольки входным параметрам будем уменьшено количество термов "), Category("Параметры НС")]
        public int OTSARCountShrinkVars
        {
            get { return Settings.Default.Term_shrink_and_rotate_conf_count_shrink = Settings.Default.Term_shrink_and_rotate_conf_count_shrink < max_count_shrink_vars ? Settings.Default.Term_shrink_and_rotate_conf_count_shrink > min_count_shrink_vars ? Settings.Default.Term_shrink_and_rotate_conf_count_shrink : min_count_shrink_vars : max_count_shrink_vars; }
            set
            {
                Settings.Default.Term_shrink_and_rotate_conf_count_shrink = value < max_count_shrink_vars ? value : max_count_shrink_vars;
                Settings.Default.Term_shrink_and_rotate_conf_count_shrink = Settings.Default.Term_shrink_and_rotate_conf_count_shrink < min_count_shrink_vars ? min_count_shrink_vars : Settings.Default.Term_shrink_and_rotate_conf_count_shrink;
                Settings.Default.Save();
            }
    

        }

        [DisplayName("Максимально параметров для уменьшения термов")]
        [Description("Максимальная количество параметров которое вы можете уменьшить "), Category("Параметры НС")]

        public int OTSARMaxCountShrinkTerm
        {
            get { return max_count_shrink_vars; }



        }

        [DisplayName("Значение уменьшения термов")]
                [Description("Насколько будет уменьшено количеств термов для каждой"), Category("Параметры НС")]

        public int OTSARCountShrinkTerm
        {
            get { return Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink; }
            set
            {
                Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink=value;
                Settings.Default.Save();
            }


        }

                public void loadParams(string param)
                {
                    string[] temp = param.Split('}');
                    OTSARCountShrinkVars = Extention.getParamValueInt(temp, "OTSARCountShrinkVars");
                    OTSARCountShrinkTerm = Extention.getParamValueInt(temp, "OTSARCountShrinkTerm");
                }

                public void Init(int countVars)
                {
                    max_count_shrink_vars = countVars - 1;
    
                }

    }
}