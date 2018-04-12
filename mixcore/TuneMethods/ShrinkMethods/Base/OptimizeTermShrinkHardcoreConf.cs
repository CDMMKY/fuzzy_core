using System.ComponentModel;
using Settings = ShrinkMethods.Properties.SettingsBase;

namespace FuzzySystem.FuzzyAbstract.conf
{
    public class OptimizeTermShrinkHardCoreConf : ILearnAlgorithmConf
    {
    
          public OptimizeTermShrinkHardCoreConf()
    {
    }

  
  

        [DisplayName("Значение уменьшения термов для одного параметра")]
                [Description("Насколько будет уменьшено количеств термов для входного параметра"), Category("Параметры НС")]

        public int OTSHCCountShrinkTerm
        {
            get { return Settings.Default.Term_shrink_HardCode_size_of_shrink; }
            set
            {
                Settings.Default.Term_shrink_HardCode_size_of_shrink  = value;
                Settings.Default.Save();
            }


        }

                public void loadParams(string param)
                {
                    string[] temp = param.Split('}');

                    OTSHCCountShrinkTerm = Extention.getParamValueInt(temp, "OTSARCountShrinkTerm");
                }

                public void Init(int countVars)
                {
                   
    
                }

    }
}