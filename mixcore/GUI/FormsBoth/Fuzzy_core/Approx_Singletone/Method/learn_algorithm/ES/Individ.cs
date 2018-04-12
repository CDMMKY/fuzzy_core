using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm.ES
{
    class Individ
    {
        public double error { get { return Error; } }
        public Hromosom hrom_vector;
        double Error;
        int count_step_sko;
        int count_step_rotate;
        List<double> step_sko;
        List<double> step_rotate;
        a_samples_set Data;

        public enum Alg_crossover { Унифицированный = 0, Многоточечный = 1 };

        public enum Type_init { Случайная = 0, Ограниченная = 1 };
        public enum Type_Mutate { СКО = 0, СКО_РО = 1 };


        public Individ(Knowlege_base_ARules Source, a_samples_set data, int count_vars, bool the_first, Random rand, Type_init par_type_init)
        {
            hrom_vector = new Hromosom(new Knowlege_base_ARules(Source));
            step_sko = new List<double>(count_vars);
            step_rotate = new List<double>(count_vars);
            count_step_sko = count_vars;
            count_step_rotate = count_vars;
            Data = data;
            for (int i = 0; i < count_vars; i++)
            {

                step_sko.Add(0 + rand.NextDouble() * (data.Attribute_Scatter(i) * 0.05));
                step_rotate.Add(-1 * Math.PI + rand.NextDouble() * 2 * Math.PI);

            }
            if (!the_first)
            {

                switch (par_type_init)
                {
                    case Type_init.Случайная: hrom_vector.init_random(rand, data); break;
                    case Type_init.Ограниченная: hrom_vector.init_constrain(rand, data); break;


                }
            }

        }
        public Individ(Individ the_individ)
        {
            hrom_vector = new Hromosom(the_individ.hrom_vector);
            step_rotate = new List<double>(the_individ.step_rotate);
            step_sko = new List<double>(the_individ.step_sko);
            count_step_rotate = the_individ.count_step_rotate;

            count_step_sko = the_individ.count_step_sko;
            Data = the_individ.Data;

        }




        public Individ crossover(Individ another_parent, Alg_crossover type_cross, Random rand, double param)
        {
            Individ cross_hrom = null;

            switch (type_cross)
            {
                case Alg_crossover.Унифицированный: cross_hrom = crossover_uniform(another_parent, param, rand); break;
                case Alg_crossover.Многоточечный: cross_hrom = crossover_multipoint(another_parent, (int)param, rand); break;
                default: cross_hrom = crossover_uniform(another_parent, param, rand); break;

            }



            return cross_hrom;
        }
        public Individ mutate(Random rand, double tau1, double tau2, Individ.Type_Mutate type_mutate, double b_ro)
        {
            switch (type_mutate)
            {
                case Type_Mutate.СКО:
                    {
                        hrom_vector.mutate_SKO(step_sko, rand);
                        for (int i = 0; i < step_sko.Count; i++)
                        {
                            step_sko[i] = step_sko[i] * Math.Exp(tau1 * GaussRandom.Random_gaussian(rand) + tau2 * GaussRandom.Random_gaussian(rand));

                        }
                        break;
                    }
                case Type_Mutate.СКО_РО:
                    {
                        List<double> Covariance_matrix_pruned = new List<double>();

                        for (int i = 0; i < step_sko.Count; i++) // mutate
                        {
                            step_sko[i] = step_sko[i] * Math.Exp(tau1 * GaussRandom.Random_gaussian(rand) + tau2 * GaussRandom.Random_gaussian(rand));
                            step_rotate[i] = step_rotate[i] + b_ro * GaussRandom.Random_gaussian(rand);
                        }
                        double avr_ro =  step_rotate.Sum() /step_rotate.Count;
                        double avr_sko = step_sko.Sum() /step_sko.Count;

                         for (int i = 0; i < step_sko.Count; i++) //calc covariance_matrix_pruned
                        {
                            Covariance_matrix_pruned.Add(Math.Abs(
                                                  (step_sko[i] - avr_sko) * (step_rotate[i] - avr_ro)
                                                                    )
                                                          );
                         }

                        hrom_vector.mutate_SKO_RO(Covariance_matrix_pruned, rand);
                        break;
                    }
            }
            return this;
        }

        public void calc_Error(a_Fuzzy_System error_checker)
        {
            error_checker.Rulles_Database_Set.Add(hrom_vector.Core_Check);
            error_checker.unlaid_protection_fix(error_checker.Count_Rulles_Databases - 1);
            Error = error_checker.approx_Learn_Samples(error_checker.Count_Rulles_Databases - 1);
            error_checker.Rulles_Database_Set.Remove(hrom_vector.Core_Check);
        }



        private Individ crossover_uniform(Individ another_parent, double level_cross, Random Rand)
        {
            Individ child = new Individ(this);

            for (int i = 0; i < child.count_step_rotate; i++)
            {
                if (Rand.NextDouble() < level_cross)
                {
                    child.step_rotate[i] = another_parent.step_rotate[i];
                }

            }

            for (int i = 0; i < child.count_step_sko; i++)
            {
                if (Rand.NextDouble() < level_cross)
                {
                    child.step_sko[i] = another_parent.step_sko[i];
                }

            }


            child.hrom_vector.crossover_uniform(another_parent.hrom_vector, Rand, level_cross);

            return child;
        }


        private Individ crossover_multipoint(Individ another_parent, int count_multipoint, Random Rand)
        {

            Individ child = new Individ(this);

            int size = child.step_sko.Count;
            size += child.hrom_vector.Core_Check.Terms_Set.Count();

            size += child.hrom_vector.Core_Check.Rules_Database.Count;

            List<int> trigger_cross = new List<int>(count_multipoint);

            do
            {
                trigger_cross.Add(Rand.Next(size));
                trigger_cross = trigger_cross.Distinct().ToList();
            } while (trigger_cross.Count < count_multipoint);
            trigger_cross.Sort();





            Boolean flag_reverse = false;



            /*
            for (int i = 0; i < child.count_step_rotate; i++)
            {
                if (flag_reverse)
                {
                    child.step_rotate[i] = another_parent.step_rotate[i];
                }

            }
            */

            int current_pos = 0;


            for (int i = 0; i < child.count_step_sko; i++)
            {

                if ((trigger_cross.Count > 0) && (current_pos == trigger_cross[0]))
                {
                    flag_reverse = !flag_reverse;
                    trigger_cross.RemoveAt(0);

                }

                if (flag_reverse)
                {
                    child.step_sko[i] = another_parent.step_sko[i];
                }
                current_pos++;
            }


            child.hrom_vector.crossover_multipoint(another_parent.hrom_vector, Rand, current_pos, trigger_cross, flag_reverse);

            return child;



        }
    }
}
