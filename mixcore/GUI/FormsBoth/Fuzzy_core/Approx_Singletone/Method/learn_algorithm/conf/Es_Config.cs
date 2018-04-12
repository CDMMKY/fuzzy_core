using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fuzzy_system.Approx_Singletone.learn_algorithm.ES;
using Mix_core.Properties;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm.conf
{
    class Es_Config:Abstract_learn_algorithm_conf
    {
        private int count_vars;
        private int size_of_individ;

        public Es_Config(int Count_vars)
        {
            count_vars = Count_vars;

            size_of_individ = Count_vars; //Sqare


            size_of_individ += 6; // Terms
            



            size_of_individ +=(int)Math.Pow(6,Count_vars); // Kons
           int temp =0;
            size_of_individ =(int) Math.DivRem(  size_of_individ, 4, out temp);
            Settings.Default.ES_method_Count_Multipoint = size_of_individ; 
            Settings.Default.Save();

        }

        [Description("Сколько тактов выполниться алгоритм"), Category("Итерации")]
        public int Количество_итераций
        {
            get { return Settings.Default.ES_method_Count_iteration; }
            set { Settings.Default.ES_method_Count_iteration = value; Settings.Default.Save(); }
        }

        [Description("Особей в популяции"), Category("Параметры алгоритма")]
        public int Особей_в_популяции
        {
            get { return Settings.Default.ES_method_size_population; }
            set { Settings.Default.ES_method_size_population = value; Settings.Default.Save(); }
        }
        [Description("Генерируемых потомков"), Category("Параметры алгоритма")]
        public int Потомки
        {
            get { return Settings.Default.ES_method_size_child; }
            set { Settings.Default.ES_method_size_child = value; Settings.Default.Save(); }
        }
     
        [Description("Коэффициент t1"), Category("Параметры алгоритма")]
        public double Коэффициент_t1
        {
            get { return 1 / (Math.Sqrt(2 * count_vars));}
            set { Settings.Default.ES_method_conf_t = value; Settings.Default.Save(); }
        }
        [Description("Коэффициент t2"), Category("Параметры алгоритма")]
        public double Коэффициент_t2
        {
            get { return 1 / (Math.Sqrt(2 *  Math.Sqrt(count_vars))); }
            set { Settings.Default.ES_method_conf_b = value; Settings.Default.Save(); }
        }


        [Description("Тип скрещивания"), Category("Параметры алгоритма")]
        public Individ.Alg_crossover Алгоритм_Скрещивания
        {
            get { return (Individ.Alg_crossover)Settings.Default.ES_method_Count_type_cross; }
            set { Settings.Default.ES_method_Count_type_cross = (int)value; Settings.Default.Save(); }
       }


        [Description("Вероятность скрещивания"), Category("Параметры алгоритма")]
        public double Вероятность_скрещивания
        {
            get { return  Settings.Default.ES_method_Count_uniform_level; }
            set { Settings.Default.ES_method_Count_uniform_level = value; Settings.Default.Save(); }
        }
        [Description("Количество точек скрещивания"), Category("Параметры алгоритма")]
        public int Точек_Скрещивания
        {
            get { return Settings.Default.ES_method_Count_Multipoint; }
            set { Settings.Default.ES_method_Count_Multipoint = value; Settings.Default.Save(); }
        }


        [Description("Тип инициализации"), Category("Параметры алгоритма")]
        public Individ.Type_init Алгоритм_Инициализации
        {
            get { return (Individ.Type_init)Settings.Default.ES_method_type_init; }
            set { Settings.Default.ES_method_type_init = (int)value; Settings.Default.Save(); }
        }


        [Description("Тип мутации"), Category("Параметры алгоритма")]
        public Individ.Type_Mutate Алгоритм_Мутации
        {
            get { return (Individ.Type_Mutate)Settings.Default.ES_method_type_mutate; }
            set { Settings.Default.ES_method_type_mutate = (int)value; Settings.Default.Save(); }
        }


        [Description("Изменение угла ротации"), Category("Параметры алгоритма")]
        public double Изменение_РО
        {
            get { return Settings.Default.ES_method_b_rotate; }
            set { Settings.Default.ES_method_b_rotate = value; Settings.Default.Save(); }
        }


    }
}
