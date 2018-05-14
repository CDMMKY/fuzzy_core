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
            get { return  Settings.Default.count_iterations; }
            set { Settings.Default.count_iterations = value; Settings.Default.Save(); }
        }

        [DisplayName("Размер популяции")]
        [Description("Размер популяции"), Category("Итерации")]
         public int TRSCCountparticles
         {
             get { return Settings.Default.count_particles; }
             set { Settings.Default.count_particles = value;  Settings.Default.Save(); }
         }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            TRSCCountIteration = Extention.getParamValueInt(temp, "TRSCCountIteration");
            TRSCCountparticles = Extention.getParamValueInt(temp, "TRSCCountRules");

        }

        public void Init(int countVars)
        {

        }
    }
}
