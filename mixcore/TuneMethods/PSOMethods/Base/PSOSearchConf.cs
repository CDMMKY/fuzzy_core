using FuzzySystem.FuzzyAbstract.conf;
using Settings = PSOMethods.Properties.SettingsBase;
using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public class PSOSearchConf:ILearnAlgorithmConf
    {
       [DisplayName("Количество итераций")]
        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int PSOSCCountIteration
        {
            get { return Settings.Default.PSO_iter; }
            set { Settings.Default.PSO_iter = value; Settings.Default.Save(); }
        }

       [DisplayName("Особей в популяции")]    
        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int PSOSCPopulationSize
        {
            get { return Settings.Default.PSO_population; }
            set { Settings.Default.PSO_population = value; Settings.Default.Save(); }
        }


       [DisplayName("Коэффициент C1")]
        [Description("Коэффициент c1"), Category("Параметры алгоритма")]
        public double PSOSCC1
        {
            get { return Settings.Default.PSO_c1; }
            set { Settings.Default.PSO_c1 = value; Settings.Default.Save(); }
        }
      
       [DisplayName("Коэффициент C2")]
       [Description("Коэффициент c2"), Category("Параметры алгоритма")]
        public double PSOSCC2
        {
            get { return Settings.Default.PSO_c2; }
            set { Settings.Default.PSO_c2 = value; Settings.Default.Save(); }
        }

        public virtual void loadParams(string param)
        {     string[] temp = param.Split('}');

        PSOSCCountIteration = Extention.getParamValueInt(temp, "PSOSCCountIteration");
        PSOSCPopulationSize = Extention.getParamValueInt(temp, "PSOSCPopulationSize");
        PSOSCC1 = Extention.getParamValueDouble(temp, "PSOSCC1");
        PSOSCC2 = Extention.getParamValueDouble(temp, "PSOSCC2");
        }
           public void Init(int countVars)
        {        }

    }
}
