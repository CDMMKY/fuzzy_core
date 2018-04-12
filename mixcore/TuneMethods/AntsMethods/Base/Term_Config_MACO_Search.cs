using System.ComponentModel;
using Settings = AntsMethods.Properties.SettingsBase;
using FuzzySystem.FuzzyAbstract.conf;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf

{
    public class MACOSearchConf:ACOSearchConf
    {
        [DisplayName ("Элитных решений в архиве")]
        [Description("Количество элитных муравьев"), Category("Модификация муравьев")]
        public int MACOCountElite
        {
            get { return Settings.Default.ACO_CountEliteAnt; }
            set
            {
                Settings.Default.ACO_CountEliteAnt = value;
                Settings.Default.Save();
            }
        }


        [DisplayName("Порог застревания архивов решений в экстремуме")]
        [Description("Условие замены архива решений на элитные"), Category("Модификация муравьев")]
        public int MACOCountExtremum
        {
            get { return Settings.Default.ACO_ExtremumCount; }
            set
            {
                Settings.Default.ACO_ExtremumCount = value;
                Settings.Default.Save();
            }
        }

        public override void loadParams(string param)
        {
            base.loadParams(param);
            string[] temp = param.Split('}');
            MACOCountElite = Extention.getParamValueInt(temp, "MACOCountElite");
            MACOCountExtremum = Extention.getParamValueInt(temp, "MACOCountExtremum");

        }

    }
}
