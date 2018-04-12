using FuzzySystem.FuzzyAbstract.conf;
using System.ComponentModel;
using GeneticAlgorithmTune.Properties;


namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
   public class GeneticConf:ILearnAlgorithmConf
    {

        public enum Alg_Init_Type { Локальный = 0, Глобальный = 1 };
        public enum Alg_Crossover_Type { Унифицированный = 0, Многоточечный = 1 };
        public enum Alg_Selection_Type { Рулетка = 0, Элитарный = 1, Случайный = 2 };

        [DisplayName("Тип инициализации")]
        [Description("Тип инициализации"), Category("Параметры алгоритма")]
        public Alg_Init_Type GENCTypeInit
        {
            get { return (Alg_Init_Type)Settings.Default.InitType;}
            set { Settings.Default.InitType = (int)value; Settings.Default.Save(); }
        }

        [DisplayName("Тип скрещивания")]
        [Description("Тип скрещивания"), Category("Параметры алгоритма")]
        public Alg_Crossover_Type GENCTypeCrossover
        {
            get { return (Alg_Crossover_Type)Settings.Default.CrossoverType; }
            set { Settings.Default.CrossoverType = (int)value; Settings.Default.Save(); } 
        }

        [DisplayName("Вероятность скрещивания")]
        [Description("Вероятность скрещивания"), Category("Параметры алгоритма")]
        public double GENCPopabilityCrossover
        {
            get { return Settings.Default.CrossoverProb; }
            set { Settings.Default.CrossoverProb = value; Settings.Default.Save(); }
        }

        [DisplayName("Точек скрещивания")]
        [Description("Точек скрещивания"), Category("Параметры алгоритма")]
        public double GENCPointCrossover
        {
            get { return Settings.Default.PointsCrossoverVar; }
            set { Settings.Default.PointsCrossoverVar = value; Settings.Default.Save(); }
        }

        [DisplayName("Тип селекции")]
        [Description("Тип селекции"), Category("Параметры алгоритма")]
        public Alg_Selection_Type GENCTypeSelection
        {
            get { return (Alg_Selection_Type)Settings.Default.SelectionType; }
            set { Settings.Default.SelectionType = (int)value; Settings.Default.Save(); }
        }

        [DisplayName("Доля отклонения при инициализации")]
        [Description("Доля отклонения при инициализации"), Category("Параметры алгоритма")]
        public double GENCScateDeverceInit
        {
            get { return Settings.Default.InitScale; }
            set { Settings.Default.InitScale = value; Settings.Default.Save(); }
        }

        [DisplayName("Доля отклонения при мутации")]
        [Description("Доля отклонения при мутации"), Category("Параметры алгоритма")]
        public double GENCScateDeverceMutate
        {
            get { return Settings.Default.MutationScale; }
            set { Settings.Default.MutationScale = value; Settings.Default.Save(); }
        }
        [DisplayName("Количество итераций")]
        [Description("Сколько тактов выполняется алгоритм"), Category("Итерации")]
        public int GENCCountIteration
        {
            get { return Settings.Default.Gen_iter; }
            set { Settings.Default.Gen_iter = value; Settings.Default.Save(); }
        }

        [DisplayName("Особей в популяции")]
        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int GENCPopulationSize
        {
            get {return Settings.Default.Gen_population;}
            set { Settings.Default.Gen_population = value; Settings.Default.Save(); }
        }

        [DisplayName("Количество генерируемых потомков")]
        [Description("Количество генерируемых потомков"), Category("Параметры алгоритма")]
        public int GENCCountChild
        {
            get { return Settings.Default.Gen_children; }
            set { Settings.Default.Gen_children = value; Settings.Default.Save(); }
        }


        public virtual void loadParams(string param)
        {
            string[] temp = param.Split('}');
            string stemp= Extention.getParamValueString(temp, "GENCTypeInit");
            switch (stemp)
            {
                case "Global": GENCTypeInit = Alg_Init_Type.Глобальный; break;
                case "Local": GENCTypeInit = Alg_Init_Type.Локальный; break;
                default: GENCTypeInit = Alg_Init_Type.Локальный; break;
            }

            stemp = Extention.getParamValueString(temp, "GENCTypeCrossover");
            switch (stemp)
            {
                case "MultiPoint": GENCTypeCrossover = Alg_Crossover_Type.Многоточечный; break;
                case "Unified": GENCTypeCrossover = Alg_Crossover_Type.Унифицированный; break;
                default: GENCTypeCrossover = Alg_Crossover_Type.Унифицированный; break; 
            }
            GENCPopabilityCrossover = Extention.getParamValueDouble(temp, "GENCPopabilityCrossover");
            GENCPointCrossover = Extention.getParamValueDouble(temp, "GENCPointCrossover");
            stemp = Extention.getParamValueString(temp, "GENCTypeSelection");
            switch (stemp)
            {
                case "Roulete": GENCTypeSelection = Alg_Selection_Type.Рулетка; break;
                case "Random": GENCTypeSelection = Alg_Selection_Type.Случайный; break;
                case "Elite": GENCTypeSelection = Alg_Selection_Type.Элитарный; break;
                default: GENCTypeSelection = Alg_Selection_Type.Рулетка; break; 
            }

            GENCScateDeverceInit = Extention.getParamValueDouble(temp, "GENCScateDeverceInit");
            GENCScateDeverceMutate = Extention.getParamValueDouble(temp, "GENCScateDeverceMutate");
            GENCCountIteration = Extention.getParamValueInt(temp, "GENCCountIteration");
            GENCPopulationSize = Extention.getParamValueInt(temp, "GENCPopulationSize");
            GENCCountChild = Extention.getParamValueInt(temp, "GENCCountChild");
        }

        public virtual void Init(int countVars)
        {
        }
        public override string ToString()
        {
            return "Для задания настроек ГА раскройте список";
        }
    }
}
