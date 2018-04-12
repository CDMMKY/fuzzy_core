using System.ComponentModel;
using Settings = PSOMethods.Properties.SettingsBase;
using FuzzySystem.FuzzyAbstract.conf;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public class PSOBacterySearchConf:PSOSearchConf
    {
       [DisplayName("Отправляемые роящимися частицами")]
            [Description("Количество решений отправляемых роящимися частицами"), Category("Параметры алгоритма")]
        public int  PSOBacteryHOCountSend
        {
            get { return Settings.Default.SendByPSO; }
            set { Settings.Default.SendByPSO = value; Settings.Default.Save(); }
        }

       [DisplayName ("Отправляемых решений алгоритмом перемещения бактерий")]
         [Description("Количество решений отправляемых алгоритмом перемещения бактерий"), Category("Параметры алгоритма")]
        public int PSOBacteryHOCountGet
        {
            get { return Settings.Default.SendByBactery; }
            set { Settings.Default.SendByBactery = value; Settings.Default.Save(); }
        }

       [DisplayName("Обмен решениями через итераций")]
         [Description("Через сколько итераций обмениваться решениями"), Category("Параметры алгоритма")]
         public int PSOBacteryHOCountChange
         {
             get { return Settings.Default.IteratePSOtoSend; }
             set { Settings.Default.IteratePSOtoSend = value; Settings.Default.Save(); }
         }

       public override void loadParams(string param)
       {
           base.loadParams(param);
           string[] temp = param.Split('}');
           PSOBacteryHOCountSend = Extention.getParamValueInt(temp, "PSOBacteryHOCountSend");
           PSOBacteryHOCountGet = Extention.getParamValueInt(temp, "PSOBacteryHOCountGet");
           PSOBacteryHOCountChange = Extention.getParamValueInt(temp, "PSOBacteryHOCountChange");

       }
    }
}
