using FuzzySystem.FuzzyAbstract.conf;

namespace FuzzySystem.FuzzyAbstract
{
    public interface IAbstractLearnAlgorithm:IAlgorithm
    {
     IFuzzySystem TuneUpFuzzySystem(IFuzzySystem Approximate, ILearnAlgorithmConf conf);
     ILearnAlgorithmConf getConf(int CountFeatures);
    }
}
