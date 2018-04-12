using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = CMeanInit.Properties.SettingsBase;


namespace FuzzySystem.FuzzyAbstract.AddGenerators.conf
{
    public enum Type_k_mean_algorithm
    {
        FCM = 0,
        GathGeva = 1,
        GustafsonKessel = 2
    }
    public class kMeanRulesGeneratorConfig : IGeneratorConf
    {
        [DisplayName("Вид функций принадлежности")]
        [Description("Тип функций принадлежности"), Category("Термы")]
        public TypeTermFuncEnum KMRGTypeFunc
        {
            get { return (TypeTermFuncEnum)Settings.Default.k_mean_rules_generator_conf_type_func; }
            set { Settings.Default.k_mean_rules_generator_conf_type_func = (int)value; Settings.Default.Save(); }
        }
        [DisplayName("Количество правил-кластеров")]
        [Description("Количество правил-кластеров"), Category("Правила")]
        public int KMRGCountRules
        {
            get { return Settings.Default.k_mean_rules_generator_conf_count_rules; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_count_rules = value;
                Settings.Default.Save();
            }
        }
        [DisplayName("Вид модификация алгоритма")]
        [Description("Вид модификация алгоритма")]
        public Type_k_mean_algorithm KMRGTypeAlg
        {
            get { return (Type_k_mean_algorithm)Settings.Default.k_mean_rules_generator_conf_type_alg; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_type_alg = (int)value;
                Settings.Default.Save();
            }
        }
        [DisplayName("Экспоненциальный вес")]
        [Description("Экспоненциальный вес")]
        public double KMRGExponentialWeight
        {
            get { return Settings.Default.k_mean_rules_generator_conf_nebulisation_factor; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_nebulisation_factor = value;
                Settings.Default.Save();
            }
        }
        [DisplayName("Максиммальное количество итераций")]
        [Description("Максиммальное количество итераций")]
        public int KMRGIteraton
        {
            get { return Settings.Default.k_mean_rules_generator_conf_Max_iterate; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_Max_iterate = value;
                Settings.Default.Save();
            }
        }
        [DisplayName("Требуемая точность")]
        [Description("Требуемая точность")]
        public double KMRGAccuracy
        {
            get { return Settings.Default.k_mean_rules_generator_conf_need_precision; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_need_precision = value;
                Settings.Default.Save();
            }
        }


        public void Init(int countVars)
        {

        }

        public void loadParams(string param)
        {
            string stemp = "";
            string[] temp = param.Split('}');

            stemp = Extention.getParamValueString(temp, "KMRGTypeFunc");
            switch (stemp)
            {
                case "Triangle": KMRGTypeFunc = TypeTermFuncEnum.Треугольник; break;
                case "Gauss": KMRGTypeFunc = TypeTermFuncEnum.Гауссоида; break;
                case "Parabola": KMRGTypeFunc = TypeTermFuncEnum.Парабола; break;
                case "Trapezium": KMRGTypeFunc = TypeTermFuncEnum.Трапеция; break;
                default: KMRGTypeFunc = TypeTermFuncEnum.Треугольник; break;
            }

            KMRGCountRules = Extention.getParamValueInt(temp,"KMRGCountRules");

            stemp = Extention.getParamValueString(temp, "KMRGTypeAlg");
            switch (stemp)
            {
                case "FCM": KMRGTypeAlg = Type_k_mean_algorithm.FCM; break;
                case "GathGeva": KMRGTypeAlg = Type_k_mean_algorithm.GathGeva; break;
                case "GustafsonKessel": KMRGTypeAlg = Type_k_mean_algorithm.GustafsonKessel; break;

                default: KMRGTypeAlg = Type_k_mean_algorithm.GustafsonKessel; break;
            }

            KMRGExponentialWeight = Extention.getParamValueDouble(temp, "KMRGExponentialWeight");

            KMRGIteraton = Extention.getParamValueInt(temp, "KMRGIteraton");

            KMRGAccuracy = Extention.getParamValueDouble(temp, "KMRGAccuracy");


        }


    }



}
