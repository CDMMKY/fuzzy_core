using FuzzySystem.FuzzyAbstract;
using System.Linq;
using System.Xml;

namespace FuzzyCore.FuzzySystem.FuzzyAbstract
{
    public static class BaseUFSWriter
    {


        public static void writeAboutRows(XmlWriter writer, SampleSet samplesSet)
        {
            writer.WriteStartElement("Rows");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.DataRows.Count()));
            for (int i = 0; i < samplesSet.DataRows.Count; i++)
            {
                writer.WriteStartElement("Row");
                for (int j = 0; j < samplesSet.CountVars; j++)
                {
                    if (samplesSet.InputAttributes[j].LabelsValues.Count() > 0)
                    {
                        writer.WriteElementString(XmlConvert.EncodeName(samplesSet.InputAttributes[j].Name),
                                                  samplesSet.DataRows[i].InputAttributeString[j]);
                    }
                    else
                    {
                        writer.WriteElementString(XmlConvert.EncodeName(samplesSet.InputAttributes[j].Name),
                                                  XmlConvert.ToString(samplesSet.DataRows[i].InputAttributeValue[j]));
                    }

                }
                if (samplesSet.OutputAttribute.Type == SampleSet.AttributeInfo.TypeAttribute.nominate)
                {
                    writer.WriteElementString(XmlConvert.EncodeName(samplesSet.OutputAttribute.Name), samplesSet.DataRows[i].StringOutput);

                }
                else
                {
                    writer.WriteElementString(XmlConvert.EncodeName(samplesSet.OutputAttribute.Name), XmlConvert.ToString( samplesSet.DataRows[i].DoubleOutput));

                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public static void writeAboutAttribute(XmlWriter writer, SampleSet.AttributeInfo attribuneInfo)
        {
            writer.WriteStartElement("Attribute");
            writer.WriteAttributeString("Name", attribuneInfo.Name);
            if (attribuneInfo.LabelsValues.Count > 0)
            {
                writer.WriteAttributeString("Type", "Enum");
                writer.WriteStartElement("Enum");
                writer.WriteAttributeString("Count", XmlConvert.ToString(attribuneInfo.LabelsValues.Count));
                for (int i = 0; i < attribuneInfo.LabelsValues.Count; i++)
                {
                    writer.WriteStartElement("Enum");
                    writer.WriteAttributeString("Value", attribuneInfo.LabelsValues[i]);
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


        public static void writeAboutTable(XmlWriter writer, SampleSet samplesSet, IFuzzySystem Classifier)
        {
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", samplesSet.FileName);
            if (samplesSet == Classifier.LearnSamplesSet)
            {
                writer.WriteAttributeString("Type", "Training");
            }
            else
            {
                writer.WriteAttributeString("Type", "Testing");
            }
            writer.WriteAttributeString("Output", samplesSet.OutputAttribute.Name);
            writer.WriteStartElement("Attributes");
            writer.WriteAttributeString("Count", XmlConvert.ToString(samplesSet.CountVars + 1));
            for (int i = 0; i < samplesSet.CountVars; i++)
            {
                writeAboutAttribute(writer, samplesSet.InputAttributes[i]);
            }
            writeAboutAttribute(writer, samplesSet.OutputAttribute);
            writer.WriteEndElement();
            writeAboutRows(writer, samplesSet);


            writer.WriteEndElement();
        }

        public static void writeAboutObservation(XmlWriter writer, IFuzzySystem Classifier)
        {
            writer.WriteStartElement("Observations");
            if (Classifier.TestSamplesSet != null)
            {
                writer.WriteAttributeString("CountTable", XmlConvert.ToString(2));
                writeAboutTable(writer, Classifier.LearnSamplesSet, Classifier);
                writeAboutTable(writer, Classifier.TestSamplesSet, Classifier);
            }
            else
            {
                writer.WriteAttributeString("CountTable", XmlConvert.ToString(1));
                writeAboutTable(writer, Classifier.LearnSamplesSet, Classifier);
            }


            writer.WriteEndElement();
        }


        public static void writeAboutTerm(XmlWriter writer,  int index, Term term)
        {
            writer.WriteStartElement("Term");
            writer.WriteAttributeString("Name",
                                       XmlConvert.ToString(index));
            switch (term.TermFuncType)
            {
                case TypeTermFuncEnum.Треугольник:
                    writer.WriteAttributeString("Type", "Triangle");
                    break;
                case TypeTermFuncEnum.Гауссоида:
                    writer.WriteAttributeString("Type", "Gauss");
                    break;
                case TypeTermFuncEnum.Парабола:
                    writer.WriteAttributeString("Type", "Parabolic");
                    break;
                case TypeTermFuncEnum.Трапеция:
                    writer.WriteAttributeString("Type", "Trapezoid");
                    break;

            }

            writer.WriteStartElement("Params");
            for (int i = 0; i < term.CountParams; i++)
            {
                writer.WriteStartElement("Param");
                writer.WriteAttributeString("Number", XmlConvert.ToString(i));
                writer.WriteAttributeString("Value", XmlConvert.ToString(term.Parametrs[i]));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
