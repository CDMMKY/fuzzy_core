using System;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;
using System.Linq;



namespace FuzzySystem.PittsburghClassifier.add_generators
{
    public class GeneratorRulesBySamples : AbstractNotSafeGenerator
    {
        private TypeTermFuncEnum type_func;

      

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

        public override PCFuzzySystem Generate(PCFuzzySystem Classifier, IGeneratorConf config)
        {
            PCFuzzySystem result = Classifier;

            //Filtre(result);

            InitBySamplesConfig config1 = config as InitBySamplesConfig;
            type_func = config1.IBSTypeFunc;
            calc_min_max_for_class(result.LearnSamplesSet);

            InitRulesBySamples(Classifier,type_func);

            result.RulesDatabaseSet[0].TermsSet.Trim();

            //Chiu(result);

            //Console.WriteLine("Обуч: " + Math.Round(result.ClassifyLearnSamples(result.RulesDatabaseSet[0]), 2));
            //Console.WriteLine("Тест: " + Math.Round(result.ClassifyTestSamples(result.RulesDatabaseSet[0]), 2));
            //Console.WriteLine();

            return result;
        }

        private void Filtre(PCFuzzySystem result)
        {
            List<double> attributes = new List<double>();
            int level = 50;
            for (int i = 0; i < result.LearnSamplesSet.InputAttributes.Count; i++)
            {
                for (int j = 0; j < result.LearnSamplesSet.DataRows.Count; j++)
                {
                    attributes.Add(result.LearnSamplesSet.DataRows[j].InputAttributeValue[i]);
                }
            }
            for (int i = 0; i < result.LearnSamplesSet.InputAttributes.Count; i++)
            {
                double h = (result.LearnSamplesSet.InputAttributes[i].Max - result.LearnSamplesSet.InputAttributes[i].Min) / (level - 1);
                for (int j = 0; j < result.LearnSamplesSet.DataRows.Count; j++)
                {
                    result.LearnSamplesSet.DataRows[j].InputAttributeValue[i] = Math.Round((result.LearnSamplesSet.DataRows[j].InputAttributeValue[i] - result.LearnSamplesSet.InputAttributes[i].Min) / h) + 1;
                }
            }
            List<double> Entrophies = new List<double>();
            List<double> ConditionalEntrophies = new List<double>();
            List<double> ClassEntrophies = new List<double>();
            List<double> IG = new List<double>();
            double ClassEntrophy = 0;
            List<double> SU = new List<double>();

            for (int i = 0; i < result.CountFeatures; i++)
            {
                Entrophies.Add(Entrophy(result, i, level));
            }

            ClassEntrophy = Entrophy(result);

            for (int j = 0; j < result.CountFeatures; j++)
            {
                ClassEntrophies.Add(ConditionalEntrophy(result, j, level));
            }

            for (int j = 0; j < result.CountFeatures; j++)
            {
                IG.Add(Entrophies[j] - ClassEntrophies[j]);
            }

            for (int j = 0; j < result.CountFeatures; j++)
            {
                SU.Add((2 * IG[j]) / (Entrophies[j] + ClassEntrophy));
            }
            double e = SU.Sum() / SU.Count;
            for (int i = 0; i < SU.Count; i++)
            {
                if (SU[i] < e)
                {
                    result.AcceptedFeatures[i] = false;
                }
            }

            //for (int i = 0; i < result.AcceptedFeatures.Length - 1; i++)
            //{
            //    if (result.AcceptedFeatures[i] == false)
            //        continue;
            //    for (int j = i + 1; j < result.AcceptedFeatures.Length; j++)
            //    {
            //        if (result.AcceptedFeatures[i] == true && result.AcceptedFeatures[j] == true)
            //        {
            //            double temp_su = (2 * (Entrophies[i] - ConditionalEntrophy(result, i, j, level))) / (Entrophies[i] + Entrophies[j]);
            //            if (temp_su > SU[j])
            //            {
            //                result.AcceptedFeatures[j] = false;
            //            }
            //        }
            //    }
            //}

            //for (int i = 0; i < result.AcceptedFeatures.Length; i++)
            //{
            //    if (result.AcceptedFeatures[i] == true)
            //        Console.Write(Convert.ToString(i) + ' ');
            //}
            //Console.WriteLine();
            for (int i = 0; i < result.LearnSamplesSet.InputAttributes.Count; i++)
            {
                for (int j = 0; j < result.LearnSamplesSet.DataRows.Count; j++)
                {
                    result.LearnSamplesSet.DataRows[j].InputAttributeValue[i] = attributes[0];
                    attributes.RemoveAt(0);
                }
            }
        }

