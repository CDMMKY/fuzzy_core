using FuzzySystem.FuzzyAbstract;
using System.Collections.Generic;

namespace FuzzySystem.TakagiSugenoApproximate.AddGenerators
{
    public  class test : AbstractNotSafeGenerator
    {
       public override TSAFuzzySystem Generate(TSAFuzzySystem Approximate, FuzzyAbstract.conf.IGeneratorConf config)
       {
           return base.Generate(Approximate, config);
       }
       public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
       {
           get
           {

               return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate };
           }
       }

       public override string ToString(bool withParam = false)
       {
           if (withParam)
           { }

           return "Тестоваый 1 ";
       }
       public override FuzzyAbstract.conf.IGeneratorConf getConf(int CountFeatures)
       {
           return base.getConf(CountFeatures);
       }
    }
}
