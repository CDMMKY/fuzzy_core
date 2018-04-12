using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using BeesMethods.Properties;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
  public   class BeeStructureOceabHybrideConfig:BeeStructureConf
    {

                [DisplayName("Отправлять через")]
        [Description("Через сколько итерация будут отправлены решения"),  Category("Гибридизация")]
      
      
        public int ABCSHOSendEach
        {
            get { return SettingsBase.Default.ABCS_HybrideSendEach; }
            set { SettingsBase.Default.ABCS_HybrideSendEach = value; SettingsBase.Default.Save(); }
        }
       
        [DisplayName("Принимать через")]
        [Description("Через сколько итерация будут получены решения из окена"), Category("Гибридизация")]
        public int ABCSHOGetEach
        {
            get { return SettingsBase.Default.ABCS_HybrideGetEach; }
            set { SettingsBase.Default.ABCS_HybrideGetEach = value; SettingsBase.Default.Save(); }
        }
        public override string ToString()
        {
            return "Для задания настроек структурных пчел раскройте список";
        }


        public override void loadParams(string param)
        {
            base.loadParams(param);
            string[] temp = param.Split('}');
            
            ABCSHOSendEach = Extention.getParamValueInt(temp, "ABCSHOSendEach");

            ABCSHOGetEach = Extention.getParamValueInt(temp, "ABCSHOGetEach");
        }

    }
}