        private double Entrophy(PCFuzzySystem result, int x, int level)
        {
            int m = result.LearnSamplesSet.CountSamples;
            List<double> probability = new List<double>();
            for (int j = 1; j <= level; j++)
            {
                int counter = 0;
                for (int i = 0; i < result.LearnSamplesSet.DataRows.Count; i++)
                {
                    if (result.LearnSamplesSet.DataRows[i].InputAttributeValue[x] == j)
                        counter++;
                }
                probability.Add(Convert.ToDouble(counter) / Convert.ToDouble(m));
            }
            double entrophy = 0;
            for (int i = 0; i < probability.Count; i++)
            {
                if (probability[i] != 0)
                    entrophy += probability[i] * Math.Log(probability[i], 2);
            }
            entrophy *= -1;
            return entrophy;
        }

        private double Entrophy(PCFuzzySystem result)
        {
            int m = result.LearnSamplesSet.CountSamples;
            List<double> probability = new List<double>();
            for (int j = 0; j < result.LearnSamplesSet.OutputAttribute.LabelsValues.Count; j++)
            {
                int counter = 0;
                for (int i = 0; i < result.LearnSamplesSet.DataRows.Count; i++)
                {
                    if (result.LearnSamplesSet.DataRows[i].StringOutput == result.LearnSamplesSet.OutputAttribute.LabelsValues[j])
                        counter++;
                }
                probability.Add(Convert.ToDouble(counter) / Convert.ToDouble(m));
            }
            double entrophy = 0;
            for (int i = 0; i < probability.Count; i++)
            {
                if (probability[i] != 0)
                    entrophy += probability[i] * Math.Log(probability[i], 2);
            }
            entrophy *= -1;
            return entrophy;
        }

        private double ConditionalEntrophy(PCFuzzySystem result, int x, int y, int level)
        {
            int m = result.LearnSamplesSet.CountSamples;
            List<double> probability = new List<double>();
            List<double> conditional_probability = new List<double>();
            List<double> y_counter = new List<double>();
            for (int j = 1; j <= level; j++)
            {
                double counter = 0;
                for (int i = 0; i < result.LearnSamplesSet.DataRows.Count; i++)
                {
                    if (result.LearnSamplesSet.DataRows[i].InputAttributeValue[y] == j)
                        counter += 1;
                }
                y_counter.Add(counter);
                probability.Add(counter / Convert.ToDouble(m));
            }
            for (int j = 1; j <= level; j++)
            {
                for (int i = 1; i <= level; i++)
                {
                    double counter = 0;
                    for (int n = 0; n < result.LearnSamplesSet.DataRows.Count; n++)
                    {
                        if (result.LearnSamplesSet.DataRows[n].InputAttributeValue[y] == j && result.LearnSamplesSet.DataRows[n].InputAttributeValue[x] == i)
                            counter += 1;
                    }
                    if (y_counter[j - 1] == 0)
                        conditional_probability.Add(0);
                    else
                        conditional_probability.Add(Convert.ToDouble(counter) / Convert.ToDouble(y_counter[j - 1]));
                }
            }
            double entrophy = 0;
            for (int j = 0; j < level; j++)
            {
                double temp = 0;
                for (int i = 0; i <= level; i++)
                {
                    if (conditional_probability[i + j] != 0)
                        temp += conditional_probability[i + j] * Math.Log(conditional_probability[i + j], 2);
                }
                if (probability[j] != 0)
                    entrophy += probability[j] * temp;
            }
            entrophy *= -1;
            return entrophy;
        }

        private double ConditionalEntrophy(PCFuzzySystem result, int x, int level)
        {
            int m = result.LearnSamplesSet.CountSamples;
            List<double> probability = new List<double>();
            List<double> conditional_probability = new List<double>();
            List<double> y_counter = new List<double>();
            for (int j = 0; j < result.LearnSamplesSet.OutputAttribute.LabelsValues.Count; j++)
            {
                double counter = 0;
                for (int i = 0; i < result.LearnSamplesSet.DataRows.Count; i++)
                {
                    if (result.LearnSamplesSet.DataRows[i].StringOutput == result.LearnSamplesSet.OutputAttribute.LabelsValues[j])
                        counter += 1;
                }
                y_counter.Add(counter);
                probability.Add(counter / Convert.ToDouble(m));
            }
            for (int j = 0; j < result.LearnSamplesSet.OutputAttribute.LabelsValues.Count; j++)
            {
                for (int i = 1; i <= level; i++)
                {
                    double counter = 0;
                    for (int n = 0; n < result.LearnSamplesSet.DataRows.Count; n++)
                    {
                        if (result.LearnSamplesSet.DataRows[n].StringOutput == result.LearnSamplesSet.OutputAttribute.LabelsValues[j] && result.LearnSamplesSet.DataRows[n].InputAttributeValue[x] == i)
                            counter += 1;
                    }
                    if (y_counter[j] == 0)
                        conditional_probability.Add(0);
                    else
                        conditional_probability.Add(Convert.ToDouble(counter) / Convert.ToDouble(y_counter[j]));
                }
            }
            double entrophy = 0;
            for (int j = 0; j < result.LearnSamplesSet.OutputAttribute.LabelsValues.Count; j++)
            {
                double temp = 0;
                for (int i = 0; i < level; i++)
                {
                    if (conditional_probability[i + j] != 0)
                        temp += conditional_probability[i + j] * Math.Log(conditional_probability[i + j], 2);
                }
                if (probability[j] != 0)
                    entrophy += probability[j] * temp;
            }
            entrophy *= -1;
            return entrophy;
        }

