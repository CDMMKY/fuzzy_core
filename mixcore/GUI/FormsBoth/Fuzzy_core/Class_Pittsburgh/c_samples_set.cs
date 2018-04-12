using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh
{
    public class c_samples_set: sample_set
    {
        #region Visible public methods

        public override int Count_Samples
        {
            get { return data_Rows.Count(); }
        }



        public List<c_samples_set.Row_Sample> Data_Rows
        {
            get { return data_Rows; }
        }

        public int Count_Class
        {
            get { return output_Attribute.labels_values.Count(); }
        }
        
        public  c_samples_set.Row_Sample this[int index]
        {
            get { return data_Rows[index]; }
        }



        public double get_class_var_min(int num_class, int num_var)
        {
            return min_for_class[num_class][num_var];
        }

        
        public double get_class_var_max(int num_class, int num_var)
        {
            return max_for_class[num_class][num_var];
        }

        public bool exist_class_var_max(int num_class)
        { return max_for_class[num_class]!=null;
        }
        public  bool exist_class_var_min(int num_class)
        {
            return min_for_class[num_class] != null;
        }





        public c_samples_set(string file_Name, int type_file_ext = 0):base(file_Name,type_file_ext)
        {
           /* if (type_file_ext == 0) // 0 - KEEL data file
            {
                string temp_Line;
                StreamReader in_File = new StreamReader(file_Name);
                this.file_name = new FileInfo(file_Name).Name;
                temp_Line = in_File.ReadLine();
                while (temp_Line[0] != '@')
                {
                    temp_Line = in_File.ReadLine();

                }
                while (KEEL_separator(temp_Line))
                {
                    temp_Line = in_File.ReadLine();

                }

                while (!in_File.EndOfStream)
                {
                    temp_Line = in_File.ReadLine();
                    data_separator(temp_Line);
                }
             */               calc_min_max_for_class();
           // }
        }


         public c_samples_set(string File_Name, List<c_samples_set.Row_Sample> Data_Rows, List<Attribune_Info> Input_Attribute, Attribune_Info Output_Attribute ): base(File_Name, Input_Attribute,Output_Attribute)
        {
            data_Rows = Data_Rows;
        }




        #endregion

        #region private interstruct


        private void calc_min_max_for_class()
        {
            min_for_class = new double[Count_Class][];
            max_for_class = new double[Count_Class][];
            for (int i = 0; i < Count_Class; i++)
            {
                string test_label = output_Attribute.labels_values[i];
                

                List<Row_Sample> temp_rows = data_Rows.FindAll(x => x.Class_Label == test_label);
                if (temp_rows.Count > 0)
                {
                max_for_class[i] = new double[Count_Vars];
                min_for_class[i] = new double[Count_Vars];
                
                    for (int j = 0; j < Count_Vars; j++)
                    {
                        max_for_class[i][j] = temp_rows.Max(x => x.Input_Attribute_Value[j]);
                        min_for_class[i][j] = temp_rows.Min(x => x.Input_Attribute_Value[j]);


                    }
                }
            }

            for (int i = 0; i < Count_Vars; i++)
            {int max_index = 0;
                
                int min_index = 0;
             
                double current_min = Math.Abs(Attribute_Min(i)-min_for_class[min_index][i]);
                double current_max = Math.Abs(Attribute_Max(i)-max_for_class[max_index][i]);
                for (int j = 1; j < Count_Class; j++)
                {  
                    if ((min_for_class[j]==null)||(max_for_class[j]==null))
                    {
                        continue;
                    }

                    if  (Math.Abs(min_for_class[j][i]-Attribute_Min(i))<current_min)
                 {
                    min_index = j;
                    current_min = Math.Abs(Attribute_Min(i) - min_for_class[min_index][i]);
                 }
                    if ( Math.Abs(max_for_class[j][i]-Attribute_Max(i))<current_max)
                 {
                    max_index = j;
                    current_max = Math.Abs(Attribute_Max(i) - max_for_class[max_index][i]);
                 }
                }

               if (min_for_class[min_index]!=null) {min_for_class[min_index][i] = Attribute_Min(i)*0.9;}
               if (max_for_class[max_index]!=null)  {max_for_class[max_index][i] = Attribute_Max(i)*1.1;}
            }


            for (int i =0;i<Count_Vars;i++)
            {for (int j=0;j<Count_Class;j++)
             {if ((min_for_class[j]==null)|| (max_for_class[j]==null))
             {
                 continue;
             }

                 int neates_index = 1;
                double current_nearest = Math.Abs(max_for_class[j][i] - min_for_class[neates_index][i]);
                bool laid = max_for_class[j][i]>min_for_class[neates_index][i];
                for (int k=1;k<Count_Class;k++ )
                    { if ((j==k) ||(max_for_class[j]==null)||(min_for_class[k]==null))
                        {continue;
                            
                        }
                     if (max_for_class[j][i]>min_for_class[k][i])
                     {
                         laid = true;
                     }
                     else
                     {
                         double temp_nearest = Math.Abs(max_for_class[j][i] - min_for_class[k][i]);
                          if (temp_nearest<current_nearest)
                          {
                              current_nearest = temp_nearest;
                              neates_index = k;
                          }
                     }
                    }
                if (!laid)
                {
                    double temp = max_for_class[j][i];
                    max_for_class[j][i] = min_for_class[neates_index][i];
                    min_for_class[neates_index][i] = temp;
                }

             }
            }

        }



        protected override void data_separator(string line)
        {
            string[] temp_lines = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp_lines.Count() > 0)
            {

                string [] input_attribute_string = new string[input_Attribute.Count()];
                double [] input_attribute_value = new double[input_Attribute.Count()];
                for (int i = 0; i < input_Attribute.Count(); i++)
                {
                    if (input_Attribute[i].labels_values.Count == 0)
                    {
                        double temp;
                        double.TryParse(temp_lines[i].Replace('.', ','), out temp);

                        input_attribute_value[i] = temp;
                    }
                    else
                    {
                        input_attribute_string[i] = temp_lines[i];

                    }
                }
                
              
                string class_label = temp_lines[temp_lines.Count() - 1].ToLowerInvariant();
                  Row_Sample temp_Row_sample = new Row_Sample(input_attribute_value,input_attribute_string,class_label);
             
                data_Rows.Add(temp_Row_sample);
            }
        }

        protected double[][] min_for_class;
        protected double[][] max_for_class;
        protected List<c_samples_set.Row_Sample> data_Rows = new List<c_samples_set.Row_Sample>();
        public new class Row_Sample : sample_set.Row_Sample
        {
            public Row_Sample(double[] input_attribute_double, string[] input_attribute_strings, string class_name)
                : base(input_attribute_double, input_attribute_strings)
            {
                class_label = class_name;
            }
           

            private string class_label;
 
            internal string Class_Label { get { return class_label; } }



        }


        #endregion

    }
}
