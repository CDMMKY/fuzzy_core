using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using UnionAndUnCrossTermsMethods.Properties;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf

{
    public class UnionTermsConf : ILearnAlgorithmConf
    {
        [DisplayName("Допустимый процент перекрытия по площади термов")]
        [Description("Процент допустимого перекрытия по площади "), Category("Границы объдинения")]
        public double UTCPercentCrossSquare
        {
            get { return Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent; }
            set
            {
                Settings.Default.Pareto_simpler_UnionTerms_bySqarePercent = value;
                Settings.Default.Save();
            }


        }

        [DisplayName("Допустимый процент перекрытия по границам")]
        [Description("Процент допустимого перекрытия граници к расстоянию между пиками термов "), Category("Границы объдинения")]
        public double UTCPercentCrossBorder
        {
            get { return Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent; }
            set
            {
                Settings.Default.Pareto_simpler_UnionTerms_byBorderPercent = value;
                Settings.Default.Save();
            }
          }


        public void loadParams(string param)
        {
            string[] temp = param.Split('}');
            UTCPercentCrossSquare = Extention.getParamValueDouble(temp, "UTCPercentCrossSqare");
            UTCPercentCrossBorder = Extention.getParamValueDouble(temp, "UTCPercentCrossBorder");
        }

        public void Init(int countVars)
        {

        }
        }
   

    

}
