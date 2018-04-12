using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.SingletoneApproximate;

namespace FuzzyCoreUtils
{
    static partial class Extensions
    {
        public static List<KnowlegeBaseSARules> Clone(this IList<KnowlegeBaseSARules> listToClone)
        {
            return listToClone.Select(item => (new KnowlegeBaseSARules (item))).ToList();
        }
    }
    public static class ListSingletonApproximateTool
    {
        public static KnowlegeBaseSARules[] SortRules(this KnowlegeBaseSARules[] Source, SAFuzzySystem Approx)
       {
            double[] keys = new double[Source.Count()];
            KnowlegeBaseSARules[] tempSol = Source.Clone() as KnowlegeBaseSARules[];
            for (int i = 0; i < Source.Count(); i++)
            {
              keys[i]=  Approx.approxLearnSamples( Source[i]);
            }
            Array.Sort(keys, tempSol);
       return tempSol;
       }

        public static List <KnowlegeBaseSARules> SortRules(this List <KnowlegeBaseSARules> Source, SAFuzzySystem Approx)
        {
            return Source.ToArray().SortRules(Approx).ToList(); 
        }

        public static   KnowlegeBaseSARules[] SelectBest(this KnowlegeBaseSARules[] Source, SAFuzzySystem Approx, int CountBest)
        {
            KnowlegeBaseSARules[] result = new KnowlegeBaseSARules[CountBest];
            Source.SortRules(Approx).ToList().CopyTo(0,result,0,CountBest);
            return result;
        }

        public static List<KnowlegeBaseSARules> SelectBest(this List<KnowlegeBaseSARules> Source, SAFuzzySystem Approx, int CountBest)
        {
           KnowlegeBaseSARules[] result = new KnowlegeBaseSARules[CountBest];
           Source.ToArray().SortRules(Approx).ToList().CopyTo(0, result, 0, CountBest);
         return result.ToList();
        }

        public static void Inject(this List<KnowlegeBaseSARules> Destination, int indexStartDestination,  List<KnowlegeBaseSARules> Source, int indexStartSource, int CountIjected, SAFuzzySystem Approx )
        {
            Destination = Destination.SortRules(Approx);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.Clone());
        }

        public static void Inject(this List<KnowlegeBaseSARules> Destination, int indexStartDestination, KnowlegeBaseSARules[] Source, int indexStartSource, int CountIjected, SAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.ToArray().Clone() as KnowlegeBaseSARules []);
        }

        public static void Inject(this KnowlegeBaseSARules [] Destination, int indexStartDestination,  List<KnowlegeBaseSARules> Source, int indexStartSource, int CountIjected, SAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBaseSARules (Source[i + indexStartSource]);
            }
        }

        public static void Inject(this KnowlegeBaseSARules[] Destination, int indexStartDestination, KnowlegeBaseSARules[] Source, int indexStartSource, int CountIjected, SAFuzzySystem Approx)
        {
            Destination = Destination.SortRules(Approx);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBaseSARules(Source[i + indexStartSource]);
            }
        }
    }
}
