using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    abstract class Base_for_ApproxInit : Base_for_Approx

    {

        protected string file_learn;
        protected string file_test;

        protected IGeneratorConf conf;
        protected IAbstractGenerator generator;


    }
}
