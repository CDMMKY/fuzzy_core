using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.Hybride;


namespace FuzzySystem.PittsburghClassifier.Hybride
{
    public class PittsburgHybride : FuzzyHybrideBase
    {
        protected List<PittsburgElementofStorage> StorageOfSolutions = null;
        PCFuzzySystem Checker;
        doubleReverse ReverseSorter = new doubleReverse();
        ElemSorter ElemSort = new ElemSorter();
        Random rand = new Random();
        protected List<KnowlegeBasePCRules> ElemToKnowledge(List<PittsburgElementofStorage> Source)
        {
            List<KnowlegeBasePCRules> Result = new List<KnowlegeBasePCRules>();
            for (int i = 0; i < Source.Count; i++)
            {
                Result.Add(new KnowlegeBasePCRules(Source[i].Element));
            }
            return Result;
        }

        public void Store(List<KnowlegeBasePCRules> Source, string AlgName)
        {

            if (StorageOfSolutions == null) { StorageOfSolutions = new List<PittsburgElementofStorage>(); }
            lock (StorageOfSolutions)
            {
                for (int i = 0; i < Source.Count; i++)
                {
                    StorageOfSolutions.Add(new PittsburgElementofStorage(Checker, Source[i], AlgName));
                }
            }
        }

        public List<KnowlegeBasePCRules> Get(int countForeings, goodness typeOfGoodness, islandStrategy typeofIslandStrategy, string nameofPair = "")
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

        protected List<KnowlegeBasePCRules> GetByBestAll(int countForeings)
        {
            List<KnowlegeBasePCRules> Result = new List<KnowlegeBasePCRules>();
            if (StorageOfSolutions != null)
            {
                int returned = countForeings > StorageOfSolutions.Count ? StorageOfSolutions.Count : countForeings;
                List<PittsburgElementofStorage> tempRes = StorageOfSolutions.GetRange(0, returned);
                tempRes.Sort(ElemSort);
                Result = ElemToKnowledge(tempRes);

            }
            return Result;
        }

        protected List<KnowlegeBasePCRules> GetByBestOne(int countForeings, string nameAlg)
        {
            List<PittsburgElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();
            int returned = countForeings > temp.Count ? temp.Count : countForeings;
            return ElemToKnowledge(temp.GetRange(0, returned));
        }

        protected List<KnowlegeBasePCRules> GetByRandomAll(int countForeings)
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

            List<KnowlegeBasePCRules> Result = new List<KnowlegeBasePCRules>();
            foreach (int i in numbers)
            {
                Result.Add(new KnowlegeBasePCRules(StorageOfSolutions[i].Element));
            }
            return Result;
        }

        protected List<KnowlegeBasePCRules> GetByRandomOne(int countForeings, string nameAlg)
        {
            List<PittsburgElementofStorage> temp = StorageOfSolutions.Where(x => x.AlgName.Equals(nameAlg)).ToList();

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

            List<KnowlegeBasePCRules> Result = new List<KnowlegeBasePCRules>();
            foreach (int i in numbers)
            {
                Result.Add(new KnowlegeBasePCRules(temp[i].Element));
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

        public PittsburgHybride(PCFuzzySystem Source)
        {
            Checker = Source;
        }

        public sealed class ElemSorter : IComparer<PittsburgElementofStorage>
        {
            Comparer<double> noReverse = Comparer<double>.Default;
             int IComparer<PittsburgElementofStorage>.Compare(PittsburgElementofStorage x, PittsburgElementofStorage y)
            {
                return noReverse.Compare(x.LearnError, y.LearnError);
            }
        }

    }
}
