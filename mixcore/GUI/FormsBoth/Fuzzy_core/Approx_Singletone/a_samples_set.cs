using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone
{
    public class a_samples_set:sample_set
    {
        #region Visible public methods

    /*    public string File_Name
        {
            get { return file_name; }

        }

        public List<Attribune_Info> Input_Attributes
        {
         get {return input_Attribute;}
        }

        public Attribune_Info Output_Attributes
        {
            get { return output_Attribute; }
        }

        public List<Row_Sample> Data_Rows
        {
            get { return data_Rows; }
        }

        public int Count_Samples
        {
            get { return data_Rows.Count(); }
        }

        public int Count_Vars
        {
            get { return input_Attribute.Count(); }
        }

    
        public Row_Sample this[int index]
        {
            get { return data_Rows[index]; }
        }




        public double Attribute_Min(int index)
        {
            return input_Attribute[index].Min;
        }


        public double Attribute_Max(int index)
        {
            return input_Attribute[index].Max;
        }


        public double Attribute_Scatter(int index)
        {
            return input_Attribute[index].Scatter; 
        }
     

        */


        public List<a_samples_set.Row_Sample> Data_Rows
        {
            get { return data_Rows; }
        }


        public a_samples_set.Row_Sample this[int index]
        {
            get { return data_Rows[index]; }
        }

        public override int Count_Samples
        {
            get { return data_Rows.Count(); }
        }



        public a_samples_set(string file_Name, int type_file_ext = 0):base(file_Name,type_file_ext)
        {
        }

        public a_samples_set(string File_Name, List<a_samples_set.Row_Sample> Data_Rows, List<Attribune_Info> Input_Attribute, Attribune_Info Output_Attribute ): base(File_Name, Input_Attribute,Output_Attribute)
        {
            data_Rows = Data_Rows;
        }


        #endregion

        #region private interstruct

       protected List<a_samples_set.Row_Sample> data_Rows = new List<a_samples_set.Row_Sample>();
                 
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
                
              
                string _label = temp_lines[temp_lines.Count() - 1].ToLowerInvariant();
                double temp_out;
                double.TryParse(_label.Replace('.', ','), out temp_out);
                a_samples_set.Row_Sample temp_Row_sample = new a_samples_set.Row_Sample(input_attribute_value,input_attribute_string,temp_out);
             
                data_Rows.Add(temp_Row_sample);
            }
        }

        public new class Row_Sample:sample_set.Row_Sample
        {
            public Row_Sample(double[] input_attribute_double, string[] input_attribute_strings, double approx_name)
                : base(input_attribute_double, input_attribute_strings)
            {
                approx_value = approx_name;
            }
           

            protected double approx_value;

            internal double Approx_Value { get { return approx_value; } }



        }


        #endregion

    }
}
