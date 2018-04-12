using System;
using System.Linq;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract;
using FuzzyCore.FuzzySystem.FuzzyAbstract;


namespace RunInit
{
    class Wrapper99 : Base_for_Approx_learn
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

            Approx_learn_set = BaseUFSLoader.LoadLearnFromUFS(file_in);
            Console.WriteLine("Tra load");

            Approx_test_set = BaseUFSLoader.LoadTestFromUFS(file_in);
            Console.WriteLine("Tst load");
            conf = new PSOBacterySearchConf();
            conf.Init(Approx_learn_set.CountVars);
            conf.loadParams(confParams);
            Console.WriteLine("Conf Filed");

            Approx_Singletone = new SAFuzzySystem(Approx_learn_set, Approx_test_set);
            Approx_Singletone = SAFSUFSLoader.loadUFS(Approx_Singletone, file_in);

            Console.WriteLine("Classifier created");
            optimaze = new PSOMethods.Approx.Term_config_PSO_Bactery();
            Approx_Singletone = optimaze.TuneUpFuzzySystem(Approx_Singletone, conf);
            Console.WriteLine("Optimization complite");
            SAFSUFSWriter.saveToUFS(Approx_Singletone, file_out);
            Console.WriteLine("Saved");
            return 1;
        }



    }
}
