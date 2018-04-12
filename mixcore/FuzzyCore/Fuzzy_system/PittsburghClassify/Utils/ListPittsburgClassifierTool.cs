using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.PittsburghClassifier;

namespace FuzzyCoreUtils
{
    static partial class Extensions
    {
        public static List<KnowlegeBasePCRules> Clone(this IList<KnowlegeBasePCRules> listToClone)
        {
            return listToClone.Select(item => (new KnowlegeBasePCRules (item))).ToList();
        }
    }
    public static class ListPittsburgClassifierTool
    {

        public static KnowlegeBasePCRules[] SortRules(this KnowlegeBasePCRules[] Source, PCFuzzySystem Classifier)
       {
            double[] keys = new double[Source.Count()];
            KnowlegeBasePCRules[] tempSol = Source.Clone() as KnowlegeBasePCRules[];
            for (int i = 0; i < Source.Count(); i++)
            {
              keys[i] = Classifier.ErrorLearnSamples(Source[i]);
            }
            Array.Sort(keys, tempSol);
       return tempSol;
       }

        public static List <KnowlegeBasePCRules> SortRules(this List <KnowlegeBasePCRules> Source, PCFuzzySystem Classifier)
        {
            return Source.ToArray().SortRules(Classifier).ToList(); 
        }


        public static   KnowlegeBasePCRules[] SelectBest(this KnowlegeBasePCRules[] Source, PCFuzzySystem Classifier, int CountBest)
        {
            KnowlegeBasePCRules[] result = new KnowlegeBasePCRules[CountBest];
            Source.SortRules(Classifier).ToList().CopyTo(0,result,0,CountBest);
            return result;
        }

        public static List<KnowlegeBasePCRules> SelectBest(this List<KnowlegeBasePCRules> Source, PCFuzzySystem Classifier, int CountBest)
        {
          KnowlegeBasePCRules[] result = new KnowlegeBasePCRules[CountBest];
          Source.ToArray().SortRules(Classifier).ToList().CopyTo(0, result, 0, CountBest);
         return result.ToList();
        }

        public static void Inject(this List<KnowlegeBasePCRules> Destination, int indexStartDestination,  List<KnowlegeBasePCRules> Source, int indexStartSource, int CountIjected, PCFuzzySystem Classifier )
        {
            Destination = Destination.SortRules(Classifier);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.Clone());
        }

        public static void Inject(this List<KnowlegeBasePCRules> Destination, int indexStartDestination, KnowlegeBasePCRules[] Source, int indexStartSource, int CountIjected, PCFuzzySystem Classifier)
        {
            Destination = Destination.SortRules(Classifier);
            Destination.RemoveRange(indexStartDestination, CountIjected);
            Destination.AddRange(Source.ToArray().Clone() as KnowlegeBasePCRules []);
        }


        public static void Inject(this KnowlegeBasePCRules [] Destination, int indexStartDestination,  List<KnowlegeBasePCRules> Source, int indexStartSource, int CountIjected, PCFuzzySystem Classifier)
        {
            Destination = Destination.SortRules(Classifier);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBasePCRules (Source[i + indexStartSource]);
            }
        }

        public static void Inject(this KnowlegeBasePCRules[] Destination, int indexStartDestination, KnowlegeBasePCRules[] Source, int indexStartSource, int CountIjected, PCFuzzySystem Classifier)
        {
            Destination = Destination.SortRules(Classifier);
            for (int i = 0; i < CountIjected; i++)
            {
                Destination[i + indexStartDestination] = new KnowlegeBasePCRules(Source[i + indexStartSource]);
            }
        }

    }
}
