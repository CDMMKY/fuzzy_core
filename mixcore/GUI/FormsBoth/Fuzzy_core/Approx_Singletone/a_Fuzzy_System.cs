using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using Fuzzy_system.Approx_Singletone.add_generators.conf;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;
using Fuzzy_system.Fuzzy_Abstract.add_generators.conf;

namespace Fuzzy_system.Approx_Singletone
{
    public class a_Fuzzy_System : Fuzzy_System
    {
        #region Visible public methods

     /*   public int Count_Rulles_Databases
        {
            get { return rulles_database_set.Count(); }
        }
        */




        public new a_samples_set Learn_Samples_set
        {
            get { return learn_samples_set; }
        }

        public new a_samples_set Test_Samples_set
        {
            get { return test_samples_set; }
        }

        public override int Count_Rules(int index = 0)
        {
            return rulles_database_set[index].Rules_Database.Count;
        }

        public int Count_Rulles_Databases
        {
            get { return rulles_database_set.Count(); }
        }

        public List<Knowlege_base_ARules> Rulles_Database_Set { get { return rulles_database_set; } }

       
      /*  public int Count_Samples
        {
            get { return learn_samples_set.Count_Samples; }
        }*/
    /*    public int value_complexity(int index = 0)
        {
            return rulles_database_set[index].Rules_Database.Count + rulles_database_set[index].Terms_Set.Count;
        }
        */
      /*  public int Count_Vars
        {
            get { return learn_samples_set.Count_Vars; }
        }
        */
       

         public override int value_complexity(int index = 0)
        {
            return rulles_database_set[index].Rules_Database.Count + rulles_database_set[index].Terms_Set.Count;
        }


       



        #region constructor

     

