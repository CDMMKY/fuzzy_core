using System;
using System.Linq;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.PittsburghClassifier.add_generators;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
namespace RunInit
{
    class Wrapper36:Base_Class_init
    {
        protected TypeTermFuncEnum func = 0;

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

            string temp = (args.Where(x => x.Contains("typeFunc"))).ToArray()[0];
            switch (temp)
            {
                case "Triangle": func = TypeTermFuncEnum.Треугольник; break;
                case "Gauss": func = TypeTermFuncEnum.Гауссоида; break;
                case "Parabola": func = TypeTermFuncEnum.Парабола; break;
                case "Trapezium": func = TypeTermFuncEnum.Трапеция; break;
                default: func = TypeTermFuncEnum.Треугольник; break;
            }
        }


        protected override void fill_conf()
        {
           // InitEveryoneWithEveryone conf1 = conf as InitEveryoneWithEveryone;
           // conf1.IEWE_FuncType = func;
           // int[] param = new int[Approx_learn_set.CountVars];
           // for (int i = 0; i < Approx_learn_set.CountVars; i++)
           // {
            //    param[i] = CountN[i];
           // }
        }

        public override int Run(string[] args)
        {

            Console.WriteLine("Start");
            fill_params(args);
            Console.WriteLine("Params get \nfile tra {0} \nfile name tst {1} ", file_learn, file_test);
            Class_learn_set = new SampleSet(file_learn);
            Class_learn_set = new SampleSet(file_learn);
            Console.WriteLine("Tra create");
            Class_test_set = new SampleSet(file_test);
            Console.WriteLine("Tst create");
            conf = new InitBySamplesConfig();
            conf.Init(Class_learn_set.CountVars);

            fill_conf();
            Console.WriteLine("Conf Filed");
            Class_Pittsburg = new PCFuzzySystem(Class_learn_set, Class_test_set);
            Console.WriteLine("Classifier created");
            generator = new GeneratorRulesBySamples();
           
            Class_Pittsburg = generator.Generate(Class_Pittsburg , conf);
   //         GeneratorRulesBySamples.InitRulesBySamples(Class_Pittsburg, func);
        //    SingletoneApproximate = generator.Generate(SingletoneApproximate, conf);
            Console.WriteLine("Gereration complite");

            PCFSUFSWriter.saveToUFS(Class_Pittsburg, file_out);
            Console.WriteLine("Saved");
            return 1;
        }

    }
}
