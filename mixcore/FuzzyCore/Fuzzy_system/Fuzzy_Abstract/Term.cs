using System;
using System.Collections.Generic;

namespace FuzzySystem.FuzzyAbstract
{

    public enum TypeTermFuncEnum
    {
        Треугольник = 0,
        Гауссоида = 1,
        Парабола = 2,
        Трапеция = 3
    }


    public class Term
    {
        public double[] Parametrs
        {
            get { return parametrs; }
            set { parametrs = value; }
        }
        protected double[] parametrs;

        protected int number_of_input_var;
        protected TypeTermFuncEnum term_tunc_type;
        public TypeTermFuncEnum TermFuncType
        {
            get { return term_tunc_type; }
        }
        public int NumberOfInputVar
        {
            get { return number_of_input_var; }
        }

        public Term(double[] paramtrs, TypeTermFuncEnum type_term, int num_var)
        {
            parametrs = paramtrs;
            term_tunc_type = type_term;
            number_of_input_var = num_var;
        }
        public Term(Term source)
        {
            parametrs = (double[])source.parametrs.Clone();
            term_tunc_type = ((TypeTermFuncEnum)((int)source.TermFuncType));
            number_of_input_var = source.number_of_input_var;

        }

        public double Max
        {
            get
            {
                switch (term_tunc_type)
                {
                    case TypeTermFuncEnum.Треугольник: return parametrs[2];
                    case TypeTermFuncEnum.Гауссоида: return parametrs[0];
                    case TypeTermFuncEnum.Парабола: return parametrs[1];
                    case TypeTermFuncEnum.Трапеция: return parametrs[3];
                }
                return double.NegativeInfinity;
            }
            set
            {
                switch (term_tunc_type)
                {
                    case TypeTermFuncEnum.Треугольник: parametrs[2] = value; break;
                    case TypeTermFuncEnum.Гауссоида: parametrs[0] = value; break;
                    case TypeTermFuncEnum.Парабола: parametrs[1] = value; break;
                    case TypeTermFuncEnum.Трапеция: parametrs[3] = value; break;
                }

            }
        }
        public double Min
        {

            get { return parametrs[0]; }
            set { parametrs[0] = value; }
        }


        public static Term Make_Term(double center, double distance, TypeTermFuncEnum type_func, int number_of_input_var)
        {
            Term Result = new Term(generate_params_for_term(center, distance, type_func), type_func, number_of_input_var);
            return Result;
        }

