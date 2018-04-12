using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.conf;
using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    class WeigthsConfigRandomSearch : AbstractNotSafeLearnAlgorithm
    {
        int count_iteration;
        int count_generation_by_iteration;

        Random rand = new Random();


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }
        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            count_iteration = ((WeigthsRandomSearchConfig)conf).WRSCCountIteration;
            count_generation_by_iteration =
                ((WeigthsRandomSearchConfig)conf).WRSCCountRules;

            PCFuzzySystem result = Classifier;


            for (int i = 0; i < count_iteration; i++)
            { 
                double [][] Weigths = new double[count_generation_by_iteration+1][];
                Weigths[0] = Classifier.RulesDatabaseSet[0].Weigths;
                double best_result = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                
                int best_index = 0;
                for (int j = 1; j < count_generation_by_iteration+1; j++)
                {  
                    Weigths[j]= new double[Weigths[0].Count()];
                    for (int k=0; k<Weigths[0].Count();k++)
                    {
                        Weigths[j][k] = rand.NextDouble();
                    }

                    result.RulesDatabaseSet[0].Weigths = Weigths[j];
                    double current_result = result.ClassifyLearnSamples(result.RulesDatabaseSet[0]);
                    if (current_result > best_result)
                    {
                        best_result = current_result;
                        best_index = j;
                    }
                    result.RulesDatabaseSet[0].Weigths = Weigths[best_index];
                }
            }
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "случайная оптимизация (весов) {";
                result += "Итераций =" + count_iteration.ToString() + " ; " + Environment.NewLine;

                result += "Вариантов баз правил за итерацию =" + count_generation_by_iteration.ToString() + " ; " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "случайная оптимизация (весов)";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new RandomSearchConf();
            result.Init(CountFeatures);
            return result;
        }

    }
}
