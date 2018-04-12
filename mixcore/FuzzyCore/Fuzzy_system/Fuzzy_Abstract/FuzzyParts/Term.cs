using System;
using System.Collections.Generic;

namespace FuzzySystem.FuzzyAbstract
{
    /// <summary>
    /// Перечисление доступных видов функций принадлежности
    /// </summary>
    public enum TypeTermFuncEnum
    {
        Треугольник = 0,
        Гауссоида = 1,
        Парабола = 2,
        Трапеция = 3
    }

    /// <summary>
    /// Класс описывающий функций принадлежности нечетким множествам
    /// </summary>
    public class Term
    {/// <summary>
    /// Параметры функции принадлежности, вектор переменной длины
    /// </summary>
        public double[] Parametrs
        {
            get;
            set;
        }
    
        /// <summary>
        /// Тип функции принадлежности
        /// </summary>
        public TypeTermFuncEnum TermFuncType
        {
            get;
            protected set;
        }
        /// <summary>
        ///  Для какого по номеру входного параметра определен терм 
        /// </summary>
        public int NumVar
        {
            get;
            protected set;
        }

        /// <summary>
        /// Конструктор для создания функции принадлежности
        /// </summary>
        /// <param name="paramtrs">Вектор параметров функции принадлежности</param>
        /// <param name="type_term">Тип функции принадлежности</param>
        /// <param name="num_var">Номер входного параметра для которого определена функция принадлежности</param>
        public Term(double[] paramtrs, TypeTermFuncEnum type_term, int num_var)
        {
            Parametrs = paramtrs;
            TermFuncType = type_term;
            NumVar = num_var;
        }
        /// <summary>
        /// Клонирующий конструктор, создающий полную копию заданной функции принадлежности
        /// </summary>
        /// <param name="source">Заданная функций принадлежности</param>
        public Term(Term source)
        {
            Parametrs = (double[])source.Parametrs.Clone();
            TermFuncType = ((TypeTermFuncEnum)((int)source.TermFuncType));
            NumVar = source.NumVar;

        }

        /// <summary>
        /// Максимальное по величине значение параметров терма 
        /// </summary>
        public double Max
        {
            get
            {
                switch (TermFuncType)
                {
                    case TypeTermFuncEnum.Треугольник: return Parametrs[2];
                    case TypeTermFuncEnum.Гауссоида: return Parametrs[0] +3.0 * Parametrs[1];
                    case TypeTermFuncEnum.Парабола: return Parametrs[1];
                    case TypeTermFuncEnum.Трапеция: return Parametrs[3];
                }
                return double.NegativeInfinity;
            }
            set
            {
                switch (TermFuncType)
                {
                    case TypeTermFuncEnum.Треугольник: Parametrs[2] = value; break;
                    case TypeTermFuncEnum.Гауссоида: Parametrs[0] = value-3.0 * Parametrs[1]; break;
                    case TypeTermFuncEnum.Парабола: Parametrs[1] = value; break;
                    case TypeTermFuncEnum.Трапеция: Parametrs[3] = value; break;
                }

            }
        }

        /// <summary>
        /// Минимальное по величине значение параметров терма
        /// </summary>
        public double Min
        {
            
            get { if (TermFuncType == TypeTermFuncEnum.Гауссоида) return Parametrs[0] - 3.0 * Parametrs[1];
                    return Parametrs[0]; }
            set {
                if (TermFuncType == TypeTermFuncEnum.Гауссоида) Parametrs[0] = value + 3.0 * Parametrs[1];
                else Parametrs[0] = value; }
        }

