using System;
using System.Collections.Generic;
using System.Xml;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.UFS
{
    public static class PCFSUFSLoader
    {


        public static PCFuzzySystem loadUFS(this PCFuzzySystem Classifier, string fileName)
        {  XmlDocument Source = new XmlDocument();
            Source.Load(fileName);
            return Classifier.loadUFS(Source);
               
        }


        public static PCFuzzySystem loadUFS(this PCFuzzySystem Classifier, XmlDocument Source)
        {
            PCFuzzySystem result = Classifier;
            KnowlegeBasePCRules New_dataBase = new KnowlegeBasePCRules();
            List<string> added_term = new List<string>();
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
                     TypeTermFuncEnum type_term = TypeTermFuncEnum.Треугольник;
                     int num_var=  Classifier.LearnSamplesSet.InputAttributes.IndexOf(Classifier.LearnSamplesSet.InputAttributes.Find (x=>x.Name.Equals(antecedent_node.ChildNodes[j].Attributes.GetNamedItem("Variable").Value,StringComparison.OrdinalIgnoreCase)));
                 string  name_term=antecedent_node.ChildNodes[j].Attributes.GetNamedItem("Term").Value;
                     if (added_term.Contains(name_term) )
                     {Order_term[j]=added_term.IndexOf(name_term);}
                     else{
                    XmlNode term_node =varibles_node.SelectSingleNode("descendant::Term[@Name='"+name_term+"']");
                 int count_MB=0;
                     switch (term_node.Attributes.GetNamedItem("Type").Value)
                     {case "Triangle":{ count_MB=3; type_term = TypeTermFuncEnum.Треугольник; break;}
                      case "Gauss":{ count_MB=2; type_term = TypeTermFuncEnum.Гауссоида; break;}
                      case "Parabolic":{count_MB=2; type_term = TypeTermFuncEnum.Парабола; break;}
                      case "Trapezoid":{count_MB=4; type_term = TypeTermFuncEnum.Трапеция; break;}
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
                     
                     New_dataBase.TermsSet.Add(temp_term);
                added_term.Add(name_term);
                     Order_term[j]=New_dataBase.TermsSet.Count-1;
                     }
                 }
                XmlNode consequnt_node = rulles_node.ChildNodes[i].SelectSingleNode("Consequent");
                string  Classifier_value = consequnt_node.Attributes.GetNamedItem("Class").Value;
                double Classifier_Weigths =XmlConvert.ToDouble (consequnt_node.Attributes.GetNamedItem("CF").Value); 
                PCRule temp_rule = new PCRule(New_dataBase.TermsSet,Order_term,Classifier_value,Classifier_Weigths);
                New_dataBase.RulesDatabase.Add(temp_rule);
            }
            result.RulesDatabaseSet.Clear();
            result.RulesDatabaseSet.Add(New_dataBase);
            GC.Collect();
                return result;
        }

    }
}
