using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;
namespace RunInit
{
    abstract class Base_for_Approx_learn : Base_for_Approx
    {
        protected string file_in;

        protected AbstractNotSafeLearnAlgorithm optimaze;
        protected ILearnAlgorithmConf conf;

    }
}
