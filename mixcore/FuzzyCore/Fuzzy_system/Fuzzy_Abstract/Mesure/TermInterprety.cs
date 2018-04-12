using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract.Mesure
{
    public static class TermOnterpreting
    {
        public static double getIndexByCentersClose(Term A, Term B, int countTermByFeature, double scatterFeature)
        {
            double result = 1;
            if (A.NumVar != B.NumVar) { return result; }
            double interval = scatterFeature / (double)(countTermByFeature - 1);
            double distance = Math.Abs(A.Pick - B.Pick);
            if (distance >= interval)
            {
                return 1;

            }
            else
            {
                result = distance / interval;
            }

            return result;
        }


        public static double getIndexByBordersClose(Term A, Term B, double goodPercentofCenterDistance)
        {
            double result = 1;
            if (A.NumVar != B.NumVar) { return result; }

            if (isCrossedTerms(A, B))
            {

                Term Left = A;
                Term Rigth = B;
                if (A.Pick > B.Pick)
                {
                    Left = B;
                    Rigth = A;
                }
                double centerDistance = Math.Abs(A.Pick - B.Pick);
                double gooddistance = centerDistance * goodPercentofCenterDistance;
                double distance = Left.Max - Rigth.Min;
                if (distance <= gooddistance) { return 1; }
                else { result = gooddistance / distance; }
            }

            return result;
        }

        public static bool isCrossedTerms(Term A, Term B)
        {
            bool result = false;
            if (A.NumVar != B.NumVar) { return result; }
            Term Left = A;
            Term Rigth = B;
            if (A.Pick > B.Pick)
            {
                Left = B;
                Rigth = A;
            }

            if (Left.Max > Rigth.Min)
            { result = true; }

            return result;
        }

        public static double getIndexByAreaTerms(Term A, Term B, double goodPercent)
        {
            double result = 1;


            if (A.NumVar != B.NumVar) { return result; }

            Term Left = A;
            Term Right = B;
            if (A.Pick > B.Pick)
            {
                Left = B;
                Right = A;
            }


            double sqareCross = getSqareCross(Left, Right);
            if (sqareCross < 0)
            {
                return result;
            }

            double SqareUnion = getSqareByUnion(Left, Right);
            if (SqareUnion < 0)
            {
                return result;
            }


            double borderSqare = SqareUnion * goodPercent;

            if (sqareCross < borderSqare)
            {
                return 1;
            }
            else
            {
                result = borderSqare / sqareCross;
            }

            return result;
        }


        public static double getSqareCross(Term A, Term B)
        {
            double result = 0;
            if (A.NumVar != B.NumVar) { return result; }


            double higth = (B.Min - A.Max) / (B.Min - B.Pick + A.Pick - A.Max);
            double ground = A.Max - B.Min;
            result = 0.5 * higth * ground;

            return result;
        }

        public static double getSqareByUnion(Term A, Term B)
        {
            double result = 1.0;
            if (A.NumVar != B.NumVar) { return result; }

            double sqareA = 0.5 * (A.Max - A.Min);
            double sqareB = 0.5 * (B.Max - B.Min);
            result = sqareA + sqareB - getSqareCross(A, B);
            if (result < 0)
            {
                return result;
            }
            return result;
        }





        public static double getG3(Term A, Term B, int countTerms, double attributeScatter, double goodforBorders, double goodforAreas)
        {
            double result = 1.0;
            double indexCenter = getIndexByCentersClose(A, B, countTerms, attributeScatter);
            double indexBorder = getIndexByBordersClose(A, B, goodforBorders);
            double indexAreas = getIndexByAreaTerms(A, B, goodforAreas);
            double power = 1.0 / 3.0;
            result = Math.Pow(indexAreas * indexBorder * indexCenter, power);

            return result;
        }


        public static double getIndexByLinds(Term A, Term B, List<Term> termsOnFeature)
        {
            double result = 1;

            if (A.NumVar != B.NumVar) { return result; }

            IComparer<Term> toSort = new CompararerByPick();
            List<Term> sortedSource = termsOnFeature.ToArray().ToList();
            sortedSource.Sort(toSort);
            if (!A.Equals(B))
            {
                if (isCrossedTerms(A, B))
                {
                    int indexA = sortedSource.IndexOf(A);
                    int indexB = sortedSource.IndexOf(B);
                    result = 1 - (Math.Abs(indexA - indexB) - 1.0) / (double)sortedSource.Count();

                }
            }
            return result;
        }
       
    }
}
