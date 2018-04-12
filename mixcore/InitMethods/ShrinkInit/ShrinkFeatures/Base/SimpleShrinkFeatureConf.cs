using Settings = ShrinkInit.Properties.SettingsBase;
using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.conf
{
    class SimpleShrinkFeatureConf : IGeneratorConf
    {
    [DisplayName ("Усечённые признаки")]
        public string ShrinkFeature
        {
            get { return Settings.Default.ShrinkFeatures; }
            set { Settings.Default.ShrinkFeatures = value; Settings.Default.Save(); }
        }
        
    
        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            ShrinkFeature = Extention.getParamValueString(temp, "ShrinkFeatures");
        }
        public void Init(int countVars)
        { }
    }
}

