using System.Collections.Generic;

using FuzzySystem.PittsburghClassifier.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public interface ILearnHybrideAvalibleToUse : IAbstractLearnAlgorithm
    {

        PCFuzzySystem TuneUpFuzzySystem(PittsburgHybride Ocean, PCFuzzySystem Approximate, ILearnAlgorithmConf conf);
        List<KnowlegeBasePCRules> chooseDiscovers(int count);
        void assimilateOutSiders();
        void oneIterate(PCFuzzySystem result);
        void Init(ILearnAlgorithmConf Config);
        void Final();

    
    }
}