        private void Chiu(PCFuzzySystem result)
        {
            double best_res = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
            double res = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
            for (int j = 0; j < result.AcceptedFeatures.Length; j++)
            {
                int n = 0;
                for (int i = 0; i < result.AcceptedFeatures.Length; i++)
                {
                    if (result.AcceptedFeatures[i] == true)
                    {
                        result.AcceptedFeatures[i] = false;
                        if (result.ClassifyLearnSamples(result.RulesDatabaseSet[0]) >= res)
                        {
                            res = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                            n = i;
                        }
                        else
                            result.AcceptedFeatures[i] = true;
                    }
                }
                if (res >= best_res)
                    best_res = res;
                else
                    break;
            }

            //for (int i = 0; i < result.AcceptedFeatures.Length; i++)
            //{
            //    if (result.AcceptedFeatures[i] == true)
            //        Console.Write(Convert.ToString(i) + ' ');
            //}

            //Console.WriteLine();
        }

        protected void InitRulesBySamples(PCFuzzySystem Classifier,TypeTermFuncEnum typeFunc)
        {
            if ((Classifier.RulesDatabaseSet == null) || (Classifier.RulesDatabaseSet.Count == 0))
            {
                KnowlegeBasePCRules temp_rules = new KnowlegeBasePCRules();
               Classifier.RulesDatabaseSet.Add(temp_rules);
            }

            for (int i = 0; i < Classifier.CountClass; i++)
            {
                if (!ExistClassFeatureMax(i) || !ExistClassFeatureMin(i))
                {
                    continue;

                }
                int[] order_terms = new int[Classifier.CountFeatures];

                for (int j = 0; j < Classifier.CountFeatures; j++)
                {

                    if (Classifier.AcceptedFeatures[j] == false)
                    { continue; }


                    double[] paramerts = new double[Term.CountParamsinSelectedTermType(typeFunc)];
                    switch (typeFunc)
                    {
                        case TypeTermFuncEnum.Треугольник:
                            paramerts[0] =  min_for_class[i][j] - 0.001 * ( max_for_class[i][j] -  min_for_class[i][j]);
                            paramerts[2] =  max_for_class[i][j] + 0.001 * ( max_for_class[i][j] -  min_for_class[i][j]);
                            paramerts[1] = (paramerts[0] + paramerts[2]) / 2;
                            break;
                        case TypeTermFuncEnum.Гауссоида:
                            paramerts[0] = ( max_for_class[i][j] +
                                             min_for_class[i][j]) / 2;
                            paramerts[1] = (paramerts[0] -  min_for_class[i][j]) / 3; // rule of 3g
                            break;
                        case TypeTermFuncEnum.Парабола:
                            paramerts[0] =  min_for_class[i][j] - 0.001 * ( max_for_class[i][j] -  min_for_class[i][j]);
                            paramerts[1] =  max_for_class[i][j] + 0.001 * ( max_for_class[i][j] -  min_for_class[i][j]);
                            break;
                        case TypeTermFuncEnum.Трапеция:
                            paramerts[0] =  min_for_class[i][j] - 0.001 * ( max_for_class[i][j] - min_for_class[i][j]);
                            paramerts[3] =  max_for_class[i][j] + 0.001 * ( max_for_class[i][j] -  min_for_class[i][j]);
                            paramerts[1] = paramerts[0] + 0.4 * (paramerts[3] - paramerts[0]);
                            paramerts[2] = paramerts[0] + 0.6 * (paramerts[3] - paramerts[0]);
                            break;

                    }
                    Term temp_term = new Term(paramerts, typeFunc, j);
                    Classifier.RulesDatabaseSet[0].TermsSet.Add(temp_term);
                    order_terms[j] = Classifier.RulesDatabaseSet[0].TermsSet.Count - 1;

                }
                PCRule temp_Rule = new PCRule(Classifier.RulesDatabaseSet[0].TermsSet, order_terms,
                                                          Classifier.LearnSamplesSet.OutputAttribute.LabelsValues[i], 1);
                Classifier.RulesDatabaseSet[0].RulesDatabase.Add(temp_Rule);
            }

        }

