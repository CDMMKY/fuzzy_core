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
    public class MBA : AbstractNotSafeLearnAlgorithm
    {
        int count_populate;
        int count_iteration;
        int exploration;
        int reduce_koef;

        public override PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem Classifier, ILearnAlgorithmConf conf)
        {
            count_iteration = ((Param)conf).Количество_итераций;
            count_populate = ((Param)conf).Число_осколков;
            exploration = ((Param)conf).Фактор_исследования;
            reduce_koef = ((Param)conf).Уменьшающий_коэффициент;

            int iter = 0, i, j, count_terms, var = 0;
            int count_cons;
            double RMSE_best, cosFi;
            int Nd, variables, k = 1, best = 0;
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
            RMSE_best = Classifier.ClassifyLearnSamples(0);
            count_cons = Classifier.RulesDatabaseSet[0].Weigths.Count();
            double[] RMSE = new double[count_populate];
            double[] RMSE_tst = new double[count_populate];
            double[] RMSE2 = new double[count_populate];
            double[] RMSE_pred = new double[2];
            double[] cons_best = new double[count_cons];
            count_terms = Classifier.RulesDatabaseSet[0].TermsSet.Count;
            variables = Classifier.LearnSamplesSet.CountVars;
            int[] terms = new int[variables];

            
            KnowlegeBasePCRules[] X = new KnowlegeBasePCRules[count_populate];
            for (int s = 0; s < count_populate - 1; s++)
            {
                X[s] = new KnowlegeBasePCRules(Classifier.RulesDatabaseSet[0]);
                Classifier.RulesDatabaseSet.Add(X[s]);
            }
            RMSE_best = Classifier.ClassifyLearnSamples(0);
            for (int h = 0; h < count_terms; h++)
            {
                if (Classifier.RulesDatabaseSet[0].TermsSet[h].NumberOfInputVar == var) terms[var]++;
                else
                {
                    terms[var + 1]++;
                    var++;
                }
            }
            for (iter = 0; iter <= count_iteration; iter++)
            {
                best = 0;
                if (iter == 0)
                {
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            shrapnel[0, k] = Classifier.RulesDatabaseSet[0].TermsSet[h].Parametrs[p];
                            X_best[k] = shrapnel[0, k];
                            X_pred[0, k] = shrapnel[0, k];
                            X_pred[1, k] = shrapnel[0, k];
                            k++;
                        }
                    RMSE_pred[0] = Classifier.ClassifyLearnSamples(0);
                    RMSE_pred[1] = Classifier.ClassifyLearnSamples(0);
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            d[0, k] = RandomNext(Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[h].NumberOfInputVar).Min, Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[h].NumberOfInputVar).Max);
                            k++;
                        }
                }
                for (i = 1; i <= Nd; i++)
                {
                    if (exploration > iter)
                    {
                        for (j = 1; j < count_populate; j++)
                        {
                            int sum = 0, sum2 = 0;
                        generate:
                            sum++;
                            sum2++;
                            //формула расстояния исправлена

                            d[j, i] = d[j - 1, i] * randn();

                            //double sluch = randn();
                            //if (sluch < 0) d[j, i] = d[j - 1, i] * (-1) * Math.Pow(sluch, 2);
                            //else d[j, i] = d[j - 1, i] * Math.Pow(sluch, 2);
                            explosion[j, i] = d[j, i] * cosFi;
                            if (sum > 20)
                            {
                                if ((i + (type - 2)) % type == 0)
                                {
                                    shrapnel[j, i] = (shrapnel[0, i] + explosion[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                    if (sum2 > 2)
                                    {
                                        shrapnel[j, i] = (shrapnel[0, i] + explosion[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]);
                                        sum = 19;
                                    }
                                    if (sum2 > 3)
                                    {
                                        shrapnel[j, i] = (shrapnel[0, i] + explosion[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                        sum = 19;
                                        sum2 = 0;
                                    }
                                }
                                else
                                {
                                    shrapnel[j, i] = (shrapnel[0, i] + explosion[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                    sum = 19;
                                }
                            }
                            else shrapnel[j, i] = shrapnel[0, i] + explosion[j, i];

                            if ((i == 2) || (i == 1))
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / type)].NumberOfInputVar)
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min; goto exit;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - type) / type)].NumberOfInputVar)
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min; goto exit;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != (variables - 1))
                            {
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i) / type)].NumberOfInputVar)
                                {
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max; goto exit;
                                }
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i + 1) / type)].NumberOfInputVar)
                                {
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max; goto exit;
                                }
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar == (variables - 1))
                            {
                                if ((i == (count_terms * 3 - 1)) || (i == (count_terms * 3)))
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max;
                            }

                            if (((i + (type - 2)) % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                if (shrapnel[j, i] == Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min) i--;
                                goto generate;
                            }
                            if ((i % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                goto generate;
                            }
                            if (i != 1)
                            {
                                if (((i - (type - 2)) % type == 0) && ((shrapnel[j, i] > shrapnel[j, i - 1]) || (shrapnel[j, i] > Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Max) || (shrapnel[j, i] < Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min)))
                                {
                                    goto generate;
                                }
                            }
                            if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min) || (shrapnel[j, i] > Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Max)))
                            {
                                goto generate;
                            }
                        exit:
                            if (i > type)
                            {
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != 0)
                                {
                                    if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1 - type) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar - 1)
                                    {
                                        if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < shrapnel[j, i - type])))
                                        {
                                            goto generate;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        d[0, i] = d2(X_pred[0, i], X_pred[1, i], RMSE_pred[0], RMSE_pred[1]);

                        for (j = 1; j < count_populate; j++)
                        {
                            if ((X_pred[1, i] - X_pred[0, i]) != 0)
                            {
                                direction[j, i] = m(X_pred[0, i], X_pred[1, i], RMSE_pred[0], RMSE_pred[1]);
                            }
                            else direction[j, i] = 1;
                            int sum = 0, sum2 = 0;
                        generate:
                            sum++;
                            sum2++;
                            double random;
                            random = randn();
                            if (random < 0) explosion[j, i] = d[j - 1, i] * rand.NextDouble() * cosFi * (-1);
                            else explosion[j, i] = d[j - 1, i] * rand.NextDouble() * cosFi;


                            if (sum > 20)
                            {
                                if ((i + (type - 2)) % type == 0)
                                {
                                    shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                    if (sum2 > 2)
                                    {
                                        shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]);
                                        sum = 19;
                                    }
                                    if (sum2 > 3)
                                    {
                                        shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                        sum = 19;
                                        sum2 = 0;
                                    }
                                }
                                else
                                {
                                    shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                                    sum = 19;
                                }
                            }
                            else shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]);

                            if ((i == 2) || (i == 1))
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / type)].NumberOfInputVar)
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - type) / type)].NumberOfInputVar)
                            {
                                shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Min;
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != (variables - 1))
                            {
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i) / type)].NumberOfInputVar)
                                {
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max;
                                }
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i + 1) / type)].NumberOfInputVar)
                                {
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max;
                                }
                            }
                            if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar == (variables - 1))
                            {
                                if ((i == (count_terms * 3 - 1)) || (i == (count_terms * 3)))
                                    shrapnel[j, i] = Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar).Max;
                            }

                            if (((i + (type - 2)) % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                if (shrapnel[j, i] == Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min) i--;
                                goto generate;
                            }
                            if ((i % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                goto generate;
                            }
                            if (i != 1)
                            {
                                if (((i - (type - 2)) % type == 0) && ((shrapnel[j, i] > shrapnel[j, i - 1]) || (shrapnel[j, i] > Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Max) || (shrapnel[j, i] < Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min)))
                                {
                                    goto generate;
                                }
                            }
                            if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Min) || (shrapnel[j, i] > Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar).Max)))
                            {
                                goto generate;
                            }
                            if (i > type)
                            {
                                if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != 0)
                                {
                                    if (Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1 - type) / type)].NumberOfInputVar != Classifier.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar - 1)
                                    {
                                        if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < shrapnel[j, i - type])))
                                        {
                                            goto generate;
                                        }
                                    }
                                }
                            }
                            d[j, i] = d[j - 1, i] / Math.Pow(Math.E, (double)iter / (double)reduce_koef);
                        }
                    }
                }

                for (int z = 0; z < count_populate; z++)
                {
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            Classifier.RulesDatabaseSet[z].TermsSet[h].Parametrs[p] = shrapnel[z, k];
                            k++;
                        }
                }
                for (j = 0; j < count_populate; j++)
                {
                    RMSE[j] = Classifier.ClassifyLearnSamples(j);
                    RMSE_tst[j] = Classifier.ClassifyTestSamples(j);
                    if (RMSE[j] > RMSE_best)
                    {
                        RMSE_best = RMSE[j];
                        best = j;
                    }
                }

                k = 1;
                if (iter % 10 == 0)
                {
                    k = 1;
                }
                for (int h = 0; h < count_terms; h++)
                    for (int p = 0; p < type; p++)
                    {
                        shrapnel[0, k] = shrapnel[best, k];
                        if (exploration > iter) d[0, k] = RandomNext(Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[h].NumberOfInputVar).Min, Classifier.LearnSamplesSet.InputAttribute(Classifier.RulesDatabaseSet[0].TermsSet[h].NumberOfInputVar).Max);
                        Classifier.RulesDatabaseSet[0].TermsSet[h].Parametrs[p] = shrapnel[0, k];
                        k++;
                    }


                if (RMSE_pred[1] < RMSE[best])
                {
                    for (k = 1; k <= Nd; k++)
                    {
                        X_pred[0, k] = X_pred[1, k];
                        X_pred[1, k] = shrapnel[best, k];
                    }
                    RMSE_pred[0] = RMSE_pred[1];
                    RMSE_pred[1] = RMSE[best];
                }
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

        public double d2(double X1, double X2, double RMSE1, double RMSE2)
        {
            return Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(RMSE2 - RMSE1, 2));
        }
        public double ReduceD(double d, double stepen)
        {
            return d / Math.Pow(Math.E, stepen);
        }
        public double Shrapnel(double xe, double x, double d, double m)
        {
            double s = -Math.Sqrt((double)(Math.Abs(m / d)));
            return xe + x * Math.Pow(Math.PI, s);
        }

        public double m(double X1, double X2, double RMSE1, double RMSE2)
        {
            return (RMSE2 - RMSE1) / (X2 - X1);
        }
        public double Normalization(double x, double max, double d1, double d2)
        {
            return x * (d2 - d1) / max + d1;
        }
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Mine blust alghoritm{";
                // result+= param1+Environment.NewLine;
                result += "}";
                return result;
            }
            return "Mine blust alghoritm";
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
                return new List<FuzzySystemRelisedList.TypeSystem>() {FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
            }
        }

    }
}

