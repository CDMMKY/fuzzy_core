using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm.ES
{
    public class Hromosom
    {
        KnowlegeBaseSARules core_check;
        public KnowlegeBaseSARules Core_Check { get { return core_check; } }
        List<Term> Fenotip_terms;
        List<double> Fenotip_kons;

        public Hromosom(KnowlegeBaseSARules Source)
        {
            core_check = Source;
            Fenotip_kons = core_check.all_conq_of_rules.ToList();
            Fenotip_terms = core_check.TermsSet.ToList();


        }



        public Hromosom(Hromosom the_Source)
        {
            core_check = new KnowlegeBaseSARules(the_Source.core_check);
            Fenotip_kons = core_check.all_conq_of_rules.ToList();
            Fenotip_terms = core_check.TermsSet.ToList();


        }


        public void init_random(Random rand, SampleSet Data)
        {
            for (int j = 0; j < Fenotip_terms.Count; j++)
            {
                Term temp_term = Fenotip_terms[j];
                for (int i = 0; i < temp_term.Parametrs.Count(); i++)
                {

                    temp_term.Parametrs[i] = Data.InputAttributes[temp_term.NumVar].Min + Data.InputAttributes[temp_term.NumVar].Scatter * rand.NextDouble();
                }

            }

            for (int j = 0; j < Fenotip_kons.Count; j++)
            {
                Fenotip_kons[j] = Data.OutputAttribute.Min + Data.OutputAttribute.Scatter * rand.NextDouble();
            }


        }

        public void init_constrain(Random rand, SampleSet Data)
        {
            for (int j = 0; j < Fenotip_terms.Count; j++)
            {
                Term temp_term = Fenotip_terms[j];
                for (int i = 0; i < temp_term.Parametrs.Count(); i++)
                {

                    temp_term.Parametrs[i] = temp_term.Parametrs[i] + Data.InputAttributes[temp_term.NumVar].Scatter * (rand.NextDouble() - 0.5) * 0.1;
                }

            }

     

        }









        public Hromosom crossover_uniform(Hromosom another_Hrom, Random Rand, double level_cross)
        {
            Hromosom child_hrom = new Hromosom(this);

            for (int i = 0; i < Fenotip_terms.Count; i++)
            {
                if (Rand.NextDouble() < level_cross)
                {
                    child_hrom.Fenotip_terms[i] = another_Hrom.Fenotip_terms[i];
                }
            }

            for (int i = 0; i < Fenotip_kons.Count; i++)
            {
                if (Rand.NextDouble() < level_cross)
                {
                    child_hrom.Fenotip_kons[i] = another_Hrom.Fenotip_kons[i];
                }
            }




            return child_hrom;



        }






        public Hromosom crossover_multipoint(Hromosom another_Hrom, Random Rand, int current_pos, List<int> trigger_cross, bool flag_reverse)
        {
            Hromosom child_hrom = new Hromosom(this);


            for (int i = 0; i < Fenotip_terms.Count; i++)
            {

                if ((trigger_cross.Count > 0) && (current_pos == trigger_cross[0]))
                {
                    flag_reverse = !flag_reverse;
                    trigger_cross.RemoveAt(0);

                }



                if (flag_reverse)
                {
                    child_hrom.Fenotip_terms[i] = another_Hrom.Fenotip_terms[i];
                }
                current_pos++;
            }

            for (int i = 0; i < Fenotip_kons.Count; i++)
            {

                if ((trigger_cross.Count > 0) && (current_pos == trigger_cross[0]))
                {
                    flag_reverse = !flag_reverse;
                    trigger_cross.RemoveAt(0);

                }



                if (flag_reverse)
                {
                    child_hrom.Fenotip_kons[i] = another_Hrom.Fenotip_kons[i];
                }
                current_pos++;

            }




            return child_hrom;



        }


        public Hromosom mutate_SKO(List<double> sko, Random rand)
        {
            for (int i = 0; i < Fenotip_terms.Count; i++)
            {
                for (int j = 0; j < Fenotip_terms[i].Parametrs.Count(); j++)
                {
                    Fenotip_terms[i].Parametrs[j] = Fenotip_terms[i].Parametrs[j] +
                      sko[Fenotip_terms[i].NumVar] *
                      GaussRandom.Random_gaussian(rand);
                }
            }


            for (int i = 0; i < Fenotip_kons.Count; i++)
            {
                Fenotip_kons[i] = GaussRandom.Random_gaussian(rand, Fenotip_kons[i]);
            }

            return this;
        }




        public Hromosom mutate_SKO_RO(List<double> Covariance_matrix_pruned, Random rand)
        {
            for (int i = 0; i < Fenotip_terms.Count; i++)
            {
                for (int j = 0; j < Fenotip_terms[i].Parametrs.Count(); j++)
                {
                    Fenotip_terms[i].Parametrs[j] = Fenotip_terms[i].Parametrs[j] +
                      
                      GaussRandom.Random_gaussian(rand,0,Covariance_matrix_pruned[Fenotip_terms[i].NumVar]);
                }
            }


            for (int i = 0; i < Fenotip_kons.Count; i++)
            {
                Fenotip_kons[i] = GaussRandom.Random_gaussian(rand, Fenotip_kons[i], Fenotip_kons[i]*0.1);
            }

            return this;
        }
    }

}
