using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.TakagiSugenoApproximate;
using KLI.Properties;
using RecursiveLeastSquares.Base;


namespace KLI
{
    public class KLI : AbstractNotSafeGenerator
    {
        protected TypeTermFuncEnum type_func;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get { return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate }; }
        }


        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            TSAFuzzySystem result = Approximate;
            type_func = TypeTermFuncEnum.Гауссоида;
           
               
                
                var kliConf = config as KLI_conf;
                if (kliConf != null)
                {
                {

                    double meanValue = result.LearnSamplesSet.DataRows.Select(x => x.DoubleOutput).Average();

                    var mayError = kliConf.MaxValue * meanValue;
                    kliGenerate(result, type_func, mayError);
        //            File.AppendAllLines("./out.txt", new List<string>() { mayError + " "+ result.RulesDatabaseSet[0].ValueComplexity + " " + result.RMSEtoMSEforLearn(result.ErrorLearnSamples())/2 + " " + result.RMSEtoMSEforTest(result.ErrorTestSamples())/2 });
                }
            }
            return result;
        }

        public static void kliGenerate(TSAFuzzySystem Approximate, TypeTermFuncEnum typeFunc, double mayError)
        {

            var ssr = new List<SampleSet.RowSample>();
            for (int index = 0; index < Approximate.LearnSamplesSet.DataRows.Count; index++)
            {
                var row = new SampleSet.RowSample();
                List<double> InputAttributeValueLIST = new List<double>();
                for (int i = 0; i < Approximate.AcceptedFeatures.Count(); i++)
                {
                    double q = Convert.ToDouble(Approximate.LearnSamplesSet.DataRows[index].InputAttributeValue[i].ToString());
                    if (Approximate.AcceptedFeatures[i])
                        InputAttributeValueLIST.Add(q);


                    row.InputAttributeValue = InputAttributeValueLIST.ToArray();
                   row.DoubleOutput = Approximate.LearnSamplesSet.DataRows[index].DoubleOutput;
                }
               

                ssr.Add(row);
            }


            List<SampleSet.AttributeInfo> ss =
                            new List<SampleSet.AttributeInfo>(Approximate.LearnSamplesSet.InputAttributes);
                        for (int i = (Approximate.AcceptedFeatures.Count()-1); i >=0 ; i--)
                        {
                            if (!Approximate.AcceptedFeatures[i]) ss.RemoveAt(i);
                        }

            if ((Approximate.RulesDatabaseSet == null) || (Approximate.RulesDatabaseSet.Count == 0))
            {
                Approximate.RulesDatabaseSet.Add(new KnowlegeBaseTSARules());
                    //wtf????????????? что за, если null придет то всё плохо плохо, а мы его пропускаем выше лол
            }
            var originalSimpleSet = new List<SampleSet.RowSample>(ssr);
           
            int ruleIndex = 0;

            while (originalSimpleSet.Count > 0)
            {
                //а тут нормировка особой роли не играет, хммм ну или почти не играет
                var maxPoint = originalSimpleSet.OrderByDescending(x => (Math.Sqrt(x.InputAttributeValue.Sum(y => Math.Pow(y, 2.0))/
                    x.InputAttributeValue.Count()))).First();
                var iterClasterPoints = new List<SampleSet.RowSample>() {maxPoint};
                var mnk = new MNK() {n = maxPoint.InputAttributeValue.Count()};
                var P = new Matrix(new[] {new double[] {0}});
                var B = new HyperVector(0, 0);

                mnk.mnkIter(iterClasterPoints.Last().InputAttributeValue.ToList(), iterClasterPoints.Last().DoubleOutput,
                    ref P, ref B, true);

                var errorIter = errorsMnk(B, iterClasterPoints);
                while (errorIter < mayError)
                {
                    originalSimpleSet.RemoveAll(x => iterClasterPoints.Contains(x));
                    if (originalSimpleSet.Count == 0) break;
                    //это без нормировки
                    // var nextPoint =
                    // originalSimpleSet.OrderBy(x =>(Math.Sqrt(x.InputAttributeValue.Sum(y => Math.Pow(y -maxPoint.InputAttributeValue[x.InputAttributeValue.ToList().IndexOf(y)],2.0))/x.InputAttributeValue.Count()))).First();

                    //а это с нормировкой
                    var nextPoint = originalSimpleSet.OrderBy(simple => Math.Sqrt(simple.InputAttributeValue.Select((inputValue, index) => Math.Pow((inputValue - maxPoint.InputAttributeValue[index]) / (ss[index].Scatter), 2.0)).Sum())).First();

                    mnk.mnkIter(nextPoint.InputAttributeValue.ToList(), nextPoint.DoubleOutput, ref P, ref B, false);
                    errorIter = errorsMnk(B, new List<SampleSet.RowSample>(iterClasterPoints) {nextPoint});
                    if (errorIter < mayError) iterClasterPoints.Add(nextPoint);
                }

                var numbersRule = new List<int>();
                int spear = 0;
      
                for (int i = 0; i < Approximate.AcceptedFeatures.Count(); i++)//.CountVars
                {
                    if (!Approximate.AcceptedFeatures[i])
                    {
                        spear++;
                        continue;
                    }
                    var parametrs = new double[Term.CountParamsinSelectedTermType(typeFunc)];
                        parametrs[0] = iterClasterPoints.Sum(x => x.InputAttributeValue[i- spear])/iterClasterPoints.Count;
                        parametrs[1] =
                            Math.Sqrt(
                                iterClasterPoints.Sum(x => Math.Pow(x.InputAttributeValue[i - spear] - parametrs[0], 2.0))*2/
                                iterClasterPoints.Count);

                        var temp_term = new Term(parametrs, typeFunc, i);
                        Approximate.RulesDatabaseSet[0].TermsSet.Add(temp_term);
                        numbersRule.Add(ruleIndex + i-spear);
                }
                ruleIndex += Approximate.AcceptedFeatures.Where(x=>x==true).Count();//.CountVars
      
                var temp_rule = new TSARule(Approximate.RulesDatabaseSet[0].TermsSet, numbersRule.ToArray(), B.Elements[0].Elements[0], B.Elements.Skip(1).Select(x => x.Elements[0]).ToArray());
                Approximate.RulesDatabaseSet[0].RulesDatabase.Add(temp_rule);
            }
        }

        public static double errorsMnk(HyperVector B, List<SampleSet.RowSample> samples)
        {
            return Math.Sqrt((from x in samples let summ = x.InputAttributeValue.ToList().Select((a, i) => a*B.Elements[i + 1].Elements[0]).Sum() + B.Elements[0].Elements[0] select Math.Pow((summ - x.DoubleOutput), 2.0)).Sum());
        }

        public override string ToString(bool with_param = false)
        {
            return with_param ? @"КЛИ: Функции принадлежности= "+Term.ToStringTypeTerm(type_func) + Environment.NewLine : "КЛИ";
        }

        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new KLI_conf();
            result.Init(CountFeatures);
            return result;
        }
    }

    public class KLI_conf : IGeneratorConf
    {
        [Description("Относительная ошибка от среднего выходного значения"), DisplayName("Относительная ошибка"), Category("Параметры алгоритма")]
        public Double MaxValue
        {
            get { return Settings.Default.Error; }
            set
            {
                Settings.Default.Error = value;
                Settings.Default.Save();
            }
        }

        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }

        public void Init(int countVars)
        {
        }
    }

    //и тут УРЕЗАННАЯ ВЕРСИЯ МНК.
    internal class MNK
    {
        public int n = 1; // указать при инициализации
        private int R = 1;
        private int m = 1;

        private HyperVector[] xiBoldValue;

        public void mnkIter(List<double> x, double y, ref Matrix Pout, ref HyperVector Bout, bool useInit)
        {
            var a = 2000;

            #region Basic initialization

            double lambda = 1;

            #endregion

            #region B

            double[][] consequents = new double[n + 1][];

            // B[0]
            consequents[0] = new double[R];
            for (int i = 0; i < R; i++)
                consequents[0][i] = 1d;

            // B[1..n+1]
            for (int i = 1; i < n + 1; i++)
            {
                consequents[i] = new double[R];
                for (int j = 0; j < R; j++)
                    consequents[i][j] = 0;
            }

            var B = useInit ? new HyperVector(consequents) : Bout;

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

            var P = useInit ? new Matrix(p) : Pout;

            #endregion

            GC.Collect();
            xiBoldValue = new HyperVector[m];
            EvalXiBold(new double[][] {x.ToArray()});

            #region The Cycle

            {
                {
                    var temp1 = -1d*P;
                    var temp3 = temp1*xiBoldValue[0];

                    var temp5 = xiBoldValue[0]*P*xiBoldValue[0];
                    var temp6 = lambda + temp5;
                    var temp7 = temp3*(1d/temp6);
                    var temp8 = xiBoldValue[0]*P;
                    var temp9 = temp7 ^ temp8;
                    P += temp9;
                    P /= lambda;
                    B += P*xiBoldValue[0]*(y - xiBoldValue[0]*B);
                }
            }

            #endregion

            Pout = P;
            Bout = B;
        }

        protected void EvalXiBold(double[][] X)
        {
            // foreach dataVector in dataSet
            for (int i = 0; i < m; i++)
            {
                xiBoldValue[i] = new HyperVector(n + 1, R);

                // xiBoldValue[i][0] <- xiValue[i]
                for (int j = 0; j < R; j++)
                    xiBoldValue[i].Elements[0].Elements[j] = 1; //xiValue[i][j];

                // xiBoldValue[i][j] <- xiValue[i] * x[j]
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < R; k++)
                        xiBoldValue[i].Elements[j + 1].Elements[k] = X[i][j]*1; //xiValue[i][k];
                }
            }
        }
    }
}
