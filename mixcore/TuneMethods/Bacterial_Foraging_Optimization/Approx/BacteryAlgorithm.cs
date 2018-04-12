using BacterialForagingOptimization.Base;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.FuzzyAbstract.conf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FuzzySystem.FuzzyAbstract;

namespace BacterialForagingOptimization.Approx
{
    public class BacteryAlgorithm : AbstractNotSafeLearnAlgorithm
    {
        SAFuzzySystem result;

        int sendPSO = 1;
        int sendBactery = 5;
        int interPSOtoSend = 100;

        string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\BacteryAlg\\Classifier\\";
        string PathAlgSource = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\toBactery\\Classifier\\";
        string PathAlgDestiny = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\fromBactery\\Classifier\\";

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }


        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            BacteryAlgorithmConfig Config = conf as BacteryAlgorithmConfig;

            sendBactery = Config.BFOCountSolution;
            interPSOtoSend = Config.BFOCountIteration;
           result = Approximate;

            
           if (result.RulesDatabaseSet.Count < 1)
           {
               throw new InvalidDataException("Нечеткая система не проинициализированна");
 
           }
           KnowlegeBaseSARules backSave =new KnowlegeBaseSARules(  result.RulesDatabaseSet[0]);
           double backResult = result.approxLearnSamples(result.RulesDatabaseSet[ 0]);

           savetoUFS(result.RulesDatabaseSet, 0, 0, 0);
           BacteryRunner();
           KnowlegeBaseSARules[] solutions = loadDatabase();
           solutions = sortSolution(solutions);
           if (solutions.Count() < 1) { result.RulesDatabaseSet[0] = backSave; return result; }
           result.RulesDatabaseSet[0] = solutions[0];
           double newResult = result.approxLearnSamples(result.RulesDatabaseSet[ 0]);
           if (newResult > backResult)
           {
               result.RulesDatabaseSet[0] = backSave;
           }

           result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        public void BacteryRunner()
        {
      

            System.Diagnostics.Process runner = new System.Diagnostics.Process();
            runner.StartInfo.WorkingDirectory = PathAlg;
            runner.StartInfo.FileName = PathAlg + "bactria.bat";
            runner.StartInfo.Arguments = "/SourceDir=" + PathAlgSource + " /DestinyDir=" + PathAlgDestiny + " /FromCount=" + sendPSO + " /ToCount=" + sendBactery + " /RunIter=" + interPSOtoSend.ToString();
            runner.Start();
            runner.WaitForExit();
        }

        protected void savetoUFS(List<KnowlegeBaseSARules> Source, int startpos, int endPos, int TryNum)
        {
            KnowlegeBaseSARules temp = result.RulesDatabaseSet[0];

          
             if (!Directory.Exists(PathAlgSource))
             { Directory.CreateDirectory(PathAlgSource); }
             string[] files = Directory.GetFiles(PathAlgSource, "*.ufs");
            foreach (string file in files)
            {
                File.Delete(file);
            }

            for (int i = startpos; i <= endPos; i++)
            {
                result.RulesDatabaseSet[0] = Source[i];

                SAFSUFSWriter.saveToUFS(result, PathAlgSource + TryNum.ToString() + "_" + i.ToString() + ".ufs");
            }
            result.RulesDatabaseSet[0] = temp;
        }

        protected KnowlegeBaseSARules[] loadDatabase()
        {
            KnowlegeBaseSARules temp = result.RulesDatabaseSet[0];
          
           if (!Directory.Exists(PathAlgDestiny))
           { Directory.CreateDirectory(PathAlgDestiny); }

           string[] files = Directory.GetFiles(PathAlgDestiny, "*.ufs", SearchOption.AllDirectories);
            KnowlegeBaseSARules[] tempResult = new KnowlegeBaseSARules[files.Count()];

            for (int i = 0; i < files.Count(); i++)
            {
                result = SAFSUFSLoader.loadUFS(result, files[i]);
                tempResult[i] = result.RulesDatabaseSet[0];
                File.Delete(files[i]);
            }

            result.RulesDatabaseSet[0] = temp;

            return tempResult;
        }

        protected KnowlegeBaseSARules[] sortSolution(KnowlegeBaseSARules[] Source)
        {
            KnowlegeBaseSARules temp = result.RulesDatabaseSet[0];
            double[] keys = new double[Source.Count()];

            KnowlegeBaseSARules[] tempSol = Source.Clone() as KnowlegeBaseSARules[];
            for (int i = 0; i < Source.Count(); i++)
            {
                result.RulesDatabaseSet[0] = Source[i];
                keys[i] = result.approxLearnSamples(result.RulesDatabaseSet[0]);

            }

            Array.Sort(keys, tempSol);

            result.RulesDatabaseSet[0] = temp;
            return tempSol;
        }



        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Алгоритм перемещения бактерии {";
                result += "Итераций= " + interPSOtoSend .ToString() + " ;" + Environment.NewLine;
                result += "Количество различных решений= " + sendBactery.ToString() + " ;" + Environment.NewLine;
                result += "}";
            }

            return "Алгоритм перемещения бактерии";
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new  BacteryAlgorithmConfig ();
            result.Init(CountFeatures);
            return result;
        }

    }
}