        public static double[] generate_params_for_term(double Center, double distance, TypeTermFuncEnum type_func)
        {
            double[] result = new double[CountParamsinSelectedTermType(type_func)];
            switch (type_func)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        result[0] = Center;
                        result[1] = distance / 3;
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



        public double Pick
        {
            get
            {

                switch (term_tunc_type)
                {
                    case TypeTermFuncEnum.Треугольник: return parametrs[1];
                    case TypeTermFuncEnum.Гауссоида: return parametrs[0];
                    case TypeTermFuncEnum.Парабола: return (parametrs[1] + parametrs[0]) / 2;
                    case TypeTermFuncEnum.Трапеция: return (parametrs[1] + parametrs[2]) / 2;
                }
                return double.NaN;
            }
            set
            {
                switch (term_tunc_type)
                {
                    case TypeTermFuncEnum.Треугольник: parametrs[1] = value; break;
                    case TypeTermFuncEnum.Гауссоида: parametrs[0] = value; break;
                    case TypeTermFuncEnum.Парабола:
                        {
                            double interval = (parametrs[1] + parametrs[0]) / 2;
                            parametrs[0] = value + interval;
                            parametrs[1] = value - interval;
                        } break;
                    case TypeTermFuncEnum.Трапеция:
                        {
                            double interval = (parametrs[1] + parametrs[2]) / 2;
                            parametrs[1] = value + interval;
                            parametrs[2] = value - interval;
                        } break;
                }

            }

        }


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


        public int CountParams
        {
            get
            {
                return CountParamsinSelectedTermType(term_tunc_type);
            }
        }



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

        public string TypeTerm
        {
            get
            {
                return ToStringTypeTerm(term_tunc_type);

            }
        }


        public double LevelOfMembership(double[] x)
        {
            return LevelOfMembership(x[NumberOfInputVar]);
        }

        public double LevelOfMembership(double x)
        {
            FixTermParams();
            switch (term_tunc_type)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        return Math.Exp((-1 * Math.Pow((x - parametrs[0]), 2)) / (2 * Math.Pow(parametrs[1], 2)));

                    }
                case TypeTermFuncEnum.Парабола:
                    {
                        if ((x >= parametrs[0]) && (x <= parametrs[1]))
                        {
                            return 4 * (x - parametrs[0]) * (x - parametrs[1]) * (-1) / ((parametrs[0] - parametrs[1]) * (parametrs[0] - parametrs[1]));
                        }
                        return double.Epsilon;
                    }
                case TypeTermFuncEnum.Трапеция:
                    {
                        if ((x >= parametrs[0]) && (x <= parametrs[3]))
                        {
                            if ((x >= parametrs[1]) && (x <= parametrs[2])) { return 1; }
                            if (x < parametrs[1]) { return (x - parametrs[0]) / (parametrs[1] - parametrs[0]); }
                            if (x > parametrs[2]) { return (parametrs[3] - x) / (parametrs[3] - parametrs[2] + 0.0000000000000000000000000000000001); }
                        }
                        return double.Epsilon;

                    }
                case TypeTermFuncEnum.Треугольник:
                    {
                        if (x == parametrs[1]) { return 1; }

                        if ((x >= parametrs[0]) && (x <= parametrs[2]))
                        {
                            if (x < parametrs[1])
                            {
                                return (x - parametrs[0]) / (parametrs[1] - parametrs[0]);
                            }
                            if (x > parametrs[1])
                            {
                                return (parametrs[2] - x) / (parametrs[2] - parametrs[1]);
                            }
                        }
                        return double.Epsilon;
                    }

                default: { return double.Epsilon; }
            }
        }



        public double[] getXValueByLevelMembership(double Level)
        {
            FixTermParams();
            List<double> result = new List<double>();
            switch (term_tunc_type)
            {

                case TypeTermFuncEnum.Треугольник:
                    {
                        result.Add((parametrs[0] + (parametrs[1]) - parametrs[0]) * Level);
                        result.Add((parametrs[2] - (parametrs[2]) - parametrs[1]) * Level);
                        break;
                    }
                case TypeTermFuncEnum.Гауссоида:
                    {
                        result.Add((parametrs[0] - (parametrs[1] * Math.Sqrt(-2 * Math.Log(Level)))));
                        result.Add((parametrs[0] + (parametrs[1] * Math.Sqrt(-2 * Math.Log(Level)))));
                        break;
                    }
                case TypeTermFuncEnum.Парабола:
                    {
                        double sum = parametrs[0] + parametrs[1];
                        double sumddouble = sum * sum;
                        double subdouble = Math.Pow(parametrs[0] - parametrs[1], 2);
                        double mul = parametrs[0] * parametrs[1];
                        result.Add((sum - Math.Sqrt(sumddouble - 4 * mul - Level * subdouble)) / 2);
                        result.Add((sum + Math.Sqrt(sumddouble - 4 * mul - Level * subdouble)) / 2);
                        break;
                    }
                case TypeTermFuncEnum.Трапеция:
                    {
                        result.Add((parametrs[0] + (parametrs[1]) - parametrs[0]) * Level);
                        result.Add((parametrs[3] - (parametrs[3]) - parametrs[2]) * Level);
                        break;
                    }
            }


            return result.ToArray();
        }




        public void FixTermParams()
        {

            switch (TermFuncType)
            {
                case TypeTermFuncEnum.Гауссоида:
                    {
                        if (parametrs[1] < double.Epsilon)
                        {
                            parametrs[1] = double.Epsilon;
                        }
                        break;
                    }
                case TypeTermFuncEnum.Парабола:
                    {
                        Array.Sort(parametrs);
                        break;
                    }
                case TypeTermFuncEnum.Трапеция:
                    {
                        Array.Sort(parametrs);
                        break;
                    }
                case TypeTermFuncEnum.Треугольник:
                    {
                        Array.Sort(parametrs);
                        break;
                    }
            }
        }




    }

}
