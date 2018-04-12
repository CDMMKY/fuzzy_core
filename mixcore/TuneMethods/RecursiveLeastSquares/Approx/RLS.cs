namespace RecursiveLeastSquares.Approx
{
    using System;
    using System.Collections.Generic;
    using FuzzySystem.SingletoneApproximate;
    using FuzzySystem.FuzzyAbstract;
    using FuzzySystem.FuzzyAbstract.conf;
    using FuzzySystem.TakagiSugenoApproximate;

    using RecursiveLeastSquares.Base;

    public class RLS : AbstractNotSafeLearnAlgorithm
    {
        private const double EPSILON = 1e-18;

        private double[] dividerXi;

        private double[][] xiValue;

        private HyperVector[] xiBoldValue;

        private int numberOfIterations;

        private int n; // number of input variables

        private int R; // number of rules

        private int m; // number of samples in learnSet

        private double lambda;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone, FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
            }
        }
        
        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            SAFuzzySystem result = Approximate;
            var config = (RLSconfig)conf;
            numberOfIterations = config.NumberOfIterantions;
            lambda = config.ForgettingFactor;
            
            var knowledgeBaseToOptimize = new KnowlegeBaseSARules(result.RulesDatabaseSet[0]);

            // r[0]
            double[] consequents = new double[knowledgeBaseToOptimize.all_conq_of_rules.Length];
            knowledgeBaseToOptimize.all_conq_of_rules.CopyTo(consequents, 0);
            dividerXi = new double[result.LearnSamplesSet.CountSamples];
            xiValue = new double[result.LearnSamplesSet.CountSamples][];

            Vector r = new Vector(consequents);

            // p[0]
            double[][] p = new double[consequents.Length][];
            for (int i = 0; i < consequents.Length; i++)
            {
                p[i] = new double[consequents.Length];
                for (int j = 0; j < consequents.Length; j++)
                {
                    p[i][j] = (i == j) ? 2000 : 0;
                }
            }

            Matrix P = new Matrix(p);

            // x, y
            double[][]   x = new double[result.LearnSamplesSet.CountSamples][];
            double[] y = new double[result.LearnSamplesSet.CountSamples];
            for (int i = 0; i < result.LearnSamplesSet.CountSamples; i++)
            {
                x[i] = result.LearnSamplesSet.DataRows[i].InputAttributeValue;
                y[i] = result.LearnSamplesSet.DataRows[i].DoubleOutput;
                
                xiValue[i] = new double[knowledgeBaseToOptimize.all_conq_of_rules.Length];
            }
            
            EvalXi(x, y, knowledgeBaseToOptimize);

            // Main cycle
            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int j = 0; j < result.LearnSamplesSet.CountSamples; j++)
                {
                    var temp1 = -1d * P;
                    var temp2 = new Matrix(Xi(j), false);
                    var temp3 = temp1 * temp2;

                    var temp4 = new Matrix(Xi(j), true);
                    var temp5 = temp4 * P * temp2;
                    var temp6 = lambda + temp5.Elements[0][0]; //// 1 >> lambda
                    var temp7 = temp3 / temp6;
                    var temp8 = temp4 * P;
                    var temp9 = temp7 * temp8;
                    P += temp9;
                    P /= lambda;

                    P = (P + -1 * P * temp2 / (lambda + (temp4 * P * temp2).Elements[0][0]) * temp4 * P) / lambda;

                    r += P * Xi(j) * (y[j] - Xi(j) * r);
                }
            }

            knowledgeBaseToOptimize.all_conq_of_rules = r.Elements;

            // Get the best knowledge base on the 1st place
            result.RulesDatabaseSet.Add(knowledgeBaseToOptimize);
            double errorBefore = result.approxLearnSamples(result.RulesDatabaseSet[0]),
                   errorAfter = result.approxLearnSamples(result.RulesDatabaseSet[ result.RulesDatabaseSet.Count - 1]);

            if (errorAfter < errorBefore)
            {
                result.RulesDatabaseSet.Remove(knowledgeBaseToOptimize);
                result.RulesDatabaseSet.Insert(0, knowledgeBaseToOptimize);
            }

            return result;
        }

        public override TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            
            TSAFuzzySystem result = Approximate;

            double errorBefore = result.approxLearnSamples(result.RulesDatabaseSet[0]);

            var a = 2000;
            
            #region Basic initialization
            var config = (RLSconfig)conf;
            numberOfIterations = config.NumberOfIterantions;
            lambda = config.ForgettingFactor;
            
            var knowledgeBaseToOptimize = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);
            var kbToOptimize = new KnowlegeBaseTSARules(result.RulesDatabaseSet[0]);

            R = knowledgeBaseToOptimize.RulesDatabase.Count;   // Number of rules
            m = result.LearnSamplesSet.CountSamples;   // Number of samples
            n = knowledgeBaseToOptimize.RulesDatabase[0].RegressionConstantConsequent.Length;  // Number of variables
            #endregion

            #region x, y
            double[][] x = new double[m][];
            double[] y = new double[m];
            for (int i = 0; i < m; i++)
            {
                x[i] = result.LearnSamplesSet.DataRows[i].InputAttributeValue;
                y[i] = result.LearnSamplesSet.DataRows[i].DoubleOutput;
            }
            #endregion

            #region B
            double[][] consequents = new double[n + 1][];

            // B[0]
            consequents[0] = new double[R];
            for (int i = 0; i < R; i++)
                consequents[0][i] = knowledgeBaseToOptimize.RulesDatabase[i].IndependentConstantConsequent;

            // B[1..n+1]
            for (int i = 1; i < n + 1; i++)
            {
                consequents[i] = new double[R];
                for (int j = 0; j < R; j++)
                    consequents[i][j] = knowledgeBaseToOptimize.RulesDatabase[j].RegressionConstantConsequent[i - 1];
            }
            
            HyperVector B = new HyperVector(consequents);
            #endregion

            #region P
            double[][] p = new double[n + 1][];
            for (int i = 0; i < n + 1; i++)
            {
                p[i] = new double[n + 1];
                for (int j = 0; j < n + 1; j++)
                {
                    p[i][j] = (i == j) ? a : 0;
                }
            }

            Matrix P = new Matrix(p);
            #endregion

            #region Xi
            dividerXi = new double[m];
            xiValue = new double[m][];
            for (int i = 0; i < m; i++)
                xiValue[i] = new double[R];
            EvalXi(x, y, knowledgeBaseToOptimize); 
            
            // XiBold
            xiBoldValue = new HyperVector[m];
            EvalXiBold(x); 
            #endregion

            GC.Collect();

            //double[][] aDoubles = new[]
            //                          {
            //                              new[] { 0.1, 0.2, 0.123 },
            //                              new[] { 1.213, 2.1, 1.2 },
            //                              new[] { 13.3, 0.1231, 31.1 }
            //                          },
            //           bDoubles = new[]
            //                          {
            //                              new[] { 0.1, 12.2, 5.445 },
            //                              new[] { 5.3, 4.553, 1.545 },
            //                              new[] { 3.4, 87.545, 0.255 }
            //                          };
            //Matrix aMatrix = new Matrix(aDoubles);
            //HyperVector bVector = new HyperVector(bDoubles);
            //HyperVector cVector = new HyperVector(aDoubles);

            //var c = aMatrix * bVector;
            //var cc = c;
            //var d = bVector * cVector;
            //var dd = d;
            //var e = bVector ^ cVector;
            //var ee = e;
            //var f = cVector * aMatrix;
            //var ff = f;



            #region The Cycle
            for (int i = 0; i < numberOfIterations; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    var temp1 = -1d * P;
                    var temp3 = temp1 * xiBoldValue[j];

                    var temp5 = xiBoldValue[j] * P * xiBoldValue[j];
                    var temp6 = lambda + temp5;
                    var temp7 = temp3 * (1d / temp6);
                    var temp8 = xiBoldValue[j] * P;
                    var temp9 = temp7 ^ temp8;
                    P += temp9;
                    P /= lambda;

                    ////P = (P + (-1d * P * xiBoldValue[j] * (1d / (lambda + (xiBoldValue[j] * P * xiBoldValue[j]))) ^ xiBoldValue[j] * P)) / lambda;

                    B += P * xiBoldValue[j] * (y[j] - xiBoldValue[j] * B);
                }
            }
            #endregion

            #region Comparison
            // Get consequents into the KB
            for (int i = 0; i < R; i++)
            {
                knowledgeBaseToOptimize.RulesDatabase[i].IndependentConstantConsequent = B.Elements[0].Elements[i];

                for (int j = 1; j < n + 1; j++)
                    knowledgeBaseToOptimize.RulesDatabase[i].RegressionConstantConsequent[j - 1] = B.Elements[j].Elements[i]; // NOT WORKING!!!
                    
            }

            // Get the best knowledge base on the 1st place
            double errorAfter = result.approxLearnSamples(kbToOptimize);

            if (errorAfter < errorBefore)
            {
            result.RulesDatabaseSet.Insert(0, knowledgeBaseToOptimize);
            }
            else
            {
                result.RulesDatabaseSet.Insert(0, kbToOptimize);
                //    result.RulesDatabaseSet.Add(kbToOptimize);
            }

            return result;
            #endregion
        }
        
        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Рекуррентный МНК{";
                result += Environment.NewLine;
                result += "Количество итераций: " + numberOfIterations.ToString() + Environment.NewLine;
                result += "Фактор забывания: " + lambda.ToString() + Environment.NewLine;
                result += "}";
                return result;
            }

            return "Рекуррентный МНК";
        }

        

        public Vector Xi(int s)
        {
            double[] answer = xiValue[s];
            return new Vector(answer);
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new RLSconfig();
            result.Init(CountFeatures);
            return result;
        }

        protected void EvalDividerXi(double[][] X, double[] Y, KnowlegeBaseSARules knowlegeBaseA)
        {
            // foreach dataVector in dataSet
            for (int s = 0; s < Y.Length; s++)
            {
                double sum = 0;
                double[] x = X[s]; // result.Learn_Samples_set.Data_Rows[s].Input_Attribute_Value;

                // foreach rule in base.Rules
                foreach (SARule rule in knowlegeBaseA.RulesDatabase)
                {
                    double mul = 1;

                    // foreach term in rule.terms
                    foreach (Term term in rule.ListTermsInRule)
                    {
                        mul *= term.LevelOfMembership(x);
                        if (Math.Abs(mul - 0) < EPSILON)
                        {
                            break;
                        }
                    }

                    sum += mul;
                }

                dividerXi[s] = sum;
            }
        }

        protected void EvalDividerXi(double[][] X, double[] Y, KnowlegeBaseTSARules knowlegeBaseA)
        {
            double sum, mul;

            // foreach dataVector in dataSet
            for (int i = 0; i < m; i++)
            {
                sum = 0;
                double[] x = X[i]; // result.Learn_Samples_set.Data_Rows[s].Input_Attribute_Value;

                // foreach rule in base.Rules
                foreach (TSARule rule in knowlegeBaseA.RulesDatabase)
                {
                    mul = 1;

                    // foreach term in rule.terms
                    foreach (Term term in rule.ListTermsInRule)
                    {
                        mul *= term.LevelOfMembership(x);
                        if (Math.Abs(mul - 0) < EPSILON)
                        {
                            break;
                        }
                    }

                    sum += mul;
                }

                dividerXi[i] = sum;
            }
        }

        protected void EvalXi(double[][] X, double[] Y, KnowlegeBaseSARules knowlegeBaseA)
        {
            EvalDividerXi(X, Y, knowlegeBaseA);
            
            // foreach dataVector in dataSet
            for (int s = 0; s < Y.Length; s++)
            {
                double[] x = X[s];   // result.Learn_Samples_set.Data_Rows[s].Input_Attribute_Value;

                // foreach rule in base.rules
                for (int k = 0; k < R; k++)
                {
                    double mul = 1;

                    // foreach term in rule.terms
                    foreach (Term term in knowlegeBaseA.RulesDatabase[k].ListTermsInRule)
                    {
                        mul *= term.LevelOfMembership(x);
                        if (Math.Abs(mul - 0) < EPSILON)
                        {
                            break;
                        }
                    }

                    xiValue[s][k] = mul / dividerXi[s];
                }
            }
        }

        protected void EvalXi(double[][] X, double[] Y, KnowlegeBaseTSARules knowlegeBaseA)
        {
            EvalDividerXi(X, Y, knowlegeBaseA);

            // foreach dataVector in dataSet
            for (int i = 0; i < m; i++)
            {
                double[] x = X[i];   // result.Learn_Samples_set.Data_Rows[s].Input_Attribute_Value;

                // foreach rule in base.rules
                for (int j = 0; j < R; j++)
                {
                    double mul = 1;

                    // foreach term in rule.terms
                    foreach (Term term in knowlegeBaseA.RulesDatabase[j].ListTermsInRule)
                    {
                        mul *= term.LevelOfMembership(x);
                        if (Math.Abs(mul - 0) < EPSILON)
                        {
                            break;
                        }
                    }

                    xiValue[i][j] = mul / dividerXi[i];
                }
            }
        }

        protected void EvalXiBold(double[][] X)
        {
            // foreach dataVector in dataSet
            for (int i = 0; i < m; i++)
            {
                xiBoldValue[i] = new HyperVector(n + 1, R);
                
                // xiBoldValue[i][0] <- xiValue[i]
                for (int j = 0; j < R; j++)
                    xiBoldValue[i].Elements[0].Elements[j] = xiValue[i][j];
                
                // xiBoldValue[i][j] <- xiValue[i] * x[j]
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < R; k++)
                        xiBoldValue[i].Elements[j + 1].Elements[k] = X[i][j] * xiValue[i][k];
                }
            }
        }
    }
}
