using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = BeesMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf

{
    public class BeeDisStructureConf : ILearnAlgorithmConf
    {
       [DisplayName("Количество разведчиков")]
        [Description("Сколько разведчиков отправлено"), Category("Алгоритм пчелиной колонии дискретный")]
        public int ABCDSCountScout
        {
            get { return Settings.Default.ABCDS_CountScout; }
            set { Settings.Default.ABCDS_CountScout = value; Settings.Default.Save(); }
        }

       [DisplayName("Количество рабочих пчел")]
        [Description("Сколько рабочих пчел отправлено"), Category("Алгоритм пчелиной колонии дискретный")]
        public int ABCDSCountWorkers
        {
            get { return Settings.Default.ABCDS_CountWorkers; }
            set { Settings.Default.ABCDS_CountWorkers = value; Settings.Default.Save(); }
        }




        [ DisplayName("Количество интераций"),Description("Количество итераций алгоритма"), Category("Алгоритм пчелиной колонии дискретный")]
        public int ABCDSCountIter
        {
            get { return Settings.Default.ABCDS_Iter; }
            set { Settings.Default.ABCDS_Iter = value; Settings.Default.Save(); }
        }

        [DisplayName("Сохраняемых вариантов"), Description("Количество лучших наборов признаков"), Category("Алгоритм пчелиной колонии дискретный")]
        public int ABCDS_CountOfBestBase
        {
            get { return Settings.Default.ABCDS_CountOfBestBase; }
            set { Settings.Default.ABCDS_CountOfBestBase = value; Settings.Default.Save(); }
        }



        public virtual void loadParams(string param)
        {

            string[] temp = param.Split('}');
            ABCDSCountScout = Extention.getParamValueInt(temp, "ABCDSCountScout");
            ABCDSCountWorkers = Extention.getParamValueInt(temp, "ABCDSCountWorkers");
            ABCDSCountIter = Extention.getParamValueInt(temp, "ABCDSIter");
            ABCDS_CountOfBestBase = Extention.getParamValueInt(temp, "ABCDS_CointOfBestBase");
      
           }

        public void Init(int countVars)
        {

        }

     
    }
}
