using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate.Mesure;
using FuzzyCore.FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.UFS
{
    public class SAFSUFSWriter : Object
    {
        protected static void writeAboutVariblesAndTerms(XmlWriter writer, SAFuzzySystem Approximate)
        {
            writer.WriteStartElement("Variables");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Approximate.CountFeatures));
            for (int i = 0; i < Approximate.CountFeatures; i++)
            {
                if (Approximate.AcceptedFeatures[i] == false) { continue; }
                writer.WriteStartElement("Variable");
                writer.WriteAttributeString("Name", Approximate.LearnSamplesSet.InputAttributes[i].Name);
                writer.WriteAttributeString("Min",
                                           XmlConvert.ToString(Approximate.LearnSamplesSet.InputAttributes[i].Min));
                writer.WriteAttributeString("Max",
                                           XmlConvert.ToString(Approximate.LearnSamplesSet.InputAttributes[i].Max));
                List<Term> terms_for_varrible =
                    Approximate.RulesDatabaseSet[0].TermsSet.Where(x => x.NumVar == i).ToList();
                writer.WriteStartElement("Terms");
                writer.WriteAttributeString("Count", XmlConvert.ToString(terms_for_varrible.Count));
                foreach (var term in terms_for_varrible)
                {
                   BaseUFSWriter.writeAboutTerm(writer, Approximate.RulesDatabaseSet[0].TermsSet.IndexOf(term), term);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        protected static void writeAboutRules(XmlWriter writer, SAFuzzySystem Approximate)
        {
            writer.WriteStartElement("Rules");
            writer.WriteAttributeString("Count", XmlConvert.ToString(Approximate.RulesDatabaseSet[0].RulesDatabase.Count));
            foreach (SARule rule in Approximate.RulesDatabaseSet[0].RulesDatabase)
            {
                writer.WriteStartElement("Rule");

                writer.WriteStartElement("Antecedent");
                writer.WriteAttributeString("Count", XmlConvert.ToString(rule.ListTermsInRule.Count));
                foreach (Term term in rule.ListTermsInRule)
                {
                    writer.WriteStartElement("Pair");
                    writer.WriteAttributeString("Variable", Approximate.LearnSamplesSet.InputAttributes[term.NumVar].Name);
                    writer.WriteAttributeString("Term", XmlConvert.ToString(Approximate.RulesDatabaseSet[0].TermsSet.IndexOf(term)));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteStartElement("Consequent");
                writer.WriteAttributeString("Value", XmlConvert.ToString(rule.IndependentConstantConsequent));
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

        }

        protected static void writeAboutEstimates(XmlWriter writer, SAFuzzySystem Approximate)
        {
            writer.WriteStartElement("Estimates");
            if (Approximate.TestSamplesSet != null)
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(22));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approxLearnSamples(Approximate.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "MSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.RMSEtoMSEforLearn( Approximate.approxLearnSamples(Approximate.RulesDatabaseSet[0]))));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.TestSamplesSet.FileName);
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approxTestSamples(Approximate.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.TestSamplesSet.FileName);
                writer.WriteAttributeString("Type", "MSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.RMSEtoMSEforTest( Approximate.approxTestSamples(Approximate.RulesDatabaseSet[0]))));
                writer.WriteEndElement();
       
            }
            else
            {
                writer.WriteAttributeString("Count", XmlConvert.ToString(20));
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "RMSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.approxLearnSamples(Approximate.RulesDatabaseSet[0])));
                writer.WriteEndElement();
                writer.WriteStartElement("Estimate");
                writer.WriteAttributeString("Table", Approximate.LearnSamplesSet.FileName);
                writer.WriteAttributeString("Type", "MSE");
                writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.RMSEtoMSEforLearn( Approximate.approxLearnSamples(Approximate.RulesDatabaseSet[0]))));
                writer.WriteEndElement();
            }
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGIBNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGIBSumStrait()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GIBSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGIBSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGICNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGICSumStraigth()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GICSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGICSumReverce()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGISNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGISSumStraigt()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "GISSumReverce");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getGISSumReverce()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisNormal");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getLindisNormal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisSumStraigh");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getLindisSumStraight()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "LindisSumReverse");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getLindisSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "NormalIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getNormalIndex()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "RealIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getIndexReal()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "SumStraigthIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getIndexSumStraigt()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "SumReverseIndex");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getIndexSumReverse()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "ComplexitIt");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getComplexit()));
            writer.WriteEndElement();
            writer.WriteStartElement("Estimate");
            writer.WriteAttributeString("Type", "CountRules");
            writer.WriteAttributeString("Value", XmlConvert.ToString(Approximate.getRulesCount()));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        public static bool saveToUFS(SAFuzzySystem Approximate, string fileName)
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
            writer.WriteAttributeString("Type", "ApproximatorSingleton");
            writeAboutVariblesAndTerms(writer, Approximate);
            writeAboutRules(writer, Approximate);
           BaseUFSWriter.writeAboutObservation(writer, Approximate);
            writeAboutEstimates(writer, Approximate);
            writer.WriteEndElement();
            //   writer.Flush();
            writer.Close();
            return false;
        }
    }
}
