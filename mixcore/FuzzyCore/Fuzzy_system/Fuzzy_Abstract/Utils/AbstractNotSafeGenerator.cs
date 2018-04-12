using FuzzySystem.FuzzyAbstract.conf;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;

namespace FuzzySystem.FuzzyAbstract
{
    public class AbstractNotSafeGenerator : IAbstractGenerator
    {

        IFuzzySystem IAbstractGenerator.Generate(IFuzzySystem FuzzySystem, conf.IGeneratorConf config)
        {
            if (FuzzySystem is PittsburghClassifier.PCFuzzySystem)
            {   return Generate(FuzzySystem as PittsburghClassifier.PCFuzzySystem, config);
            }

            if (FuzzySystem is TakagiSugenoApproximate.TSAFuzzySystem)
            {
                return Generate(FuzzySystem as TakagiSugenoApproximate.TSAFuzzySystem, config);
            }

            if (FuzzySystem is SingletoneApproximate.SAFuzzySystem)
            {
                return Generate(FuzzySystem as SingletoneApproximate.SAFuzzySystem, config);
            }

            throw new NotImplementedException("Неизвестный вид нечеткой системы");
        }

        public virtual PittsburghClassifier.PCFuzzySystem Generate(PittsburghClassifier.PCFuzzySystem Classifier, conf.IGeneratorConf config)
        {
            throw new NotImplementedException("Алгоритм не реализован для нечеткой системы типа питтсбургский классиификатор");
        }

        public virtual SingletoneApproximate.SAFuzzySystem Generate(SingletoneApproximate.SAFuzzySystem Approximate, conf.IGeneratorConf config)
        {
            throw new NotImplementedException("Алгоритм не реализован для нечеткой системы синглтон");
        }

        public virtual TakagiSugenoApproximate.TSAFuzzySystem Generate(TakagiSugenoApproximate.TSAFuzzySystem Approximate, conf.IGeneratorConf config)
        {
            throw new NotImplementedException("Алгоритм не реализован для нечеткой системы Такаги Сугено");
        }

        public virtual string ToString(bool withParam = false)
        {
            throw new NotImplementedException("Алгоритму не задано имя");
        }



        public virtual IGeneratorConf getConf(int CountFeatures)
        {
            IGeneratorConf conf = new NullConfForAll();
            conf.Init(0);
            return conf;
        }

        public virtual List<FuzzySystemRelisedList.TypeSystem> SupportedFS { get { return new List<FuzzySystemRelisedList.TypeSystem>(); } }



    }
}