        private void calc_min_max_for_class(SampleSet Set)
        {
            min_for_class = new double[Set.CountClass][];
            max_for_class = new double[Set.CountClass][];
            for (int i = 0; i < Set.CountClass; i++)
            {
                string test_label = Set.OutputAttribute.LabelsValues[i];

                List<FuzzySystem.FuzzyAbstract.SampleSet.RowSample> temp_rows = Set.DataRows.FindAll(x => x.StringOutput == test_label);
                if (temp_rows.Count > 0)
                {
                    max_for_class[i] = new double[Set.CountVars];
                    min_for_class[i] = new double[Set.CountVars];

                    for (int j = 0; j < Set.CountVars; j++)
                    {
                        max_for_class[i][j] = temp_rows.Max(x => x.InputAttributeValue[j]);
                        min_for_class[i][j] = temp_rows.Min(x => x.InputAttributeValue[j]);
                    }
                }
            }

            for (int i = 0; i < Set.CountVars; i++)
            {
                int max_index = 0;

                int min_index = 0;

                double current_min = Math.Abs(Set.InputAttributes[i].Min - min_for_class[min_index][i]);
                double current_max = Math.Abs(Set.InputAttributes[i].Max - max_for_class[max_index][i]);
                for (int j = 1; j < Set.CountClass; j++)
                {
                    if ((min_for_class[j] == null) || (max_for_class[j] == null))
                    {
                        continue;
                    }

                    if (Math.Abs(min_for_class[j][i] - Set.InputAttributes[i].Min) < current_min)
                    {
                        min_index = j;
                        current_min = Math.Abs(Set.InputAttributes[i].Min - min_for_class[min_index][i]);
                    }
                    if (Math.Abs(max_for_class[j][i] - Set.InputAttributes[i].Max) < current_max)
                    {
                        max_index = j;
                        current_max = Math.Abs(Set.InputAttributes[i].Max - max_for_class[max_index][i]);
                    }
                }

                if (min_for_class[min_index] != null) { min_for_class[min_index][i] = Set.InputAttributes[i].Min * 0.9; }
                if (max_for_class[max_index] != null) { max_for_class[max_index][i] = Set.InputAttributes[i].Max * 1.1; }
            }


            for (int i = 0; i < Set.CountVars; i++)
            {
                for (int j = 0; j < Set.CountClass; j++)
                {
                    if ((min_for_class[j] == null) || (max_for_class[j] == null))
                    {
                        continue;
                    }

                    int neates_index = 1;
                    double current_nearest = Math.Abs(max_for_class[j][i] - min_for_class[neates_index][i]);
                    bool laid = max_for_class[j][i] > min_for_class[neates_index][i];
                    for (int k = 1; k < Set.CountClass; k++)
                    {
                        if ((j == k) || (max_for_class[j] == null) || (min_for_class[k] == null))
                        {
                            continue;

                        }
                        if (max_for_class[j][i] > min_for_class[k][i])
                        {
                            laid = true;
                        }
                        else
                        {
                            double temp_nearest = Math.Abs(max_for_class[j][i] - min_for_class[k][i]);
                            if (temp_nearest < current_nearest)
                            {
                                current_nearest = temp_nearest;
                                neates_index = k;
                            }
                        }
                    }
                    if (!laid)
                    {
                        double temp = max_for_class[j][i];
                        max_for_class[j][i] = min_for_class[neates_index][i];
                        min_for_class[neates_index][i] = temp;
                    }
                }
            }
        }

        protected double[][] min_for_class;
        protected double[][] max_for_class;

        public bool ExistClassFeatureMax(int num_class)
        {
            return max_for_class[num_class] != null;
        }
        public bool ExistClassFeatureMin(int num_class)
        {
            return min_for_class[num_class] != null;
        }


        public override string ToString(bool with_param = false)
        { if(with_param)
        {
            string result = "По экстремумам классов {";
            result += "Функции принадлежности= " +Term.ToStringTypeTerm(type_func) +" ;"+Environment.NewLine;
            result +="}";
            return result; 
            }
        return "По экстремумам классов";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new InitBySamplesConfig();
            result.Init(CountFeatures);
            return result;
        }
    }
}
