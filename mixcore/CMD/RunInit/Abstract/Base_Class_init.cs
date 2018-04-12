using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    abstract class Base_Class_init : Base_for_Class
    {

        protected string file_learn;
        protected string file_test;

        protected IGeneratorConf conf;
        protected AbstractNotSafeGenerator generator;

    }
}
