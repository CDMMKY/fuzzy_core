using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    abstract class Base_for_Approx : Base
    {
        protected SampleSet Approx_learn_set = null;
        protected SampleSet Approx_test_set = null;
        protected SAFuzzySystem Approx_Singletone = null;



    }
}
