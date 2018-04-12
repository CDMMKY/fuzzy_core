using System;
using System.Linq;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm;
using FuzzySystem.FuzzyAbstract;
using FuzzyCore.FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    class Wrapper999 : Base_for_Class_learn
    {
        protected TypeTermFuncEnum func = 0;
   

        protected override void fill_conf()
        { }
        protected override void fill_params(string[] args)
        {
            file_in = (args.Where(x => x.Contains("in"))).ToArray()[0];
            Console.WriteLine("Before cut {0}", file_in);
            file_in = file_in.Remove(0, 3);
            Console.WriteLine("After cut {0}", file_in);
            file_out = (args.Where(x => x.Contains("out"))).ToArray()[0];
            file_out = file_out.Remove(0, 4);
            fill_conf();
            toStringParams(args);


        }

    
     


        public override int Run(string[] args)
        {

            Console.WriteLine("Start");
            fill_params(args);
            Console.WriteLine("Params get \nfile in {0} \n", file_in);

            Class_learn_set = BaseUFSLoader.LoadLearnFromUFS(file_in);
            Console.WriteLine("Tra load");

            Class_test_set = BaseUFSLoader.LoadTestFromUFS(file_in);
            Console.WriteLine("Tst load");
            conf = new PSOBacterySearchConf();
            conf.Init(Class_learn_set.CountVars);
            conf.loadParams(confParams);
            Console.WriteLine("Conf Filed");

            Class_Pittsburg = new PCFuzzySystem(Class_learn_set, Class_test_set);
            Class_Pittsburg = PCFSUFSLoader.loadUFS(Class_Pittsburg, file_in);

            Console.WriteLine("Classifier created");
            optimaze = new Term_config_PSO_Bactery();
            Class_Pittsburg = optimaze.TuneUpFuzzySystem(Class_Pittsburg, conf);
            Console.WriteLine("Optimization complite");
            PCFSUFSWriter.saveToUFS(Class_Pittsburg, file_out);
            Console.WriteLine("Saved");
            return 1;
        }



    }
}



