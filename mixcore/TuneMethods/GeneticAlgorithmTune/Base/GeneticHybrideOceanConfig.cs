using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using GeneticAlgorithmTune.Properties;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GeneticHybrideOceanConfig : GeneticConf
    {
        [DisplayName("Отправлять через")]
        [Description("Через сколько итерация будут отправлены решения"),  Category("Гибридизация")]
      
      
        public int GENCHOSendEach
        {
            get { return Settings.Default.HybrideSendEach; }
            set { Settings.Default.HybrideSendEach = value; Settings.Default.Save(); }
        }
       
        [DisplayName("Принимать через")]
        [Description("Через сколько итерация будут получены решения из окена"), Category("Гибридизация")]
        public int GENCHOGetEach
        {
            get { return Settings.Default.HybrideGetEach; }
            set { Settings.Default.HybrideGetEach = value; Settings.Default.Save(); }
        }

        public override void loadParams(string param)
        {
            base.loadParams(param);
            string[] temp = param.Split('}');
            GENCHOSendEach = Extention.getParamValueInt(temp, "GENCHOSendEach");
            GENCHOGetEach = Extention.getParamValueInt(temp, "GENCHOGetEach");
        }
    
    }
    
}
