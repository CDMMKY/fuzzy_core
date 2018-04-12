using System.ComponentModel;
using Settings = ShrinkInit.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class TermShrinkAndRotateConf : InitEveryoneWithEveryone
    {


        int max_count_shrink_vars;
        readonly int min_count_shrink_vars = 1;



        public TermShrinkAndRotateConf()
            : base()
        {

        }
        public override void Init(int count_vars)
        {
            base.Init(count_vars);
            max_count_shrink_vars = count_vars - 1;
        }

        [DisplayName("Число параметров для уменьшения термов")]
        [Description("По скольки входным параметрам будем уменьшено количество термов "), Category("Параметры НС")]
        public int TSARCShrinkVars
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

        public int TSARCShrinkMax
        {
            get { return max_count_shrink_vars; }

        }

        [DisplayName("МЗначение уменьшения термов")]
        [Description("Насколько будет уменьшено количеств термов для каждой"), Category("Параметры НС")]

        public int TSARCShrinkTerm
        {
            get { return Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink; }
            set
            {
                Settings.Default.Term_shrink_and_rotate_conf_size_of_shrink = value;
                Settings.Default.Save();
            }


        }

        public override void loadParams(string param)
        {
            base.loadParams(param);
            
            string[] temp = param.Split('}');
            TSARCShrinkVars = Extention.getParamValueInt(temp, "TSARCShrinkVars");
            TSARCShrinkTerm = Extention.getParamValueInt(temp, "TSARCShrinkTerm");
        }


    }
}