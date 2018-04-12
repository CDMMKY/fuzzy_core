using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fuzzy_system.Fuzzy_Abstract
{
    public abstract class sample_set:Object
    {

                #region Visible public methods

        public string File_Name
        {
            get { return file_name; }

        }

        public Attribune_Info Input_Attribute(int index)
        {
            return input_Attribute[index];
        }


        public List<Attribune_Info> Input_Attributes
        {
            get { return input_Attribute; }
        }



        public Attribune_Info Output_Attributes
        {
            get { return output_Attribute; }
        }


        abstract public int Count_Samples
        {
            get;
        }

        public int Count_Vars
        {
            get { return input_Attribute.Count(); }
        }


      /*  private Row_Sample this[int index]
        {
            get { return data_Rows[index]; }
        }
        */



        public double Attribute_Min(int index)
        {
            return input_Attribute[index].Min;
        }


        public  double Attribute_Max(int index)
        {
            return input_Attribute[index].Max;
        }


        public double Attribute_Scatter(int index)
        {
            return input_Attribute[index].Scatter;
        }




               public sample_set(string file_Name, int type_file_ext = 0)
        {
            if (type_file_ext == 0) // 0 - KEEL data file
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

              // Only for class KEEL  calc_min_max_for_class();
            }
        }



           public sample_set(string File_Name, List<Attribune_Info> Input_Attribute, Attribune_Info Output_Attribute )
           {
           
              file_name = File_Name;
              input_Attribute = Input_Attribute;
             output_Attribute = Output_Attribute;
//             data_Rows = new List<Row_Sample>();

           }



        #endregion



        protected bool KEEL_separator(string Line)
        {
            if (Line[0] == '@')
            {
                string[] temp_lines = Line.Split(new char[] { ',', '@', ' ', '[', ']' },
                                                 StringSplitOptions.RemoveEmptyEntries);
                if (temp_lines[0].Equals("relation", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (temp_lines[0].Equals("attribute", StringComparison.OrdinalIgnoreCase))
                {
                    Attribune_Info temp_Attrtibute = new Attribune_Info();
                    temp_Attrtibute.Name = temp_lines[1];
                    if (temp_lines[2].Equals("integer", StringComparison.OrdinalIgnoreCase) ||
                        temp_lines[2].Equals("real", StringComparison.OrdinalIgnoreCase))
                    {
                        double temp;
                        double.TryParse(temp_lines[3].Replace('.', ','), out temp);
                        temp_Attrtibute.Min = temp;
                        double.TryParse(temp_lines[4].Replace('.', ','), out temp);
                        temp_Attrtibute.Max = temp;
                        input_Attribute.Add(temp_Attrtibute);
                    }
                    else
                    {
                        if (temp_lines.Where(i => i.Contains('{')).Count() > 0)
                        {
                            temp_Attrtibute.Name = temp_lines[1].Split(new char[] { '{', '}' })[0];
                            for (int i = 2; i < temp_lines.Count(); i++)
                            {

                                string[] temp_set_line = temp_lines[i].Split(new char[] { '{', ',', '}' },
                                                                             StringSplitOptions.RemoveEmptyEntries);
                                temp_Attrtibute.labels_values.Add(temp_set_line[0].ToLowerInvariant());

                            }
                            input_Attribute.Add(temp_Attrtibute);

                        }
                    }
                    return true;
                }

                if (temp_lines[0].Equals("inputs", StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 1; i < temp_lines.Count(); i++)
                    {
                        if (
                            input_Attribute.First(x => x.Name.Equals(temp_lines[i], StringComparison.OrdinalIgnoreCase)) ==
                            null)
                        {
                            return false;
                        }

                    }
                    return true;
                }
                if (temp_lines[0].Equals("outputs", StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 1; i < temp_lines.Count(); i++)
                    {
                        output_Attribute =
                            input_Attribute.First(x => x.Name.Equals(temp_lines[i], StringComparison.OrdinalIgnoreCase));
                        input_Attribute.RemoveAll(x => x == output_Attribute);


                    }
                    return true;
                }

                if (temp_lines[0].Equals("data", StringComparison.OrdinalIgnoreCase))
                {

                    return false;
                }


            }
            return false;
        }

        protected abstract void data_separator(string line);
     

        protected string file_name;
      //  protected List<Row_Sample> data_Rows ;
        protected List<Attribune_Info> input_Attribute = new List<Attribune_Info>();
        protected Attribune_Info output_Attribute;
        public class Attribune_Info
        {
            internal double Min { get; set; }
            internal double Max { get; set; }
            internal double Scatter { get { return Max - Min; } }
            internal string Name { get; set; }
            internal List<string> labels_values { get; set; }
            internal Attribune_Info()
            {
                labels_values = new List<string>();
            }
        }

        public class Row_Sample
        {
            protected double[] input_attribute_value;
            protected string[] input_attribute_string;

            public Row_Sample(double[] input_attribute_double, string[] input_attribute_strings)
            {
                input_attribute_value = input_attribute_double;
                input_attribute_string = input_attribute_strings;
        
            }
           

            internal double[] Input_Attribute_Value { get { return input_attribute_value; } }

            internal string[] Input_Attribute_String { get { return input_attribute_string; } }

         


        }









    }
}
