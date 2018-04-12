using System.Collections.Generic;


namespace FuzzySystem.FuzzyAbstract
{
    public  class CompararerByPick : IComparer<Term>
    {


            // Compares by Height, Length, and Width.
            public int Compare(Term x, Term y)
            {
                double xCenter = getValueofCenter(x);
                double yCenter = getValueofCenter(y);
                if (xCenter == yCenter) { return 0; }
                if (xCenter < yCenter) { return -1; }
                if (xCenter > yCenter) { return 1; }

                return 0;
            }

            private double getValueofCenter(Term Term)
            {
                double Center = 0;
                switch (Term.TermFuncType)
                {
                    case TypeTermFuncEnum.Треугольник: { Center = Term.Parametrs[1]; break; }
                    case TypeTermFuncEnum.Трапеция: { Center = (Term.Parametrs[1] + Term.Parametrs[2]) / 2; break; }
                    case TypeTermFuncEnum.Парабола: { Center = (Term.Parametrs[0] + Term.Parametrs[1]) / 2; break; }
                    case TypeTermFuncEnum.Гауссоида: { Center = Term.Parametrs[0]; break; }
                }
                return Center;

            }

        }




    }

