using System;
using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using Settings = EsMethods.Properties.SettingsBase;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ESConfig : ILearnAlgorithmConf
    {

        public enum Alg_crossover { Унифицированный = 0, Многоточечный = 1 };

        public enum Type_init { Случайная = 0, Ограниченная = 1 };
        public enum Type_Mutate { СКО = 0, СКО_РО = 1 };

        protected int count_vars;
        protected int size_of_individ;

        public ESConfig()
        {

        }

        public void Init(int Count_vars)
        {
            count_vars = Count_vars;
            size_of_individ = Count_vars; //Sqare
            size_of_individ += 6; // Terms
            size_of_individ += (int)Math.Pow(6, Count_vars); // Kons
            int temp = 0;
            size_of_individ = (int)Math.DivRem(size_of_individ, 4, out temp);
            Settings.Default.ES_method_Count_Multipoint = size_of_individ;
            Settings.Default.Save();

        }

        [DisplayName("Количество итераций")]
        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int ESCCountIteration
        {
            get { return Settings.Default.ES_method_Count_iteration; }
            set { Settings.Default.ES_method_Count_iteration = value; Settings.Default.Save(); }
        }

        [DisplayName("Особей в популяции")]
        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int ESCPopulationSize
        {
            get { return Settings.Default.ES_method_size_population; }
            set { Settings.Default.ES_method_size_population = value; Settings.Default.Save(); }
        }
        [DisplayName("Количество потомков")]
        [Description("Генерируемых потомков"), Category("Параметры алгоритма")]
        public int ESCCountChild
        {
            get { return Settings.Default.ES_method_size_child; }
            set { Settings.Default.ES_method_size_child = value; Settings.Default.Save(); }
        }
        [DisplayName("Коэффициент t1")]
        [Description("Коэффициент t1"), Category("Параметры алгоритма")]
        public double ESCT1
        {
            get { return 1 / (Math.Sqrt(2 * count_vars)); }
        }
        [DisplayName("Коэффициент t2")]
        [Description("Коэффициент t2"), Category("Параметры алгоритма")]
        public double ESCT2
        {
            get { return 1 / (Math.Sqrt(2 * Math.Sqrt(count_vars))); }
        }


        [DisplayName("Алгоритм cкрещивания")]
        [Description("Тип скрещивания"), Category("Параметры алгоритма")]
        public Alg_crossover ESCCrossoverType
        {
            get { return (Alg_crossover)Settings.Default.ES_method_Count_type_cross; }
            set { Settings.Default.ES_method_Count_type_cross = (int)value; Settings.Default.Save(); }
        }

        [DisplayName("Вероятность скрещивания")]
        [Description("Вероятность скрещивания"), Category("Параметры алгоритма")]
        public double ESCCrossoverPropability
        {
            get { return Settings.Default.ES_method_Count_uniform_level; }
            set { Settings.Default.ES_method_Count_uniform_level = value; Settings.Default.Save(); }
        }
        [DisplayName("Количество точек скрещивания")]
        [Description("Количество точек скрещивания"), Category("Параметры алгоритма")]
        public int ESCCountCrossoverPoint
        {
            get { return Settings.Default.ES_method_Count_Multipoint; }
            set { Settings.Default.ES_method_Count_Multipoint = value; Settings.Default.Save(); }
        }

        [DisplayName("Алгоритм инициализации")]
        [Description("Тип инициализации"), Category("Параметры алгоритма")]
        public Type_init ESCInitType
        {
            get { return (Type_init)Settings.Default.ES_method_type_init; }
            set { Settings.Default.ES_method_type_init = (int)value; Settings.Default.Save(); }
        }

        [DisplayName("Алгоритм мутации")]
        [Description("Тип мутации"), Category("Параметры алгоритма")]
        public Type_Mutate ESCMutateAlg
        {
            get { return (Type_Mutate)Settings.Default.ES_method_type_mutate; }
            set { Settings.Default.ES_method_type_mutate = (int)value; Settings.Default.Save(); }
        }

        [DisplayName("Коэффициент угла ротации")]
        [Description("Изменение угла ротации"), Category("Параметры алгоритма")]
        public double ESCAngleRotateB
        {
            get { return Settings.Default.ES_method_b_rotate; }
            set { Settings.Default.ES_method_b_rotate = value; Settings.Default.Save(); }
        }

        public virtual void loadParams(string param)
        {

            string stemp = "";
            string[] temp = param.Split('}');
            ESCCountIteration = Extention.getParamValueInt(temp, "ESCCountIteration");
            ESCPopulationSize = Extention.getParamValueInt(temp, "ESCPopulationSize");
            ESCCountChild = Extention.getParamValueInt(temp, "ESCCountChild");
            stemp = Extention.getParamValueString(temp, "ESCCrossoverType");
            
           switch(stemp)
           {
               case "MultiPoint": ESCCrossoverType = Alg_crossover.Многоточечный;  break;
                    case "Unified": ESCCrossoverType = Alg_crossover.Унифицированный;  break;
               default : ESCCrossoverType = Alg_crossover.Унифицированный; break;
        }

           ESCCrossoverPropability = Extention.getParamValueDouble(temp, "ESCCrossoverPropability");
           ESCCountCrossoverPoint = Extention.getParamValueInt(temp, "ESCCountCrossoverPoint");
         
            stemp = Extention.getParamValueString(temp, "ESCInitType");
            switch(stemp)
            {
                case "Constaint": ESCInitType = Type_init.Ограниченная; break;
                case "Random": ESCInitType = Type_init.Случайная; break;
                default: ESCInitType = Type_init.Ограниченная; break;
            }

            
            stemp = Extention.getParamValueString(temp, "ESCMutateAlg");
            switch (stemp)
            {
                case "RMSE": ESCMutateAlg = Type_Mutate.СКО; break;
                case "RMSEAR": ESCMutateAlg = Type_Mutate.СКО_РО; break;
                default: ESCMutateAlg = Type_Mutate.СКО_РО; break;
            }

            ESCAngleRotateB = Extention.getParamValueDouble(temp, "ESCAngleRotateB");

        }

        public override string ToString()
        {
            return "Для задания настроек ЭС раскройте список";
        }

    }
}
