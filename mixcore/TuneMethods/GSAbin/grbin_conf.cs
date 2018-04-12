using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using FuzzySystem;
using System.Linq;
using GSAbin.Properties;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Utils;

namespace GSAbin
{
 
    public class grbin_conf : ILearnAlgorithmConf
    { 

        public virtual void Init(int countVars)
        {  }
   
  


        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            GSAMaxVars = Extention.getParamValueInt(temp, "GSAMaxVars");
            GSAMCount = Extention.getParamValueInt(temp, "GSAMCount");
            GSAMInter = Extention.getParamValueInt(temp, "GSAMInter");
            GSAG0 = Extention.getParamValueInt(temp, "GSAG0");
            GSAAlpha = Extention.getParamValueInt(temp, "GSAAlpha");
            GSAEpsilon = Extention.getParamValueDouble(temp, "GSAEpsilon");
            GSAErrorBest = Extention.getParamValueDouble(temp, "GSAErrorBest");
        }


        [DisplayName("Желательное количество входных признаков")]
        [Description("Желательное количество входных признаков для выбора хороших частиц"), Category("Дискретный алгоритм")]
        public int GSAMaxVars
        {
            get { return Settings1.Default.maxFeature; }

            set
            {
                Settings1.Default.maxFeature = value;
                Settings1.Default.Save();
            }
        }

        [DisplayName("Количество частиц")]
        [Description("Количество векторов"), Category("Параметры алгоритма")]
        public int GSAMCount
        {
            get { return Settings1.Default.NumberAgents; }
            set { Settings1.Default.NumberAgents = value; Settings1.Default.Save(); }
        }

        [DisplayName("Количество итераций")]
        [Description("Количество итераций"), Category("Дискретный алгоритм")]
        public int GSAMInter
        {
            get { return Settings1.Default.gsa_iter; }
            set { Settings1.Default.gsa_iter = value; Settings1.Default.Save(); }
        }
        [DisplayName("Гравитационная постоянная")]
        [Description("Начальное значение гравитационной постоянной"), Category("Параметры алгоритма")]
        public int GSAG0
        {
            get { return Settings1.Default.gsa_G0; }
            set { Settings1.Default.gsa_G0 = value; Settings1.Default.Save(); }
        }
        [DisplayName("Коэффициент уменьшения")]
        [Description("Коэффициент уменьшения Альфа"), Category("Параметры алгоритма")]
        public int GSAAlpha
        {
            get { return Settings1.Default.gsa_alpha; }
            set { Settings1.Default.gsa_alpha = value; Settings1.Default.Save(); }
        }
        [DisplayName("Малая константа")]
        [Description("Малая константа Эпсилон"), Category("Параметры алгоритма")]
        public double GSAEpsilon
        {
            get { return Settings1.Default.gsa_epsilon; }
            set { Settings1.Default.gsa_epsilon = value; Settings1.Default.Save(); }
        }
        [DisplayName("Пользовательская ошибка")]
        [Description("Верхняя граница ошибки для тестирования"), Category("Дискретный алгоритм")]
        public double GSAErrorBest
        {
            get { return Settings1.Default.ErrorBestUser; }
            set { Settings1.Default.ErrorBestUser = value; Settings1.Default.Save(); }
        }

       
        [DisplayName("Способ сортировки")]
        [Description("Способ сортировки результата"), Category("Дискретный алгоритм")]
        public SortType GSASortWay
        {
            get { return (SortType)Settings1.Default.gsa_sortWay; }
            set { Settings1.Default.gsa_sortWay = (int)value; Settings1.Default.Save(); }
        }
    }
}
