using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.add_generators
{
    class Generator_Rulees_simple_random : Abstract_generator
    {
        Type_Term_Func_Enum type_term ;
        int stable_terms = 0;
        int count_rules = 0;
            

        private Type_Term_Func_Enum Generator_type_term()
        {
            int min = (int)Type_Term_Func_Enum.Треугольник;
            int max = (int)Type_Term_Func_Enum.Трапеция;
            return (Type_Term_Func_Enum)(new Random()).Next(min, max);
        }

        public override a_Fuzzy_System Generate(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_generator_conf config)
        {
            Random rand = new Random();
            a_Fuzzy_System result = Approximate;
            if (result.Count_Rulles_Databases == 0)
            {
                Knowlege_base_ARules temp_rules = new Knowlege_base_ARules();
                result.Rulles_Database_Set.Add(temp_rules);
            }



            

            type_term = ((Generator_Rulles_simple_random_conf)config).Функция_принадлежности;
            stable_terms = (int)((Generator_Rulles_simple_random_conf)config).Тип_Термов;
            count_rules = ((Generator_Rulles_simple_random_conf)config).Количество_правил;
            

            for (int j = 0; j < count_rules; j++)
            {
                int[] order = new int[result.Count_Vars];
                Type_Term_Func_Enum temp_type_term;
                if (stable_terms == 0)
                {
                    temp_type_term = type_term;
                }
                else
                {
                    temp_type_term = Generator_type_term();
                }

                List< Term> temp_term_list = new List<Term>();
                for (int k = 0; k < result.Count_Vars; k++)
                {
                    double[] parametrs = new double[Member_Function.Count_Params_For_Term(temp_type_term)];
                
                    switch (temp_type_term)
                    {
                        case Type_Term_Func_Enum.Треугольник:
                            parametrs[0] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[1] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[2] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            Array.Sort(parametrs);
                            break;
                        case Type_Term_Func_Enum.Гауссоида: parametrs[0] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[1] = (rand.NextDouble() + 0.01) * 0.5 *
                                           (result.Learn_Samples_set.Attribute_Max(k) -
                                            result.Learn_Samples_set.Attribute_Min(k));
                            break;
                        case Type_Term_Func_Enum.Парабола: parametrs[0] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[1] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            Array.Sort(parametrs);
                            break;
                        case Type_Term_Func_Enum.Трапеция: parametrs[0] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[1] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[2] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));
                            parametrs[3] = result.Learn_Samples_set.Attribute_Min(k) + rand.NextDouble() * (result.Learn_Samples_set.Attribute_Max(k) - result.Learn_Samples_set.Attribute_Min(k));

                            Array.Sort(parametrs);

                            break;

                    }
                    Term temp_term = new Term(parametrs, temp_type_term, k);
                    result.Rulles_Database_Set[0].Terms_Set.Add(temp_term);
                    temp_term_list.Add(temp_term);
                    order[k] = result.Rulles_Database_Set[0].Terms_Set.Count - 1;
                }
                double approx_Value = result.Nearest_Approx(temp_term_list);
                ARule temp_Rule = new ARule(result.Rulles_Database_Set[0].Terms_Set,order,approx_Value);
                result.Rulles_Database_Set[0].Rules_Database.Add(temp_Rule);
            }



            result.unlaid_protection_fix();
            GC.Collect();
            return result;
            
        }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "случайная генерация {";
                result += "Функции принадлежности= " + Member_Function.ToString(type_term) + " ;" + Environment.NewLine;
                result += "Генерируется правил =" + this.count_rules.ToString() + " ; "+Environment.NewLine;
                result += "Генерация правил случайной функции принадлежности =";
                if (stable_terms == 0) { result += "Нет"; }
                     else {result +="Да";}

                result += " ; " + Environment.NewLine;

                result += "}";
                return result;
            }
            return "случайная генерация";
        }
    }
}
