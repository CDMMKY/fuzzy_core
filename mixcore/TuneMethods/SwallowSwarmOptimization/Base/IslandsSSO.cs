using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = SwallowSwarmOptimization.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    class IslandsSSO : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Основные")]
        public int Количество_итераций
        {
            get { return Settings.Default.Количество_итераций; }
            set { Settings.Default.Количество_итераций = value; Settings.Default.Save(); }
        }
        [Description("Количество популяций"), Category("Основные")]
        public int Количество_популяций
        {
            get { return Settings.Default.Количество_популяций; }
            set { Settings.Default.Количество_популяций = value; Settings.Default.Save(); }
        }
        [Description("Итераций до обмена"), Category("Острова")]
        public int Обмен
        {
            get { return Settings.Default.Итераций_до_обмена; }
            set { Settings.Default.Итераций_до_обмена = value; Settings.Default.Save(); }
        }
        [Description("Количество всех частиц"), Category("Параметры алгоритма")]
        public int Количество_всех_частиц
        {
            get { return Settings.Default.Количество_всех_частиц; }
            set { Settings.Default.Количество_всех_частиц = value; Settings.Default.Save(); }
        }

        [Description("Количество локальных лидеров"), Category("Параметры алгоритма")]
        public int Количество_лок_лидеров
        {
            get { return Settings.Default.Количество_локальных_лидеров; }
            set { Settings.Default.Количество_локальных_лидеров = value; Settings.Default.Save(); }
        }

        [Description("Количество бесцельных частиц"), Category("Параметры алгоритма")]
        public int Количество_бесц_част
        {
            get { return Settings.Default.Количество_бесцельных_частиц; }
            set { Settings.Default.Количество_бесцельных_частиц = value; Settings.Default.Save(); }
        }
        public void Init(int CountVars)
        {

        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            Количество_итераций = Extention.getParamValueInt(temp, "SSOIter");
            Количество_всех_частиц = Extention.getParamValueInt(temp, "SSOAllParts");
            Количество_лок_лидеров = Extention.getParamValueInt(temp, "SSOLocalLeaders");
            Количество_бесц_част = Extention.getParamValueInt(temp, "SSOAimlessParts");
            Количество_популяций = Extention.getParamValueInt(temp, "SSOIslands");
        }
    }
}
