using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Approx_Singletone;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.learn_algorithm
{
    class Term_Config_PSO : Abstract_learn_algorithm
    {
        int count_iteration = 0;
        double c1 = 0;
        double c2 = 0;
        double w = 1;
        int count_particle = 0;

     

        public override Fuzzy_system.Approx_Singletone.a_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Approx_Singletone.a_Fuzzy_System Approximate, Abstract_learn_algorithm_conf conf)
        {
            count_iteration = ((Term_Config_PSO_Search_conf)conf).Количество_итераций;
            c1 = ((Term_Config_PSO_Search_conf)conf).Коэффициент_c1;
            c2 = ((Term_Config_PSO_Search_conf)conf).Коэффициент_c2;
            w = 1;
            count_particle = ((Term_Config_PSO_Search_conf)conf).Особей_в_популяции;

            a_Fuzzy_System result = Approximate;

            Knowlege_base_ARules[] X = new Knowlege_base_ARules[count_particle];
            Knowlege_base_ARules[] V = new Knowlege_base_ARules[count_particle];
            Knowlege_base_ARules[] Pi = new Knowlege_base_ARules[count_particle];
            Knowlege_base_ARules Pg = new Knowlege_base_ARules();
            double[] Errors = new double[count_particle];
            double[] OldErrors = new double[count_particle];
            double minError = 0;
            Random rnd = new Random();
            for (int i = 0; i < count_particle; i++)
            {
                Knowlege_base_ARules temp_c_Rule = new Knowlege_base_ARules(result.Rulles_Database_Set[0]);
                X[i] = temp_c_Rule;
                Errors[i] = result.approx_Learn_Samples(0);
                OldErrors[i] = Errors[i];
                Pi[i] = new Knowlege_base_ARules(X[i]);
                V[i] = new Knowlege_base_ARules(X[i]);
                //
                for (int j = 0; j < V[i].Terms_Set.Count; j++)
                {
                    for (int k = 0; k < Member_Function.Count_Params_For_Term(V[i].Terms_Set[j].Term_Func_Type); k++)
                    {
                        if (i == 0)
                        {
                            V[i].Terms_Set[j].Parametrs[k] = 0;
                        }
                        else
                        {
                            V[i].Terms_Set[j].Parametrs[k] = rnd.NextDouble() - 0.5;
                        }
                    }
                    double[] bf = new double[V[i].all_conq_of_rules.Length];
                    for (int k = 0; k < V[i].all_conq_of_rules.Length; k++)
                    {
                        if (i == 0)
                        {
                            bf[k] = 1;
                        }
                        else
                        {
                            bf[k] = rnd.NextDouble()/200;
                        }
                    }
                    V[i].all_conq_of_rules = bf;
                    
                }
            }
            Pg = new Knowlege_base_ARules(result.Rulles_Database_Set[0]);
            minError = Errors[0];
            for (int i = 0; i < count_iteration; i++)
            {
                for (int j = 0; j < count_particle; j++)
                {
                    w = 1 / (1 + Math.Exp(-(Errors[j] - OldErrors[j]) / 0.01));
                    for (int k = 0; k < X[j].Terms_Set.Count; k++)
                    {
                        for (int q = 0; q < Member_Function.Count_Params_For_Term(X[j].Terms_Set[k].Term_Func_Type); q++)
                        {
                            
                            double bp = Pi[j].Terms_Set[k].Parametrs[q];
                            V[j].Terms_Set[k].Parametrs[q] = V[j].Terms_Set[k].Parametrs[q] * w + c1 * rnd.NextDouble() * (bp - X[j].Terms_Set[k].Parametrs[q]) +
                                c2 * rnd.NextDouble() * (Pg.Terms_Set[k].Parametrs[q] - X[j].Terms_Set[k].Parametrs[q]);
                            X[j].Terms_Set[k].Parametrs[q] += V[j].Terms_Set[k].Parametrs[q];
                        }
                    }
                    double[] bf = new double[V[j].all_conq_of_rules.Length];
                    double[] bfw = new double[V[j].all_conq_of_rules.Length];
                    for (int k = 0; k < V[j].all_conq_of_rules.Length; k++)
                    {

                        bfw[k] = V[j].all_conq_of_rules[k] * w + c1 * rnd.NextDouble() * (Pi[j].all_conq_of_rules[k] - X[j].all_conq_of_rules[k]) +
                            c2 * rnd.NextDouble() * (Pg.all_conq_of_rules[k] - X[j].all_conq_of_rules[k]);
                        double sw = X[j].all_conq_of_rules[k] + bfw[k];
                        if (sw > 0 && sw <= 2)
                        {
                            bf[k] = sw;

                        }
                        else
                        {
                            bf[k] = X[j].all_conq_of_rules[k];
                            bfw[k] = V[j].all_conq_of_rules[k];
                        }

                    }
                    X[j].all_conq_of_rules = bf;
                    V[j].all_conq_of_rules = bfw;
                    double newError = 0;
                    result.Rulles_Database_Set.Add(X[j]);
                    int temp_index = result.Rulles_Database_Set.Count - 1;
                    bool success = true;
                    try
                    {
                        newError = result.approx_Learn_Samples(temp_index);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                    result.Rulles_Database_Set.RemoveAt(temp_index);
                    if (success && (newError > Errors[j]))
                    {
                        OldErrors[j] = Errors[j];
                        Errors[j] = newError;

                        Pi[j] = new Knowlege_base_ARules(X[j]);
                    }
                    if (minError < newError)
                    {
                        minError = newError;
                        Pg = new Knowlege_base_ARules(X[j]);
                    }

                }

            }
            result.Rulles_Database_Set[0] = Pg;
            GC.Collect();
            return result;
        }


        public override string ToString(bool with_param = false)
        {

            if (with_param)
            {
                string result = "роящиеся частицы {";
                result += "Итераций= " + count_iteration.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c1= " + c1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c2= " + c2.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_particle.ToString() + " ;" + Environment.NewLine;
               


                result += "}";
                return result;
            }
            return "роящиеся частицы";
        }


    }
}
