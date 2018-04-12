using FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.TakagiSugenoApproximate.UFS;
using System;
using System.Collections.Generic;
using System.Xml;

namespace FuzzyCore.FuzzySystem.FuzzyAbstract
{
    public static class BaseUFSLoader
    {

        public static SampleSet LoadLearnFromUFS(this SampleSet tableSet, string fileName)
        {
            XmlDocument Source = new XmlDocument();
            Source.Load(fileName);
            return LoadLearnFromUFS(Source);
        }

        public static SampleSet LoadLearnFromUFS(this SampleSet tableSet, XmlDocument Source)
        {       //init 
            return LoadLearnFromUFS(Source);
        }

        public static SampleSet LoadLearnFromUFS(string fileName)
        {
            XmlDocument Source = new XmlDocument();
            Source.Load(fileName);
            return LoadLearnFromUFS(Source);
        }


        public static SampleSet LoadLearnFromUFS(XmlDocument Source)
        {       //init
            SampleSet temp_set = null;
            List<SampleSet.AttributeInfo> input_Attribute = new List<SampleSet.AttributeInfo>();
            List<SampleSet.RowSample> data_Row = new List<SampleSet.RowSample>();
            SampleSet.AttributeInfo output_Attribute = new SampleSet.AttributeInfo();
            string opened_dataset;

            //parse_start


            XmlNode table_node = Source.DocumentElement.SelectSingleNode("descendant::Table[@Type='Training'] "); //We get learning table
            if (table_node == null) { throw new System.FormatException("В файле нет таблиц данных"); }
            opened_dataset = table_node.Attributes.GetNamedItem("Name").Value;
            output_Attribute.Name = table_node.Attributes.GetNamedItem("Output").Value;

            XmlNode attrib_node = table_node.SelectSingleNode("Attributes"); //We get atribute's tags

            int count_attribs = XmlConvert.ToInt32(attrib_node.Attributes.GetNamedItem("Count").Value);
            for (int k = 0; k < count_attribs; k++)
            {
                SampleSet.AttributeInfo temp_attib = new SampleSet.AttributeInfo();
                temp_attib.Name = attrib_node.ChildNodes[k].Attributes.GetNamedItem("Name").Value;  // We get one attribute tag
                foreach (XmlNode Value in attrib_node.ChildNodes[k].ChildNodes)
                {

                    switch (Value.Name)
                    {
                        case "Min": { temp_attib.Min = XmlConvert.ToDouble(Value.InnerXml); break; }
                        case "Max": { temp_attib.Max = XmlConvert.ToDouble(Value.InnerXml); break; }
                        case "Enum":
                            {
                                foreach (XmlNode tempValue in Value.ChildNodes)
                                {
                                    temp_attib.LabelsValues.Add(tempValue.Attributes.GetNamedItem("Value").Value);
                                }

                                break;
                            }
                    }
                }
                if (temp_attib.LabelsValues.Count == 0) { temp_attib.Type = SampleSet.AttributeInfo.TypeAttribute.real; } else { temp_attib.Type = SampleSet.AttributeInfo.TypeAttribute.nominate; }
                if (temp_attib.Name.Equals(output_Attribute.Name, StringComparison.OrdinalIgnoreCase)) {
                    output_Attribute = temp_attib; }
                else
                {
                    input_Attribute.Add(temp_attib);
                }
            }

            XmlNode rows_node = table_node.SelectSingleNode("Rows"); //We get data rows
            int count_input_attrib = count_attribs - 1;

            int count_rows = XmlConvert.ToInt32(rows_node.Attributes.GetNamedItem("Count").Value);
            for (int r = 0; r < count_rows; r++)
            {
                string Classifier_value;
                double[] double_value = new double[count_input_attrib];
                string[] string_value = new string[count_input_attrib];
                for (int a = 0; a < count_input_attrib; a++)
                {
                    XmlNode value = rows_node.ChildNodes[r].SelectSingleNode(XmlConvert.EncodeName(input_Attribute[a].Name));
                    try { double_value[a] = XmlConvert.ToDouble(value.InnerXml); }
                    catch
                    {
                        string_value[a] = value.InnerText;
                    }

                }

                XmlNode outvalue = rows_node.ChildNodes[r].SelectSingleNode(XmlConvert.EncodeName(output_Attribute.Name));
                Classifier_value = outvalue.InnerText;
                double doublevalue = double.NaN;
                if (output_Attribute.Type == SampleSet.AttributeInfo.TypeAttribute.real)
                {
                    doublevalue = XmlConvert.ToDouble(Classifier_value);
                    Classifier_value = "";
                }

                SampleSet.RowSample temp_rows = new SampleSet.RowSample(double_value, string_value, doublevalue, Classifier_value);
                data_Row.Add(temp_rows);
            }

            temp_set = new SampleSet(opened_dataset, data_Row, input_Attribute, output_Attribute);
            if (temp_set.OutputAttribute.Type == SampleSet.AttributeInfo.TypeAttribute.nominate) { temp_set.Type = SampleSet.TypeSampleSet.Classifier; } else { temp_set.Type = SampleSet.TypeSampleSet.Approximation; }
            GC.Collect();
            return temp_set;

        }


