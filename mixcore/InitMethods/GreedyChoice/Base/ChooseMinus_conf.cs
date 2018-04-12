using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using System.Linq;
using GreedyChoice.Properties;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.Utils;

namespace GreedyChoice
{
    public class GreedyChoiceConfigMinus : ILearnAlgorithmConf
    {

      

   
        public virtual void Init(int countVars)
        {
         
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
          
            GCCSortWay =(SortType) Extention.getParamValueInt(temp, "GCCSortWay");
        }


    }



}
