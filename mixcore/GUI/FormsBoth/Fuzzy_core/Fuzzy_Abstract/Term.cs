using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fuzzy_system;

namespace Fuzzy_system.Fuzzy_Abstract
{
    public class Term
    {
        public double[] Parametrs
        {
            get { return parametrs; }
        }
        protected double[] parametrs;

        protected int number_of_input_var;
        protected Type_Term_Func_Enum term_tunc_type;
        public Type_Term_Func_Enum Term_Func_Type
        {
            get { return term_tunc_type; }
        }
        public int Number_of_Input_Var
        {
            get { return number_of_input_var; }
        }

        public Term(double[] paramtrs, Type_Term_Func_Enum type_term, int num_var)
        {
            parametrs = paramtrs;
            term_tunc_type = type_term;
            number_of_input_var = num_var;
        }
        public Term(Term source)
        {
            parametrs = (double[])source.parametrs.Clone();
            term_tunc_type = ((Type_Term_Func_Enum)((int)source.Term_Func_Type));
            number_of_input_var = source.number_of_input_var;

        }

        public double Max
        {
            get
            {
                switch (term_tunc_type)
                {
                    case Type_Term_Func_Enum.Треугольник: return parametrs[2];
                    case Type_Term_Func_Enum.Гауссоида: return parametrs[0];
                    case Type_Term_Func_Enum.Парабола: return parametrs[1];
                    case Type_Term_Func_Enum.Трапеция: return parametrs[3];
                }
                return double.NegativeInfinity;
            }
            set
            {
                switch (term_tunc_type)
                {
                    case Type_Term_Func_Enum.Треугольник: parametrs[2] = value; break;
                    case Type_Term_Func_Enum.Гауссоида: parametrs[0] = value; break;
                    case Type_Term_Func_Enum.Парабола: parametrs[1] = value; break;
                    case Type_Term_Func_Enum.Трапеция: parametrs[3] = value; break;
                }

            }
        }
        public double Min
        {

            get { return parametrs[0]; }
            set { parametrs[0] = value; }
        }


        public static Term Make_Term(double center, double distance, Type_Term_Func_Enum type_func, int number_of_input_var)
        {
            Term Result = new Term(generate_params_for_term(center, distance, type_func), type_func, number_of_input_var);
            return Result;
        }

        public static double[] generate_params_for_term(double Center, double distance, Type_Term_Func_Enum type_func)
        {
            double[] result = new double[Member_Function.Count_Params_For_Term(type_func)];
            switch (type_func)
            {
                case Type_Term_Func_Enum.Гауссоида:
                    {
                        result[0] = Center;
                        result[1] = distance / 3;
                    } break;
                case Type_Term_Func_Enum.Треугольник:
                    {
                        result[0] = Center - distance;
                        result[1] = Center;
                        result[2] = Center + distance;
                    } break;
                case Type_Term_Func_Enum.Парабола:
                    {
                        result[0] = Center - distance;
                        result[1] = Center + distance;
                    } break;
                case Type_Term_Func_Enum.Трапеция:
                    {
                        result[0] = Center - distance;
                        result[3] = Center + distance;
                        result[1] = result[0] + 0.8 * distance;
                        result[2] = result[0] + 1.2 * distance;

                    } break;
            }
            return result;
        }



    }

}
