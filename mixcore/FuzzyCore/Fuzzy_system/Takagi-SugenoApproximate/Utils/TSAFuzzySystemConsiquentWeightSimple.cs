using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using Matrix_component.MatrixN;

namespace FuzzySystem.TakagiSugenoApproximate
{
    public static class LSMWeghtReqursiveSimple
    {
        public static double EvaluteConsiquent(TSAFuzzySystem System,TermSetGlobal<Term> termSet, out double[] RegressionCoefficents)
        {
            return EvaluteConsiquent(System,termSet.ToList(), out RegressionCoefficents);
        }
        public static double EvaluteConsiquent(TSAFuzzySystem System, TermSetInRule<Term> termSet, out double[] RegressionCoefficents)
        {
            return EvaluteConsiquent(System, termSet.ToList(), out RegressionCoefficents);
        }
        public static double EvaluteConsiquent(TSAFuzzySystem System,List<Term> termSet, out double[] RegressionCoefficents)
        {
            Matrix nominateVector, PnominatorMul1, PnominatorMul2, PnominatorMul3, UnidenominatorPart1, Unidenominator2, TetaResultPart1, Tetanominator1, TetaResultBr2Part1;
            double TetaResultBr2, UniDenominator;
            RegressionCoefficents = null;
            int countParams = System.CountFeatures;
            double a = Math.Pow(10,100);
            Matrix P=new Matrix (countParams+1,countParams+1,0);
            for (int i =0;i<countParams+1;i++)
            {P.SetElement(i,i, a);  }
            Matrix Teta = new Matrix(countParams + 1, 1, 0);
            List<SampleSet.RowSample> RealValue = SelectPoint(System, termSet, countParams);
            List<SampleSet.RowSample> NominateValue = MakeXValue(System,RealValue);
            
           for (int i =0;i<NominateValue.Count;i++)
           {
              nominateVector =new  Matrix(NominateValue[i ].InputAttributeValue,countParams+1);
              UnidenominatorPart1 = nominateVector.Transpose().Multiply(P);
              Unidenominator2 = UnidenominatorPart1.Multiply(nominateVector);
              UniDenominator = 1.0 / (1.0 / (calcWeigth(NominateValue[i], termSet)) + Unidenominator2.GetElement(0, 0));
              Tetanominator1 = P.Multiply(nominateVector);
              TetaResultPart1 = Tetanominator1.Multiply(UniDenominator);
              TetaResultBr2Part1 = nominateVector.Transpose().Multiply(Teta);
              TetaResultBr2 = NominateValue[i].DoubleOutput - TetaResultBr2Part1.GetElement(0, 0);
              Teta = Teta + TetaResultPart1.Multiply(TetaResultBr2);
              PnominatorMul1 = P.Multiply(nominateVector);
              PnominatorMul2 = PnominatorMul1.Multiply(nominateVector.Transpose());
              PnominatorMul3  =PnominatorMul2.Multiply(P);
               P = P - PnominatorMul3.Multiply(UniDenominator);
            }
           RegressionCoefficents = denominateCoefficient(System, Teta);
            return Teta.GetElement(0,0);
        }
        public static double[] denominateCoefficient(TSAFuzzySystem System, Matrix Teta)
        {
            double[] result = new double[Teta.RowDimension - 1];
            for (int i = 1; i < Teta.RowDimension;i++ )
            {
                result[i - 1] = (Teta.GetElement(i, 0)   );
            }
            return result;
        }
        public static void printArray (double [,] Source)
        {
            for (int i = 0; i < Source.GetLength(0); i++)
            {
                for (int j = 0; j < Source.GetLength(1); j++)
                {
                    Console.Write(Source[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static List<SampleSet.RowSample> SelectPoint(TSAFuzzySystem System, List<Term> termSet, int CountParams, double SelectLevel = 0.000000001)
        {
            List<SampleSet.RowSample> result = new List<SampleSet.RowSample>();
            List<double> Distance = new List<double>();
            double[] min = new double[termSet.Count];
            double[] max = new double[termSet.Count];
            double[] temp;
            for (int i = 0; i < termSet.Count; i++)
            {
                temp = termSet[i].getXValueByLevelMembership(SelectLevel);
                min[termSet[i].NumVar] = temp[0];
                max[termSet[i].NumVar] = temp[1];
            }

            for (int i = 0; i <System.LearnSamplesSet.CountSamples; i++)
            {   bool flag = true;
                for (int j = 0; j < termSet.Count; j++)
                {   if (System.AcceptedFeatures[termSet[j].NumVar] == false) { continue; }
                    if ((System.LearnSamplesSet.DataRows[i].InputAttributeValue[termSet[j].NumVar] <= min[termSet[j].NumVar])
                        && (System.LearnSamplesSet.DataRows[i].InputAttributeValue[termSet[j].NumVar] >= max[termSet[j].NumVar]))
                    {flag = false;
                     break;
                    }
                }
                if (flag)
                { result.Add(System.LearnSamplesSet[i]);
                }
            }
            return result;
        }
        private static List<SampleSet.RowSample> MakeXValue(TSAFuzzySystem System,List<SampleSet.RowSample> Source)
        {
            List<SampleSet.RowSample> result = new List<SampleSet.RowSample>();
            for (int i = 0; i < Source.Count; i++)
            {
                double[] inputValue = new double[Source[i].InputAttributeValue.Length+1];
                inputValue[0] = 1;
                for (int j = 1; j < Source[i].InputAttributeValue.Length+1; j++)
                {  
                    inputValue[j] = ( Source[i].InputAttributeValue[j - 1]   ); 
                }
                double Value = (Source[i].DoubleOutput   );
                result.Add(new SampleSet.RowSample(inputValue, null, Value,""));
            }
            return result;
        }

        private static double calcWeigth(SampleSet.RowSample Value, List <Term> Terms)
        {
            double mul = 1.0;
            foreach (Term tr in Terms)
            {
                mul *= tr.LevelOfMembership(Value.InputAttributeValue[tr.NumVar + 1]); 
            }
            return mul;
        }
    }
}
