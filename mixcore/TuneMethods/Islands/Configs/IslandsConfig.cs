using System;
using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using PropertyGridUtils;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(PropertySorter))]
    public class IslandsConfig : FilterablePropertyBase, ILearnAlgorithmConf
    {
        public PSOHybrideOceanConf PSOConf = new PSOHybrideOceanConf();


        public void Init(int countVars)
        {
            PSOConf.Init(countVars);
        }

        public void loadParams(string param)
        {
            throw new NotImplementedException();
        }

        [DisplayName("Использовать АРЧ")]
        [Description("Использовать использовать алгоритма роящихся частиц")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        [PropertyOrder(40)]
        public bool ИспользоватьPSO
        {
            get { return PSOMethods.Properties.SettingsBase.Default.PSO_Used; }
            set { PSOMethods.Properties.SettingsBase.Default.PSO_Used = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }

        [DisplayName("Настройки АРЧ")]
        [Description("Настройки алгоритма роящихся частиц")]
        [DynamicPropertyFilter("ИспользоватьPSO", "Да,Есть,True")]
        [PropertyOrder(41)]

        public PSOHybrideOceanConf НастройкиPSO
        {
            get { return PSOConf; }
            set { PSOConf = value; PSOMethods.Properties.SettingsBase.Default.Save(); }
        }
    }
}
