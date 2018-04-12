using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system.Class_Pittsburgh;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh.add_generators
{
    class Generator_Rulles_simple_random : Abstract_generator
    {


        private Type_Term_Func_Enum Generator_type_term()
        {
            int min = (int)Type_Term_Func_Enum.Треугольник;
            int max = (int)Type_Term_Func_Enum.Трапеция;
            return (Type_Term_Func_Enum)(new Random()).Next(min, max);
        }

        public override c_Fuzzy_System Generate(Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System Classifier, Abstract_generator_conf config)
        {
            Random rand = new Random();
            c_Fuzzy_System result = Classifier;
            if (result.Count_Rulles_Databases == 0)
            {
                Knowlege_base_CRules temp_rules = new Knowlege_base_CRules();
                result.Rulles_Database_Set.Add(temp_rules);
            }



            

            Type_Term_Func_Enum type_term = ((Generator_Rulles_simple_random_conf)config).Функция_принадлежности;
            int stable_terms = (int)((Generator_Rulles_simple_random_conf)config).Тип_Термов;
            int count_rules = ((Generator_Rulles_simple_random_conf)config).Количество_правил;

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
                    double[] parametrs = new double[c_Fuzzy_System.Count_Params_For_Term(temp_type_term)];
                
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
                string class_label = result.Nearest_Class(temp_term_list);
                CRule temp_Rule = new CRule(result.Rulles_Database_Set[0].Terms_Set,order,class_label,1.0);
                result.Rulles_Database_Set[0].Rules_Database.Add(temp_Rule);
            }



            result.unlaid_protection_fix();
            return result;
            
        }
    }
}
