using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzySystem.FuzzyAbstract.Utils
{

    public static class DistanceTools
    {
        public enum DistanceType
        {
            Euclidean = 0,
            Manhattan = 1,
            //     Mahalanobis = 2,
            //   Gaussian = 3
        }

        #region Distances

        #region RealDistances

        /// <summary>
        /// Позволяе расчитать дистанцию между двумя объектами  SampleSet.RowSample.
        /// Учитываются только входные параметры.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample </param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getInputDistance(this SampleSet.RowSample A, SampleSet.RowSample B, DistanceType type = DistanceType.Euclidean)
        {
            if (A.InputAttributeValue.LongLength != B.InputAttributeValue.LongLength) throw new InvalidOperationException("Входные данные разных размерностей");

            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow(A.InputAttributeValue[i] - B.InputAttributeValue[i], 2.0);
                        }
                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {

                            result += Math.Abs(A.InputAttributeValue[i] - B.InputAttributeValue[i]);
                        }
                        return result;

                    }
                //     case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //     case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow(A.InputAttributeValue[i] - B.InputAttributeValue[i], 2.0);
                        }
                        return Math.Sqrt(result);
                    }
            }
        }

        /// <summary>
        /// Позволяе расчитать дистанцию между двумя объектами  SampleSet.RowSample.
        /// Учитываются только выходной параметр.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample </param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getOutputDistance(this SampleSet.RowSample A, SampleSet.RowSample B, DistanceType type = DistanceType.Euclidean)
        {

            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        result += Math.Pow(A.DoubleOutput - B.DoubleOutput, 2.0);
                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        result += Math.Abs(A.DoubleOutput - B.DoubleOutput);
                        return result;

                    }
                //     case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //      case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        result += Math.Pow(A.DoubleOutput - B.DoubleOutput, 2.0);
                        return Math.Sqrt(result);
                    }
            }
        }

        /// <summary>
        /// Позволяе расчитать дистанцию между двумя объектами  SampleSet.RowSample.
        /// Учитываются и входные и выходной параметры.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample </param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getFullDistance(this SampleSet.RowSample A, SampleSet.RowSample B, DistanceType type = DistanceType.Euclidean)
        {
            if (A.InputAttributeValue.LongLength != B.InputAttributeValue.LongLength) throw new InvalidOperationException("Входные данные разных размерностей");

            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow(A.InputAttributeValue[i] - B.InputAttributeValue[i], 2.0);
                        }
                        result += Math.Pow(A.DoubleOutput - B.DoubleOutput, 2.0);
                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Abs(A.InputAttributeValue[i] - B.InputAttributeValue[i]);
                        }
                        result += Math.Abs(A.DoubleOutput - B.DoubleOutput);
                        return result;

                    }
                //    case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //   case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow(A.InputAttributeValue[i] - B.InputAttributeValue[i], 2.0);
                        }
                        result += Math.Pow(A.DoubleOutput - B.DoubleOutput, 2.0);
                        return Math.Sqrt(result);
                    }
            }
        }
        #endregion

        #region NormalizedDistance

        /// <summary>
        /// Позволяе расчитать отнормированную дистанцию между двумя объектами  SampleSet.RowSample.
        ///  Учитываются входные параметры.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample</param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений вектора B</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getInputNormalizedDistance(this SampleSet.RowSample A, SampleSet.RowSample B, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (A.InputAttributeValue.LongLength != B.InputAttributeValue.LongLength) throw new InvalidOperationException("Входные данные разных размерностей");
            if (A.InputAttributeValue.LongLength != dataInfoA.InputAttributes.Count) throw new InvalidOperationException("Описание не соотвествует данным");
            if (dataInfoB == null) { dataInfoB = dataInfoA; }
            if (B.InputAttributeValue.LongLength != dataInfoB.InputAttributes.Count) throw new InvalidOperationException("Описание не соотвествует данным");

            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow((dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i])) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]), 2.0);
                        }
                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {

                            result += Math.Abs(dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i]) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]));
                        }
                        return result;

                    }
                //     case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //    case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow((dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i])) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]), 2.0);
                        }
                        return Math.Sqrt(result);
                    }
            }
        }

        /// <summary>
        /// Позволяе расчитать отнормированную дистанцию между двумя объектами  SampleSet.RowSample.
        ///  Учитывает только выходной параметр.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample</param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений вектора B</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getOutputNormalizedDistance(this SampleSet.RowSample A, SampleSet.RowSample B, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (dataInfoB == null) { dataInfoB = dataInfoA; }
            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        result += Math.Pow(dataInfoA.OutputAttribute.EvaluteNormalisedValue(A.DoubleOutput) - dataInfoB.OutputAttribute.EvaluteNormalisedValue(B.DoubleOutput), 2.0);
                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        result += Math.Abs(dataInfoA.OutputAttribute.EvaluteNormalisedValue(A.DoubleOutput) - dataInfoB.OutputAttribute.EvaluteNormalisedValue(B.DoubleOutput));
                        return result;

                    }
                //   case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //    case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        result += Math.Pow(A.DoubleOutput - B.DoubleOutput, 2.0);
                        return Math.Sqrt(result);
                    }
            }
        }

        /// <summary>
        /// Позволяет расчитать отнормированную дистанцию между двумя объектами  SampleSet.RowSample.
        ///  Учитывает входные и выходной параметры.
        /// </summary>
        /// <param name="A">Первый входной вектор типа SampleSet.RowSample</param>
        /// <param name="B">Второй входной вектор типа SampleSet.RowSample</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений вектора B</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Растояние типа double</returns>
        public static double getFullNormalizedDistance(this SampleSet.RowSample A, SampleSet.RowSample B, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (A.InputAttributeValue.LongLength != B.InputAttributeValue.LongLength) throw new InvalidOperationException("Входные данные разных размерностей");
            if (A.InputAttributeValue.LongLength != dataInfoA.InputAttributes.Count) throw new InvalidOperationException("Описание не соотвествует данным");
            if (dataInfoB == null) { dataInfoB = dataInfoA; }
            if (B.InputAttributeValue.LongLength != dataInfoB.InputAttributes.Count) throw new InvalidOperationException("Описание не соотвествует данным");

            double result = 0;
            switch (type)
            {
                case DistanceType.Euclidean:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow((dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i])) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]), 2.0);
                        }
                        result += Math.Pow(dataInfoA.OutputAttribute.EvaluteNormalisedValue(A.DoubleOutput) - dataInfoB.OutputAttribute.EvaluteNormalisedValue(B.DoubleOutput), 2.0);

                        return Math.Sqrt(result);
                    }
                case DistanceType.Manhattan:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {

                            result += Math.Abs(dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i]) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]));
                        }
                        result += Math.Abs(dataInfoA.OutputAttribute.EvaluteNormalisedValue(A.DoubleOutput) - dataInfoB.OutputAttribute.EvaluteNormalisedValue(B.DoubleOutput));

                        return result;

                    }
                //    case DistanceType.Mahalanobis: throw new NotImplementedException("Нет реализации необходимы формулы");
                //   case DistanceType.Gaussian: throw new NotImplementedException("Нет реализации необходимы формулы");
                default:
                    {
                        for (int i = 0; i < A.InputAttributeValue.LongLength; i++)
                        {
                            result += Math.Pow((dataInfoA.InputAttributes[i].EvaluteNormalisedValue(A.InputAttributeValue[i])) - dataInfoB.InputAttributes[i].EvaluteNormalisedValue(B.InputAttributeValue[i]), 2.0);
                        }
                        result += Math.Pow(dataInfoA.OutputAttribute.EvaluteNormalisedValue(A.DoubleOutput) - dataInfoB.OutputAttribute.EvaluteNormalisedValue(B.DoubleOutput), 2.0);

                        return Math.Sqrt(result);
                    }
            }
        }
        #endregion
        #endregion
        #region NearestIndex
        #region RealNearestIndex

        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета дистанции.
        /// Учитываются входные  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestInputIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, DistanceType type = DistanceType.Euclidean)
        {
            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getInputDistance(A, ListValue[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }

        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитываются входные  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestInputIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, DistanceType type = DistanceType.Euclidean)
        { 
            double[] dist =new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
              dist[i]=  getInputDistance(A, Array[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }

        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета дистанции.
        /// Учитывается только выходной  параметр.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestOutputIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, DistanceType type = DistanceType.Euclidean)
        {
            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getOutputDistance(A, ListValue[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }



        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитывается только выходной  параметр.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestOutputIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, DistanceType type = DistanceType.Euclidean)
        {
            double[] dist = new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
                dist[i] = getOutputDistance(A, Array[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }


        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета дистанции.
        /// Учитываются входные и выходной  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestFullIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, DistanceType type = DistanceType.Euclidean)
        {
            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getFullDistance(A, ListValue[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }



        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитываются входные и выходной  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestFullIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, DistanceType type = DistanceType.Euclidean)
        {
            double[] dist = new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
                dist[i] = getFullDistance(A, Array[i], type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }

        #endregion

        #region NormalizedNearestIndex
        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета нормированной дистанции.
        /// Учитываются входные  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedInputIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
          
      
            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getInputNormalizedDistance(A, ListValue[i], dataInfoA,dataInfoB,  type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }

        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитываются входные  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedInputIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
         
            double[] dist = new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
                dist[i] = getInputNormalizedDistance(A, Array[i], dataInfoA, dataInfoB, type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }

        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета дистанции.
        /// Учитывается только выходной  параметр.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedOutputIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (dataInfoB == null) { dataInfoB = dataInfoA; }
            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getOutputNormalizedDistance(A, ListValue[i], dataInfoA, dataInfoB, type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }



        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитывается только выходной  параметр.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedOutputIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (dataInfoB == null) { dataInfoB = dataInfoA; }

            double[] dist = new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
                dist[i] = getOutputNormalizedDistance(A, Array[i], dataInfoA, dataInfoB, type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }


        /// <summary>
        /// Позволяет найти ближайшую точку в списке ListValue на основе заданного способа рассчета дистанции.
        /// Учитываются входные и выходной  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в списке ListValue</param>
        /// <param name="ListValue">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedFullIndex(this SampleSet.RowSample A, List<SampleSet.RowSample> ListValue, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (dataInfoB == null) { dataInfoB = dataInfoA; }

            double[] dist = new double[ListValue.Count];
            for (int i = 0; i < ListValue.Count; i++)
            {
                dist[i] = getFullNormalizedDistance(A, ListValue[i], dataInfoA, dataInfoB, type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }



        /// <summary>
        /// Позволяет найти ближайшую точку в массиве Array на основе заданного способа рассчета дистанции.
        /// Учитываются входные и выходной  параметры.
        /// </summary>
        /// <param name="A">Точка для которой исчется ближайшая в массиве Array</param>
        /// <param name="Array">Вектор точек</param>
        /// <param name="dataInfoA">SampleSet используемый для корректной нормировки значений вектора A</param>
        /// <param name="dataInfoB">SampleSet используемый для корректной нормировки значений в списке ListValue</param>
        /// <param name="type">Тип вычисляемого расстояния Евклидово, Манхетенское и др.</param>
        /// <returns>Индекс ближайшей точки типа int</returns>
        public static int NearestNormalizedFullIndex(this SampleSet.RowSample A, SampleSet.RowSample[] Array, SampleSet dataInfoA, SampleSet dataInfoB = null, DistanceType type = DistanceType.Euclidean)
        {
            if (dataInfoB == null) { dataInfoB = dataInfoA; }

            double[] dist = new double[Array.LongLength];
            for (int i = 0; i < Array.LongLength; i++)
            {
                dist[i] = getFullNormalizedDistance(A, Array[i], dataInfoA, dataInfoB, type);
            }
            return dist.ToList().IndexOf(dist.Min());
        }
        #endregion
        #endregion

    }
}
