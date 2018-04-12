namespace DynamicTune
{
    using System.ComponentModel;

    using FuzzySystem.FuzzyAbstract.conf;

    using Properties;
    using FuzzySystem.FuzzyAbstract;

    public class DynamicTuneConfGenerator : DynamicTuneConf, IGeneratorConf
    {
        protected TypeTermFuncEnum type_term_func;

        [DisplayName("Функция принадлежности")]
        [Description("Вид функции принадлежности"), Category("Термы")]
        public TypeTermFuncEnum IEWOTypeFunc
        {
            get { return type_term_func; }
            set
            {
                type_term_func = value;
                Settings.Default.Type_func_int = (int)value;
                Settings.Default.Save();

            }
        }
    }
}