using System.ComponentModel;

using BacterialForagingOptimization;
using FuzzySystem.FuzzyAbstract.conf;
using Bacterial_Foraging_Optimization.Properties;

namespace BacterialForagingOptimization.Base
{
    public class BacteryAlgorithmConfig : ILearnAlgorithmConf
    {
       [DisplayName("Количество решений")]
        [Description("Количество отличных между собой решений"), Category("Параметры алгоритма")]
        public int BFOCountSolution
        {
            get { return Settings.Default.countUsedBFO; }
            set { Settings.Default.countUsedBFO = value; Settings.Default.Save(); }
        }
       [DisplayName("Количество итераций")]
        [Description("Количество итераций"), Category("Параметры алгоритма")]
        public int BFOCountIteration
        {
            get { return Settings.Default.countIteration; }
            set { Settings.Default.countIteration = value; Settings.Default.Save(); }
        }

        public void loadParams(string param)
        {
           
           string[] temp = param.Split('}');
        
           BFOCountSolution = Extention.getParamValueInt(temp, "BFOCountSolution");

           BFOCountIteration = Extention.getParamValueInt(temp, "BFOCountIteration");

            //  throw (new NotImplementedException());
        }
        public void Init(int countVars)
        { }
    }
}