        public static SampleSet LoadTestFromUFS(this SampleSet tableSet, string fileName)
        {       //init
            XmlDocument Source = new XmlDocument();
            Source.Load(fileName);
            return LoadTestFromUFS(Source);
        }
        public static SampleSet LoadTestFromUFS(this SampleSet tableSet, XmlDocument Source)
        {       //init
            return LoadTestFromUFS(Source);
        }

        public static SampleSet LoadTestFromUFS(string fileName)
        {
            XmlDocument Source = new XmlDocument();
            Source.Load(fileName);
            return LoadTestFromUFS(Source);
        }

        public static SampleSet LoadTestFromUFS(XmlDocument Source)
        {       //init
            SampleSet temp_set = null;
            List<SampleSet.AttributeInfo> input_Attribute = new List<SampleSet.AttributeInfo>();
            List<SampleSet.RowSample> data_Row = new List<SampleSet.RowSample>();
            SampleSet.AttributeInfo output_Attribute = new SampleSet.AttributeInfo();
            string opened_dataset;

            //parse_start


            XmlNode table_node = Source.DocumentElement.SelectSingleNode("descendant::Table[@Type='Testing'] "); //We get learning table
            if (table_node == null) { throw new System.FormatException("В файле нет таблиц данных"); }
            opened_dataset = table_node.Attributes.GetNamedItem("Name").Value;
            output_Attribute.Name = table_node.Attributes.GetNamedItem("Output").Value;

            XmlNode attrib_node = table_node.SelectSingleNode("Attributes"); //We get atribute's tags

            int count_attribs = XmlConvert.ToInt32(attrib_node.Attributes.GetNamedItem("Count").Value);
            for (int k = 0; k < count_attribs; k++)
            {
                SampleSet.AttributeInfo temp_attib = new SampleSet.AttributeInfo();
                temp_attib.Name = attrib_node.ChildNodes[k].Attributes.GetNamedItem("Name").Value;  // We get one attribute tag
                foreach (XmlNode Value in attrib_node.ChildNodes[k].ChildNodes)
                {

                    switch (Value.Name)
                    {
                        case "Min": { temp_attib.Min = XmlConvert.ToDouble(Value.InnerXml); ; break; }
                        case "Max": { temp_attib.Max = XmlConvert.ToDouble(Value.InnerXml); ; break; }

                        case "Enum":
                            {
                                foreach (XmlNode tempValue in Value.ChildNodes)
                                {
                                    temp_attib.LabelsValues.Add(tempValue.Attributes.GetNamedItem("Value").Value);
                                } break;
                            }

                    }
                }
                if (temp_attib.LabelsValues.Count == 0) { temp_attib.Type = SampleSet.AttributeInfo.TypeAttribute.real; } else { temp_attib.Type = SampleSet.AttributeInfo.TypeAttribute.nominate; }
                if (temp_attib.Name.Equals(output_Attribute.Name, StringComparison.OrdinalIgnoreCase)) { output_Attribute = temp_attib; }
                else
                {
                    input_Attribute.Add(temp_attib);
                }
            }

            XmlNode rows_node = table_node.SelectSingleNode("Rows"); //We get data rows
            int count_input_attrub = count_attribs - 1;

            int count_rows = XmlConvert.ToInt32(rows_node.Attributes.GetNamedItem("Count").Value);
            for (int r = 0; r < count_rows; r++)
            {
                string Classifier_value;
                double[] double_value = new double[count_input_attrub];
                string[] string_value = new string[count_input_attrub];
                for (int a = 0; a < count_input_attrub; a++)
                {
                    XmlNode value = rows_node.ChildNodes[r].SelectSingleNode(XmlConvert.EncodeName(input_Attribute[a].Name));
                    try { double_value[a] = XmlConvert.ToDouble(value.InnerXml); }
                    catch
                    {
                        string_value[a] = value.InnerText;
                    }

                }

                XmlNode outvalue = rows_node.ChildNodes[r].SelectSingleNode(XmlConvert.EncodeName(output_Attribute.Name));
                Classifier_value = outvalue.InnerText;
                double doublevalue = double.NaN;
                if (output_Attribute.Type == SampleSet.AttributeInfo.TypeAttribute.real)
                {
                    doublevalue = XmlConvert.ToDouble(Classifier_value);
                    Classifier_value = "";
                }

                SampleSet.RowSample temp_rows = new SampleSet.RowSample(double_value, string_value, doublevalue, Classifier_value);
                data_Row.Add(temp_rows);
            }
            temp_set = new SampleSet(opened_dataset, data_Row, input_Attribute, output_Attribute);
            if (temp_set.OutputAttribute.Type == SampleSet.AttributeInfo.TypeAttribute.nominate) { temp_set.Type = SampleSet.TypeSampleSet.Classifier; } else { temp_set.Type = SampleSet.TypeSampleSet.Approximation; }
            GC.Collect();
            return temp_set;

        }


