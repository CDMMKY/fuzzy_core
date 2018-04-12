using System;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh
{
    public class c_Fuzzy_System:Fuzzy_System
    {
        #region Visible public methods

       /* public int Count_Rulles_Databases
        {
            get { return rulles_database_set.Count(); }
        }
        */

        public List<Knowlege_base_CRules> Rulles_Database_Set { get { return rulles_database_set; } }

        public override int value_complexity(int index = 0)
        {
            return rulles_database_set[index].Rules_Database.Count + rulles_database_set[index].Terms_Set.Count;
        }

        public int Count_Rulles_Databases
        {
            get { return rulles_database_set.Count(); }
        }



        public override int Count_Rules(int index = 0)
        {
            return rulles_database_set[index].Rules_Database.Count;
        }


        public int Count_Class
        {
            get { return learn_samples_set.Count_Class; }
        }
        
        public new c_samples_set Learn_Samples_set {get {return learn_samples_set; }
        }

        public new c_samples_set Test_Samples_set
        {
            get { return test_samples_set; }
        }

       /* public int Count_Rules(int index)
        {
            return rulles_database_set[index].Rules_Database.Count;
        }
        */
       /* public int Count_Samples
        {
            get { return learn_samples_set.Count_Samples; }
        }
        */
       /* public int Count_Vars
        {
            get { return learn_samples_set.Count_Vars; }
        }
        */
    
    

        public static int Count_Params_For_Term(Type_Term_Func_Enum type_term)
        {
            switch (type_term)
            {
                case Type_Term_Func_Enum.Треугольник:
                    return 3;
                case Type_Term_Func_Enum.Гауссоида:
                    return 2;
                case Type_Term_Func_Enum.Парабола:
                    return 2;
                case Type_Term_Func_Enum.Трапеция:
                    return 4;

            }
            return 0;
        }



        #region constructor 

      
        public c_Fuzzy_System(c_samples_set learn_set, c_samples_set test_set):base(learn_set,test_set)
        {
            learn_samples_set = learn_set;
            if (test_set != null)
            {
                test_samples_set = test_set;
                for (int i = 0; i < Count_Vars; i++)
                {

                    if (
                        !learn_samples_set.Input_Attribute(i).Name.Equals(test_samples_set.Input_Attribute(i).Name,
                                                                           StringComparison.OrdinalIgnoreCase))
                    {
                        throw (new InvalidEnumArgumentException("Атрибуты обучающей таблицы и тестовой не совпадают"));
                    }

                }
            }

        }

        #endregion

        #region init methods

        public void Init_Rules_By_samples(Type_Term_Func_Enum type_func)
        {
            if (Count_Rulles_Databases == 0)
            {
                Knowlege_base_CRules temp_rules = new Knowlege_base_CRules();
                rulles_database_set.Add(temp_rules);
            }

            for (int i = 0; i < Count_Class; i++)
            {
                if (!learn_samples_set.exist_class_var_max(i) || !learn_samples_set.exist_class_var_min(i))
                {
                    continue;
                    
                }
                int[] order_terms = new int[Count_Vars];

                for (int j = 0; j < Count_Vars; j++)
                {
                    double[] paramerts = new double[c_Fuzzy_System.Count_Params_For_Term(type_func)];
                    switch (type_func)
                    {
                        case Type_Term_Func_Enum.Треугольник:
                            paramerts[0] = learn_samples_set.get_class_var_min(i, j) - 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            paramerts[2] = learn_samples_set.get_class_var_max(i, j) + 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            paramerts[1] = (paramerts[0] + paramerts[2])/2;
                            break;
                        case Type_Term_Func_Enum.Гауссоида:
                            paramerts[0] = (learn_samples_set.get_class_var_max(i, j) +
                                            learn_samples_set.get_class_var_min(i, j))/2;
                            paramerts[1] = (paramerts[0] - learn_samples_set.get_class_var_min(i, j))/3; // rule of 3g
                            break;
                        case Type_Term_Func_Enum.Парабола:
                            paramerts[0] = learn_samples_set.get_class_var_min(i, j) - 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            paramerts[1] = learn_samples_set.get_class_var_max(i, j) + 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            break;
                        case Type_Term_Func_Enum.Трапеция:
                            paramerts[0] = learn_samples_set.get_class_var_min(i, j) - 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            paramerts[3] = learn_samples_set.get_class_var_max(i, j) + 0.001 * (learn_samples_set.get_class_var_max(i, j) - learn_samples_set.get_class_var_min(i, j));
                            paramerts[1] = paramerts[0] + 0.4*(paramerts[3] - paramerts[0]);
                            paramerts[2] = paramerts[0] + 0.6*(paramerts[3] - paramerts[0]);
                            break;

                    }
                    Term temp_term = new Term(paramerts, type_func,j);
                    rulles_database_set[0].Terms_Set.Add(temp_term);
                    order_terms[j] = rulles_database_set[0].Terms_Set.Count - 1;

                }
                CRule temp_Rule = new CRule(rulles_database_set[0].Terms_Set, order_terms,
                                                          learn_samples_set.Output_Attributes.labels_values[i], 1);
                rulles_database_set[0].Rules_Database.Add(temp_Rule);
            }

        }

        public void Init_Rules_everyone_with_everyone(Type_Term_Func_Enum type_func, int [] count_slice__for_var)
        {
            if (Count_Rulles_Databases == 0)
            {
                Knowlege_base_CRules temp_rules = new Knowlege_base_CRules();
                rulles_database_set.Add(temp_rules);
            }
            int[][] position_of_terms= new int[Count_Vars][];
            for (int i=0;i<Count_Vars;i++)
            {position_of_terms[i]= new int[count_slice__for_var[i]];
                double current_value = learn_samples_set.Attribute_Min(i);
                double coeef = (learn_samples_set.Attribute_Max(i) - learn_samples_set.Attribute_Min(i))/
                               (count_slice__for_var[i] - 1);
                for (int j=0; j< count_slice__for_var[i];j++)
                {double [] parametrs = new double[Count_Params_For_Term(type_func) ];
                    switch (type_func)
                    {
                        case Type_Term_Func_Enum.Треугольник:
                            parametrs[1] = current_value;
                            parametrs[0] = parametrs[1] - coeef;
                            parametrs[2] = parametrs[1] + coeef;
                            break;
                        case Type_Term_Func_Enum.Гауссоида:
                            parametrs[0] = current_value;
                            parametrs[1] = coeef/3;

                            break;
                        case Type_Term_Func_Enum.Парабола:
                            parametrs[0] = current_value - coeef;
                            parametrs[1] = current_value + coeef;

                            break;
                        case Type_Term_Func_Enum.Трапеция:
                            parametrs[0] = current_value - coeef;
                            parametrs[3] = current_value + coeef;
                            parametrs[1] = parametrs[0]+ 0.4*(parametrs[3] - parametrs[0]);
                            parametrs[2] = parametrs[0]+ 0.6*(parametrs[3] - parametrs[0]);
                           
                            break;

                    }
                    Term temp_term = new Term(parametrs, type_func,i);
                    rulles_database_set[0].Terms_Set.Add(temp_term);
                    position_of_terms[i][j] = rulles_database_set[0].Terms_Set.Count - 1;
                    
                    current_value += coeef;
                }
            }

            int [] counter = new int[Count_Vars];
            for (int i =0; i<Count_Vars;i++)
            {counter [i] = count_slice__for_var[i]-1;
            }
            while (counter[0]>=0)
            {List<Term > temp_term_set= new List<Term>();
                int [] order=new int[Count_Vars];
                for (int i =0; i<Count_Vars;i++)
                {temp_term_set.Add(rulles_database_set[0].Terms_Set[ position_of_terms[i][counter[i]] ] );
                    order[i] = position_of_terms[i][counter[i]];
                }
                string class_label = Nearest_Class(temp_term_set);
                
                CRule temp_rule = new CRule(rulles_database_set[0].Terms_Set,order,class_label,1.0);
                rulles_database_set[0].Rules_Database.Add(temp_rule);
                counter = dec_count(counter, count_slice__for_var);
            }


        }

  /*       private int  [] dec_count(int [] counter, int [] slice_count)
        {
            int[] result = counter;
            int j = Count_Vars - 1;
            result[j]--;
            while ((result[j]<0) &&(j>0) )
            {
                result[j] = slice_count[j] - 1;
                j--;
                result[j]--;
            }
            return result;
        }

        */
        public string Nearest_Class (List <Term>  term_set  )
        {
            double min_diff = double.PositiveInfinity;
            int min_diff_index=0;

            for (int c =0; c<learn_samples_set.Count_Samples;c++)
            {
                double current_diff = 0;
                for (int i = 0; i < term_set.Count; i++)
                {
                    if (term_set[i]!=null)
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
                                double argv = (term_set[i].Parametrs[0] + term_set[i].Parametrs[1])/2;
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
                if (current_diff<min_diff)
                {
                    min_diff = current_diff;
                    min_diff_index = c;
                }
            }
            return learn_samples_set.Data_Rows[min_diff_index].Class_Label;
        }

        #endregion
        #region Classify
        public double Classify_Test_Samples(int number_of_rules_database = 0 )
        {
            int result = 0;
            for (int i=0;i<test_samples_set.Count_Samples;i++)
            if (test_samples_set.Data_Rows[i].Class_Label.Equals(classify_base(test_samples_set.Data_Rows[i].Input_Attribute_Value,number_of_rules_database)))
            {
                result++;
            }
            return (double) result/test_samples_set.Count_Samples*100;
        }


        public double Classify_Learn_Samples(int number_of_rules_database = 0) //attention only for multicore processor optimized 
        {
            
            int count_save_using_processors;
             if (Environment.ProcessorCount>1)
             {
                 count_save_using_processors = Environment.ProcessorCount - 1;
             }
             else
             {
                 count_save_using_processors = 1;
             }
            int current_pos = 0;
            int coeff = learn_samples_set.Count_Samples/count_save_using_processors;
            Thread [] classificate_threads= new Thread[count_save_using_processors];
            temperal_results_for_classificate= new int[count_save_using_processors];
            for (int i = 0; i < count_save_using_processors;i++ )
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
                classificate_threads[i] =
                           new Thread(
                               () =>
                               {
                                   classify_part_of_learn_Samples(start, end,
                                                                                  num_of_rul_db, z);
                               });
                try
                {
                    classificate_threads[i].Start();
                 
                }
                catch(Exception )
                {
                }
                current_pos += coeff;
            }
            bool theads_is_run = true;
            while (theads_is_run)
            {
                theads_is_run = false;
                for (int i=0; i<count_save_using_processors;i++)
            {
                theads_is_run = theads_is_run || classificate_threads[i].IsAlive;
            }
                
            }
            int sum = temperal_results_for_classificate.Sum();
            

            double result = (double)sum/learn_samples_set.Count_Samples *100;
            return result;

        }

        
        private  void classify_part_of_learn_Samples(int start, int end,int number_of_rules_database , int thread)
        {
           int result = 0;
            for (int i =start;i<end;i++)
            {
               if  (learn_samples_set.Data_Rows[i].Class_Label.Equals(classify_base(learn_samples_set.Data_Rows[i].Input_Attribute_Value, number_of_rules_database),StringComparison.OrdinalIgnoreCase))
               {
                   result++;
               }
            }
           
            temperal_results_for_classificate[thread] = result;
        }



        public string classify_base(double[] object_c, int num_vector = 0)
        {
            int index_class = 0;
            double max_sum = 0;
            bool correct_classificate = false;
            for (int j = 0; j < Count_Class; j++)
            {  List< 
                CRule > temp_all_rules_for_chosen_class =
                    rulles_database_set[num_vector].Rules_Database.FindAll(
                        x => x.Label_of_Class == learn_samples_set.Output_Attributes.labels_values[j]);
          
                double sum = 0;
                for (int k = 0; k < temp_all_rules_for_chosen_class.Count; k++)
                {
                    double mul = 1;
                    for (int q = 0; q < temp_all_rules_for_chosen_class[k].Term_of_Rule_Set.Count; q++)
                    {
                        double[] par;
                        mul *= Member_Function.Func((int)temp_all_rules_for_chosen_class[k].Term_of_Rule_Set[q].Term_Func_Type ,temp_all_rules_for_chosen_class[k].Term_of_Rule_Set[q].Parametrs
                                  , object_c[temp_all_rules_for_chosen_class[k].Term_of_Rule_Set[q].Number_of_Input_Var], out par);
                      if (mul==0)
                      {break;
                      }
                    }
                    sum += mul*temp_all_rules_for_chosen_class[k].CF;
                }
                if (max_sum < sum)
                {
                    correct_classificate = true;
                    max_sum = sum;
                    index_class = j;
                }

            }
            if (!correct_classificate)
            {
                return "nonResult";
                //throw (new ArgumentException(
                //  "Невозможно классифицировать объект скорее всего существуют не накрытые участи на признаках"));

            }
            return learn_samples_set.Output_Attributes.labels_values[index_class];
        }



        

        #endregion

        
public void unlaid_protection_fix(int number_of_database=0)
        { 
            for (int i =0; i<Count_Vars;i++)
            { List< Term > all_terms_for_var =
                    rulles_database_set[number_of_database].Terms_Set.FindAll(x => x.Number_of_Input_Var == i);
              if (all_terms_for_var.Find(x=>x.Term_Func_Type== Type_Term_Func_Enum.Гауссоида)!=null)
              {
                  continue;
                  
              }
              else
              {
                  double min = all_terms_for_var.Min(x => x.Parametrs[0]);
                  int min_index = rulles_database_set[number_of_database].Terms_Set.FindIndex(x => (x.Parametrs[0] == min) &&(x.Number_of_Input_Var==i));
                  rulles_database_set[number_of_database].Terms_Set[min_index].Parametrs[0] =
                      Learn_Samples_set.Attribute_Min(i) -
                      0.001*(Learn_Samples_set.Attribute_Max(i) - Learn_Samples_set.Attribute_Min(i));

                  double max = double.NegativeInfinity;
                  Term current_term = null; 
                  for (int j=0;j<all_terms_for_var.Count;j++)
                  {
                      double current_max=double.NegativeInfinity;
                      switch (all_terms_for_var[j].Term_Func_Type)
                      {
                          case Type_Term_Func_Enum.Треугольник:
                              current_max = all_terms_for_var[j].Parametrs[2];
                              break;

                          case Type_Term_Func_Enum.Парабола:
                              current_max = all_terms_for_var[j].Parametrs[1];
                              break;
                          case Type_Term_Func_Enum.Трапеция:
                              current_max = all_terms_for_var[j].Parametrs[3];
                              break;
                      }

                      
                      if (max<current_max)
                      {
                          max = current_max;
                          current_term = all_terms_for_var[j];
                      }

                  }

                  int max_index = rulles_database_set[number_of_database].Terms_Set.FindIndex(x => x == current_term);
                 
                  switch (current_term.Term_Func_Type)
                  {
                      case Type_Term_Func_Enum.Треугольник: rulles_database_set[number_of_database].Terms_Set[max_index].Parametrs[2] =
                       Learn_Samples_set.Attribute_Max(i) +
                       0.001 * (Learn_Samples_set.Attribute_Max(i) - Learn_Samples_set.Attribute_Min(i));
                          break;
                      case Type_Term_Func_Enum.Парабола: rulles_database_set[number_of_database].Terms_Set[max_index].Parametrs[1] =
                 Learn_Samples_set.Attribute_Max(i) +
                 0.001 * (Learn_Samples_set.Attribute_Max(i) - Learn_Samples_set.Attribute_Min(i));
                          break;
                      case Type_Term_Func_Enum.Трапеция: rulles_database_set[number_of_database].Terms_Set[max_index].Parametrs[3] =
                 Learn_Samples_set.Attribute_Max(i) +
                 0.001 * (Learn_Samples_set.Attribute_Max(i) - Learn_Samples_set.Attribute_Min(i));
                          break;
                          



                  }
                 

              }
            }

        }







        #endregion

        #region  private interstruct




        protected new List<Knowlege_base_CRules> rulles_database_set = new List<Knowlege_base_CRules>();
        protected new  c_samples_set learn_samples_set;
        protected new  c_samples_set test_samples_set;
        private  int [] temperal_results_for_classificate; 
        #endregion


    }
}
