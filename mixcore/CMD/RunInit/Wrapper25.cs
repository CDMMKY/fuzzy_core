using System;
using System.Linq;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using FuzzyCore.FuzzySystem.FuzzyAbstract;


namespace RunInit
{
    class Wrapper25:Base_for_Approx_learn
    { 
        FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init type_init;
        FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate type_mutate;
        FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover type_cross;
        double pcross;
        double cpcross;
        double angleR;
        double iterate;
        double cindiv;
        double cchild;
        double coeft1;
        double coeft2;

        protected override void fill_params(string[] args)
        {
            file_in = (args.Where(x => x.Contains("in"))).ToArray()[0];
            Console.WriteLine("Before cut {0}", file_in);
            file_in = file_in.Remove(0, 3);
            Console.WriteLine("After cut {0}", file_in);
            file_out = (args.Where(x => x.Contains("out"))).ToArray()[0];
            file_out = file_out.Remove(0, 4);


            string temp = (args.Where(x => x.Contains("init_alg"))).ToArray()[0];
            temp = temp.Split(':')[1];

            switch (temp)
            {
                case "Random": type_init = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init.Случайная; break;
                case "Constrain": type_init = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init.Ограниченная; break;
                default: type_init = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_init.Ограниченная; break;
            }



            temp = (args.Where(x => x.Contains("type_mutate"))).ToArray()[0];
            temp = temp.Split(':')[1];

            switch (temp)
            {
                case "Adaptive": type_mutate = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate.СКО; break;
                case "CovarianceMatrix": type_mutate = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate.СКО_РО; break;
                default: type_mutate = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Type_Mutate.СКО_РО; break;
            }

            temp = (args.Where(x => x.Contains("type_cross"))).ToArray()[0];
            temp = temp.Split(':')[1];

            switch (temp)
            {
                case "Unified": type_cross =  FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Унифицированный; break;
                case "Multipoint": type_cross = FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Многоточечный; break;
                default: type_cross =FuzzySystem.FuzzyAbstract.learn_algorithm.conf.ESConfig.Alg_crossover.Унифицированный; break;
            }


            temp = (args.Where(x => x.Contains("pcross"))).ToArray()[0];
            temp = temp.Split(':')[1];
            string comma = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;         
            
            temp = temp.Replace(".", comma);
            double.TryParse(temp, out pcross);

            temp = (args.Where(x => x.Contains("cpcross"))).ToArray()[0];
            temp = temp.Split(':')[1];

            temp = temp.Replace(".", comma);
            double.TryParse(temp, out cpcross);

            temp = (args.Where(x => x.Contains("angleR"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp= temp.Replace(".", comma);
            double.TryParse(temp, out angleR);

            temp = (args.Where(x => x.Contains("iterate"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp = temp.Replace(".", comma);
            double.TryParse(temp, out iterate);

            temp = (args.Where(x => x.Contains("cindiv"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp = temp.Replace(".", comma);
            double.TryParse(temp, out cindiv);

            temp = (args.Where(x => x.Contains("cchild"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp = temp.Replace(".", comma);
            double.TryParse(temp, out cchild);


            temp = (args.Where(x => x.Contains("coeft1"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp = temp.Replace(".",comma);
            double.TryParse(temp, out coeft1);

            temp = (args.Where(x => x.Contains("coeft2"))).ToArray()[0];
            temp = temp.Split(':')[1];
            temp = temp.Replace(".",comma);
            double.TryParse(temp, out coeft2);


        }


        protected override void fill_conf()
        {
            ESConfig conf1 = conf as ESConfig;
            conf1.ESCInitType = type_init;
                conf1.ESCMutateAlg = type_mutate;
                conf1.ESCCrossoverType =type_cross;
                conf1.ESCCrossoverPropability =pcross;
                conf1.ESCCountCrossoverPoint = (int) cpcross;
                conf1.ESCAngleRotateB = angleR;
                conf1.ESCCountIteration =(int)iterate;
                conf1.ESCPopulationSize =(int)cindiv;
                conf1.ESCCountChild = (int)cchild;
            
                
        }

        public override int Run(string[] args)
        {

            Console.WriteLine("Start");
            fill_params(args);
            Console.WriteLine("Params get \nfile in {0} ", file_in);
            Approx_learn_set = BaseUFSLoader.LoadLearnFromUFS(file_in);
            Console.WriteLine("Tra load");
            Approx_test_set = BaseUFSLoader.LoadTestFromUFS(file_in);
            Console.WriteLine("Tst load");
            conf = new ESConfig();
            conf.Init(Approx_learn_set.CountVars);

            fill_conf();
            Console.WriteLine("Conf Filed");
            Approx_Singletone = new SAFuzzySystem(Approx_learn_set, Approx_test_set);
            Approx_Singletone = SAFSUFSLoader.loadUFS(Approx_Singletone,file_in);
            Console.WriteLine("Classifier created");
            optimaze = new ESMethod();
            Approx_Singletone = optimaze.TuneUpFuzzySystem(Approx_Singletone, conf);
            Console.WriteLine("Optimization complite");
            SAFSUFSWriter.saveToUFS(Approx_Singletone, file_out);
            Console.WriteLine("Saved");
            return 1;
        }

    }
}
