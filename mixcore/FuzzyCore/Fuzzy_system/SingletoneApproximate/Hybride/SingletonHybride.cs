using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.SingletoneApproximate.Hybride
{
    public class SingletonHybride : FuzzyHybrideBase
    {
        protected List<SingletonElementofStorage> StorageOfSolutions;
        SAFuzzySystem Checker;
        doubleReverse ReverseSorter = new doubleReverse();
        ElemSorter ElemSort = new ElemSorter();
        Random rand = new Random();
        protected List<KnowlegeBaseSARules> ElemToKnowledge(List<SingletonElementofStorage> Source)
        {
            List<KnowlegeBaseSARules> Result = new List<KnowlegeBaseSARules>();
            for (int i = 0; i < Source.Count; i++)
            {
                Result.Add(new KnowlegeBaseSARules(Source[i].Element));
            }
            return Result;
        }

        public void Store(List<KnowlegeBaseSARules> Source, string AlgName)
        {

            if (StorageOfSolutions == null) { StorageOfSolutions = new List<SingletonElementofStorage>(); }
            lock (StorageOfSolutions)
            {
                for (int i = 0; i < Source.Count; i++)
                {
                    StorageOfSolutions.Add(new SingletonElementofStorage(Checker, Source[i], AlgName));
                }
            }
        }

        public List<KnowlegeBaseSARules> Get(int countForeings, goodness typeOfGoodness, islandStrategy typeofIslandStrategy, string nameofPair = "")
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

        protected List<KnowlegeBaseSARules> GetByBestAll(int countForeings)
        {
            List<KnowlegeBaseSARules> Result = new List<KnowlegeBaseSARules>();
            if (StorageOfSolutions != null)
            {
                int returned = countForeings > StorageOfSolutions.Count ? StorageOfSolutions.Count : countForeings;
                List<SingletonElementofStorage> tempRes = StorageOfSolutions.GetRange(0, returned);
                tempRes.Sort(ElemSort);
                Result = ElemToKnowledge(tempRes);
            }
            return Result;
        }

        protected List<KnowlegeBaseSARules> GetByBestOne(int countForeings, string nameAlg)
        {
            List<SingletonElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();
            int returned = countForeings > temp.Count ? temp.Count : countForeings;
            return ElemToKnowledge(temp.GetRange(0, returned));
        }

        protected List<KnowlegeBaseSARules> GetByRandomAll(int countForeings)
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
            List<KnowlegeBaseSARules> Result = new List<KnowlegeBaseSARules>();
            foreach (int i in numbers)
            {
                Result.Add(new KnowlegeBaseSARules(StorageOfSolutions[i].Element));
            }
            return Result;
        }

        protected List<KnowlegeBaseSARules> GetByRandomOne(int countForeings, string nameAlg)
        {
            List<SingletonElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();
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
            List<KnowlegeBaseSARules> Result = new List<KnowlegeBaseSARules>();
            foreach (int i in numbers)
            {
                Result.Add(new KnowlegeBaseSARules(temp[i].Element));
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

        public SingletonHybride(SAFuzzySystem Source)
        {
            Checker = Source;
        }

        public sealed class ElemSorter : IComparer<SingletonElementofStorage>
        {
            Comparer<double> noReverse = Comparer<double>.Default;
            int IComparer<SingletonElementofStorage>.Compare(SingletonElementofStorage x, SingletonElementofStorage y)
            {
                return noReverse.Compare(x.LearnError, y.LearnError);
            }
        }

    }
}
