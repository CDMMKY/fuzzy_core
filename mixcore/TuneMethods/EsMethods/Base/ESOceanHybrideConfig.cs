using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using EsMethods.Properties;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ESOceanHybrideConfig : ESConfig
        {
            [DisplayName("Отправлять через")]
            [Description("Через сколько итерация будут отправлены решения"), Category("Гибридизация")]


            public int ESCHOSendEach
            {
                get { return SettingsBase.Default.HybrideSendEach; }
                set { SettingsBase.Default.HybrideSendEach = value; SettingsBase.Default.Save(); }
            }

            [DisplayName("Принимать через")]
            [Description("Через сколько итерация будут получены решения из окена"), Category("Гибридизация")]
            public int ESCHOGetEach
            {
                get { return SettingsBase.Default.HybrideGetEach; }
                set { SettingsBase.Default.HybrideGetEach = value; SettingsBase.Default.Save(); }
            }
            public override void loadParams(string param)
            {
                base.loadParams(param);
                string[] temp = param.Split('}');

                ESCHOSendEach = Extention.getParamValueInt(temp,"ESCHOSendEach");
                ESCHOGetEach = Extention.getParamValueInt(temp, "ESCHOGetEach");

            }
        }

    


}
