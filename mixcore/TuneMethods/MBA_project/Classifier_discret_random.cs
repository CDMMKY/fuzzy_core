using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm;
using FuzzySystem.FuzzyAbstract.conf;

namespace MBA_Methods
{
    public class Classifier_discret_random : AbstractNotSafeLearnAlgorithm
    {
        int count_populate;
        int count_iteration;
        string iskl_prizn;
        string priznaki_usech;
        int iter_descrete;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            iskl_prizn = "";
            count_iteration = ((Param)conf).Количество_итераций;
            count_populate = ((Param)conf).Число_осколков;
            priznaki_usech = ((Param)conf).Усечённые_признаки;
            iter_descrete = ((Param)conf).Итерации_дискр_алг;

            int iter = 0, iter2, i, j, count_terms;//, count_iter ;
       //     int count_best2 = 0;//, best_pred ;
            double RMSE_best, cosFi, RMSE_best2;
            int Nd, variables, k = 1, best2 = 0;
            PCFuzzySystem result = Classifier;
            int type = Classifier.RulesDatabaseSet[0].TermsSet[0].CountParams;
            Nd = Classifier.RulesDatabaseSet[0].TermsSet.Count * type;
            double[] X_best = new double[Nd + 1];
            double[,] X_pred = new double[2, Nd + 1];
            double[,] direction = new double[count_populate, Nd + 1];
            double[,] d = new double[count_populate, Nd + 1];
            double[,] explosion = new double[count_populate, Nd + 1];
            double[,] shrapnel = new double[count_populate, Nd + 1];
            cosFi = Math.Cos(2 * Math.PI / count_populate);
            RMSE_best = Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[0]);
            RMSE_best2 = Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[0]);
            double[] RMSE2 = new double[count_populate];
            double[] RMSE_pred = new double[2];
            variables = Classifier.LearnSamplesSet.CountVars;
            count_terms = Classifier.RulesDatabaseSet[0].TermsSet.Count;
            int[] terms = new int[variables];

            double[] X_best2 = new double[variables];
            double[,] d3 = new double[count_populate, variables];
            double[,] priznak = new double[count_populate, variables];
            for (i = 0; i < variables; i++)
            {
                Random random = new Random();
                if (random.NextDouble() < 0.5) priznak[0, i] = 1;
                else priznak[0, i] = 0;
            }
            KnowlegeBasePCRules[] X = new KnowlegeBasePCRules[count_populate];
            for (int s = 0; s < count_populate - 1; s++)
            {
                X[s] = new KnowlegeBasePCRules(Classifier.RulesDatabaseSet[0]);
                Classifier.RulesDatabaseSet.Add(X[s]);
            }

            for (iter2 = 0; iter2 < iter_descrete; iter2++)
            {
                best2 = 0;
                if (iter == 0)
                {
                    for (k = 0; k < variables; k++)
                    {
                        d3[0, k] = RandomNext(Classifier.LearnSamplesSet.InputAttributes[k].Min, Classifier.LearnSamplesSet.InputAttributes[k].Max);
                    }
                }
                for (i = 0; i < variables; i++)
                {
                    for (j = 1; j < count_populate; j++)
                    {
                    generate:
                        d3[j, i] = d3[j - 1, i] * randn();
                        priznak[j, i] = d3[j, i] * cosFi;

                        if ((priznak[j, i] < Classifier.LearnSamplesSet.InputAttributes[i].Min) || (priznak[j, i] > Classifier.LearnSamplesSet.InputAttributes[i].Max))
                        {
                            goto generate;
                        }
                        Random random = new Random();
                        if (random.NextDouble() < descret(priznak[j, i])) priznak[j, i] = 1;
                        else priznak[j, i] = 0;
                    }
                }

                for (j = 1; j < count_populate; j++)
                {
                    for (int h = 0; h < variables; h++)
                    {
                        if (priznak[j, h] == 1) Classifier.AcceptedFeatures[h] = true;
                        else Classifier.AcceptedFeatures[h] = false;
                    }
                    RMSE2[j] = Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[j]);
                    if (RMSE2[j] > RMSE_best2)
                    {
                        RMSE_best2 = RMSE2[j];
                        best2 = j;
                    }
                }
       
                for (k = 0; k < variables; k++)
                    priznak[0, k] = priznak[best2, k];
            }

            for (k = 0; k < variables; k++)
                if (priznak[best2, k] == 1)
                    Classifier.AcceptedFeatures[k] = true;
                else
                {
                    Classifier.AcceptedFeatures[k] = false;
                    iskl_prizn += (k + 1).ToString() + " ";
                }

            return result;
        }

        Random rand = new Random();
        public double RandomNext(double min, double max)
        {
            return (max - min) * rand.NextDouble() + min;
        }

        public double d0(double LB, double UB)
        {
            return UB - LB;
        }

        public double randn()
        {
            double x = rand.NextDouble(), z = 0;
            double y = rand.NextDouble();
            if (x != 0 & y != 0)
            {
                z = Math.Cos(2 * Math.PI * y) * Math.Sqrt(-2 * Math.Log10(x));
            }
            return z;
        }

        public double descret(double x)
        {
            double a = 1;
            return a / (1 + Math.Exp(-x));
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Mine blust alghoritm (discret2){";
                result += "Исключённые признаки= " + iskl_prizn + " ;" + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Mine blust alghoritm (discret2)";
        }
        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new Param();
            result.Init(CountFeatures);
            return result;
        }

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }
    }
}



