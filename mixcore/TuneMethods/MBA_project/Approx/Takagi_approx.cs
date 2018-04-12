using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;

namespace MBA_project.Approx
{
    public class Takagi_approx : AbstractNotSafeLearnAlgorithm
    {
        int count_populate;
        int count_iteration;
        int exploration;
        int reduce_koef;
        string iskl_prizn;
        string priznaki_usech;
        int countRules;
        double RMSE_best2;

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            iskl_prizn = "";
            count_iteration = ((Param)conf).Количество_итераций;
            count_populate = ((Param)conf).Число_осколков;
            exploration = ((Param)conf).Фактор_исследования;
            reduce_koef = ((Param)conf).Уменьшающий_коэффициент;
            priznaki_usech = ((Param)conf).Усечённые_признаки;

            int iter = 0, i, j, count_terms;
            double cosFi;
            int Nd, variables, k = 1, best = 0;
            string[] buf;
            buf = priznaki_usech.Split(' ');
            TSAFuzzySystem result = Approx;
            int type = Approx.RulesDatabaseSet[0].TermsSet[0].CountParams;
            Nd = Approx.RulesDatabaseSet[0].TermsSet.Count * type;
            double[] X_best = new double[Nd + 1];
            double[,] X_pred = new double[2, Nd + 1];
            double[,] d = new double[count_populate, Nd + 1];
            double[,] explosion = new double[count_populate, Nd + 1];
            double[,] shrapnel = new double[count_populate, Nd + 1];
            cosFi = Math.Cos(2 * Math.PI / count_populate);
            double RMSE_best = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
            double[] RMSE = new double[count_populate];
            double[] RMSE2 = new double[count_populate];
            double[] RMSE_pred = new double[2];
            count_terms = Approx.RulesDatabaseSet[0].TermsSet.Count;
            variables = Approx.LearnSamplesSet.CountVars;
            double[] X_best2 = new double[variables];
            double[,] priznak = new double[count_populate, variables];
            for (i = 0; i < variables; i++)
            {
                priznak[0, i] = 1;
                X_best2[i] = 1;
            }
            KnowlegeBaseTSARules[] X = new KnowlegeBaseTSARules[count_populate];
            for (int s = 0; s < count_populate - 1; s++)
            {
                X[s] = new KnowlegeBaseTSARules(Approx.RulesDatabaseSet[0]);
                Approx.RulesDatabaseSet.Add(X[s]);
            }

            if (buf[0] != "")
            {
                for (k = 0; k < buf.Count(); k++)
                {
                    Approx.AcceptedFeatures[int.Parse(buf[k]) - 1] = false;
                    priznak[0, int.Parse(buf[k]) - 1] = 0;
                    //iskl_prizn += buf[k] + " ";
                }
            }
            for (k = 0; k < variables; k++)
            {
                if(Approx.AcceptedFeatures[k] == false)
                iskl_prizn += (k+1).ToString() + " ";
            }
            RMSE_best2 = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
            //for (j = 0; j < count_populate; j++)
            //{
            //    for (int h = 0; h < variables; h++)
            //    {
            //        if (priznak[0, h] == 1) Approx.AcceptedFeatures[h] = true;
            //        else
            //        {
            //            Approx.AcceptedFeatures[h] = false;
            //            for (int h1 = 0; h1 < Approx.RulesDatabaseSet[0].RulesDatabase.Count(); h1++)
            //            {
            //                Approx.RulesDatabaseSet[j].RulesDatabase[h1].RegressionConstantConsequent[h] = 0;
            //            }
            //        }
            //    }
            //}
            countRules = Approx.RulesDatabaseSet[0].RulesDatabase.Count();
            RMSE_best = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
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
                            X_pred[0, k] = shrapnel[0, k];
                            X_pred[1, k] = shrapnel[0, k];
                            k++;
                        }
                    RMSE_pred[0] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
                    RMSE_pred[1] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
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
                        for (j = 1; j < count_populate; j++)
                        {
                            int sum = 0, sum2 = 0;
                        generate:
                            sum++;
                            sum2++;
                     
                            d[j, i] = d[j - 1, i] * randn();

                            explosion[j, i] = d[j, i] * cosFi;
                            if (type == 2)
                            {
                                if (sum > 20)
                                {
                                    if ((i + 1) % type == 0)
                                    {
                                        if (i != 1)
                                        {
                                            shrapnel[j, i] = (shrapnel[0, i] + explosion[j, i]) + (shrapnel[j, i] - shrapnel[j, i - 2]);
                                        }
                                    }
                                    if (sum2 > 1000) { sum = 0; sum2 = 0; }
                                }
                                else shrapnel[j, i] = shrapnel[0, i] + explosion[j, i];
                                if (i != 1)
                                {
                                    if (((i + 1) % 2 == 0) && (shrapnel[j, i] < shrapnel[j, i - 2]) && (Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar))
                                    {
                                        goto generate;
                                    }
                                }
                            if (((i + 1) % 2 == 0) && (shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i) / type].NumVar].Min))
                            {
                                goto generate;
                            }
                            if (((i + 1) % 2 == 0) && (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i) / type].NumVar].Max))
                            {
                                goto generate;
                            }
                            if ((i % 2 == 0) && (shrapnel[j, i] < 0))
                                {
                                    goto generate;
                                }
                            }
                        if (type != 2)
                        {
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
                        }
                        
                            if (type != 2)
                            {
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
                                    if ((i == (count_terms * 3 - 1)) || (i == (count_terms * 3)))
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

                        //    else
                        //    {
                        //    if (i > 1)
                        //    {
                        //        if ((i%2!=0) && (shrapnel[j, i] < shrapnel[j, i - 2]) && (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / 2)].NumberOfInputVar == Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / 2)].NumberOfInputVar))
                        //        {
                        //            goto generate;
                        //        }
                        //        if((i%2!=0) && (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttribute(Approx.RulesDatabaseSet[0].TermsSet[(int)(i / 2)].NumberOfInputVar).Max) && (Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / 2)].NumberOfInputVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 2) / 2)].NumberOfInputVar))
                        //        {
                        //            goto generate;
                        //        }
                        //    }
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
                    RMSE[j] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ j]);
                    if (RMSE[j] < RMSE_best)
                    {
                        RMSE_best = RMSE[j];
                        best = j;
                    }
                }
    
                k = 1;
                for (int h = 0; h < count_terms; h++)
                    for (int p = 0; p < type; p++)
                    {
                        shrapnel[0, k] = shrapnel[best, k];
                        if (exploration > iter) d[0, k] = RandomNext(Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Min, Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Max);
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

        public double Shrapnel(double xe, double x, double d, double m)
        {
            double s = -Math.Sqrt((double)(Math.Abs(m / d)));
            return xe + x * Math.Pow(Math.PI, s);
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Mine blust alghoritm{";
                result += "Исключённые признаки= " + iskl_prizn +  " ;    "+ countRules + Environment.NewLine;
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
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }
    }
}
