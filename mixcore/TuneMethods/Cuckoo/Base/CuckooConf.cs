using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = Cuckoo.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    class CuckooConf:ILearnAlgorithmConf
    {
        [DisplayName("Количество итераций")]
        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int CuckooCountIterate
        {
            get { return Settings.Default.Cuckoo_iter; }
            set { Settings.Default.Cuckoo_iter = value; Settings.Default.Save(); }
        }

        [DisplayName("Особей в популяции")]
        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int CuckooPopulationSize
        {
            get { return Settings.Default.Cuckoo_population; }
            set { Settings.Default.Cuckoo_population = value; Settings.Default.Save(); }
        }
           [DisplayName("Худшие")]
        [Description("Количество рассматриваемых худших решений"), Category("Параметры алгоритма")]
        public int CuckooWorse
        {
            get { return Settings.Default.Cuckoo_m; }
            set { Settings.Default.Cuckoo_m = value; Settings.Default.Save(); }
        }

        [DisplayName("Вероятность выжить")]
        [Description("Вероятность выжить у худшего"), Category("Параметры алгоритма")]
        public double CuckooLifeChance
        {
            get { return Settings.Default.Cuckoo_p; }
            set { Settings.Default.Cuckoo_p = value; Settings.Default.Save(); }
        }

         [DisplayName("Параметр Beta")]
        [Description("Beta"), Category("Параметры алгоритма")]
        public double CuckooBeta
        {
            get { return Settings.Default.Cuckoo_beta; }
            set { Settings.Default.Cuckoo_beta = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {

            string[] temp = param.Split('}');
            CuckooCountIterate = Extention.getParamValueInt(temp, "CuckooCountIterate");
            CuckooPopulationSize = Extention.getParamValueInt(temp, "CuckooPopulationSize");
            CuckooWorse = Extention.getParamValueInt(temp, "CuckooWorse");
            CuckooLifeChance = Extention.getParamValueDouble(temp, "CuckooLifeChance");
            CuckooBeta = Extention.getParamValueDouble(temp, "CuckooBeta");

        }
           public void Init(int countVars)
        {        }

    }
}