        public static IFuzzySystem LoadUFS(string FileName, out FuzzySystemRelisedList.TypeSystem TypeFS)
        {
            IFuzzySystem result = null;
            TypeFS = FuzzySystemRelisedList.TypeSystem.PittsburghClassifier;
            XmlDocument Source = new XmlDocument();
            Source.Load(FileName);
            XmlNode temp = Source.SelectSingleNode("FuzzySystem");
            string typestring = temp.Attributes.GetNamedItem("Type").Value.ToLowerInvariant();
            switch (typestring)
            {
                case "approximatortakagisugeno":
                    {
                        TSAFuzzySystem TSAFS = new TSAFuzzySystem(LoadLearnFromUFS(Source), LoadTestFromUFS(Source));
                        TSAFS.loadUFS(Source);
                        result = TSAFS;
                        TypeFS = FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate;
                        break;
                    }
                case "approximatorsingleton":
                    {
                        SAFuzzySystem SAFS = new SAFuzzySystem(LoadLearnFromUFS(Source), LoadTestFromUFS(Source));
                        SAFS.loadUFS(Source);
                        result = SAFS;
                        TypeFS = FuzzySystemRelisedList.TypeSystem.Singletone;
                        break;
                    }
                case "classifierpittsburgh":
                    {
                        PCFuzzySystem PCFS = new PCFuzzySystem(LoadLearnFromUFS(Source), LoadTestFromUFS(Source));
                        PCFS.loadUFS(Source);
                        result = PCFS;
                        TypeFS = FuzzySystemRelisedList.TypeSystem.PittsburghClassifier;
                        break;
                    }
                    
            }

            return result;
        }



    }
}
