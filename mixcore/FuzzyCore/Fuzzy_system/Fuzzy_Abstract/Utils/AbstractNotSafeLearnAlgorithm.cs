using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.TakagiSugenoApproximate;
using System;
using System.Collections.Generic;


namespace FuzzySystem.FuzzyAbstract
{
   public  class AbstractNotSafeLearnAlgorithm:FuzzySystem.FuzzyAbstract.IAbstractLearnAlgorithm
   {

      IFuzzySystem IAbstractLearnAlgorithm.TuneUpFuzzySystem(IFuzzySystem FuzzySystem, ILearnAlgorithmConf conf)
        {
       
            if (FuzzySystem is PCFuzzySystem)
            {
                if (((PCFuzzySystem)FuzzySystem).RulesDatabaseSet.Count < 1) throw new ArgumentException("В нечеткой системе нет правил");
                return TuneUpFuzzySystem(FuzzySystem as PCFuzzySystem, conf); }

            if (FuzzySystem is TSAFuzzySystem)
            {
                if (((TSAFuzzySystem)FuzzySystem).RulesDatabaseSet.Count < 1) throw new ArgumentException("В нечеткой системе нет правил");

                return TuneUpFuzzySystem(FuzzySystem as TSAFuzzySystem, conf); }

            if (FuzzySystem is SAFuzzySystem)
            {
                if (((SAFuzzySystem)FuzzySystem).RulesDatabaseSet.Count < 1) throw new ArgumentException("В нечеткой системе нет правил");

                return TuneUpFuzzySystem(FuzzySystem as SAFuzzySystem, conf); }

            throw new  NotImplementedException("Нечеткая система неизвестного типа");
        }

             public virtual SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem System, ILearnAlgorithmConf conf)
        {
            throw new  NotImplementedException("Алгоритм не реализован для нечеткой системы синглтон");
             }


             public virtual PCFuzzySystem TuneUpFuzzySystem(PCFuzzySystem System, ILearnAlgorithmConf conf)
             {
                 throw new NotImplementedException("Алгоритм не реализован для нечеткой системы типа питтсбургский классификатор");
             }

             public virtual TSAFuzzySystem TuneUpFuzzySystem(TSAFuzzySystem System, ILearnAlgorithmConf conf)
             {
                 throw new NotImplementedException("Алгоритм не реализован для нечеткой системы Такаги-Сугено");
             }

             public virtual string ToString(bool with_param = false)
             {
                 throw new NotImplementedException("Алгоритму не задано название");
             }

             public virtual ILearnAlgorithmConf getConf(int CountFeatures)
             {
                 ILearnAlgorithmConf conf = new NullConfForAll();
                 conf.Init(CountFeatures);
                 return conf;
             }

     public virtual  List<FuzzySystemRelisedList.TypeSystem> SupportedFS { get { return new List<FuzzySystemRelisedList.TypeSystem>(); } }

    }
}
