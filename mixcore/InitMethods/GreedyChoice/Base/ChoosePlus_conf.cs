using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using System.Linq;
using GreedyChoice.Properties;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Utils;

namespace GreedyChoice
{
    public class GreedyChoiceConfigPlus : ILearnAlgorithmConf
    {

      

   
        public virtual void Init(int countVars)
        {
         
        }


      
        [DisplayName("Максимальное количество входных признаков")]
        [Description("Максимальное количество входных признаков")]
        public int GCCMaxVars
        {
            get { return Settings.Default.GCCmaxFeature; }

            set
            {   Settings.Default.GCCmaxFeature = value;
                Settings.Default.Save();
            }
        }

        [DisplayName("Способ сортировки")]
        [Description("Способ сортировки результата")]
        public SortType GCCSortWay
        {
            get { return (SortType)Settings.Default.GCCSortType; }
            set { Settings.Default.GCCSortType = (int)value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            GCCMaxVars = Extention.getParamValueInt(temp, "GCCMaxVars");
            GCCSortWay =(SortType) Extention.getParamValueInt(temp, "GCCSortWay");
        }


    }



}
