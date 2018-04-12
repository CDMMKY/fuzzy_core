using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier.Mesure;
using FuzzyCore.FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.UFS

{
    public class PCFSUFSWriter : Object
    {
        private static void writeAboutVariblesAndTerms(XmlWriter writer, PCFuzzySystem Classifier)
        {
            writer.WriteStartElement("Variables");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Classifier.CountFeatures));
            for (int i = 0; i < Classifier.CountFeatures; i++)
            {
                if (Classifier.AcceptedFeatures[i] == false) { continue; }
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", Classifier.LearnSamplesSet.InputAttributes[i].Name);
                writer.WriteAttributeString("Min",
                                           XmlConvert.ToString(Classifier.LearnSamplesSet.InputAttributes[i].Min));
                writer.WriteAttributeString("Max",
                                           XmlConvert.ToString(Classifier.LearnSamplesSet.InputAttributes[i].Max));
                List<Term> terms_for_varrible =
                    Classifier.RulesDatabaseSet[0].TermsSet.Where(x => x.NumVar == i).ToList();
                writer.WriteStartElement("Terms");
                writer.WriteAttributeString("Count", XmlConvert.ToString(terms_for_varrible.Count));

                foreach (var term in terms_for_varrible)
                {
                    BaseUFSWriter.writeAboutTerm(writer, Classifier.RulesDatabaseSet[0].TermsSet.IndexOf(term), term);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void writeAboutRules(XmlWriter writer, PCFuzzySystem Classifier)
        {
            writer.WriteStartElement("Rules");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Classifier.RulesDatabaseSet[0].RulesDatabase.Count));

            foreach (PCRule rule in Classifier.RulesDatabaseSet[0].RulesDatabase)
            {
                writer.WriteStartElement("Rule");

                writer.WriteStartElement("Antecedent");
                writer.WriteAttributeString("Count", XmlConvert.ToString(rule.ListTermsInRule.Count));
                foreach (Term term in rule.ListTermsInRule)
                {
                    writer.WriteStartElement("Pair");
                    writer.WriteAttributeString("Variable", Classifier.LearnSamplesSet.InputAttributes[term.NumVar].Name);
                    writer.WriteAttributeString("Term", XmlConvert.ToString(Classifier.RulesDatabaseSet[0].TermsSet.IndexOf(term)));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteStartElement("Consequent");
                writer.WriteAttributeString("Class", rule.LabelOfClass);
                writer.WriteAttributeString("CF", XmlConvert.ToString(rule.CF));
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        private static void writeAboutEstimates(XmlWriter writer, PCFuzzySystem Classifier)
        {
            writer.WriteStartElement("Estimates");
            if (Classifier.TestSamplesSet != null)
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(22));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "ErrorPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.ErrorLearnSamples(Classifier.RulesDatabaseSet[0]) ));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.TestSamplesSet.FileName);
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.ClassifyTestSamples(Classifier.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.TestSamplesSet.FileName);
                writer.WriteAttributeString("Type", "ErrorPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString( Classifier.ErrorTestSamples(Classifier.RulesDatabaseSet[0])));
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(20));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "PrecisionPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Classifier.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "ErrorPercent");
                writer.WriteAttributeString("Value", XmlConvert.ToString( Classifier.ErrorLearnSamples(Classifier.RulesDatabaseSet[0])));
                writer.WriteEndElement();

            }
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGIBNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGIBSumStrait()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGIBSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGICNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGICSumStraigth()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGICSumReverce()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGISNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGISSumStraigt()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISSumReverce");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getGISSumReverce()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getLindisNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getLindisSumStraight()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getLindisSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "NormalIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getNormalIndex()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "RealIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getIndexReal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "SumStraigthIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getIndexSumStraigt()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "SumReverseIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getIndexSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "ComplexitIt");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getComplexit()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "CountRules");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Classifier.getRulesCount()));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }


        public static bool saveToUFS(PCFuzzySystem Classifier, string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineChars = Environment.NewLine;
            settings.NewLineHandling = NewLineHandling.None;
            settings.OmitXmlDeclaration = false;
            XmlWriter writer = XmlTextWriter.Create(fileName, settings);
            writer.WriteStartElement("FuzzySystem");
            writer.WriteAttributeString("Type", "ClassifierPittsburgh");
            writeAboutVariblesAndTerms(writer, Classifier);
            writeAboutRules(writer, Classifier);
            BaseUFSWriter.writeAboutObservation(writer, Classifier);
            writeAboutEstimates(writer, Classifier);
            writer.WriteEndElement();
            //   writer.Flush();
            writer.Close();
            return false;
        }
    }
}
