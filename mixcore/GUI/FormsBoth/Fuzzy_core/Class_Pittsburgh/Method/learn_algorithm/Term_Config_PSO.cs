using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system.Approx_Singletone.learn_algorithm.conf;
using Fuzzy_system.Class_Pittsburgh;
using Fuzzy_system.Fuzzy_Abstract.learn_algorithm.conf;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh.learn_algorithm
{
    class Term_Config_PSO : Abstract_term_config
    {
        int count_iteration = 0;
        double c1 = 0;
        double c2 = 0;
        double w = 1;
        int count_particle = 0;

        public override Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System TuneUpFuzzySystem(Fuzzy_system.Class_Pittsburgh.c_Fuzzy_System Classifier, Abstract_learn_algorithm_conf conf)
        {
            count_iteration = ((Term_Config_PSO_Search_conf)conf).Количество_итераций;
            c1 = ((Term_Config_PSO_Search_conf)conf).Коэффициент_c1;
            c2 = ((Term_Config_PSO_Search_conf)conf).Коэффициент_c2;
            w = 1;
            count_particle = ((Term_Config_PSO_Search_conf)conf).Особей_в_популяции;

            c_Fuzzy_System result = Classifier;

            Knowlege_base_CRules[] X = new Knowlege_base_CRules[count_particle];
            Knowlege_base_CRules[] V = new Knowlege_base_CRules[count_particle];
            Knowlege_base_CRules[] Pi = new Knowlege_base_CRules[count_particle];
            Knowlege_base_CRules Pg = new Knowlege_base_CRules();
            double[] Errors = new double[count_particle];
            double[] OldErrors = new double[count_particle];
            double minError = 0;
            Random rnd = new Random();
            for (int i = 0; i < count_particle; i++)
            {
                Knowlege_base_CRules temp_c_Rule = new Knowlege_base_CRules(result.Rulles_Database_Set[0]);
                X[i] = temp_c_Rule;
                Errors[i] = result.Classify_Learn_Samples(0);
                OldErrors[i] = Errors[i];
                Pi[i] = new Knowlege_base_CRules(X[i]);
                V[i] = new Knowlege_base_CRules(X[i]);
                //
                for (int j = 0; j < V[i].Terms_Set.Count; j++)
                {
                    for (int k = 0; k < c_Fuzzy_System.Count_Params_For_Term(V[i].Terms_Set[j].Term_Func_Type); k++)
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
                    double[] bf = new double[V[i].Weigth.Length];
                    for (int k = 0; k < V[i].Weigth.Length; k++)
                    {
                        if (i == 0)
                        {
                            bf[k] = 1;
                        }
                        else
                        {
                            //System.Windows.Forms.MessageBox.Show(rnd.NextDouble().ToString());
                            bf[k] = rnd.NextDouble()/200;
                        }
                    }
                    V[i].Weigth = bf;

                }
            }
            Pg = new Knowlege_base_CRules(result.Rulles_Database_Set[0]);
            minError = Errors[0];
            for (int i = 0; i < count_iteration; i++)
            {
                for (int j = 0; j < count_particle; j++)
                {
                    w = 1 / (1 + Math.Exp(-(Errors[j] - OldErrors[j]) / 0.01));
                    for (int k = 0; k < X[j].Terms_Set.Count; k++)
                    {
                        for (int q = 0; q < c_Fuzzy_System.Count_Params_For_Term(X[j].Terms_Set[k].Term_Func_Type); q++)
                        {
                            
                            double bp = Pi[j].Terms_Set[k].Parametrs[q];
                            V[j].Terms_Set[k].Parametrs[q] = V[j].Terms_Set[k].Parametrs[q] * w + c1 * rnd.NextDouble() * (bp - X[j].Terms_Set[k].Parametrs[q]) +
                                c2 * rnd.NextDouble() * (Pg.Terms_Set[k].Parametrs[q] - X[j].Terms_Set[k].Parametrs[q]);
                            X[j].Terms_Set[k].Parametrs[q] += V[j].Terms_Set[k].Parametrs[q];
                        }
                    }
                    double[] bf = new double[V[j].Weigth.Length];
                    double[] bfw = new double[V[j].Weigth.Length];
                    for (int k = 0; k < V[j].Weigth.Length; k++)
                    {

                        bfw[k] = V[j].Weigth[k] * w + c1 * rnd.NextDouble() * (Pi[j].Weigth[k] - X[j].Weigth[k]) +
                            c2 * rnd.NextDouble() * (Pg.Weigth[k] - X[j].Weigth[k]);
                        double sw = X[j].Weigth[k] + bfw[k];
                        if (sw > 0 && sw <= 2)
                        {
                            bf[k] = sw;

                        }
                        else
                        {
                            bf[k] = X[j].Weigth[k];
                            bfw[k] = V[j].Weigth[k];
                        }

                    }
                    X[j].Weigth = bf;
                    V[j].Weigth = bfw;
                    double newError = 0;
                    result.Rulles_Database_Set.Add(X[j]);
                    int temp_index = result.Rulles_Database_Set.Count - 1;
                    bool success = true;
                    try
                    {
                        newError = result.Classify_Learn_Samples(temp_index);
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

                        Pi[j] = new Knowlege_base_CRules(X[j]);
                    }
                    if (minError < newError)
                    {
                        minError = newError;
                        Pg = new Knowlege_base_CRules(X[j]);
                    }

                }

            }
            result.Rulles_Database_Set[0] = Pg;
            return result;
        }
    }
}
