using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Fuzzy_system;
using Fuzzy_system.Fuzzy_Abstract;

namespace Fuzzy_system.Approx_Singletone.UFS
{
    internal class a_FS_UFS_Writer : Object
    {

        private static void write_about_term(XmlWriter writer, a_Fuzzy_System Approximate, Term term)
        {
            writer.WriteStartElement("Term");
            writer.WriteAttributeString("Name",
                                       XmlConvert.ToString(Approximate.Rulles_Database_Set[0].Terms_Set.IndexOf(term)));
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
            for (int i = 0; i < Member_Function.Count_Params_For_Term(term.Term_Func_Type); i++)
            {
                writer.WriteStartElement("Param");

                writer.WriteAttributeString("Number", XmlConvert.ToString(i));
                writer.WriteAttributeString("Value", XmlConvert.ToString(term.Parametrs[i]));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();



            writer.WriteEndElement();
        }

        private static void write_about_varibles_and_terms(XmlWriter writer, a_Fuzzy_System Approximate)
        {
            writer.WriteStartElement("Variables");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Approximate.Count_Vars));


            for (int i = 0; i < Approximate.Count_Vars; i++)
            {
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", Approximate.Learn_Samples_set.Input_Attributes[i].Name);
                writer.WriteAttributeString("Min",
                                           XmlConvert.ToString(Approximate.Learn_Samples_set.Input_Attributes[i].Min));
                writer.WriteAttributeString("Max",
                                           XmlConvert.ToString(Approximate.Learn_Samples_set.Input_Attributes[i].Max));
                List<Term> terms_for_varrible =
                    Approximate.Rulles_Database_Set[0].Terms_Set.Where(x => x.Number_of_Input_Var == i).ToList();
                writer.WriteStartElement("Terms");
                writer.WriteAttributeString("Count", XmlConvert.ToString(terms_for_varrible.Count));

                foreach (var term in terms_for_varrible)
                {
                    write_about_term(writer, Approximate, term);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void write_about_rows(XmlWriter writer, a_samples_set samplesSet)
        {
            writer.WriteStartElement("Rows");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.Data_Rows.Count()));
            for (int i = 0; i < samplesSet.Data_Rows.Count; i++)
            {
                writer.WriteStartElement("Row");
                for (int j = 0; j < samplesSet.Count_Vars; j++)
                {
                    if (samplesSet.Input_Attributes[j].labels_values.Count() > 0)
                    {
                        writer.WriteElementString(samplesSet.Input_Attributes[j].Name,
                                                  samplesSet.Data_Rows[i].Input_Attribute_String[j]);
                    }
                    else
                    {
                        writer.WriteElementString(samplesSet.Input_Attributes[j].Name,
                                                  XmlConvert.ToString(samplesSet.Data_Rows[i].Input_Attribute_Value[j]));
                    }

                }
                writer.WriteElementString(samplesSet.Output_Attributes.Name, XmlConvert.ToString(samplesSet.Data_Rows[i].Approx_Value));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private static void write_about_rules(XmlWriter writer, a_Fuzzy_System Approximate)
        {
            writer.WriteStartElement("Rules");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Approximate.Rulles_Database_Set[0].Rules_Database.Count));

            foreach (ARule rule in Approximate.Rulles_Database_Set[0].Rules_Database)
            {
                writer.WriteStartElement("Rule");

                writer.WriteStartElement("Antecedent");
                writer.WriteAttributeString("Count", XmlConvert.ToString(rule.Term_of_Rule_Set.Count));
                foreach (Term term in rule.Term_of_Rule_Set)
                {
                    writer.WriteStartElement("Pair");
                    writer.WriteAttributeString("Variable", Approximate.Learn_Samples_set.Input_Attributes[term.Number_of_Input_Var].Name);
                    writer.WriteAttributeString("Term", XmlConvert.ToString(Approximate.Rulles_Database_Set[0].Terms_Set.IndexOf(term)));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("Consequent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(rule.Kons_approx_Value));
                //  writer.WriteAttributeString("CF",XmlConvert.ToString(rule.CF));
                writer.WriteEndElement();

                writer.WriteEndElement();
            }



            writer.WriteEndElement();

        }

        private static void write_about_attribute(XmlWriter writer, a_samples_set.Attribune_Info attribuneInfo)
        {
            writer.WriteStartElement("Attribute");
            writer.WriteAttributeString("Name", attribuneInfo.Name);
            if (attribuneInfo.labels_values.Count > 0)
            {
                writer.WriteAttributeString("Type", "Enum");
                writer.WriteStartElement("Enum");
                writer.WriteAttributeString("Count", XmlConvert.ToString(attribuneInfo.labels_values.Count));
                for (int i = 0; i < attribuneInfo.labels_values.Count; i++)
                {
                    writer.WriteStartElement("Enum");
                    writer.WriteAttributeString("Value", attribuneInfo.labels_values[i]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("Type", "Interval");
                writer.WriteElementString("Min", XmlConvert.ToString(attribuneInfo.Min));
                writer.WriteElementString("Max", XmlConvert.ToString(attribuneInfo.Max));
            }

            writer.WriteEndElement();
        }

        private static void write_about_table(XmlWriter writer, a_samples_set samplesSet, a_Fuzzy_System Approximate)
        {
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", samplesSet.File_Name);
            if (samplesSet == Approximate.Learn_Samples_set)
            {
                writer.WriteAttributeString("Type", "Training");
            }
            else
            {
                writer.WriteAttributeString("Type", "Testing");
            }
            writer.WriteAttributeString("Output", samplesSet.Output_Attributes.Name);
            writer.WriteStartElement("Attributes");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.Count_Vars));
            for (int i = 0; i < samplesSet.Count_Vars; i++)
            {
                write_about_attribute(writer, samplesSet.Input_Attributes[i]);
            }
            write_about_attribute(writer, samplesSet.Output_Attributes);
            writer.WriteEndElement();
            write_about_rows(writer, samplesSet);


            writer.WriteEndElement();
        }

        private static void write_about_observation(XmlWriter writer, a_Fuzzy_System Approximate)
        {
            writer.WriteStartElement("Observations");
            if (Approximate.Test_Samples_set != null)
            {
                writer.WriteAttributeString("CountTable", XmlConvert.ToString(2));
                write_about_table(writer, Approximate.Learn_Samples_set, Approximate);
                write_about_table(writer, Approximate.Test_Samples_set, Approximate);
            }
            else
            {
                writer.WriteAttributeString("CountTable", XmlConvert.ToString(1));
                write_about_table(writer, Approximate.Learn_Samples_set, Approximate);
            }

            writer.WriteEndElement();
        }


        private static void write_about_Estimates(XmlWriter writer, a_Fuzzy_System Approximate)
        {
            writer.WriteStartElement("Estimates");
            if (Approximate.Test_Samples_set != null)
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(2));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.Learn_Samples_set.File_Name.Remove(Approximate.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approx_Learn_Samples()));
                writer.WriteEndElement();



                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.Test_Samples_set.File_Name.Remove(Approximate.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approx_Test_Samples()));
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(1));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.Learn_Samples_set.File_Name.Remove(Approximate.Learn_Samples_set.File_Name.Length - 4, 4));
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approx_Learn_Samples()));
                writer.WriteEndElement();

            }


            writer.WriteEndElement();
        }


        public static bool save_to_UFS(a_Fuzzy_System Approximate, string file_name)
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
            writer.WriteAttributeString("Type", "ApproximatorSingleton");
            write_about_varibles_and_terms(writer, Approximate);
            write_about_rules(writer, Approximate);
            write_about_observation(writer, Approximate);
            write_about_Estimates(writer, Approximate);
            writer.WriteEndElement();
            //   writer.Flush();
            writer.Close();



            return false;
        }



        public static a_Fuzzy_System load_UFS(string file_name)
        {
            a_Fuzzy_System result = null;
            return result;

        }
    }
}
