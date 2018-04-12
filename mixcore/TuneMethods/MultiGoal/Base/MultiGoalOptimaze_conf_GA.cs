using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial class MultiGoalOptimaze_conf
    {


        #region GA
        [Description("Использовать ГА"), Category("ГА")]
        public YesNo Использовать_ГА
        {
            get { return toYesNo(GeneticAlgorithmTune.Properties.Settings.Default.Used_GA); }
            set
            {
                GeneticAlgorithmTune.Properties.Settings.Default.Used_GA = toBool(value);
                GeneticAlgorithmTune.Properties.Settings.Default.Save();
            }
        }

        [Description("Использовать раз за такт ГА"), Category("ГА")]
        public int Использовать_ГА_раз_за_такт
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.Count_MultiGoal; }
            set
            {
                GeneticAlgorithmTune.Properties.Settings.Default.Count_MultiGoal = value;
                GeneticAlgorithmTune.Properties.Settings.Default.Save();
            }


        }



        [Description("Тип инициализации"), Category("ГА")]
        public GeneticConf.Alg_Init_Type Тип_инициализации_ГА
        {
            get { return (GeneticConf.Alg_Init_Type)GeneticAlgorithmTune.Properties.Settings.Default.InitType; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.InitType = (int)value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Тип скрещивания"), Category("ГА")]
        public GeneticConf.Alg_Crossover_Type Тип_скрещивания_ГА
        {
            get { return (GeneticConf.Alg_Crossover_Type)GeneticAlgorithmTune.Properties.Settings.Default.CrossoverType; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.CrossoverType = (int)value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Вероятность скрещивания"), Category("ГА")]
        public double Вероятность_скрещивания_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.CrossoverProb; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.CrossoverProb = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Точка деления"), Category("ГА")]
        public double Точка_деления_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.PointsCrossoverVar; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.PointsCrossoverVar = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Тип селекции"), Category("ГА")]
        public GeneticConf.Alg_Selection_Type Тип_селекции_ГА
        {
            get { return (GeneticConf.Alg_Selection_Type)GeneticAlgorithmTune.Properties.Settings.Default.SelectionType; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.SelectionType = (int)value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Доля отклонения при инициализации"), Category("ГА")]
        public double Доля_отклонения_при_инициализации_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.InitScale; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.InitScale = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Доля отклонения при мутации"), Category("ГА")]
        public double Доля_отклонения_при_мутации_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.MutationScale; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.MutationScale = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Сколько тактов выполняется алгоритм"), Category("ГА")]
        public int Количество_итераций_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.Gen_iter; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.Gen_iter = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Особей в популяции"), Category("ГА")]
        public int Особей_в_популяции_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.Gen_population; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.Gen_population = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        [Description("Количество генерируемых потомков"), Category("ГА")]
        public int Количество_генерируемых_потомков_ГА
        {
            get { return GeneticAlgorithmTune.Properties.Settings.Default.Gen_children; }
            set { GeneticAlgorithmTune.Properties.Settings.Default.Gen_children = value; GeneticAlgorithmTune.Properties.Settings.Default.Save(); }
        }

        #endregion

    }
}
