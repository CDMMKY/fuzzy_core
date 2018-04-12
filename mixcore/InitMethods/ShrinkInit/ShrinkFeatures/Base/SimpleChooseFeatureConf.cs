using Settings = ShrinkInit.Properties.SettingsBase;
using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.conf
{
    class SimpleChooseFeatureConf : IGeneratorConf
    {
        [DisplayName("Выбранные признаки")]
        public string ChooseFeatures
        {
            get { return Settings.Default.SimpleChooseFeatureConf; }
            set { Settings.Default.SimpleChooseFeatureConf = value; Settings.Default.Save(); }
        }
        
    
        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            ChooseFeatures = Extention.getParamValueString(temp, "ChooseFeatures");
        }
        public void Init(int countVars)
        { }
    }
}