        /// <summary>
        /// Метод упращающий создание функции принадлежности. Использует значение пика (центра ) функции принадлежности, дистанцию (разброс отностительно центра), тип функции принадлежности и указание номер входной переменной
        /// </summary>
        /// <param name="center">значение пика (центра ) функции принадлежности</param>
        /// <param name="distance"> дистанция (разброс отностительно центра)</param>
        /// <param name="TypeTerm">тип функции принадлежности</param>
        /// <param name="NumVar">Номер входной переменной</param>
        /// <returns>Новая функция принадлежности</returns>
        public static Term MakeTerm(double center, double distance, TypeTermFuncEnum TypeTerm, int NumVar)
        {
            Term Result = new Term(GenTermParams(center, distance, TypeTerm), TypeTerm, NumVar);
            return Result;
        }
        /// <summary>
        /// Метод расчитывающий вектор параметров по заданому значению пика функции принадлежности, разбросу отностельно центра и типу функции принадлежности.   
        /// </summary>
        /// <param name="Center">Значении пика (центра) функции принадлежности</param>
        /// <param name="distance">Разбросу отностельно пика (центра) функции принадлежности</param>
        /// <param name="TypeTerm">Тип функции принадлежности</param>
        /// <returns>Вектор параметров функции принадлежности</returns>
        public static double[] GenTermParams(double Center, double distance, TypeTermFuncEnum TypeTerm)
        {
            double[] result = new double[CountParamsinSelectedTermType(TypeTerm)];
            switch (TypeTerm)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        result[0] = Center;
                        result[1] = distance / 3.0;
                    } break;
                case TypeTermFuncEnum.Треугольник:
                    {
                        result[0] = Center - distance;
                        result[1] = Center;
                        result[2] = Center + distance;
                    } break;
                case TypeTermFuncEnum.Парабола:
                    {
                        result[0] = Center - distance;
                        result[1] = Center + distance;
                    } break;
                case TypeTermFuncEnum.Трапеция:
                    {
                        result[0] = Center - distance;
                        result[3] = Center + distance;
                        result[1] = result[0] + 0.8 * distance;
                        result[2] = result[0] + 1.2 * distance;

                    } break;
            }
            return result;
        }


        /// <summary>
        /// Свойство возвращающее значение пика (центра) функции принадлежности 
        /// </summary>
        public double Pick
        {
            get
            {

                switch (TermFuncType)
                {
                    case TypeTermFuncEnum.Треугольник: return Parametrs[1];
                    case TypeTermFuncEnum.Гауссоида: return Parametrs[0];
                    case TypeTermFuncEnum.Парабола: return (Parametrs[1] + Parametrs[0]) / 2.0;
                    case TypeTermFuncEnum.Трапеция: return (Parametrs[1] + Parametrs[2]) / 2.0;
                }
                return double.NaN;
            }
            set
            {
                switch (TermFuncType)
                {
                    case TypeTermFuncEnum.Треугольник: Parametrs[1] = value; break;
                    case TypeTermFuncEnum.Гауссоида: Parametrs[0] = value; break;
                    case TypeTermFuncEnum.Парабола:
                        {
                            double interval = (Parametrs[1] + Parametrs[0]) / 2.0;
                            Parametrs[0] = value + interval;
                            Parametrs[1] = value - interval;
                        } break;
                    case TypeTermFuncEnum.Трапеция:
                        {
                            double interval = (Parametrs[1] + Parametrs[2]) / 2.0;
                            Parametrs[1] = value + interval;
                            Parametrs[2] = value - interval;
                        } break;
                }

            }

        }

        /// <summary>
        /// Размер вектора параметров для заданного типа функции принадлежности.
        /// </summary>
        /// <param name="tf">Тип функции принадлежности</param>
        /// <returns>Размер вектора параметров</returns>
        public static int CountParamsinSelectedTermType(TypeTermFuncEnum tf)
        {
            switch (tf)
            {
                case TypeTermFuncEnum.Треугольник:
                    return 3;
                case TypeTermFuncEnum.Гауссоида:
                    return 2;
                case TypeTermFuncEnum.Парабола:
                    return 2;
                case TypeTermFuncEnum.Трапеция:
                    return 4;
            }
            return 0;
        }

         /// <summary>
         /// Размер вектора параметров в текущем терме.
         /// </summary>
        public int CountParams
        {
            get
            {
                return CountParamsinSelectedTermType(TermFuncType);
            }
        }


        /// <summary>
        /// Функция преобразование типа функции принадлежности в текстовое описание
        /// </summary>
        /// <param name="tf">Тип функции принадлежности</param>
        /// <returns>Строка с названием типа функции принадлежности</returns>
        public static string ToStringTypeTerm(TypeTermFuncEnum tf)
        {
            switch (tf)
            {
                case TypeTermFuncEnum.Гауссоида: return "Гауссоида";
                case TypeTermFuncEnum.Парабола: return "Парабола";
                case TypeTermFuncEnum.Трапеция: return "Трапеция";
                case TypeTermFuncEnum.Треугольник: return "Треугольник";
            }

            return "Треугольная";
        }
        /// <summary>
        /// Текствое название типа текущей функции принадлежности
        /// </summary>
        public string TypeTerm
        {
            get
            {
                return ToStringTypeTerm(TermFuncType);

            }
        }

        /// <summary>
        /// Функция расчитывающая уровень принадлежности к текущему нечеткому множеству согласно текущей функции принадлежности
        /// </summary>
        /// <param name="x">Вектор входных вещественных параметров</param>
        /// <returns>Уровень принадлежности</returns>
        public double LevelOfMembership(double[] x)
        {
            return LevelOfMembership(x[NumVar]);
        }
        /// <summary>
        /// Функция расчитывающая уровень принадлежности к текущему нечеткому множеству согласно текущей функции принадлежности 
        /// </summary>
        /// <param name="x">Вещественное значение для которого определяется уровень принадлежности</param>
        /// <returns>Уровень принадлежности</returns>
        public double LevelOfMembership(double x)
        {
            FixTermParams();
            switch (TermFuncType)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        return Math.Exp((-1 * Math.Pow((x - Parametrs[0]), 2.0)) / (2 * Math.Pow(Parametrs[1], 2.0)));
                    }
                case TypeTermFuncEnum.Парабола:
                    {
                        if ((x >= Parametrs[0]) && (x <= Parametrs[1]))
                        {
                            return 4.0 * (x - Parametrs[0]) * (x - Parametrs[1]) * (-1) / ((Parametrs[0] - Parametrs[1]) * (Parametrs[0] - Parametrs[1]));
                        }
                        return double.Epsilon;
                    }
                case TypeTermFuncEnum.Трапеция:
                    {
                        if ((x >= Parametrs[0]) && (x <= Parametrs[3]))
                        {
                            if ((x >= Parametrs[1]) && (x <= Parametrs[2])) { return 1; }
                            if (x < Parametrs[1]) { return (x - Parametrs[0]) / (Parametrs[1] - Parametrs[0]); }
                            if (x > Parametrs[2]) { return (Parametrs[3] - x) / (Parametrs[3] - Parametrs[2] + double.Epsilon); }
                        }
                        return double.Epsilon;

                    }
                case TypeTermFuncEnum.Треугольник:
                    {
                        if (x == Parametrs[1]) { return 1; }

                        if ((x >= Parametrs[0]) && (x <= Parametrs[2]))
                        {
                            if (x < Parametrs[1])
                            {
                                return (x - Parametrs[0]) / (Parametrs[1] - Parametrs[0]);
                            }
                            if (x > Parametrs[1])
                            {
                                return (Parametrs[2] - x) / (Parametrs[2] - Parametrs[1]);
                            }
                        }
                        return double.Epsilon;
                    }

                default: { return double.Epsilon; }
            }
        }


        /// <summary>
        /// Функция получающая набор значение входного параметра, с заданным уровнем принадлежности к текущему нечеткому множеству задаваемому текущей функцие принадлежности.
        /// </summary>
        /// <param name="Level">Уровень принадлежности в диапазоне от 0 по 1 </param>
        /// <returns>Вектор вещественных значений входного параметра</returns>
        public double[] getXValueByLevelMembership(double Level)
        {
            FixTermParams();
            List<double> result = new List<double>();
            switch (TermFuncType)
            {

                case TypeTermFuncEnum.Треугольник:
                    {
                        result.Add((Parametrs[0] + (Parametrs[1]) - Parametrs[0]) * Level);
                        result.Add((Parametrs[2] - (Parametrs[2]) - Parametrs[1]) * Level);
                        break;
                    }
                case TypeTermFuncEnum.Гауссоида:
                    {
                        result.Add((Parametrs[0] - (Parametrs[1] * Math.Sqrt(-2 * Math.Log(Level)))));
                        result.Add((Parametrs[0] + (Parametrs[1] * Math.Sqrt(-2 * Math.Log(Level)))));
                        break;
                    }
                case TypeTermFuncEnum.Парабола:
                    {
                        double sum = Parametrs[0] + Parametrs[1];
                        double sumddouble = sum * sum;
                        double subdouble = Math.Pow(Parametrs[0] - Parametrs[1], 2.0);
                        double mul = Parametrs[0] * Parametrs[1];
                        result.Add((sum - Math.Sqrt(sumddouble - 4.0 * mul - Level * subdouble)) / 2.0);
                        result.Add((sum + Math.Sqrt(sumddouble - 4.0 * mul - Level * subdouble)) / 2.0);
                        break;
                    }
                case TypeTermFuncEnum.Трапеция:
                    {
                        result.Add((Parametrs[0] + (Parametrs[1]) - Parametrs[0]) * Level);
                        result.Add((Parametrs[3] - (Parametrs[3]) - Parametrs[2]) * Level);
                        break;
                    }
            }


            return result.ToArray();
        }

        /// <summary>
        /// Метод исправляющая слишком узкую Гауссову функцию, а также упорядывающая параметры других функции принадлежности.
        /// </summary>
        public void FixTermParams()
        {

            switch (TermFuncType)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        if (Parametrs[1] < 0.0000000001)
                        {
                            Parametrs[1] = 0.0000000001;
                        }
                        break;
                    }
                default: Array.Sort(Parametrs); break;
            }
        }




    }

}
