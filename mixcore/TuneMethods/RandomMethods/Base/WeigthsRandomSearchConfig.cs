using System.ComponentModel;
using Settings = RandomMethods.Properties.SettingsBase;
using FuzzySystem.FuzzyAbstract.conf;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf
{
    class WeigthsRandomSearchConfig : ILearnAlgorithmConf
    {
        [DisplayName("Количество итераций")]
        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int WRSCCountIteration
        {
            get { return Settings.Default.Weigths_Config_Random_Search_count_iteration; }
            set
            {
                Settings.Default.Weigths_Config_Random_Search_count_iteration = value;
                Settings.Default.Save();
            }
        }

        [DisplayName("Количество генерируемых правил")]
        [Description("Сколько сгенерируется векторов весов за такт "), Category("Итерации")]
        public int WRSCCountRules
        {
            get
            {
                return Settings.Default.Weigths_Config_Random_Search_count_generate_by_iteration;
            }
            set
            {
                Settings.Default.Weigths_Config_Random_Search_count_generate_by_iteration = value;
                Settings.Default.Save();
            }
        }


        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            WRSCCountIteration = Extention.getParamValueInt(temp, "WRSCCountIteration");
            WRSCCountRules = Extention.getParamValueInt(temp, "WRSCCountRules");
        }

        public void Init(int countVars)
        {

        }

    }
}
