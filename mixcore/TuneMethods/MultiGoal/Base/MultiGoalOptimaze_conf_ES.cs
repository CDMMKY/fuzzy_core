using System;
using System.ComponentModel;

namespace FuzzySystem.FuzzyAbstract.learn_algorithm.conf
{
    public partial  class MultiGoalOptimaze_conf
    {

        #region ES


        private int count_vars;
        private int size_of_individ;


        [Description("Использовать ЕС"), Category("ЕС")]
        public YesNo Использовать_ЕС
        {
            get { return toYesNo(EsMethods.Properties.SettingsBase.Default.ES_Used); }
            set
            {
                EsMethods.Properties.SettingsBase.Default.ES_Used = toBool(value);
                EsMethods.Properties.SettingsBase.Default.Save();
            }


        }

        [Description("Использовать раз за такт ЕС"), Category("ЕС")]
        public int Использовать_ЕС_раз_за_такт
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_Used_times; }
            set
            {
                EsMethods.Properties.SettingsBase.Default.ES_Used_times = value;
                EsMethods.Properties.SettingsBase.Default.Save();
            }


        }




        [Description("Сколько тактов выполниться алгоритм"), Category("ЕС")]
        public int Количество_итераций_ЕС
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_Count_iteration; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_Count_iteration = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }

        [Description("Особей в популяции"), Category("ЕС")]
        public int Особей_в_популяции_ЕС
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_size_population; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_size_population = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }
        [Description("Генерируемых потомков"), Category("ЕС")]
        public int Потомки
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_size_child; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_size_child = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }

        [Description("Коэффициент t1"), Category("ЕС")]
        public double Коэффициент_t1
        {
            get { return 1 / (Math.Sqrt(2 * count_vars)); }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_conf_t = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }
        [Description("Коэффициент t2"), Category("ЕС")]
        public double Коэффициент_t2
        {
            get { return 1 / (Math.Sqrt(2 * Math.Sqrt(count_vars))); }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_conf_b = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }

        //  public enum Alg_crossover { Унифицированный = 0, Многоточечный = 1 };


        [Description("Тип скрещивания"), Category("ЕС")]
        public FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover Алгоритм_Скрещивания
        {
            get { return (FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover)EsMethods.Properties.SettingsBase.Default.ES_method_Count_type_cross; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_Count_type_cross = (int)value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Вероятность скрещивания"), Category("ЕС")]
        public double Вероятность_скрещивания
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_Count_uniform_level; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_Count_uniform_level = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }
        [Description("Количество точек скрещивания"), Category("ЕС")]
        public int Точек_Скрещивания
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_Count_Multipoint; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_Count_Multipoint = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Тип инициализации"), Category("ЕС")]
        public FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init Алгоритм_Инициализации
        {
            get { return (FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init)EsMethods.Properties.SettingsBase.Default.ES_method_type_init; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_type_init = (int)value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Тип мутации"), Category("ЕС")]
        public FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate Алгоритм_Мутации
        {
            get { return (FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate)EsMethods.Properties.SettingsBase.Default.ES_method_type_mutate; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_type_mutate = (int)value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }


        [Description("Изменение угла ротации"), Category("ЕС")]
        public double Изменение_РО
        {
            get { return EsMethods.Properties.SettingsBase.Default.ES_method_b_rotate; }
            set { EsMethods.Properties.SettingsBase.Default.ES_method_b_rotate = value; EsMethods.Properties.SettingsBase.Default.Save(); }
        }


        #endregion

    }
}
