using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system.Approx_Singletone.learn_algorithm.ES;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm.ES
{
    class Population
    {
        List<Individ> the_popualate;
        int size_populate;
        int size_child;
        List <Individ> the_parents;
        List <Individ> the_childs;
        int count_vars;
        a_samples_set Data;

        Individ best_individ; 

        public Population(int count_population, int count_child, int count_v, a_samples_set data)
        {
            size_populate = count_population;
            size_child = count_child;
            the_popualate = new List<Individ>(count_population);
            the_parents = new List<Individ>(count_population);
            the_childs = new List<Individ>(count_child);
            count_vars = count_v;
            Data = data;
        }
        public double Calc_Error(a_Fuzzy_System error_checker)
        {
            Individ temp_Best=null;
            double min_error = double.PositiveInfinity;
            foreach (Individ indiv in the_popualate)
            {indiv.calc_Error(error_checker);
                if (min_error >indiv.error )
                {
                    min_error = indiv.error;
                    temp_Best = indiv;
                }

            }


            if (temp_Best != null)
            {
                if (best_individ != null)
                { if (temp_Best.error < best_individ.error) { best_individ = temp_Best; } }
                else { best_individ = temp_Best; }
            }


            return min_error;
        }


        public void init_first(Knowlege_base_ARules base_rule, Random rand, Individ.Type_init prm_init)
        { 
            bool the_first=true ;

            int  count_ready=0;
            do
            {
                the_popualate.Add(new Individ(base_rule, Data, count_vars, the_first, rand, prm_init));

                count_ready++;
               the_first = false;
            } while (count_ready < size_populate);


        }


    


        public void select_parents_and_crossover(Random Rand, Individ.Alg_crossover Alg_cross,  double param)
        {
            the_childs.Clear();
            for (int i =0;i<size_child;i++)
        {
            int choose_one = Rand.Next(size_populate);
             int choose_two = Rand.Next(size_populate);

            the_childs.Add( the_popualate[choose_one].crossover(the_popualate[choose_two],Alg_cross,  Rand, param ));

        }
        }

        public void mutate_all(Random rand,double tau1, double tau2, Individ.Type_Mutate type_mutate, double b_ro)
        {
            for (int i = 0; i < the_childs.Count; i++)
            {
                the_childs[i].mutate(rand, tau1, tau2, type_mutate, b_ro);
            }
        }
        public void union_parent_and_child()
        {
            the_popualate.AddRange(the_childs);
 
        }

        public void select_global()
        {
            double [] keys = new double  [the_popualate.Count]; 
            int [] item_num = new int [the_popualate.Count];

            for (int i=0; i<the_popualate.Count;i++)
            { item_num[i]=i;
                keys[i]= the_popualate[i].error;

            }
            Array.Sort(keys,item_num);

            for (int i = 0; i < size_populate; i++)
            {
                the_popualate[i] = the_popualate[item_num[i]]; 
            }
            the_popualate.RemoveRange(size_populate, the_popualate.Count - size_populate);
        }

        public Knowlege_base_ARules get_best_database()
        {
            return best_individ.hrom_vector.Core_Check; 
        }
       
    }
}
