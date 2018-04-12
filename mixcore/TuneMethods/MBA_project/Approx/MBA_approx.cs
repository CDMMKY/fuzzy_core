using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem;
using FuzzySystem.SingletoneApproximate;

using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;

namespace MBA_project
{
    public class Class1 : AbstractNotSafeLearnAlgorithm
    {
        int count_populate;
        int count_iteration;
        int exploration;
        int reduce_koef;
        string iskl_prizn;
        string priznaki_usech;

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            iskl_prizn = "";
            count_iteration = ((Param)conf).Количество_итераций;
            count_populate = ((Param)conf).Число_осколков;
            exploration = ((Param)conf).Фактор_исследования;
            reduce_koef = ((Param)conf).Уменьшающий_коэффициент;
            priznaki_usech = ((Param)conf).Усечённые_признаки;
            
            int iter = 0,/* iter2,*/ i, j, count_terms/*, count_iter*/ ;
            int count_cons;//, count_best2 , best_pred ;
            double RMSE_best, cosFi, RMSE_best2, MSEbefore, MSEafter;
            int Nd, variables, k = 1, best = 0;// best2 = 0;
            string[] buf;
            buf = priznaki_usech.Split(' ');
            SAFuzzySystem result = Approx;
            int type = Approx.RulesDatabaseSet[0].TermsSet[0].CountParams;
            Nd = Approx.RulesDatabaseSet[0].TermsSet.Count * type;
            double[] X_best = new double[Nd + 1];
            double[,] X_pred = new double[2,  Nd + 1];
            double[,] direction = new double[count_populate, Nd + 1];
            double[,] d = new double[count_populate, Nd + 1];
            double[,] explosion = new double[count_populate, Nd + 1];
            double[,] shrapnel = new double[count_populate, Nd + 1];
            cosFi = Math.Cos(2 * Math.PI / count_populate);
            RMSE_best = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
            RMSE_best2 = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
            count_cons = Approx.RulesDatabaseSet[0].all_conq_of_rules.Count();
            double[] RMSE = new double[count_populate];
            double[] RMSE_all = new double[iter];
            double[] RMSE_tst = new double[count_populate];
            double[] RMSE2 = new double[count_populate];
            double[] RMSE_pred = new double[2];
            double[] cons_best = new double[count_cons];
            count_terms = Approx.RulesDatabaseSet[0].TermsSet.Count;
            variables = Approx.LearnSamplesSet.CountVars;
            int[] terms = new int[variables];

            double[] X_best2 = new double[variables];
            double[,] d3 = new double[count_populate, variables];
            double[,] priznak = new double[count_populate, variables];
            for (i = 0; i < variables; i++)
            {
                priznak[0, i] = 1;
                X_best2[i] = 1;
            }
            KnowlegeBaseSARules[] X = new KnowlegeBaseSARules[count_populate];
            for (int s = 0; s < count_populate - 1; s++)
            {
                X[s] = new KnowlegeBaseSARules(Approx.RulesDatabaseSet[0]);
                Approx.RulesDatabaseSet.Add(X[s]);
            }

        
            if (buf[0] != "")
            {
                for (k = 0; k < buf.Count(); k++)
                {
                    Approx.AcceptedFeatures[int.Parse(buf[k]) - 1] = false;
                    priznak[0, int.Parse(buf[k]) - 1] = 0;
                    iskl_prizn += buf[k] + " ";
                }
            }

            RMSE_best = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
            for (iter = 0; iter <= count_iteration; iter++)
            {
                best = 0;
                if (iter == 0)
                {
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            shrapnel[0, k] = Approx.RulesDatabaseSet[0].TermsSet[h].Parametrs[p];
                            X_best[k] = shrapnel[0, k];
                            X_pred[0,k] = shrapnel[0, k];
                            X_pred[1, k] = shrapnel[0, k];
                            k++;
                        }
                    RMSE_pred[0] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
                    RMSE_pred[1] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            d[0, k] = RandomNext(Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Min, Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Max);
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
                                shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Min;
                            }
                           
