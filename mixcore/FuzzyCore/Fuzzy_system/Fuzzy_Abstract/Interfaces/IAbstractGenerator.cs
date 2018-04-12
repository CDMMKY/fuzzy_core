using FuzzySystem.FuzzyAbstract.conf;
using System.Diagnostics.Contracts;
using static System.Diagnostics.Contracts.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzySystem.FuzzyAbstract
{  public interface IAbstractGenerator:IAlgorithm
    { IFuzzySystem Generate(IFuzzySystem FuzzySystem, IGeneratorConf config);
      IGeneratorConf getConf(int CountFeatures); 
    }

}
