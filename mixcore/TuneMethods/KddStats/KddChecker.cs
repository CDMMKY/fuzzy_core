using System;
using System.Collections.Generic;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class KddChecker : AbstractNotSafeLearnAlgorithm
    {
        double FalsePositiveLearn;
        double FalseNegativeLearn;
        double FalsePositiveTest;
        double FalseNegativeTest;
        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }


        public override FuzzySystem.PittsburghClassifier.PCFuzzySystem TuneUpFuzzySystem(FuzzySystem.PittsburghClassifier.PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            PCFuzzySystem result = Classifier;
            FalsePositiveLearn = 0;
            FalseNegativeLearn = 0;

            FalsePositiveTest = 0;
            FalseNegativeTest = 0;

            string normalClass = "normal.";

            for (int i = 0; i < Classifier.LearnSamplesSet.CountSamples; i++)
            {
                string ClassifierResult = Classifier.classifyBase(Classifier.LearnSamplesSet[i].InputAttributeValue, Classifier.RulesDatabaseSet[0]);
                if (Classifier.LearnSamplesSet.DataRows[i].StringOutput.Contains(normalClass))
                {

                    if (!ClassifierResult.Contains(normalClass))
                    {
                        FalsePositiveLearn++;
                        continue;
                    }
                }

                if (ClassifierResult.Contains(normalClass))
                {
                    if (!Classifier.LearnSamplesSet.DataRows[i].StringOutput.Contains(normalClass))
                    {
                        FalseNegativeLearn++; 
                    }
                }
            }
            FalsePositiveLearn = FalsePositiveLearn / Classifier.LearnSamplesSet.CountSamples*100;
            FalseNegativeLearn = FalseNegativeLearn / Classifier.LearnSamplesSet.CountSamples*100;

            for (int i = 0; i < Classifier.TestSamplesSet.CountSamples; i++)
            {
                string ClassifierResult = Classifier.classifyBase(Classifier.TestSamplesSet[i].InputAttributeValue, Classifier.RulesDatabaseSet[0]);
                if (Classifier.TestSamplesSet.DataRows[i].StringOutput.Contains(normalClass))
                {

                    if (!ClassifierResult.Contains(normalClass))
                    {
                        FalsePositiveTest++;
                        continue;
                    }
                }

                if (ClassifierResult.Contains(normalClass))
                {
                    if (!Classifier.TestSamplesSet.DataRows[i].StringOutput.Contains(normalClass))
                    {
                        FalseNegativeTest++;
                    }
                }
            }
            FalsePositiveTest = FalsePositiveTest / Classifier.TestSamplesSet.CountSamples*100;
            FalseNegativeTest = FalseNegativeTest / Classifier.TestSamplesSet.CountSamples*100;





            Classifier.RulesDatabaseSet[0].TermsSet.Trim();
            return Classifier;
        }

        public override string ToString(bool with_param = false)// без параметров возвращает имя алгоритма, с параметров true возвращает имя алгоритма и значения его параметров
        {
            if (with_param)
            {
                string result = "KDD ошибки первого и второго рода{";
                 result += "Ошибка первого рода на обучающей выборке= " + FalsePositiveLearn.ToString() + " (%);" + Environment.NewLine;
                 result += "Ошибка второго рода на обучающей выборке= " + FalseNegativeLearn.ToString() + " (%);" + Environment.NewLine;
                 result += "Ошибка первого рода на тестовой выборке= " + FalsePositiveTest.ToString() + " (%);" + Environment.NewLine;
                 result += "Ошибка второго рода на тестовой выборке= " + FalseNegativeTest.ToString() + " (%);" + Environment.NewLine;
                   result += "}";
                return result;
            }
            return "KDD ошибки первого и второго рода";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures) // Создание класса конфигураатора для вашего метода
        {
            ILearnAlgorithmConf result = new NullConfForAll();
            result.Init(CountFeatures);
            return result;
        }


    }
}
