using System;
using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using PropertyGridUtils;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(PropertySorter))]
    public class HybrideOceanConfig :FilterablePropertyBase, ILearnAlgorithmConf
    {
        public ESOceanHybrideConfig EsConf = new ESOceanHybrideConfig();
        public GeneticHybrideOceanConfig GAConf = new GeneticHybrideOceanConfig();
        public BeeStructureOceabHybrideConfig BeeConf = new BeeStructureOceabHybrideConfig();
        public PSOHybrideOceanConf PSOConf = new PSOHybrideOceanConf();
        public MACOHybrideConfig MACOConf = new MACOHybrideConfig();


        public void Init(int countVars)
        {
            EsConf.Init(countVars);
            GAConf.Init(countVars);
            BeeConf.Init(countVars);
            PSOConf.Init(countVars);
            MACOConf.Init(countVars);

        }

        public void loadParams(string param)
        {
            throw new NotImplementedException();
        }
    
    


[DisplayName("Использовать ЭС")]
[Description("Использовать использовать эволюционную стратегию")]
[TypeConverter(typeof(BooleanTypeConverter))]
[PropertyOrder(20)]
public bool ИспользоватьЭС
{
    get { return EsMethods.Properties.SettingsBase.Default.ES_Used; }
    set { EsMethods.Properties.SettingsBase.Default.ES_Used = value; EsMethods.Properties.SettingsBase.Default.Save(); }
}

[DisplayName("Настройки ЭС")]
[Description("Настройки алгоритма эволюционной стратегии")]
[DynamicPropertyFilter("ИспользоватьЭС", "Да,Есть,True")]
[PropertyOrder(21)]
public ESOceanHybrideConfig НастройкиES
{ get { return EsConf;}
    set { EsConf = value; EsMethods.Properties.SettingsBase.Default.Save(); }
}

[DisplayName("Использовать ГА")]
[Description("Использовать использовать генетический алгоритм")]
[TypeConverter(typeof(BooleanTypeConverter))]
[PropertyOrder(10)]
public bool ИспользоватьГА
{
    get { return GeneticAlgorithmTune.Properties.Settings.Default.Used_GA; }
    set { GeneticAlgorithmTune.Properties.Settings.Default.Used_GA = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
}





[DisplayName("Настройки ГА")]
[Description("Настройки генетического алгоритма")]
[DynamicPropertyFilter("ИспользоватьГА", "Да,Есть,True")]
[PropertyOrder(11)]
       // public GeneticHybrideOceanConfig НастройкиGA
public GeneticHybrideOceanConfig НастройкиGA
{
    get { return GAConf; }
    set { GAConf = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
}


[DisplayName("Использовать структурных пчел")]
[Description("Использовать использовать алгоритма пчелиной колонии для структурной оптимизации")]
[TypeConverter(typeof(BooleanTypeConverter))]
[PropertyOrder(30)]
public bool ИспользоватьABCS
{
    get { return BeesMethods.Properties.SettingsBase.Default.ABCS_Used; }
    set { BeesMethods.Properties.SettingsBase.Default.ABCS_Used = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
}





[DisplayName("Настройки структурных пчел")]
[Description("Настройки алгоритма пчелиной колонии для структурной оптимизации")]
[DynamicPropertyFilter("ИспользоватьABCS", "Да,Есть,True")]
[PropertyOrder(31)]

public BeeStructureOceabHybrideConfig НастройкиABCS
{
    get { return BeeConf; }
    set { BeeConf = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
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
    set { PSOConf = value; BeesMethods.Properties.SettingsBase.Default.Save(); }
}



[DisplayName("Использовать НАМК")]
[Description("Использовать использовать непрерывный алгоритм муравьиной колонии")]
[TypeConverter(typeof(BooleanTypeConverter))]
[PropertyOrder(50)]
public bool ИспользоватьMACO
{
    get { return AntsMethods.Properties.SettingsBase.Default.MACO_Used; }
    set { AntsMethods.Properties.SettingsBase.Default.MACO_Used = value; AntsMethods.Properties.SettingsBase.Default.Save(); }
}





[DisplayName("Настройки НАМК")]
[Description("Настройка непрерывного алгоритма муравьиной колони")]
[DynamicPropertyFilter("ИспользоватьMACO", "Да,Есть,True")]
[PropertyOrder(51)]

public MACOHybrideConfig НастройкиMACO
{
    get { return MACOConf; }
    set { MACOConf = value; AntsMethods.Properties.SettingsBase.Default.Save(); }
}




}
}
