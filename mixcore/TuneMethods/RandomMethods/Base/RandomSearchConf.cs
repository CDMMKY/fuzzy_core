using System.ComponentModel;
using Settings = RandomMethods.Properties.SettingsBase;


namespace FuzzySystem.FuzzyAbstract.conf
{
    public class RandomSearchConf:ILearnAlgorithmConf
    {
        [DisplayName("Количество итераций")]
         [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int TRSCCountIteration
        {
            get { return  Settings.Default.Term_Config_Random_Search_count_iteration; }
            set { Settings.Default.Term_Config_Random_Search_count_iteration = value; Settings.Default.Save(); }
        }

        [DisplayName("Количество генерируемых правил")]
        [Description("Сколько баз правил сгенерируется за такт"), Category("Итерации")]
         public int TRSCCountRules
         {
             get { return Settings.Default.Term_Config_Random_Search_count_generate_by_iteration;
             }
             set { Settings.Default.Term_Config_Random_Search_count_generate_by_iteration = value;
             Settings.Default.Save();
             }
         }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            TRSCCountIteration = Extention.getParamValueInt(temp, "TRSCCountIteration");
            TRSCCountRules = Extention.getParamValueInt(temp, "TRSCCountRules");

        }

        public void Init(int countVars)
        {

        }
    }
}
