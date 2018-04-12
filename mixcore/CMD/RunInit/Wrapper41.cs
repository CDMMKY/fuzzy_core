using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.add_generators;
using FuzzySystem.PittsburghClassifier.UFS;
using System;
using System.Linq;

namespace RunInit
{
    class Wrapper41:Base_Class_init
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
            Class_learn_set = new SampleSet(file_learn);
            Console.WriteLine("Tra create");
            Class_test_set = new SampleSet(file_test);
            Console.WriteLine("Tst create");
            conf = new InitEveryoneWithOptimal();
            conf.Init(Class_learn_set.CountVars);
            conf.loadParams(confParams);
            Console.WriteLine("Conf Filed");
            Class_Pittsburg = new PCFuzzySystem(Class_learn_set, Class_test_set);
            Console.WriteLine("Classifier created");
            generator = new GeneratorRulesEveryoneWithOptimal();
            Class_Pittsburg = generator.Generate(Class_Pittsburg, conf) as PCFuzzySystem;
            Console.WriteLine("Gereration complite");
            PCFSUFSWriter.saveToUFS(Class_Pittsburg, file_out);
            Console.WriteLine("Saved");
            return 1;
        }



    }
}
