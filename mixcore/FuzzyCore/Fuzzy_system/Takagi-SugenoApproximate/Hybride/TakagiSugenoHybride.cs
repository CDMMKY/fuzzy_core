using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.TakagiSugenoApproximate.Hybride
{

     public class TakagiSugenoHybride : FuzzyHybrideBase
     {
         protected List<TakagiSugenoElementofStorage> StorageOfSolutions;
         TSAFuzzySystem Checker;
         doubleReverse ReverseSorter = new doubleReverse();
         ElemSorter ElemSort = new ElemSorter();
         Random rand = new Random();
         protected List<KnowlegeBaseTSARules> ElemToKnowledge(List<TakagiSugenoElementofStorage> Source)
         {
             List<KnowlegeBaseTSARules> Result = new List<KnowlegeBaseTSARules>();
             for (int i = 0; i < Source.Count; i++)
             {
                 Result.Add(new KnowlegeBaseTSARules(Source[i].Element));
             }
             return Result;
         }

         public void Store(List<KnowlegeBaseTSARules> Source, string AlgName)
         {

             if (StorageOfSolutions == null) { StorageOfSolutions = new List<TakagiSugenoElementofStorage>(); }
             lock (StorageOfSolutions)
             {
                 for (int i = 0; i < Source.Count; i++)
                 {
                     StorageOfSolutions.Add(new TakagiSugenoElementofStorage(Checker, Source[i], AlgName));
                 }

             }

         }

         public List<KnowlegeBaseTSARules> Get(int countForeings, goodness typeOfGoodness, islandStrategy typeofIslandStrategy, string nameofPair = "")
         {
             if (StorageOfSolutions != null)
             {
                 int returned = countForeings > StorageOfSolutions.Count ? StorageOfSolutions.Count : countForeings;
                 lock (StorageOfSolutions)
                 {
                     switch (typeOfGoodness)
                     {
                         case goodness.best:
                             {
                                 switch (typeofIslandStrategy)
                                 {
                                     case islandStrategy.All: { return GetByBestAll(countForeings); }
                                     case islandStrategy.One: { return GetByBestOne(countForeings, nameofPair); }
                                 }
                             } break;
                         case goodness.random:
                             {
                                 switch (typeofIslandStrategy)
                                 {
                                     case islandStrategy.All: { return GetByRandomAll(countForeings); }
                                     case islandStrategy.One: { return GetByRandomOne(countForeings, nameofPair); }
                                 }

                             } break;
                     }
                 }
             }
             return GetByBestAll(countForeings);
         }

         protected List<KnowlegeBaseTSARules> GetByBestAll(int countForeings)
         {
             List<KnowlegeBaseTSARules> Result = new List<KnowlegeBaseTSARules>();
             if (StorageOfSolutions != null)
             {
                 int returned = countForeings > StorageOfSolutions.Count ? StorageOfSolutions.Count : countForeings;

                 List<TakagiSugenoElementofStorage> tempRes = StorageOfSolutions.GetRange(0, returned);
                 tempRes.Sort(ElemSort);
                 Result = ElemToKnowledge(tempRes);
             }
             return Result;
         }

         protected List<KnowlegeBaseTSARules> GetByBestOne(int countForeings, string nameAlg)
         {
             List<TakagiSugenoElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();
             int returned = countForeings > temp.Count ? temp.Count : countForeings;
             return ElemToKnowledge(temp.GetRange(0, returned));
         }

         protected List<KnowlegeBaseTSARules> GetByRandomAll(int countForeings)
         {
             int returned = countForeings > StorageOfSolutions.Count ? StorageOfSolutions.Count : countForeings;
             if (returned == StorageOfSolutions.Count)
             {
                 return ElemToKnowledge(StorageOfSolutions.ToList());
             }
             HashSet<int> numbers = new HashSet<int>();
             while (numbers.Count < returned)
             {
                 numbers.Add(rand.Next(StorageOfSolutions.Count));
             }
             List<KnowlegeBaseTSARules> Result = new List<KnowlegeBaseTSARules>();
             foreach (int i in numbers)
             {
                 Result.Add(new KnowlegeBaseTSARules(StorageOfSolutions[i].Element));
             }
             return Result;
         }

         protected List<KnowlegeBaseTSARules> GetByRandomOne(int countForeings, string nameAlg)
         {
             List<TakagiSugenoElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();
             int returned = countForeings > temp.Count ? temp.Count : countForeings;
             if (returned == temp.Count)
             {
                 return ElemToKnowledge(temp);
             }
             HashSet<int> numbers = new HashSet<int>();
             while (numbers.Count < returned)
             {
                 numbers.Add(rand.Next(temp.Count));
             }

             List<KnowlegeBaseTSARules> Result = new List<KnowlegeBaseTSARules>();
             foreach (int i in numbers)
             {
                 Result.Add(new KnowlegeBaseTSARules(temp[i].Element));
             }
             return Result;
         }

         public sealed class doubleReverse : IComparer<double>
         {
             Comparer<double> noReverse = Comparer<double>.Default;
             int IComparer<double>.Compare(double x, double y)
             {
                 return noReverse.Compare(y, x);
             }
         }

         public TakagiSugenoHybride(TSAFuzzySystem Source)
         {
             Checker = Source;
         }

         public sealed class ElemSorter : IComparer<TakagiSugenoElementofStorage>
         {
             Comparer<double> noReverse = Comparer<double>.Default;
             int IComparer<TakagiSugenoElementofStorage>.Compare(TakagiSugenoElementofStorage x, TakagiSugenoElementofStorage y)
             {
                 return noReverse.Compare(x.LearnError, y.LearnError);
             }
         }
     }
}
