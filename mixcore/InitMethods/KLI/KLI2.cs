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
using FuzzySystem.FuzzyAbstract.Utils;

namespace KLI
{
    public class KLI2 : AbstractNotSafeGenerator
    {
        protected TypeTermFuncEnum type_func;
        public List<SampleSet.RowSample> originalSimpleSet;
        public List<double> originalSimpleSetDistanteToPoint;
        protected TSAFuzzySystem ResultSystem = null;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get { return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate }; }
        }


        public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, IGeneratorConf config)
        {
            ResultSystem = Approximate;
            type_func = TypeTermFuncEnum.Гауссоида;



            var kliConf = config as KLI_conf;
            if (kliConf != null)
            {
                {
                    ResultSystem = Approximate;
                    double meanValue = ResultSystem.LearnSamplesSet.DataRows.Select(x => x.DoubleOutput).Average();
          
                    var mayError = kliConf.MaxValue * meanValue;
                    kliGenerate(ResultSystem, type_func, mayError);
                    //             File.AppendAllLines("./out.txt", new List<string>() { mayError + " " + Result.RulesDatabaseSet[0].ValueComplexity + " " + Result.RMSEtoMSEforLearn(Result.ErrorLearnSamples()) / 2 + " " + Result.RMSEtoMSEforTest(Result.ErrorTestSamples()) / 2 });
                }
            }
            return ResultSystem;
        }

        public void kliGenerate(TSAFuzzySystem Approximate, TypeTermFuncEnum typeFunc, double mayError)
        {

            if ((Approximate.RulesDatabaseSet == null) || (Approximate.RulesDatabaseSet.Count == 0))
            {
                Approximate.RulesDatabaseSet.Add(new KnowlegeBaseTSARules());
            }
            originalSimpleSet = new List<SampleSet.RowSample>(Approximate.LearnSamplesSet.DataRows);
            originalSimpleSetDistanteToPoint = new List<double>();
            CalcFarPoint();


            int ruleIndex = 0;


            while (originalSimpleSet.Count > 0)
            {
                int IndexOfMax = originalSimpleSetDistanteToPoint.IndexOf(originalSimpleSetDistanteToPoint.Max());

                var maxPoint = originalSimpleSet[IndexOfMax];
                originalSimpleSet.Remove(maxPoint);
                originalSimpleSetDistanteToPoint.RemoveAt(IndexOfMax);

                var iterClasterPoints = new List<SampleSet.RowSample>() { maxPoint };
                var mnk = new MNK() { n = maxPoint.InputAttributeValue.Count() };
                var P = new Matrix(new[] { new double[] { 0 } });
                var B = new HyperVector(0, 0);

                mnk.mnkIter(iterClasterPoints.Last().InputAttributeValue.ToList(), iterClasterPoints.Last().DoubleOutput,
                    ref P, ref B, true);

                var errorIter = errorsMnk(B, iterClasterPoints);
                while (errorIter < mayError)
                {
                    if (originalSimpleSet.Count == 0) break;

                 //   int NextIndex = maxPoint.NearestNormalizedFullIndex(originalSimpleSet, Result.LearnSamplesSet);
                    int NextIndex = maxPoint.NearestNormalizedInputIndex(originalSimpleSet, ResultSystem.LearnSamplesSet);
                 
                    var nextPoint = originalSimpleSet[NextIndex];
                    mnk.mnkIter(nextPoint.InputAttributeValue.ToList(), nextPoint.DoubleOutput, ref P, ref B, false);
                    errorIter = errorsMnk(B, new List<SampleSet.RowSample>(iterClasterPoints) { nextPoint });
                    if (errorIter < mayError)
                    {
                        iterClasterPoints.Add(nextPoint);
                        maxPoint = nextPoint;
                        originalSimpleSet.Remove(nextPoint);
                        originalSimpleSetDistanteToPoint.RemoveAt(NextIndex);

                    }

                }
                var numbersRule = new List<int>();
                List<Term> forRWLSM = new List<Term>();
                for (int i = 0; i < Approximate.CountUsedVars; i++)
                {
                    var parametrs = new double[Term.CountParamsinSelectedTermType(typeFunc)];
                    parametrs[0] = iterClasterPoints.Sum(x => x.InputAttributeValue[i]) / iterClasterPoints.Count;
                    parametrs[1] =
                        Math.Sqrt(
                            iterClasterPoints.Sum(x => Math.Pow(x.InputAttributeValue[i] - parametrs[0], 2.0)) /
                            iterClasterPoints.Count);

                    var temp_term = new Term(parametrs, typeFunc, i);
                    forRWLSM.Add(temp_term);
                    Approximate.RulesDatabaseSet[0].TermsSet.Add(temp_term);
                    numbersRule.Add(Approximate.RulesDatabaseSet[0].TermsSet.Count - 1);
                }
                ruleIndex++;
                double[] coeffs = null;
                double coef = LSMWeghtReqursiveSimple.EvaluteConsiquent(ResultSystem, forRWLSM, out coeffs);
                var temp_rule = new TSARule(Approximate.RulesDatabaseSet[0].TermsSet, numbersRule.ToArray(), coef, coeffs);
            //   var temp_rule = new TSARule(Approximate.RulesDatabaseSet[0].TermsSet, numbersRule.ToArray(), B.Elements[0].Elements[0], B.Elements.Skip(1).Select(x => x.Elements[0]).ToArray());
      
                Approximate.RulesDatabaseSet[0].RulesDatabase.Add(temp_rule);


            }
        }

        public static double errorsMnk(HyperVector B, List<SampleSet.RowSample> samples)
        {
            return Math.Sqrt((from x in samples let summ = x.InputAttributeValue.ToList().Select((a, i) => a * B.Elements[i + 1].Elements[0]).Sum() + B.Elements[0].Elements[0] select Math.Pow((summ - x.DoubleOutput), 2.0)).Sum());
        }

        public void CalcFarPoint()
        {
            //Для создания модификаций
            double[] initValue = new double[originalSimpleSet[0].InputAttributeValue.LongLength];
            double initOutput = ResultSystem.LearnSamplesSet.OutputAttribute.Min;
            for (int j = 0; j < initValue.LongLength; j++)
            {
                initValue[j] = ResultSystem.LearnSamplesSet.InputAttributes[j].Min;
            }
            SampleSet.RowSample initPoint = new SampleSet.RowSample(initValue, null, initOutput, "");
            for (int i = 0; i < originalSimpleSet.Count; i++)
            {
            //    originalSimpleSetDistanteToPoint.Add(initPoint.getFullNormalizedDistance(originalSimpleSet[i], Result.LearnSamplesSet));
                originalSimpleSetDistanteToPoint.Add(initPoint.getInputNormalizedDistance(originalSimpleSet[i], ResultSystem.LearnSamplesSet));
   
            }

        }


        public override string ToString(bool with_param = false)
        {
            return with_param ? @"КЛИ2: Функции принадлежности= " + Term.ToStringTypeTerm(type_func) + Environment.NewLine : "КЛИ2";
        }





        public override IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf result = new KLI_conf();
            result.Init(CountFeatures);
            return result;
        }

        


    }







}

