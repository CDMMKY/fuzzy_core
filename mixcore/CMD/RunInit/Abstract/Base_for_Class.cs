using FuzzySystem.PittsburghClassifier;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    abstract class Base_for_Class : Base
    {
        protected SampleSet Class_learn_set = null;
        protected SampleSet Class_test_set = null;
        protected PCFuzzySystem Class_Pittsburg = null;


    }
}