        public a_Fuzzy_System(a_samples_set learn_set, a_samples_set test_set):base(learn_set,test_set)
        {
            learn_samples_set = learn_set;
            if (test_set != null)
            {
                test_samples_set = test_set;
                for (int i = 0; i < Count_Vars; i++)
                {

                    if (
                        !learn_samples_set.Input_Attributes[i].Name.Equals(test_samples_set.Input_Attributes[i].Name,
                                                                           StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }

                }
            }

        }

        #endregion

        #region init methods

        public void Init_Rules_everyone_with_everyone(Abstract_generator_conf conf)
        { 
        }

        public void Init_Rules_everyone_with_everyone(Type_Term_Func_Enum type_func, int[] count_slice__for_var)
        {
            if (Count_Rulles_Databases == 0)
            {
                Knowlege_base_ARules temp_rules = new Knowlege_base_ARules();
                rulles_database_set.Add(temp_rules);
            }
            int[][] position_of_terms = new int[Count_Vars][];
            for (int i = 0; i < Count_Vars; i++)
            {
                position_of_terms[i] = new int[count_slice__for_var[i]];
                double current_value = learn_samples_set.Attribute_Min(i);
                double coeef = (learn_samples_set.Attribute_Scatter(i)) /
                               (count_slice__for_var[i] - 1);
                for (int j = 0; j < count_slice__for_var[i]; j++)
                {
                    double[] parametrs = new double[Member_Function.Count_Params_For_Term(type_func)];
                    switch (type_func)
                    {
                        case Type_Term_Func_Enum.Треугольник:
                            parametrs[1] = current_value;
                            parametrs[0] = parametrs[1] - coeef;
                            parametrs[2] = parametrs[1] + coeef;
                            break;
                        case Type_Term_Func_Enum.Гауссоида:
                            parametrs[0] = current_value;
                            parametrs[1] = coeef / 3;

                            break;
                        case Type_Term_Func_Enum.Парабола:
                            parametrs[0] = current_value - coeef;
                            parametrs[1] = current_value + coeef;

                            break;
                        case Type_Term_Func_Enum.Трапеция:
                            parametrs[0] = current_value - coeef;
                            parametrs[3] = current_value + coeef;
                            parametrs[1] = parametrs[0] + 0.4 * (parametrs[3] - parametrs[0]);
                            parametrs[2] = parametrs[0] + 0.6 * (parametrs[3] - parametrs[0]);

                            break;

                    }
                    Term temp_term = new Term(parametrs, type_func, i);
                    rulles_database_set[0].Terms_Set.Add(temp_term);
                    position_of_terms[i][j] = rulles_database_set[0].Terms_Set.Count - 1;

                    current_value += coeef;
                }
            }

            int[] counter = new int[Count_Vars];
            for (int i = 0; i < Count_Vars; i++)
            {
                counter[i] = count_slice__for_var[i] - 1;
            }
            while (counter[0] >= 0)
            {
                List<Term> temp_term_set = new List<Term>();
                int[] order = new int[Count_Vars];
                for (int i = 0; i < Count_Vars; i++)
                {
                    temp_term_set.Add(rulles_database_set[0].Terms_Set[position_of_terms[i][counter[i]]]);
                    order[i] = position_of_terms[i][counter[i]];
                }
                double approx_Values = Nearest_Approx(temp_term_set);

                ARule temp_rule = new ARule(rulles_database_set[0].Terms_Set, order, approx_Values);
                rulles_database_set[0].Rules_Database.Add(temp_rule);
                counter = dec_count(counter, count_slice__for_var);
            }


        } //Core init method

     /*   private int[] dec_count(int[] counter, int[] slice_count)
        {
            int[] result = counter;
            int j = Count_Vars - 1;
            result[j]--;
            while ((result[j] < 0) && (j > 0))
            {
                result[j] = slice_count[j] - 1;
                j--;
                result[j]--;
            }
            return result;
        }

        */


        public double Nearest_Approx(List<Term> term_set)
        {
            double min_diff = double.PositiveInfinity;
            int min_diff_index = 0;

            for (int c = 0; c < learn_samples_set.Count_Samples; c++)
            {
                double current_diff = 0;
                for (int i = 0; i < term_set.Count; i++)
                {
                    if (term_set[i] != null)
                    {
                        switch (term_set[i].Term_Func_Type)
                        {
                            case Type_Term_Func_Enum.Треугольник:
                                current_diff +=
                                    Math.Abs(learn_samples_set.Data_Rows[c].Input_Attribute_Value[term_set[i].Number_of_Input_Var] -
                                             term_set[i].Parametrs[1]);
                                break;
                            case Type_Term_Func_Enum.Гауссоида: current_diff +=
                               Math.Abs(learn_samples_set.Data_Rows[c].Input_Attribute_Value[term_set[i].Number_of_Input_Var] -
                                        term_set[i].Parametrs[0]);

                                break;
                            case Type_Term_Func_Enum.Парабола:
                                double argv = (term_set[i].Parametrs[0] + term_set[i].Parametrs[1]) / 2;
                                current_diff +=
                               Math.Abs(learn_samples_set.Data_Rows[c].Input_Attribute_Value[term_set[i].Number_of_Input_Var] -
                                        argv);
                                break;
                            case Type_Term_Func_Enum.Трапеция: double argvTR = (term_set[i].Parametrs[1] + term_set[i].Parametrs[2]) / 2;
                                current_diff +=
                               Math.Abs(learn_samples_set.Data_Rows[c].Input_Attribute_Value[term_set[i].Number_of_Input_Var] -
                                        argvTR);
                                break;
                        }
                    }




                }
                if (current_diff < min_diff)
                {
                    min_diff = current_diff;
                    min_diff_index = c;
                }
            }
            return learn_samples_set.Data_Rows[min_diff_index].Approx_Value;
        }

        #endregion
        #region Approx
        public double approx_Test_Samples(int number_of_rules_database = 0)
        {
            double result = 0;
            for (int i = 0; i < test_samples_set.Count_Samples; i++)
            {

                double temp_value = Math.Pow(test_samples_set.Data_Rows[i].Approx_Value - approx_base(test_samples_set.Data_Rows[i].Input_Attribute_Value, number_of_rules_database), 2);

                result += temp_value;
            }
            return (double)Math.Sqrt(result) / test_samples_set.Count_Samples;
        }


        public double approx_Learn_Samples(int number_of_rules_database = 0) //attention only for multicore processor optimized 
        {

            int count_save_using_processors;
            if (Environment.ProcessorCount > 1)
            {
                count_save_using_processors = Environment.ProcessorCount - 1;
            }
            else
            {
                count_save_using_processors = 1;
            }
            int current_pos = 0;
            int coeff = learn_samples_set.Count_Samples / count_save_using_processors;
            Thread[] approx_threads = new Thread[count_save_using_processors];
            temperal_results_for_approx = new double[count_save_using_processors];
            for (int i = 0; i < count_save_using_processors; i++)
            { //attention don't delete this is protection of late binding limbda code effects
                int z = i;
                int start = current_pos;
                int end = 0;
                int num_of_rul_db = number_of_rules_database;
                if (current_pos + coeff < learn_samples_set.Count_Samples)
                {
                    end = current_pos + coeff;
                }
                else
                {
                    end = learn_samples_set.Count_Samples;
                }
                approx_threads[i] =
                           new Thread(
                               () =>
                               {
                                   approx_part_of_learn_Samples(start, end,
                                                                                  num_of_rul_db, z);
                               });
                try
                {
                    approx_threads[i].Start();

                }
                catch (Exception)
                {
                }
                current_pos += coeff;
            }
            bool theads_is_run = true;
            while (theads_is_run)
            {
                theads_is_run = false;
                for (int i = 0; i < count_save_using_processors; i++)
                {
                    theads_is_run = theads_is_run || approx_threads[i].IsAlive;
                }

            }
            double sum = temperal_results_for_approx.Sum();
            double result = (double)Math.Sqrt(sum) / learn_samples_set.Count_Samples;
            return result;

        }


        private void approx_part_of_learn_Samples(int start, int end, int number_of_rules_database, int thread)
        {
            double result = 0;
            for (int i = start; i < end; i++)
            {
                double temp_value = Math.Pow(learn_samples_set.Data_Rows[i].Approx_Value - approx_base(learn_samples_set.Data_Rows[i].Input_Attribute_Value, number_of_rules_database), 2);

                result += temp_value;
            }

            temperal_results_for_approx[thread] = result;
        }



        public double approx_base(double[] object_c, int num_vector = 0)
        {


            List<ARule> temp_all_rules_for_chosen_approx =
                    rulles_database_set[num_vector].Rules_Database;

            double sum = 0;
            double sum2 = 0;
            for (int k = 0; k < temp_all_rules_for_chosen_approx.Count; k++)
            {
                double mul = 1;
                for (int q = 0; q < temp_all_rules_for_chosen_approx[k].Term_of_Rule_Set.Count; q++)
                {
                    double[] par;
                    mul *= Member_Function.Func((int)temp_all_rules_for_chosen_approx[k].Term_of_Rule_Set[q].Term_Func_Type, temp_all_rules_for_chosen_approx[k].Term_of_Rule_Set[q].Parametrs
                              , object_c[temp_all_rules_for_chosen_approx[k].Term_of_Rule_Set[q].Number_of_Input_Var], out par);
                    if (mul == 0)
                    {
                        break;
                    }
                }
                sum2 += mul;
                sum += mul * temp_all_rules_for_chosen_approx[k].Kons_approx_Value;
            }
            if (sum2 != 0)
            {
                return sum / sum2;
            }

            
            return double.MaxValue;

        }







        #endregion





        public void unlaid_protection_fix(int number_of_database = 0)
        {
            unlaid_protection_fix_max_min_border(number_of_database);
            unlaid_protection_in_middle(number_of_database);

        }






        public static a_Fuzzy_System Load_approx_FS(XmlTextReader RXML, string file_name)
        {
            throw (new NotImplementedException("Not relised"));
        }


        #endregion

        #region  private interstruct

        private void unlaid_protection_fix_max_min_border(int number_of_database = 0)
        {
            for (int i = 0; i < Count_Vars; i++)
            {
                List<Term> all_terms_for_var =
                      rulles_database_set[number_of_database].Terms_Set.FindAll(x => x.Number_of_Input_Var == i);
                if (all_terms_for_var.Find(x => x.Term_Func_Type == Type_Term_Func_Enum.Гауссоида) != null)
                {
                    continue;

                }
                else
                {
                    double min = all_terms_for_var.Min(x => x.Parametrs[0]);
                    int min_index = rulles_database_set[number_of_database].Terms_Set.FindIndex(x => (x.Parametrs[0] == min) && (x.Number_of_Input_Var == i));
                    rulles_database_set[number_of_database].Terms_Set[min_index].Parametrs[0] =
                        Learn_Samples_set.Attribute_Min(i) - 0.001 * Learn_Samples_set.Attribute_Scatter(i);

                    double max = double.NegativeInfinity;
                    Term current_term = null;
                    for (int j = 0; j < all_terms_for_var.Count; j++)
                    {
                        double current_max = all_terms_for_var[j].Max;
                        if (max < current_max)
                        {
                            max = current_max;
                            current_term = all_terms_for_var[j];
                        }

                    }
                    current_term.Max = Learn_Samples_set.Attribute_Max(i) +
                         0.001 * Learn_Samples_set.Attribute_Scatter(i);
               }
            }

        }


        private void unlaid_protection_in_middle(int number_of_database = 0)
        { Knowlege_base_ARules current_database = rulles_database_set[number_of_database];
        for (int i = 0; i < Count_Vars; i++)
        {
            List<Term> current_terms = current_database.Terms_Set.FindAll(x => x.Number_of_Input_Var == i);
                if (current_terms.Exists(x=>x.Term_Func_Type== Type_Term_Func_Enum.Гауссоида)) {continue;}

            for (int j = 0; j < current_terms.Count-1; j++)
            {
            
                if ((current_terms[j].Max<current_terms[j+1].Min)) {
                    double temp =current_terms[j].Max;
                    current_terms[j].Max = current_terms[j+1].Min;
                    current_terms[j+1].Min=temp;
                }
                if (current_terms[j].Max==current_terms[j+1].Min)
                {current_terms[j].Max+= learn_samples_set.Attribute_Scatter(i)*0.001;
                    current_terms[j+1].Min-= learn_samples_set.Attribute_Scatter(i)*0.001;

                }
                   }
           
        }
       }

        protected new List<Knowlege_base_ARules> rulles_database_set = new List<Knowlege_base_ARules>();
        protected new a_samples_set learn_samples_set;
        protected new a_samples_set test_samples_set;
         protected double[] temperal_results_for_approx;
        #endregion


    }
}
