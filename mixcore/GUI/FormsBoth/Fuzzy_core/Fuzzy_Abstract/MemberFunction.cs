using System;

namespace Fuzzy_system
{

    public enum Type_Term_Func_Enum
    {
        Треугольник = 0,
        Гауссоида = 1,
        Парабола = 2,
        Трапеция = 3


    }


  public static class Member_Function:Object
    {

      public static double Func(int Type_Of_Func, double[] p, double x, out double[] par)
        {
            par = p;
            switch (Type_Of_Func)
            {

                case 0: {

                    if (par[2] < par[1]) { par[1] = par[2]; }
                    if (par[1] < par[0]) { par[0] = par[1]; }




                    if (x == par[1]) { return 1; }
                    if ((x <= par[0]) || (x >= par[2])) { return 0; }

                    if (x < par[1])
                    {




                        return (x - par[0]) / (par[1] - par[0]);




                    }
                    if (x > par[1])
                    {

                        return (par[2] - x) / (par[2] - par[1]);
                    }

                    
                    
                    break; }
                case 1:
                    {
                        double m = par[1] + 0.000000000000000000001;
                        return Math.Exp((-1 * Math.Pow((x - par[0]), 2)) / (2 * Math.Pow(m, 2)));
                       
                    }
                case 2:
                    {
                        if ((x >= par[0]) && (x <= par[1]))
                        {
                            return 4 * (x - par[0]) * (x - par[1]) * (-1) / ((par[0] - par[1]) * (par[0] - par[1]));
                        }
                         return 0; 
                       
                    }
                case 3: {    
                if (par[3]<par[2]) {par[3]=par[2];}
                if (par[2]<par[1]) {par[2]=par[1];}
                if (par[1]<par[0]) {par[1]=par[0];}
                                   
     if ((x<=par[0]) || (x>=par[3])) { return 0; }
        if ((x>=par[1]) && (x<=par[2])) {  return 1; }
        if (x<par[1]) { return  (x-par[0])/(par[1]-par[0]); }
        if (x>par[2]) {return (par[3]-x)/(par[3]-par[2]+0.0000000000000000000000000000000001); }
        break;
                }
            }

                 
            return 0;
        }

      public static int Count_Params_For_Term(Type_Term_Func_Enum type_term)
      {
          switch (type_term)
          {
              case Type_Term_Func_Enum.Треугольник:
                  return 3;
              case Type_Term_Func_Enum.Гауссоида:
                  return 2;
              case Type_Term_Func_Enum.Парабола:
                  return 2;
              case Type_Term_Func_Enum.Трапеция:
                  return 4;

          }
          return 0;
      }




      public static string ToString(Type_Term_Func_Enum tf)
      {
          switch(tf)
          {case Type_Term_Func_Enum.Гауссоида: return "Гауссоида";
           case Type_Term_Func_Enum.Парабола: return "Парабола";
           case Type_Term_Func_Enum.Трапеция: return "Трапеция";
           case Type_Term_Func_Enum.Треугольник: return "Треугольник";
          }

          return "Треугольная";
      }







    }
}
