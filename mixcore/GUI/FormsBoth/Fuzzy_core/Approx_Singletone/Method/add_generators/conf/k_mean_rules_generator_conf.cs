using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fuzzy_system.Approx_Singletone;
using Mix_core.Properties;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;

namespace Fuzzy_system.Approx_Singletone.add_generators.conf
{
    public enum Type_k_mean_algorithm
    {
        FCM = 0,
        Gath_geva = 1,
        Gustafson_Kessel = 2
    }
    class k_mean_rules_generator_conf:Abstract_generator_conf
    {
     

        [Description("Тип функций принадлежности"), Category("Термы")]
        public Type_Term_Func_Enum Функция_принадлежности
        {
            get { return (Type_Term_Func_Enum)Settings.Default.k_mean_rules_generator_conf_type_func; }
            set { Settings.Default.k_mean_rules_generator_conf_type_func = (int)value; Settings.Default.Save(); }
        }
        [Description("Количество правил-кластеров"), Category("Правила")]
        public int Количество_правил
        {
            get { return Settings.Default.k_mean_rules_generator_conf_count_rules; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_count_rules = value;
                Settings.Default.Save();
            }
        }
    [Description("Модификация алгоритма"), Category("С-Алгоритм")]
        public Type_k_mean_algorithm Алгоритм
        {
            get { return (Type_k_mean_algorithm) Settings.Default.k_mean_rules_generator_conf_type_alg; }
            set
            {
                Settings.Default.k_mean_rules_generator_conf_type_alg =(int) value;
                Settings.Default.Save();
            }
        }

    [Description("Экспоненциальный вес"), Category("С-Алгоритм")]
    public double Экспоненциальный_вес_алгоритма
    {
        get { return Settings.Default.k_mean_rules_generator_conf_nebulisation_factor; }
        set
        {
            Settings.Default.k_mean_rules_generator_conf_nebulisation_factor = value;
            Settings.Default.Save();
        }
    }

    [Description("Максиммальное количество итераций"), Category("С-Алгоритм")]
    public int Итераций
    {
        get { return Settings.Default.k_mean_rules_generator_conf_Max_iterate; }
        set
        {
            Settings.Default.k_mean_rules_generator_conf_Max_iterate = value;
            Settings.Default.Save();
        }
    }
    [Description("Требуемая точность"), Category("С-Алгоритм")]
    public double Точность
    {
        get { return Settings.Default.k_mean_rules_generator_conf_need_precision; }
        set
        {
            Settings.Default.k_mean_rules_generator_conf_need_precision = value;
            Settings.Default.Save();
        }
    }



    }

}
