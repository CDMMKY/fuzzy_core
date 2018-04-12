using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.UFS
{
    public static class a_FS_UFS_Loader
    {




        public static a_samples_set Load_learn_from_UFS(this a_samples_set table_set, string file_name)
        {       //init
            a_samples_set temp_set = null;
            List<a_samples_set.Attribune_Info> input_Attribute = new List<a_samples_set.Attribune_Info>();
            List<a_samples_set.Row_Sample> data_Row = new List<a_samples_set.Row_Sample>();
            a_samples_set.Attribune_Info output_Attribute = new a_samples_set.Attribune_Info();
            string opened_dataset;

            //parse_start
            XmlDocument Source = new XmlDocument();
            Source.Load(file_name);

            XmlNode table_node = Source.DocumentElement.SelectSingleNode("descendant::Table[@Type='Training'] "); //We get learning table
            if (table_node == null) { throw new System.FormatException("В файле нет таблиц данных"); }
            opened_dataset = table_node.Attributes.GetNamedItem("Name").Value;
            output_Attribute.Name = table_node.Attributes.GetNamedItem("Output").Value;

            XmlNode attrib_node = table_node.SelectSingleNode("Attributes"); //We get atribute's tags

            int count_attribs = XmlConvert.ToInt32(attrib_node.Attributes.GetNamedItem("Count").Value);
            for (int k = 0; k <= count_attribs; k++)
            {
                a_samples_set.Attribune_Info temp_attib = new a_samples_set.Attribune_Info();
                temp_attib.Name = attrib_node.ChildNodes[k].Attributes.GetNamedItem("Name").Value;  // We get one attribute tag
                foreach (XmlNode Value in attrib_node.ChildNodes[k].ChildNodes)
                {
                    double temp_double = XmlConvert.ToDouble(Value.InnerXml);
                    switch (Value.Name)
                    {
                        case "Min": { temp_attib.Min = temp_double; break; }
                        case "Max": { temp_attib.Max = temp_double; break; }

                    }
                }
                if (temp_attib.Name.Equals(output_Attribute.Name, StringComparison.OrdinalIgnoreCase)) { output_Attribute = temp_attib; }
                else
                {
                    input_Attribute.Add(temp_attib);
                }
            }

            XmlNode rows_node = table_node.SelectSingleNode("Rows"); //We get data rows


            int count_rows = XmlConvert.ToInt32(rows_node.Attributes.GetNamedItem("Count").Value);
            for (int r = 0; r < count_rows; r++)
            {
                double Approx_value;
                double[] double_value = new double[count_attribs];
                string[] string_value = new string[count_attribs];
                for (int a = 0; a < count_attribs; a++)
                {
                    XmlNode value = rows_node.ChildNodes[r].SelectSingleNode(input_Attribute[a].Name);
                    try { double_value[a] = XmlConvert.ToDouble(value.InnerXml); }
                    catch
                    {
                        string_value[a] = value.InnerText;
                    }

                }

                XmlNode outvalue = rows_node.ChildNodes[r].SelectSingleNode(output_Attribute.Name);
                Approx_value = XmlConvert.ToDouble(outvalue.InnerXml);


                a_samples_set.Row_Sample temp_rows = new a_samples_set.Row_Sample(double_value, string_value, Approx_value);
                data_Row.Add(temp_rows);
            }
            temp_set = new a_samples_set(opened_dataset, data_Row, input_Attribute, output_Attribute);
            GC.Collect();
            return temp_set;

        }


        public static a_samples_set Load_test_from_UFS(this a_samples_set table_set, string file_name)
        {       //init
            a_samples_set temp_set = null;
            List<a_samples_set.Attribune_Info> input_Attribute = new List<a_samples_set.Attribune_Info>();
            List<a_samples_set.Row_Sample> data_Row = new List<a_samples_set.Row_Sample>();
            a_samples_set.Attribune_Info output_Attribute = new a_samples_set.Attribune_Info();
            string opened_dataset;

            //parse_start
            XmlDocument Source = new XmlDocument();
            Source.Load(file_name);

            XmlNode table_node = Source.DocumentElement.SelectSingleNode("descendant::Table[@Type='Testing'] "); //We get learning table
            if (table_node == null) { throw new System.FormatException("В файле нет таблиц данных"); }
            opened_dataset = table_node.Attributes.GetNamedItem("Name").Value;
            output_Attribute.Name = table_node.Attributes.GetNamedItem("Output").Value;

            XmlNode attrib_node = table_node.SelectSingleNode("Attributes"); //We get atribute's tags

            int count_attribs = XmlConvert.ToInt32(attrib_node.Attributes.GetNamedItem("Count").Value);
            for (int k = 0; k <= count_attribs; k++)
            {
                a_samples_set.Attribune_Info temp_attib = new a_samples_set.Attribune_Info();
                temp_attib.Name = attrib_node.ChildNodes[k].Attributes.GetNamedItem("Name").Value;  // We get one attribute tag
                foreach (XmlNode Value in attrib_node.ChildNodes[k].ChildNodes)
                {
                    double temp_double = XmlConvert.ToDouble(Value.InnerXml);
                    switch (Value.Name)
                    {
                        case "Min": { temp_attib.Min = temp_double; break; }
                        case "Max": { temp_attib.Max = temp_double; break; }

                    }
                }
                if (temp_attib.Name.Equals(output_Attribute.Name, StringComparison.OrdinalIgnoreCase)) { output_Attribute = temp_attib; }
                else
                {
                    input_Attribute.Add(temp_attib);
                }
            }

            XmlNode rows_node = table_node.SelectSingleNode("Rows"); //We get data rows


            int count_rows = XmlConvert.ToInt32(rows_node.Attributes.GetNamedItem("Count").Value);
            for (int r = 0; r < count_rows; r++)
            {
                double Approx_value;
                double[] double_value = new double[count_attribs];
                string[] string_value = new string[count_attribs];
                for (int a = 0; a < count_attribs; a++)
                {
                    XmlNode value = rows_node.ChildNodes[r].SelectSingleNode(input_Attribute[a].Name);
                    try { double_value[a] = XmlConvert.ToDouble(value.InnerXml); }
                    catch
                    {
                        string_value[a] = value.InnerText;
                    }

                }

                XmlNode outvalue = rows_node.ChildNodes[r].SelectSingleNode(output_Attribute.Name);
                Approx_value = XmlConvert.ToDouble(outvalue.InnerXml);


                a_samples_set.Row_Sample temp_rows = new a_samples_set.Row_Sample(double_value, string_value, Approx_value);
                data_Row.Add(temp_rows);
            }
            temp_set = new a_samples_set(opened_dataset, data_Row, input_Attribute, output_Attribute);
            GC.Collect();
            return temp_set;

        }


        public static a_Fuzzy_System load_UFS(this a_Fuzzy_System Approx, string file_name)
        {
            a_Fuzzy_System result = Approx;

            Knowlege_base_ARules New_dataBase = new Knowlege_base_ARules();

            List<string> added_term = new List<string>();


            XmlDocument Source = new XmlDocument();
            Source.Load(file_name);

            XmlNode rulles_node = Source.DocumentElement.SelectSingleNode("descendant::Rules");
            
            if (rulles_node == null) {throw new System.FormatException("Нет базы правил в ufs файле"); }
            int count_rulles = XmlConvert.ToInt32 (rulles_node.Attributes.GetNamedItem("Count").Value);
            XmlNode varibles_node = Source.DocumentElement.SelectSingleNode("descendant::Variables");
            if (varibles_node == null) {throw new System.FormatException("Нет термов в базе правил, ошибка UFS"); }
            for (int i = 0; i < count_rulles; i++)
            {
                XmlNode antecedent_node = rulles_node.ChildNodes[i].SelectSingleNode("Antecedent");
                int count_antecedent_term = XmlConvert.ToInt32 (antecedent_node.Attributes.GetNamedItem("Count").Value);
                int [] Order_term = new int[count_antecedent_term];
                 for (int j=0; j < count_antecedent_term;j++)
                 {
                     double[] Value_temp ;
                     Type_Term_Func_Enum type_term = Type_Term_Func_Enum.Треугольник;
                     int num_var=  Approx.Learn_Samples_set.Input_Attributes.IndexOf(Approx.Learn_Samples_set.Input_Attributes.Find (x=>x.Name.Equals(antecedent_node.ChildNodes[j].Attributes.GetNamedItem("Variable").Value,StringComparison.OrdinalIgnoreCase)));
                 string  name_term=antecedent_node.ChildNodes[j].Attributes.GetNamedItem("Term").Value;
                     if (added_term.Contains(name_term) )
                     {Order_term[j]=added_term.IndexOf(name_term);}
                     else{
                    XmlNode term_node =varibles_node.SelectSingleNode("descendant::Term[@Name='"+name_term+"']");
                 int count_MB=0;
                     switch (term_node.Attributes.GetNamedItem("Type").Value)
                     {case "Triangle":{ count_MB=3; type_term = Type_Term_Func_Enum.Треугольник; break;}
                      case "Gauss":{ count_MB=2; type_term = Type_Term_Func_Enum.Гауссоида; break;}
                      case "Parabolic":{count_MB=2; type_term = Type_Term_Func_Enum.Парабола; break;}
                      case "Trapezoid":{count_MB=4; type_term = Type_Term_Func_Enum.Трапеция; break;}
                     }
                     Value_temp = new double[count_MB];
                     term_node = term_node.SelectSingleNode("Params");
                         for (int p=0; p<count_MB;p++)
                     {
                         string tett = term_node.ChildNodes[p].Attributes.GetNamedItem("Number").Value;
                         int number_param = XmlConvert.ToInt32( term_node.ChildNodes[p].Attributes.GetNamedItem("Number").Value);
                         Value_temp[number_param] = XmlConvert.ToDouble(term_node.ChildNodes[p].Attributes.GetNamedItem("Value").Value);
                     }

                     Term temp_term = new Term(Value_temp,type_term,num_var);
                     
                     New_dataBase.Terms_Set.Add(temp_term);
                added_term.Add(name_term);
                     Order_term[j]=New_dataBase.Terms_Set.Count-1;
                     }
                 }

                XmlNode consequnt_node = rulles_node.ChildNodes[i].SelectSingleNode("Consequent");
                double Approx_value =XmlConvert.ToDouble( consequnt_node.Attributes.GetNamedItem("Value").Value); 

                ARule temp_rule = new ARule(New_dataBase.Terms_Set,Order_term,Approx_value);
                New_dataBase.Rules_Database.Add(temp_rule);
            }
            result.Rulles_Database_Set.Clear();
            result.Rulles_Database_Set.Add(New_dataBase);

            GC.Collect();
                return result;

        }




    }
}
