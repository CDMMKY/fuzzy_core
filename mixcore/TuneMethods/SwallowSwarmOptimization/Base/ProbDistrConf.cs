using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = SwallowSwarmOptimization.Properties.Settings;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ProbDistrConf : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.Количество_итераций; }
            set { Settings.Default.Количество_итераций = value; Settings.Default.Save(); }
        }

        [Description("Количетсво всех частиц"), Category("Параметры алгоритма")]
        public int Количество_всех_частиц
        {
            get { return Settings.Default.Количество_всех_частиц; }
            set { Settings.Default.Количество_всех_частиц = value; Settings.Default.Save(); }
        }

        [Description("Математическое ожидание"), Category("Параметры алгоритма")]
        public double Мат_ожидание
        {
            get { return Settings.Default.Мат_ожидание; }
            set { Settings.Default.Мат_ожидание = value; Settings.Default.Save(); }
        }

        [Description("Среднеквадратичное отклоенение"), Category("Параметры алгоритма")]
        public double Отклонение
        {
            get { return Settings.Default.Отклонение; }
            set { Settings.Default.Отклонение = value; Settings.Default.Save(); }
        }

        public void Init(int CountVars)
        {

        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            Количество_итераций = Extention.getParamValueInt(temp, "Iter");
            Количество_всех_частиц = Extention.getParamValueInt(temp, "AllParts");
            Мат_ожидание = Extention.getParamValueInt(temp, "MathExpected");
            Отклонение = Extention.getParamValueInt(temp, "Deviation");
        }
    }
}
