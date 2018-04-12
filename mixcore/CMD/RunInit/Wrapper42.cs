/*
using FuzzySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.PittsburghClassifier.LearnAlgorithm.Bee;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.FuzzyAbstract;
using FuzzyCore.FuzzySystem.FuzzyAbstract;


namespace RunInit
{
    class Wrapper42 : Base_for_Class_learn
    {

        protected TypeTermFuncEnum func = 0;
        string confParams = "";

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
            conf = new BeeStructureConf();
            conf.Init(Class_learn_set.CountVars);
            conf.loadParams(confParams);
            Console.WriteLine("Conf Filed");
            Class_Pittsburg = new PCFuzzySystem(Class_learn_set, Class_test_set);
            Class_Pittsburg = PCFSUFSLoader.loadUFS(Class_Pittsburg, file_in);

            Console.WriteLine("Classifier created");
            optimaze = new BeeStructureAlgorithm();
            Class_Pittsburg = optimaze.TuneUpFuzzySystem(Class_Pittsburg, conf);
            Console.WriteLine("Optimization complite");
            PCFSUFSWriter.saveToUFS(Class_Pittsburg, file_out);
            Console.WriteLine("Saved");
            return 1;
        }
    }
}

*/