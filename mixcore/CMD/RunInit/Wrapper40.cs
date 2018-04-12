using System;
using System.Linq;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.AddGenerators;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    class Wrapper40:Base_for_ApproxInit
    {
        protected TypeTermFuncEnum func = 0;
       

        protected override void fill_conf()
        { }
        protected override void fill_params(string[] args)
        {
            file_learn = (args.Where(x => x.Contains("tra"))).ToArray()[0];
            Console.WriteLine("Before cut {0}", file_learn);
            file_learn = file_learn.Remove(0, 4);
            Console.WriteLine("After cut {0}", file_learn);
            file_test = (args.Where(x => x.Contains("tst"))).ToArray()[0];
            file_test = file_test.Remove(0, 4);
            file_out = (args.Where(x => x.Contains("out"))).ToArray()[0];
            file_out = file_out.Remove(0, 4);
            fill_conf();
            toStringParams(args);


        }

      

     

   
        public override int Run(string[] args)
        {

            Console.WriteLine("Start");
            fill_params(args);
            Console.WriteLine("Params get \nfile tra {0} \nfile name tst {1} ", file_learn, file_test);
            Approx_learn_set = new SampleSet(file_learn);
            Console.WriteLine("Tra create");
            Approx_test_set = new SampleSet(file_test);
            Console.WriteLine("Tst create");
            conf = new InitEveryoneWithOptimal();
            conf.Init(Approx_learn_set.CountVars);
            conf.loadParams(confParams);
            Console.WriteLine("Conf Filed");
            Approx_Singletone = new SAFuzzySystem(Approx_learn_set, Approx_test_set);
            Console.WriteLine("Classifier created");
            generator = new GeneratorRulesEveryoneWithOptimal();
            Approx_Singletone = generator.Generate(Approx_Singletone, conf) as SAFuzzySystem;
            Console.WriteLine("Gereration complite");
            SAFSUFSWriter.saveToUFS(Approx_Singletone, file_out);
            Console.WriteLine("Saved");
            return 1;
        }

    }
}
