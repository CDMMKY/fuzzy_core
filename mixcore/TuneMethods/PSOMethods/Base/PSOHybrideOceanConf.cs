using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using PSOMethods.Properties;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
  public   class PSOHybrideOceanConf:PSOSearchConf
    {

                [DisplayName("Отправлять через")]
        [Description("Через сколько итерация будут отправлены решения"),  Category("Гибридизация")]
      
      
        public int PSOHOSendEach
        {
            get { return SettingsBase.Default.HybrideSendEach; }
            set { SettingsBase.Default.HybrideSendEach = value; SettingsBase.Default.Save(); }
        }
       
        [DisplayName("Принимать через")]
        [Description("Через сколько итерация будут получены решения из окена"), Category("Гибридизация")]
        public int PSOHOGetEach
        {
            get { return SettingsBase.Default.HybrideGetEach; }
            set { SettingsBase.Default.HybrideGetEach = value; SettingsBase.Default.Save(); }
        }
        public override string ToString()
        {
            return "Для задания настроек алгоритма роящихся частиц раскройте список";
        }
        public override void loadParams(string param)
        {
            base.loadParams(param);
              string[] temp = param.Split('}');
              PSOHOSendEach = Extention.getParamValueInt(temp, "PSOHOSendEach");
              PSOHOGetEach = Extention.getParamValueInt(temp, "PSOHOGetEach");
        }

    }
}




