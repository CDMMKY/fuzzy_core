using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.TakagiSugenoApproximate;

namespace FuzzyCoreUtils
{
    static partial class Extensions
    {
        public static List<KnowlegeBaseTSARules> Clone(this IList<KnowlegeBaseTSARules> listToClone)
        {
            return listToClone.Select(item => (new KnowlegeBaseTSARules(item))).ToList();
        }
    }
    public static class ListTakagiSugenoApproximateTool
    {

        public static KnowlegeBaseTSARules[] SortRules(this KnowlegeBaseTSARules[] Source, TSAFuzzySystem Approx)
        {
            double[] keys = new double[Source.Count()];
            KnowlegeBaseTSARules[] tempSol = Source.Clone() as KnowlegeBaseTSARules[];
            for (int i = 0; i < Source.Count(); i++)
            {
                keys[i] = Approx.approxLearnSamples(Source[i]);
            }
            Array.Sort(keys, tempSol);
            return tempSol;
        }

        public static List<KnowlegeBaseTSARules> SortRules(this List<KnowlegeBaseTSARules> Source, TSAFuzzySystem Approx)
        {
            return Source.ToArray().SortRules(Approx).ToList();
        }


        public static KnowlegeBaseTSARules[] SelectBest(this KnowlegeBaseTSARules[] Source, TSAFuzzySystem Approx, int CountBest)
        {
            KnowlegeBaseTSARules[] result = new KnowlegeBaseTSARules[CountBest];
            Source.SortRules(Approx).ToList().CopyTo(0, result, 0, CountBest);
            return result;
        }

        public static List<KnowlegeBaseTSARules> SelectBest(this List<KnowlegeBaseTSARules> Source, TSAFuzzySystem Approx, int CountBest)
        {
            KnowlegeBaseTSARules[] result = new KnowlegeBaseTSARules[CountBest];
            Source.ToArray().SortRules(Approx).ToList().CopyTo(0, result, 0, CountBest);
            return result.ToList();
        }

        public static void Inject(this List<KnowlegeBaseTSARules> Destination, int indexStartDestination, List<KnowlegeBaseTSARules> Source, int indexStartSource, int CountIjected, TSAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.Clone());
        }

        public static void Inject(this List<KnowlegeBaseTSARules> Destination, int indexStartDestination, KnowlegeBaseTSARules[] Source, int indexStartSource, int CountIjected, TSAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.ToArray().Clone() as KnowlegeBaseTSARules[]);
        }


        public static void Inject(this KnowlegeBaseTSARules[] Destination, int indexStartDestination, List<KnowlegeBaseTSARules> Source, int indexStartSource, int CountIjected, TSAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBaseTSARules(Source[i + indexStartSource]);
            }
        }

        public static void Inject(this KnowlegeBaseTSARules[] Destination, int indexStartDestination, KnowlegeBaseTSARules[] Source, int indexStartSource, int CountIjected, TSAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBaseTSARules(Source[i + indexStartSource]);
            }
        }
    }
}
