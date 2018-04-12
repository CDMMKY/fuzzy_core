using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Class_Pittsburgh.UFS
    
{
    internal class c_FS_UFS : Object
    {

        private static void write_about_term(XmlWriter writer, c_Fuzzy_System Classifier, Term term)
        {
            writer.WriteStartElement("Term");
            writer.WriteAttributeString("Name",
                                       XmlConvert.ToString(Classifier.Rulles_Database_Set[0].Terms_Set.IndexOf(term)));
            switch (term.Term_Func_Type)
            {
                case Type_Term_Func_Enum.Треугольник:
                    writer.WriteAttributeString("Type", "Triangle");
                    break;
                case Type_Term_Func_Enum.Гауссоида:
                    writer.WriteAttributeString("Type", "Gauss");
                    break;
                case Type_Term_Func_Enum.Парабола:
                    writer.WriteAttributeString("Type", "Parabolic");
                    break;
                case Type_Term_Func_Enum.Трапеция:
                    writer.WriteAttributeString("Type", "Trapezoid");
                    break;

            }

            writer.WriteStartElement("Params");
            for (int i = 0; i < c_Fuzzy_System.Count_Params_For_Term(term.Term_Func_Type); i++)
            {
                writer.WriteStartElement("Param");

                writer.WriteAttributeString("Number", XmlConvert.ToString(i));
                writer.WriteAttributeString("Value", XmlConvert.ToString(term.Parametrs[i]));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();



            writer.WriteEndElement();
        }

        private static void write_about_varibles_and_terms(XmlWriter writer, c_Fuzzy_System Classifier)
        {
            writer.WriteStartElement("Variables");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Classifier.Count_Vars));


            for (int i = 0; i < Classifier.Count_Vars; i++)
            {
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", Classifier.Learn_Samples_set.Input_Attribute(i).Name);
                writer.WriteAttributeString("Min",
                                           XmlConvert.ToString(Classifier.Learn_Samples_set.Input_Attribute(i).Min));
                writer.WriteAttributeString("Max",
                                           XmlConvert.ToString(Classifier.Learn_Samples_set.Input_Attribute(i).Max));
                List<Term> terms_for_varrible =
                    Classifier.Rulles_Database_Set[0].Terms_Set.Where(x => x.Number_of_Input_Var == i).ToList();
                writer.WriteStartElement("Terms");
                writer.WriteAttributeString("Count", XmlConvert.ToString(terms_for_varrible.Count));

                foreach (var term in terms_for_varrible)
                {
                    write_about_term(writer, Classifier, term);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void write_about_rows(XmlWriter writer, c_samples_set samplesSet)
        {
            writer.WriteStartElement("Rows");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.Data_Rows.Count()));
            for (int i = 0; i < samplesSet.Data_Rows.Count; i++)
            {
                writer.WriteStartElement("Row");
                for (int j = 0; j < samplesSet.Count_Vars; j++)
                {
                    if (samplesSet.Input_Attribute(j).labels_values.Count() > 0)
                    {
                        writer.WriteElementString(samplesSet.Input_Attribute(j).Name,
                                                  samplesSet.Data_Rows[i].Input_Attribute_String[j]);
                    }
                    else
                    {
                        writer.WriteElementString(samplesSet.Input_Attribute(j).Name,
                                                  XmlConvert.ToString(samplesSet.Data_Rows[i].Input_Attribute_Value[j]));
                    }
                   
                }
                writer.WriteElementString(samplesSet.Output_Attributes.Name, samplesSet.Data_Rows[i].Class_Label);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void write_about_rules(XmlWriter writer, c_Fuzzy_System Classifier)
        {
            writer.WriteStartElement("Rules");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Classifier.Rulles_Database_Set[0].Rules_Database.Count));

            foreach (CRule rule in Classifier.Rulles_Database_Set[0].Rules_Database)
            {
                writer.WriteStartElement("Rule");
                
                writer.WriteStartElement("Antecedent");
                writer.WriteAttributeString("Count",XmlConvert.ToString(rule.Term_of_Rule_Set.Count));
                 foreach (Term term in rule.Term_of_Rule_Set)
                 {
                     writer.WriteStartElement("Pair");
                     writer.WriteAttributeString("Variable", Classifier.Learn_Samples_set.Input_Attribute(term.Number_of_Input_Var).Name);
                     writer.WriteAttributeString("Term",XmlConvert.ToString(Classifier.Rulles_Database_Set[0].Terms_Set.IndexOf(term)));
                     writer.WriteEndElement();
                 }
                writer.WriteEndElement();

                writer.WriteStartElement("Consequent");
                writer.WriteAttributeString("Class",rule.Label_of_Class);
                writer.WriteAttributeString("CF",XmlConvert.ToString(rule.CF));
                writer.WriteEndElement();
                
                writer.WriteEndElement();
            }
                
            

            writer.WriteEndElement();
            
        }

        private static void write_about_attribute(XmlWriter writer, c_samples_set.Attribune_Info attribuneInfo)
        {
            writer.WriteStartElement("Attribute");
            writer.WriteAttributeString("Name",attribuneInfo.Name);
            if (attribuneInfo.labels_values.Count >0)
            {
                writer.WriteAttributeString("Type", "Enum");
                writer.WriteStartElement("Enum");
                writer.WriteAttributeString("Count", XmlConvert.ToString(attribuneInfo.labels_values.Count));
                for (int i =0;i< attribuneInfo.labels_values.Count;i++)
                {
                    writer.WriteStartElement("Enum");
                    writer.WriteAttributeString("Value",attribuneInfo.labels_values[i]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("Type", "Interval");
                writer.WriteElementString("Min",XmlConvert.ToString(attribuneInfo.Min));
                writer.WriteElementString("Max", XmlConvert.ToString(attribuneInfo.Max));
            }

            writer.WriteEndElement();
        }

        private static void  write_about_table(XmlWriter writer, c_samples_set samplesSet, c_Fuzzy_System Classifier)
        {
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name",samplesSet.File_Name.Remove(samplesSet.File_Name.Length-4,4));
            if (samplesSet ==Classifier.Learn_Samples_set)
            {
                writer.WriteAttributeString("Type", "Training");
            }else
            {
                writer.WriteAttributeString("Type", "Testing");
            }
            writer.WriteAttributeString("Output",samplesSet.Output_Attributes.Name);
            writer.WriteStartElement("Attributes");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.Count_Vars));
            for(int i =0; i< samplesSet.Count_Vars; i++)
            {write_about_attribute(writer,samplesSet.Input_Attribute(i));
            }
            write_about_attribute(writer,samplesSet.Output_Attributes);
            writer.WriteEndElement();
            write_about_rows(writer,samplesSet);


            writer.WriteEndElement();
        }

        private static void write_about_observation(XmlWriter writer, c_Fuzzy_System Classifier)
        {
            writer.WriteStartElement("Observations");
            if (Classifier.Test_Samples_set != null)
            {
            writer.WriteAttributeString("CountTable",XmlConvert.ToString(2));
                write_about_table(writer,Classifier.Learn_Samples_set,Classifier);
                write_about_table(writer, Classifier.Test_Samples_set, Classifier);
            }else
            { writer.WriteAttributeString("CountTable",XmlConvert.ToString(1));
            write_about_table(writer, Classifier.Learn_Samples_set, Classifier);
           }

            writer.WriteEndElement();
        }


        private static void write_about_Estimates(XmlWriter writer, c_Fuzzy_System Classifier)
        {
            writer.WriteStartElement("Estimates");
            if (Classifier.Test_Samples_set != null)
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(2));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table",Classifier.Learn_Samples_set.File_Name.Remove(Classifier.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.Classify_Learn_Samples()));
                writer.WriteEndElement();


               
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.Test_Samples_set.File_Name.Remove(Classifier.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.Classify_Test_Samples()));
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(1));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.Learn_Samples_set.File_Name.Remove(Classifier.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.Classify_Learn_Samples()));
                writer.WriteEndElement();

            }
        

        writer.WriteEndElement();
    }


        public static bool save_to_UFS(c_Fuzzy_System Classifier, string file_name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = Environment.NewLine;
            settings.NewLineHandling = NewLineHandling.None;
            settings.OmitXmlDeclaration = false;


            XmlWriter writer = XmlTextWriter.Create(file_name, settings);
            writer.WriteStartElement("FuzzySystem");
            writer.WriteAttributeString("Type", "ClassifierPittsburgh");
            write_about_varibles_and_terms(writer, Classifier);
            write_about_rules(writer,Classifier);
            write_about_observation(writer,Classifier);
           write_about_Estimates(writer,Classifier);
            writer.WriteEndElement();
         //   writer.Flush();
            writer.Close();



        return false;
    }



        public static c_Fuzzy_System load_UFS(string file_name)
        {
            c_Fuzzy_System result = null;
            return result;

        }
    }
}
