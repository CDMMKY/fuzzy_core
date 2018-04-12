using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.FuzzyAbstract
{
    public class SampleSet : Object
    {
        #region Visible public methods

        public enum TypeSampleSet
        {
            Approximation = 0,
            Classifier = 1
        }

        public TypeSampleSet Type { get; set; }

        public string FileName
        {
            get; protected set;
        }

        public List<AttributeInfo> InputAttributes
        {
            get;
            protected set;
        } = new List<AttributeInfo>();

        public AttributeInfo OutputAttribute
        {
            get;
            protected set;
        }


        public int CountVars
        {
            get {
                Requires(InputAttributes != null);
                return InputAttributes.Count(); }
        }


        public int CountSamples
        {
            get {
                Requires(DataRows != null);
                return DataRows.Count(); }
        }


        public List<RowSample> DataRows
        {
            get;
            set;
        } = new List<RowSample>();

        public RowSample this[int index]
        {
            get {
                Requires(DataRows != null);
                Requires(index < DataRows.Count);
                return DataRows[index]; }
        }

        public int CountClass
        { 
            get {
                Requires(OutputAttribute != null && OutputAttribute.LabelsValues != null);
                       return OutputAttribute.LabelsValues.Count(); }
        }

        public SampleSet(string file_Name, int type_file_ext = 0)
        {
            if (type_file_ext == 0) // 0 - KEEL data file
            {
                string temp_Line;
                StreamReader in_File = new StreamReader(file_Name);
                this.FileName = new FileInfo(file_Name).Name;
                temp_Line = in_File.ReadLine();
                while (temp_Line.Length>0 && temp_Line[0] != '@')
                {
                    temp_Line = in_File.ReadLine();
                }
                while (temp_Line.Length > 0 && KEEL_separator(temp_Line))
                {
                    temp_Line = in_File.ReadLine();
                }

                while (!in_File.EndOfStream)
                {
                    temp_Line = in_File.ReadLine();
                    data_separator(temp_Line);
                }
            }
        }
        #endregion


        protected bool KEEL_separator(string Line)
        {
            if (Line[0] == '@')
            {
                string[] temp_lines = Line.Split(new char[] { ',', '@', ' ', '[', ']', '{'},
                                                 StringSplitOptions.RemoveEmptyEntries);
                if (temp_lines[0].Equals("relation", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (temp_lines[0].Equals("attribute", StringComparison.OrdinalIgnoreCase))
                {
                    AttributeInfo temp_Attrtibute = new AttributeInfo();
                    temp_Attrtibute.Name = temp_lines[1];
                    if (temp_lines[2].Equals("integer", StringComparison.OrdinalIgnoreCase)) { temp_Attrtibute.Type = AttributeInfo.TypeAttribute.integer; }
                    if (temp_lines[2].Equals("real", StringComparison.OrdinalIgnoreCase)) { temp_Attrtibute.Type = AttributeInfo.TypeAttribute.real; }
                    if (temp_lines[2].Equals("integer", StringComparison.OrdinalIgnoreCase) ||
                        temp_lines[2].Equals("real", StringComparison.OrdinalIgnoreCase))
                    {
                        double temp;
                    string comma= System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;         
                        
                        double.TryParse(temp_lines[3]. Replace(".", comma), out temp);
                        temp_Attrtibute.Min = temp;
                        double.TryParse(temp_lines[4].Replace(".", comma), out temp);
                        temp_Attrtibute.Max = temp;
                       InputAttributes.Add(temp_Attrtibute);
                    }
                    else
                    {
                        temp_Attrtibute.Type = AttributeInfo.TypeAttribute.nominate;
                        if (temp_lines.Where(i => i.Contains('}')).Count() > 0)
                        {
                            temp_Attrtibute.Name = temp_lines[1].Split(new char[] { '{', '}' })[0];
                            for (int i = 2; i < temp_lines.Count(); i++)
                            {
                                string[] temp_set_line = temp_lines[i].Split(new char[] { '{', ',', '}' },
                                                                             StringSplitOptions.RemoveEmptyEntries);
                                temp_Attrtibute.LabelsValues.Add(temp_set_line[0].ToLowerInvariant());
                            }
                           InputAttributes.Add(temp_Attrtibute);
                        }
                    }
                    return true;
                }
                if (temp_lines[0].Equals("inputs", StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 1; i < temp_lines.Count(); i++)
                    {
                        if (
                        InputAttributes.First(x => x.Name.Equals(temp_lines[i], StringComparison.OrdinalIgnoreCase)) ==
                            null)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                if (temp_lines[0].Equals("outputs", StringComparison.OrdinalIgnoreCase) || temp_lines[0].Equals("output", StringComparison.OrdinalIgnoreCase))
                {
                    for (int i = 1; i < temp_lines.Count(); i++)
                    {
                       OutputAttribute  =
                           InputAttributes.First(x => x.Name.Equals(temp_lines[i], StringComparison.OrdinalIgnoreCase));
                        InputAttributes.RemoveAll(x => x == OutputAttribute);
                    }
                    return true;
                }

                if (temp_lines[0].Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    if (OutputAttribute.LabelsValues.Count() == 0)
                    { Type = TypeSampleSet.Approximation; }
                    else Type = TypeSampleSet.Classifier;
                    return false;
                }
            }
            if (OutputAttribute.LabelsValues.Count() == 0)
            { Type = TypeSampleSet.Approximation; }
            else Type = TypeSampleSet.Classifier;
            return false;
        }

        protected void data_separator(string line)
        {
            Requires(line != null);
           
            string[] temp_lines = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp_lines.Count() > 0)
            {
                string[] input_InputAttributestring = new string[InputAttributes.Count()];
                double[] input_InputAttributevalue = new double[InputAttributes.Count()];
                for (int i = 0; i < InputAttributes.Count(); i++)
                {
                    if (InputAttributes[i].LabelsValues.Count == 0)
                    {
                        double temp;
                        string comma = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;         
                        double.TryParse(temp_lines[i].Replace(".", comma), out temp);
                        input_InputAttributevalue[i] = temp;
                        input_InputAttributestring[i] = "";
                    }
                    else
                    {
                        input_InputAttributestring[i] = temp_lines[i];
                        input_InputAttributevalue[i] = double.NaN;
                    }
                }
                string class_label = temp_lines[temp_lines.Count() - 1].ToLowerInvariant();
                double temp_out = double.NaN;
                if (OutputAttribute.LabelsValues.Count == 0)
                {
                    string comma = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;         
            
                    double.TryParse(class_label.Replace(".",comma), out temp_out);
                    class_label = "";
                }
                RowSample temp_Row_sample = new RowSample(input_InputAttributevalue, input_InputAttributestring, temp_out, class_label);

                DataRows.Add(temp_Row_sample);
            }
        }


        public SampleSet(string File_Name, List<SampleSet.RowSample> Data_Rows, List<AttributeInfo> Input_Attribute, AttributeInfo Output_Attribute)
        {
           FileName  = File_Name;
            InputAttributes = Input_Attribute;
         OutputAttribute  = Output_Attribute;
            DataRows = Data_Rows;

        }

        public class AttributeInfo
        {
            public double Min { get; internal set; }
            public double Max { get; internal set; }
            public double Scatter { get { return Max - Min; } }
            public string Name { get; internal set; }
            public List<string> LabelsValues { get; internal set; }
            public enum TypeAttribute
            {
                real = 1,
                integer = 2,
                nominate = 3
            }

            public TypeAttribute Type { get; set; }

            public double EvaluteNormalisedValue (double Value)
            { return (Value - Min)/Scatter;
            }

            internal AttributeInfo()
            {
                LabelsValues = new List<string>();
            }
        }

        public class RowSample
        {
   
            public RowSample(double[] input_InputAttributedouble, string[] input_InputAttributestrings, double doubleOutputValue, string stringOutputValue)
            {
            InputAttributeValue  = input_InputAttributedouble;
            InputAttributeString  = input_InputAttributestrings;
            DoubleOutput  = doubleOutputValue;
            StringOutput  = stringOutputValue;
            }
            public RowSample()
            {
            }
            

            public double[] InputAttributeValue { get; set; }
            public string[] InputAttributeString { get; set; }
            public double DoubleOutput { get; set; }
            public string StringOutput { get; set; }

        }
    }
}
