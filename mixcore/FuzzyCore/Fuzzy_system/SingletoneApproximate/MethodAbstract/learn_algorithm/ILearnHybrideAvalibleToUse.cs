using System.Collections.Generic;
using FuzzySystem.SingletoneApproximate.Hybride;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public interface ILearnHybrideAvalibleToUse:IAbstractLearnAlgorithm
    {
        SAFuzzySystem TuneUpFuzzySystem(SingletonHybride Ocean, SAFuzzySystem Approximate, ILearnAlgorithmConf conf);
        List<KnowlegeBaseSARules> chooseDiscovers(int count);
        void assimilateOutSiders();
        void oneIterate(SAFuzzySystem result);
        void Init(ILearnAlgorithmConf Config);
        void Final();
    }
}
