using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier;

namespace FuzzySystem.FuzzyAbstract.Utils
{
    public enum SortType
    { Нет =0,
        Обучающая = 1,
        Тестовая = 2,
        Признаки = 3
    }

    public class FeatureSelectionModel : EqualityComparer<FeatureSelectionModel>, IComparer<FeatureSelectionModel>
    {
        public static SortType SortWay;
        public double Error { get; private set; }
        public double ErrorTest { get; private set; }
        public double Accuracy { get; private set; }
        public double AccuracyTest { get; private set; }
        public bool[] Features { get; private set; }
        public string Info { get; private set; }

        public FeatureSelectionModel(IFuzzySystem Fsystem, bool[] FeaturesSource)
        {
            bool[] temp = Fsystem.AcceptedFeatures;
            Features = FeaturesSource;
            Fsystem.AcceptedFeatures = Features;
            Error = Fsystem.ErrorLearnSamples(Fsystem.AbstractRulesBase()[0]);
            ErrorTest = Fsystem.ErrorTestSamples(Fsystem.AbstractRulesBase()[0]);
            Fsystem.AcceptedFeatures = temp;
            if (Fsystem is PCFuzzySystem)
            {
                Accuracy = 100.0 - Error;
                AccuracyTest = 100.0 - ErrorTest;
            }
            else
            {
                Error = Fsystem.RMSEtoMSEforLearn(Error);
                ErrorTest = Fsystem.RMSEtoMSEforTest(ErrorTest);
            }
            Info = makeNameFeatures(Fsystem, Features);
        }


        public static string makeNameFeatures(IFuzzySystem FSystem, bool[] Source)
        {
            string temp = String.Empty;
            string temp2 = String.Empty;
            for (int i = 0; i < Source.Count(); i++)
            {
                if (Source[i])
                {
                    temp += FSystem.LearnSamplesSet.InputAttributes[i].Name + ", ";
                    temp2 += $"{i + 1}, ";
                }
            }
            return temp + Environment.NewLine + temp2;
        }

        public static List<FeatureSelectionModel> Distinct(List<FeatureSelectionModel> Source)
        {
            List<FeatureSelectionModel> result = new List<FeatureSelectionModel>(Source.Count);
            result = Source.Distinct(Source[0]).ToList();
            return result;
        }

        public static void Sort(List<FeatureSelectionModel> Source, SortType SortWay)
        { if (SortWay!= SortType.Нет) { 
            FeatureSelectionModel.SortWay = SortWay;
            Source.Sort(Source[0]);
            }
        }


        public override bool Equals(FeatureSelectionModel x, FeatureSelectionModel y)
        {
            if (x.Features.Length != x.Features.Length) return false;
            for (int i = 0; i < x.Features.Length; i++)
            {
                if (x.Features[i] != y.Features[i]) return false;
            }
            return true;
        }

        public int Compare(FeatureSelectionModel x, FeatureSelectionModel y)
        {
            switch (FeatureSelectionModel.SortWay)
            {

                case SortType.Обучающая:
                    if (x.Error < y.Error) { return 1; }
                    if (x.Error > y.Error) { return -1; }
                    break;
                case SortType.Тестовая:
                    if (x.ErrorTest < y.ErrorTest) { return 1; }
                    if (x.ErrorTest > y.ErrorTest) { return -1; }
                    break;
                case SortType.Признаки:
                    if (x.Features.Count(z=>z) > y.Features.Count(z => z)) return 1;
                    if (x.Features.Count(z => z) < y.Features.Count(z => z)) return -1;
                    break;
                default: return 0;
            }
            return 0;
        }


        public static string getFullInfo(List<FeatureSelectionModel> Source, bool isClass)
        {
            string result = String.Empty;
            for (int i = 0; i < Source.Count; i++)
            {
                result += "Входные признаки{" + Source[i].Features.Count(z => z) + "} = " + Source[i].Info + Environment.NewLine;
                if (isClass)
                {
                    result += "Точность на обучающей выборке = " + Source[i].Accuracy + Environment.NewLine;
                    result += "Точность на тестовой выборке = " + Source[i].AccuracyTest + Environment.NewLine;
                }
                result += "Ошибка на обучающей выборке = " + Source[i].Error + Environment.NewLine;
                result += "Ошибка на тестовой выборке = " + Source[i].ErrorTest + Environment.NewLine;
            }
            return result;
        }

        public override int GetHashCode(FeatureSelectionModel obj)
        {
            int result = obj.Features.Length;
            for (int i = 0; i < obj.Features.Length; i++)
            {
                result += obj.Features[i] ? result ^ obj.Features.Length : 100000;
            }
            return result;
        }

    }
}
