using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;

namespace Hooke_Jeevs
{
    public class Hooke_Jeeves_approx : AbstractNotSafeLearnAlgorithm
    {
        int count_populate;
        int count_iteration;
        int iter;
        string iskl_prizn;
        string priznaki_usech;
        double e;

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            iskl_prizn = "";
            count_iteration = ((Param)conf).Количество_итераций;
            count_populate = ((Param)conf).Число_осколков;
            e = ((Param)conf).точность;
            priznaki_usech = ((Param)conf).Усечённые_признаки;
            int parametr = ((Param)conf).параметр_лямбда;
            int i, j, count_terms, imput_var, label_stop = 0;
            string[] buf;
            buf = priznaki_usech.Split(' ');
            double MSE_best, cosFi;
            int Nd, variables, k = 1, best = 0;
            SAFuzzySystem result = Approx;
            int type = Approx.RulesDatabaseSet[0].TermsSet[0].CountParams;
            Nd = Approx.RulesDatabaseSet[0].TermsSet.Count * type;
            double[] h1 = new double[Nd + 1];
            double[] var = new double[Nd + 1];
            double[,] X_pred = new double[2, Nd + 1];
            double[,] d = new double[count_populate, Nd + 1];
            double[,] explosion = new double[count_populate, Nd + 1];
            double[,] shrapnel = new double[count_populate, Nd + 1];
            cosFi = Math.Cos(2 * Math.PI / count_populate);
            double[] RMSE = new double[count_populate];
            double[] RMSE2 = new double[count_populate];
            double[] RMSE_pred = new double[2];
            variables = Approx.LearnSamplesSet.CountVars;
            count_terms = Approx.RulesDatabaseSet[0].TermsSet.Count;
            double[] X_best2 = new double[variables];
            double[,] d3 = new double[count_populate, variables];
            double[,] priznak = new double[count_populate, variables];
            for (i = 0; i < variables; i++)
            {
                priznak[0, i] = 1;
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
            k = 1;

            for (i = 0; i < Approx.RulesDatabaseSet[0].TermsSet.Count; i++)
            {
                if (Approx.RulesDatabaseSet[0].TermsSet[i].TermFuncType == TypeTermFuncEnum.Гауссоида)
                {
                    if (Approx.RulesDatabaseSet[0].TermsSet[i].Parametrs[1] < 0.0000001)
                    {
                        Approx.RulesDatabaseSet[0].TermsSet[i].Parametrs[1] = 1;

                    }
                }
            }

            MSE_best = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);

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
                            imput_var = Approx.RulesDatabaseSet[0].TermsSet[h].NumVar;
                            if (priznak[0, imput_var] != 0) var[k] = 1;
                            X_pred[0, k - 1] = shrapnel[0, k];
                            X_pred[1, k - 1] = shrapnel[0, k];
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
                    if (var[i] != 0)
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
                                    if (sum2 > 500) { sum = 0; sum2 = 0; }
                                }
                                else shrapnel[j, i] = shrapnel[0, i] + explosion[j, i];
                                shrapnel = Proverka(shrapnel, Approx, i, j, var);
                                if (shrapnel[0, 0] == 1) goto generate;
                            }
                            else
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
                                shrapnel = Proverka(shrapnel, Approx, i, j, var);
                                if (shrapnel[0, 0] == 1) goto generate;
                            }
                        }
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
                    if (RMSE[j] < MSE_best)
                    {
                        MSE_best = RMSE[j];
                        best = j;
                    }
                }

                k = 1;
                for (int h = 0; h < count_terms; h++)
                    for (int p = 0; p < type; p++)
                    {
                        shrapnel[0, k] = shrapnel[best, k];
                        d[0, k] = RandomNext(Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Min, Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[h].NumVar].Max);
                        Approx.RulesDatabaseSet[0].TermsSet[h].Parametrs[p] = shrapnel[0, k];
                        k++;
                    }

                if (RMSE_pred[1] > RMSE[best])
                {
                    for (k = 0; k < Nd; k++)
                    {
                        X_pred[0, k] = X_pred[1, k];
                        X_pred[1, k] = shrapnel[0, k + 1];
                    }
                    RMSE_pred[0] = RMSE_pred[1];
                    RMSE_pred[1] = RMSE[best];
                }
                // Хук-Дживс
                int flag = 0, flag2 = 0, f = 0, st = 0;
                for (j = 0; j < Nd; j++)
                {
                    h1[j] = Math.Abs(X_pred[0, j] - X_pred[1, j]);
                }
                double MSE_1;
                MSE_1 = RMSE[best];
                double[] point4 = new double[Nd + 1];
                double[] point3 = new double[Nd + 1];
                double[] point2 = new double[Nd + 1];
                double[] point1 = new double[Nd + 1];

                for (i = 1; i <= Nd; i++)
                    point1[i] = shrapnel[0, i];
            poisk_basis:
                if (st > 7) goto stop;
                point2 = Poisk_basis(point1, h1, count_terms, type, Approx, MSE_1, flag, e, var);
                if (MSE_1 == point2[0]) st++;
                MSE_1 = point2[0];
                if (point2[Nd + 1] == 1) goto stop;
            poisk_obraz:
                if (st > 7) goto stop;
                f++;
                for (i = 1; i <= Nd; i++)
                {
                    if (var[i] != 0) point3[i] = point1[i] + parametr * (point2[i] - point1[i]);
                }
                flag = 1;
                flag2 = 0;
                point4 = Poisk_basis(point3, h1, count_terms, type, Approx, MSE_1, flag, e, var);
                if (MSE_1 == point4[0]) st++;
                MSE_1 = point4[0];
                for (i = 1; i <= Nd; i++)
                    if (point4[i] == point3[i])
                    {
                        flag2++;
                    }
                if (flag2 == Nd)
                {
                    for (i = 1; i <= Nd; i++)
                    {
                        point1[i] = point2[i];
                    }
                    flag = 0;
                    goto poisk_basis;
                }
                else
                {
                    for (i = 1; i <= Nd; i++)
                    {
                        point1[i] = point2[i];
                        point2[i] = point4[i];
                    }
                    flag = 0;
                    if (f < 15) goto poisk_obraz;
                    else goto stop;
                }
            stop:
                for (k = 0; k < Nd; k++)
                {
                    X_pred[0, k] = X_pred[1, k];
                    X_pred[1, k] = point2[k + 1];
                }
                DB(point2, count_terms, type, Approx);
                RMSE_pred[0] = RMSE_pred[1];
                RMSE_pred[1] = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
                if (RMSE_pred[0] == RMSE_pred[1]) label_stop++;
                else label_stop = 0;
                //if (label_stop == 10) goto stop2;
                for (k = 1; k <= Nd; k++)
                {
                    shrapnel[0, k] = point2[k];
                }
            }
        //stop2:
            return result;
        }

        Random rand = new Random();
        public double RandomNext(double min, double max)
        {
            return (max - min) * rand.NextDouble() + min;
        }

        public double[] Poisk_basis(double[] basis, double[] h, int count_terms, int type, SAFuzzySystem Approx, double MSE_1, int flag2, double e, double[] var)
        {
            int i, n;
            int flag = 0;
            n = basis.Length;
            double[] shrapnel = new double[n];
            double[,] shrapnel2 = new double[2, n];
            double[] result = new double[n + 2];
            double MSE_2;
            for (i = 0; i < n; i++)
                shrapnel[i] = basis[i];
        poisk_basis:
            for (i = 1; i < n; i++)
            {
                if (var[i] != 0)
                {
                    shrapnel[i] = basis[i] + h[i - 1];
                    MSE_2 = DB(shrapnel, count_terms, type, Approx);
                    if (MSE_2 >= MSE_1)
                    {
                        shrapnel[i] = basis[i] - h[i - 1];

                        MSE_2 = DB(shrapnel, count_terms, type, Approx);
                        if (MSE_2 >= MSE_1)
                        {
                            shrapnel[i] = basis[i];
                            DB(basis, count_terms, type, Approx);
                        }
                        else
                        {
                            MSE_1 = MSE_2;
                        }
                        result[i] = shrapnel[i];
                    }
                    else
                    {
                        MSE_1 = MSE_2;
                        result[i] = shrapnel[i];
                    }
                    result[0] = MSE_1;
                }
            }
            for (i = 1; i < n; i++)
                if (basis[i] == shrapnel[i])
                {
                    flag++;
                }
            if (flag == n - 1)
            {
                if (flag2 == 1) return result;
                for (i = 0; i < n; i++)
                {
                    h[i] /= 10;
                    if (h[i] < e)
                    {
                        result[n] = 1;
                        return result;
                    }
                }
                flag = 0;
                goto poisk_basis;
            }
            else return result;
        }

        public double d0(double LB, double UB)
        {
            return UB - LB;
        }
        public double DB(double[] shrapnel, int count_terms, int type, SAFuzzySystem Approx)
        {
            int k = 1;
            for (int l = 0; l < count_terms; l++)
                for (int p = 0; p < type; p++)
                {
                    Approx.RulesDatabaseSet[0].TermsSet[l].Parametrs[p] = shrapnel[k];
                    k++;
                }
            double MSE = Approx.approxLearnSamples(Approx.RulesDatabaseSet[ 0]);
            return MSE;
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

        public double[,] Proverka(double[,] shrapnel, SAFuzzySystem Approx, int n, int m, double[] var)
        {
            int type = Approx.RulesDatabaseSet[0].TermsSet[0].CountParams;
            int variables = Approx.LearnSamplesSet.CountVars;
            int count_terms = Approx.RulesDatabaseSet[0].TermsSet.Count;
            shrapnel[0, 0] = 0;
            for (int i = 1; i <= n; i++)
            {
                if (var[i] != 0)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        if (type == 2)
                        {
                            if (i != 1)
                            {
                                if (((i + 1) % 2 == 0) && (shrapnel[j, i] < shrapnel[j, i - 2]) && (Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar != Approx.RulesDatabaseSet[0].TermsSet[(int)((i - 1) / type)].NumVar))
                                {
                                    shrapnel[0, 0] = 1;
                                }
                            }
                            if (((i + 1) % 2 == 0) && (shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i) / type].NumVar].Min))
                            {
                                shrapnel[0, 0] = 1;
                            }
                            if (((i + 1) % 2 == 0) && (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i) / type].NumVar].Max))
                            {
                                shrapnel[0, 0] = 1;
                            }
                            if ((i % 2 == 0) && (shrapnel[j, i] < 0))
                            {
                                shrapnel[0, 0] = 1;
                            }
                        }
                        else
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
                                shrapnel[0, 0] = 1;
                            }
                            if ((i % type == 0) && (shrapnel[j, i] < shrapnel[j, i - 1]))
                            {
                                shrapnel[0, 0] = 1;
                            }
                            if (i != 1)
                            {
                                if (((i - (type - 2)) % type == 0) && ((shrapnel[j, i] > shrapnel[j, i - 1]) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Max) || (shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Min)))
                                {
                                    shrapnel[0, 0] = 1;
                                }
                            }
                            if (((i + (type - 2)) % type == 0) && ((shrapnel[j, i] < Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Min) || (shrapnel[j, i] > Approx.LearnSamplesSet.InputAttributes[Approx.RulesDatabaseSet[0].TermsSet[(int)(i / type)].NumVar].Max)))
                            {
                                shrapnel[0, 0] = 1;
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
                                            shrapnel[0, 0] = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return shrapnel;
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Hooke-Jeevs{";
                result += "итерации  " + iter + " " + Environment.NewLine;
                result += "}";
                return result;
            }
            return "Hooke-Jeevs";
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
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }
    }
}



