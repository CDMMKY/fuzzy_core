using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using Settings = WaterCycleAlgorithm.Properties.Settings;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
   public class WCAConfig : ILearnAlgorithmConf
    {
        [Description("Количество итераций"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.WCAIter; }
            set { Settings.Default.WCAIter = value; Settings.Default.Save(); }
        }
        [Description("Включает/выключает испарение"), Category("Параметры алгоритма")]
        public bool Испарение
        {
            get { return Settings.Default.Испарение; }
            set { Settings.Default.Испарение = value; Settings.Default.Save(); }
        }
        [Description("Регулирует скорость движения потоков"), Category("Параметры алгоритма")]
        public double Константа
        {
            get { return Settings.Default.Const; }
            set { Settings.Default.Const = value; Settings.Default.Save(); }
        }
        [Description("Количество капель"), Category("Параметры алгоритма")]
        public int Количество_капель
        {
            get { return Settings.Default.WCAPop; }
            set { Settings.Default.WCAPop = value; Settings.Default.Save(); }
        }
        [Description("Максимальное растояние приближения рек к морю"), Category("Параметры алгоритма")]
        public double Dmax
        {
            get { return Settings.Default.WCADmax; }
            set { Settings.Default.WCADmax = value; Settings.Default.Save(); }
        }
        [Description("Количество рек"), Category("Параметры алгоритма")]
        public int Количество_рек
        {
            get { return Settings.Default.WCARiver; }
            set { Settings.Default.WCARiver = value; Settings.Default.Save(); }
        }
        public   void Init(int countVars)
        {

        }

        public  void loadParams(string param)
        {
            string[] temp = param.Split('}');
            Количество_итераций = Extention.getParamValueInt(temp, "WCAIter");
            Испарение = Extention.getParamValueBool(temp, "WCAWaip");
            Константа = Extention.getParamValueDouble(temp, "WCAConst");
            Количество_капель = Extention.getParamValueInt(temp, "WCAPop");
            Dmax = Extention.getParamValueDouble(temp, "Dmax");
            Количество_рек = Extention.getParamValueInt(temp, "WCARiwer");
        }
    }
}