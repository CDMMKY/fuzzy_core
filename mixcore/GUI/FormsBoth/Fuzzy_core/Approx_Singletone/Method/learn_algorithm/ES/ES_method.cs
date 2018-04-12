using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Approx_Singletone.learn_algorithm.ES;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Es_method : Abstract_learn_algorithm
    {
        Random rand = new Random();
        int count_populate;
        int count_child ;
        int count_iterate ;
        double coef_t1;
        double coef_t2;
        double param_crossover;
        int count_Multipoint;
        Individ.Alg_crossover alg_cross;
        Individ.Type_init type_init;
        Individ.Type_Mutate type_mutate;
        double b_ro;

        public override Fuzzy_system.Approx_Singletone.a_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf conf)
        {

            count_populate = ((Es_Config)conf).Особей_в_популяции;
             count_child = ((Es_Config)conf).Потомки;
             count_iterate = ((Es_Config)conf).Количество_итераций;
            coef_t1 = ((Es_Config)conf).Коэффициент_t1;
            coef_t2 = ((Es_Config)conf).Коэффициент_t2;
            param_crossover = ((Es_Config)conf).Вероятность_скрещивания;
            alg_cross = ((Es_Config)conf).Алгоритм_Скрещивания;
            type_init = ((Es_Config)conf).Алгоритм_Инициализации;
            count_Multipoint = ((Es_Config)conf).Точек_Скрещивания;
            type_mutate = ((Es_Config)conf).Алгоритм_Мутации;
            b_ro = ((Es_Config)conf).Изменение_РО;

            a_Fuzzy_System result = Approximate;
            Population main_pop = new Population(count_populate, count_child, result.Count_Vars, result.Learn_Samples_set);
            main_pop.init_first(result.Rulles_Database_Set[0], rand, type_init);
            for (int i = 0; i < count_iterate; i++)
            {

                double inparam = 0;
                switch (alg_cross)
                {
                    case Individ.Alg_crossover.Унифицированный: inparam = param_crossover; break;
                    case Individ.Alg_crossover.Многоточечный: inparam = count_Multipoint; break;

                }




                main_pop.select_parents_and_crossover(rand,alg_cross,inparam);
                main_pop.mutate_all(rand, coef_t1, coef_t2,type_mutate,b_ro);
                main_pop.union_parent_and_child();
                main_pop.Calc_Error(result);
                main_pop.select_global();
                
            }
            result.Rulles_Database_Set[0] = main_pop.get_best_database();
            GC.Collect();
               return result;
           }

        public override string ToString(bool with_param = false)
        {if (with_param)
            {
                string result = "эволюционная стратегия {";
                result += "Итераций= " + count_iterate.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_populate.ToString() + " ;" + Environment.NewLine;
                result += "Потомков= " + count_child.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент t1= " + coef_t1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффицент t2= " + coef_t2.ToString() + " ;" + Environment.NewLine;
                result += "Вероятность скрещивания= " + param_crossover.ToString() + " ;" + Environment.NewLine;
                result += "Точек скрещивания= " + count_Multipoint.ToString() + " ;" + Environment.NewLine;
                result += "Изменение РО= " + b_ro.ToString() + " ;" + Environment.NewLine;
                result += "Алгоритм инициализации= ";
                switch (type_init)
                {
                    case Individ.Type_init.Ограниченная: { result += "Ограниченная"; break; }
                    case Individ.Type_init.Случайная: { result += "Случайная"; break; }
                }
                
                result+=" ;" + Environment.NewLine;
                
                result += "Алгоритм скрещивания= ";
                
                  switch (alg_cross)
                  {
                      case Individ.Alg_crossover.Многоточечный: { result += "Многоточечный"; break; }
                      case Individ.Alg_crossover.Унифицированный: { result += "Унифицированный"; break; }
                  }
                  result += " ;" + Environment.NewLine;

                  result += "Алгоритм мутации= ";

                  switch (type_mutate)
                  {
                      case Individ.Type_Mutate.СКО: { result += "Простое ско"; break; }
                      case Individ.Type_Mutate.СКО_РО: { result += "На основе матрицы ковариации СКО РО"; break; }
                  }
                  result += " ;" + Environment.NewLine;
             


                result += "}";
                return result;
            }
            return "эволюционная стратегия";
        }
        
    }
}
