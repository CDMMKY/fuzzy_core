using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using AntsMethods.Properties;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
  public   class MACOHybrideConfig:MACOSearchConf
    {

        [DisplayName("Отправлять через")]
        [Description("Через сколько итерация будут отправлены решения"),  Category("Гибридизация")]
        public int MACOHOSendEach
        {
            get { return SettingsBase.Default.HybrideSendEach; }
            set { SettingsBase.Default.HybrideSendEach = value; SettingsBase.Default.Save(); }
        }
       
        [DisplayName("Принимать через")]
        [Description("Через сколько итерация будут получены решения из окена"), Category("Гибридизация")]
        public int MACOHOGetEach
        {
            get { return SettingsBase.Default.HybrideGetEach; }
            set { SettingsBase.Default.HybrideGetEach = value; SettingsBase.Default.Save(); }
        }
        public override string ToString()
        {
            return "Для задания настроек  НАМК раскройте список";
        }

        public override void loadParams(string param)
        {
            base.loadParams(param);

            string[] temp = param.Split('}');

            MACOHOSendEach = Extention.getParamValueInt(temp, "MACOHOSendEach");

            MACOHOGetEach = Extention.getParamValueInt(temp, "MACOHOGetEach");

        }

    }
}