                            if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / type)].NumVar)
                            {
                                shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Min; goto exit;
                            }
                          
                            if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - type) / type)].NumVar)
                            {
                                shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Min; goto exit;
                            }
                            if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != (variables - 1))
                            {
                                if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i) / type)].NumVar)
                                {
                                    shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Max; goto exit;
                                }
                                if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i + 1) / type)].NumVar)
                                {
                                    shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Max; goto exit;
                                }
                            }
                            if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar == (variables - 1))
                            {
                                if((i==(count_terms*3-1))||(i==(count_terms*3)))
                                    shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumVar].Max;
                            }
                           
                            if (((i + (type - 2)) % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                if (shrapnel[j, i] == Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Min) i--;
                                goto generate;
                            }
                            if ((i % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                goto generate;
                            }
                            if (i != 1)
                            {
                                if (((i - (type - 2)) % type == 0) && ((shrapnel[j, i] > shrapnel[j, i - 1]) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Max) || (shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Min)))
                                {
                                    goto generate;
                                }
                            }
                            if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Min) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Max)))
                            {
                                goto generate;
                            }
                        exit:
                            if (i > type)
                            {
                                if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar != 0)
                                {
                                    if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1 - type) / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar - 1)
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
                        //d[0, i] = d2(X_pred[0, i], X_pred[1, i], RMSE_pred[0], RMSE_pred[1]);
                        
                        //for (j = 1; j < count_populate; j++)
                        //{
                        //    if ((X_pred[1, i] - X_pred[0, i]) != 0)
                        //    {
                        //        direction[j, i] = m(X_pred[0, i], X_pred[1, i], RMSE_pred[0], RMSE_pred[1]);
                        //    }
                        //    else direction[j, i] = 1;
                        //    int sum = 0, sum2 = 0;
                        //generate:
                        //    sum++;
                        //    sum2++;
                        //    double random;
                        //    random = randn();
                        //    if(random<0) explosion[j, i] = d[j-1, i]*rand.NextDouble() * cosFi*(-1);
                        //    else explosion[j, i] = d[j - 1, i] * rand.NextDouble() * cosFi;
                        //    if (sum2 > 50) sum2 = 0;
                           
                        //    if (sum > 20)
                        //    {
                        //        if ((i + (type - 2)) % type == 0)
                        //        {
                        //            shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                        //            if (sum2 > 2)
                        //            {
                        //                shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]);
                        //                sum = 19;
                        //            }
                        //            if (sum2 > 3)
                        //            {
                        //                shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - type] - shrapnel[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                        //                sum = 19;
                        //                sum2 = 0;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            shrapnel[j, i] = Shrapnel(explosion[j, i], shrapnel[0, i], d[j - 1, i], direction[j, i]) + (shrapnel[j, i - 1] - shrapnel[j, i]);
                        //            sum = 19;
                        //        }
                        //    }
                        //    else shrapnel[j, i] = Shrapnel(explosion[j,i],shrapnel[0,i],d[j-1,i],direction[j,i]);
                        
                        //    if ((i == 2) || (i == 1))
                        //    {
                        //        shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar);
                        //    }
                        //    if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / type)].NumberOfInputVar)
                        //    {
                        //        shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar); goto exit;
                        //    }

                        //    if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - type) / type)].NumberOfInputVar)
                        //    {
                        //        shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar); goto exit;
                        //    }
                        //    if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != (variables - 1))
                        //    {
                        //        if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i) / type)].NumberOfInputVar)
                        //        {
                        //            shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMax(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar); goto exit;
                        //        }
                        //        if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i + 1) / type)].NumberOfInputVar)
                        //        {
                        //            shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMax(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar); goto exit;
                        //        }
                        //    }
                        //    if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar == (variables - 1))
                        //    {
                        //        if((i==(count_terms*3-1))||(i==(count_terms*3)))
                        //        shrapnel[j, i] = Approx.LearnSamplesSet.InputAttributeMax(Approx.RulesDatabaseSet[0].TermsSet[(int)(i - 1) / type].NumberOfInputVar);
                        //    }
                           
                        //    if (((i + (type - 2)) % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                        //    {
                        //        if (shrapnel[j, i] == Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar)) i--;
                        //        goto generate;
                        //    }
                        //    if ((i % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                        //    {
                        //        goto generate;
                        //    }
                        //    if (i != 1)
                        //    {
                        //        if (((i - (type - 2)) % type == 0) && ((shrapnel[j, i] > shrapnel[j, i - 1]) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributeMax(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar)) || (shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar))))
                        //        {
                        //            goto generate;
                        //        }
                        //    }
                        //    if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributeMin(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar)) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributeMax(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumberOfInputVar))))
                        //    {
                        //        goto generate;
                        //    }
                        //exit:
                        //    if (i > type)
                        //    {
                        //        if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar != 0)
                        //        {
                        //            if (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1 - type) / type)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumberOfInputVar - 1)
                        //            {
                        //                if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < shrapnel[j, i - type])))
                        //                {
                        //                    goto generate;
                        //                }
                        //            }
                        //        }
                        //    }
                        //    d[j, i] = d[j-1,i] / Math.Pow(Math.E, (double)iter/(double)reduce_koef);
                        //}
                    }
                }

                for (int z = 0; z < count_populate; z++)
                {
                    k = 1;
                    for (int h = 0; h < count_terms; h++)
                        for (int p = 0; p < type; p++)
                        {
                            Approx.RulesDatabaseSet[z].TermsSet[h].Parametrs[p] = shrapnel[z, k];
                            k++;
                        }
                }
                for (j = 0; j < count_populate; j++)
                {
                    RMSE[j] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[j]);
                    RMSE_tst[j] = Approx.approxTestSamples(Approx.RulesDatabaseSet[j]);
                    if (RMSE[j] < RMSE_best)
                    {
                        RMSE_best = RMSE[j];
                        best = j;
                    }
                }
                if ((iter != 0) && (iter % 1000 == 0))
                {
                    Adaptive_LSM LSM = new Adaptive_LSM();
                    MSEbefore = RMSE[best];
                    KnowlegeBaseSARules zeroSolution = new KnowlegeBaseSARules(Approx.RulesDatabaseSet[0]);
                    Approx.RulesDatabaseSet[0] = new KnowlegeBaseSARules(Approx.RulesDatabaseSet[best]);
                    KnowlegeBaseSARules tempSolution = new KnowlegeBaseSARules(Approx.RulesDatabaseSet[best]);
                    Approx = LSM.TuneUpFuzzySystem(Approx, new NullConfForAll()) as SAFuzzySystem;
                    MSEafter = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
                    if (MSEafter > MSEbefore)
                    {
                        Approx.RulesDatabaseSet[0] = tempSolution;
                        RMSE2[best] = MSEbefore;
                    }
                    else
                    {
                        RMSE2[best] = MSEafter;
                        for (int p = 0; p < count_cons; p++)
                            cons_best[p] = Approx.RulesDatabaseSet[0].all_conq_of_rules[p];
                    }
                    if (RMSE2[best] < RMSE_best)
                    {
                        RMSE_best = RMSE2[best];
                    }
                    Approx.RulesDatabaseSet[best] = new KnowlegeBaseSARules(Approx.RulesDatabaseSet[0]);
                    Approx.RulesDatabaseSet[0] = new KnowlegeBaseSARules(zeroSolution);
                    for (int z = 0; z < count_populate; z++)
                        for (int p = 0; p < count_cons; p++)
                            Approx.RulesDatabaseSet[z].RulesDatabase[p].IndependentConstantConsequent = cons_best[p];
                }
                k = 1;
                for (int h = 0; h < count_terms; h++)
                    for (int p = 0; p < type; p++)
                    {
                        shrapnel[0, k] = shrapnel[best, k];
                        if (exploration > iter) d[0, k] = RandomNext(Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Min, Approx.LearnSamplesSet.InputAttributes[ Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Max);
                        Approx.RulesDatabaseSet[0].TermsSet[h].Parametrs[p] = shrapnel[0, k];
                        k++;
                    }
             
                    if (iter % 10 == 0)
                    {
                        if (RMSE_pred[1] > RMSE2[best])
                        {
                            for (k = 1; k <= Nd; k++)
                            {
                                X_pred[0, k] = X_pred[1, k];
                                X_pred[1, k] = shrapnel[best, k];
                            }
                            RMSE_pred[0] = RMSE_pred[1];
                            RMSE_pred[1] = RMSE2[best];
                        }
                    }
                    else
                    {
                        if (RMSE_pred[1] > RMSE[best])
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
            double s = -Math.Sqrt((double)(Math.Abs(m/ d)));
            return xe +  x*Math.Pow(Math.PI, s);
        }

        public double m(double X1, double X2, double RMSE1, double RMSE2)
        {
            return (RMSE2 - RMSE1) / (X2 - X1);
        }
        public double Normalization(double x, double max, double d1, double d2)
        {
            return x * (d2 - d1) / max + d1;
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
                string result = "Mine blust alghoritm{";
                result += "Исключённые признаки= " + iskl_prizn + " ;" + Environment.NewLine;
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
                return new  List <FuzzySystemRelisedList.TypeSystem>() {FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

    }
}
