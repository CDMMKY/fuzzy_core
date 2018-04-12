using System;
using System.Linq;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.PittsburghClassifier.add_generators;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.FuzzyAbstract.conf;
using System.IO;
namespace RunInit
{
    class Wrapper36_script : Base_Class_init
    {


        protected override void fill_params(string[] args)
        {
            file_learn = (args.Where(x => x.Contains("Path"))).ToArray()[0];
            Console.WriteLine("Before cut {0}", file_learn);
            file_learn = file_learn.Remove(0, 5);

            toStringParams(args);
        }


        protected override void fill_conf()
        {

        }

        public override int Run(string[] args)
        {

            Console.WriteLine("Start");
            fill_params(args);


            foreach (string filenametra in System.IO.Directory.GetFiles(file_learn, "*tra.dat", System.IO.SearchOption.AllDirectories))
            {
                string filenameTST = filenametra.Replace("tra.dat", "tst.dat");
            
                Console.WriteLine("Params get \nfile tra {0} \nfile name tst {1} ", filenametra, filenameTST);
                Class_learn_set = new SampleSet(filenametra);
                Console.WriteLine("Tra create");
                Class_test_set = new SampleSet(filenameTST);
                Console.WriteLine("Tst create");
                conf = new InitBySamplesConfig();
                conf.Init(Class_learn_set.CountVars);

               // fill_conf();
                conf.loadParams(confParams);

                    file_out = filenametra.Replace("tra.dat", ((InitBySamplesConfig) conf).IBSTypeFunc.ToString() +"_out.ufs");
                Console.WriteLine("Conf Filed");
                Class_Pittsburg = new PCFuzzySystem(Class_learn_set, Class_test_set);
                Console.WriteLine("Classifier created");
                generator = new GeneratorRulesBySamples();

                Class_Pittsburg = generator.Generate(Class_Pittsburg, conf);
                Console.WriteLine("Generation complite");

                PCFSUFSWriter.saveToUFS(Class_Pittsburg, file_out);

                StreamWriter sw = new StreamWriter(Path.Combine(file_learn,((InitBySamplesConfig) conf).IBSTypeFunc.ToString()+ "_log.txt"),true);
                sw.WriteLine(filenametra+"\t"+Class_Pittsburg.ErrorLearnSamples(Class_Pittsburg.RulesDatabaseSet[0]));
                sw.WriteLine(filenameTST + "\t" + Class_Pittsburg.ErrorTestSamples(Class_Pittsburg.RulesDatabaseSet[0]));
                sw.Close();

                Console.WriteLine("Saved");
            }
            return 1;
        }

    }
}
