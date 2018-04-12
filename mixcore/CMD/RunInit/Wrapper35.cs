using System;
using System.Linq;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.SingletoneApproximate.AddGenerators;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract;

namespace RunInit
{
    class Wrapper35:Base_for_ApproxInit
    {
        protected TypeTermFuncEnum func = 0;
      
        protected int[] CountN = new int[8];

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
            int temp_i = 0;
            string temp = (args.Where(x => x.Contains("CountN1"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[0] = temp_i;

            temp = (args.Where(x => x.Contains("CountN2"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[1] = temp_i;

            temp = (args.Where(x => x.Contains("CountN3"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[2] = temp_i;

            temp = (args.Where(x => x.Contains("CountN4"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[3] = temp_i;

            temp = (args.Where(x => x.Contains("CountN5"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[4] = temp_i;

            temp = (args.Where(x => x.Contains("CountN6"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[5] = temp_i;

            temp = (args.Where(x => x.Contains("CountN7"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[6] = temp_i;

            temp = (args.Where(x => x.Contains("CountN8"))).ToArray()[0];
            int.TryParse(temp, out temp_i);
            CountN[7] = temp_i;


            temp = (args.Where(x => x.Contains("typeFunc"))).ToArray()[0];
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
            InitEveryoneWithEveryone conf1 = conf as InitEveryoneWithEveryone;  
            conf1.IEWEFuncType = func;
            int[] param = new int[Approx_learn_set.CountVars];
            for (int i = 0; i < Approx_learn_set.CountVars; i++)
            {
                param[i] = CountN[i];
            }
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
            conf = new InitEveryoneWithEveryone();
            conf.Init(Approx_learn_set.CountVars);
            fill_conf();
            Console.WriteLine("Conf Filed");
            Approx_Singletone = new SAFuzzySystem(Approx_learn_set, Approx_test_set);
            Console.WriteLine("Classifier created");
            generator = new GeneratorRulesEveryoneWithEveryone();
            Approx_Singletone = generator.Generate(Approx_Singletone, conf) as SAFuzzySystem;
            Console.WriteLine("Gereration complite");
            SAFSUFSWriter.saveToUFS(Approx_Singletone, file_out);
            Console.WriteLine("Saved");
            return 1;
        }

    }
}
