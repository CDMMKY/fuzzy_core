using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System.IO;
using System.Windows.Forms;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.FuzzyAbstract;

namespace FuzzySystem.PittsburghClassifier.LearnAlgorithm
{
    public class Term_config_PSO_Bactery : Term_Config_PSO
    {  protected int sendPSO=0;
    protected int sendBactery = 0;
    protected int interPSOtoSend = 0;
    protected int counterIter = 0;
    protected int trySend = 0;

    protected doubleReverse ReverseSorter = new doubleReverse();

    public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
    {
        get
        {
            return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.PittsburghClassifier };
        }
    }

        public override void Init(ILearnAlgorithmConf Config)
        {
            base.Init(Config);
            PSOBacterySearchConf CurrentConf = Config as PSOBacterySearchConf;
            count_particle = CurrentConf.PSOSCPopulationSize;
            sendBactery = CurrentConf.PSOBacteryHOCountGet;
            sendPSO = CurrentConf.PSOBacteryHOCountSend;
            interPSOtoSend = CurrentConf.PSOBacteryHOCountChange;

        }

        public override void oneIterate(PCFuzzySystem result)
        {
            base.oneIterate(result);
            counterIter++;
            if (counterIter == interPSOtoSend)
            {
                Pi = sortSolution(Pi);
                savetoUFS(Pi.ToList(), 0, sendPSO, trySend);
                BacteryRunner();
                trySend++;

                List<KnowlegeBasePCRules> tempRes = loadDatabase().ToList();

                int size = tempRes.Count;
                for (int p = tempRes.Count - 1; p >= 0; p--)
                {
                    X[p] = tempRes[0];
                
                    double newError = result.ClassifyLearnSamples(X[p]);

                    if (newError > Errors[p])
                    {
                        Pi[p] = new KnowlegeBasePCRules(X[p]);
                        OldErrors[p] = Errors[p];
                        Errors[p] = newError;

                        if (minError < newError)
                        {
                            minError = newError;
                            Pg = new KnowlegeBasePCRules(X[p]);
                        }


                    }
                    tempRes.RemoveAt(0);

                }

                counterIter = 0;
            }
        }


        public void BacteryRunner()
        {
            string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\BacteryAlg\\";
            string PathAlgSource = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\toBactery\\";
            string PathAlgDestiny = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\fromBactery\\";
    
            System.Diagnostics.Process runner = new System.Diagnostics.Process();
            runner.StartInfo.WorkingDirectory = PathAlg;
            runner.StartInfo.FileName = PathAlg + "bactria.bat";
            runner.StartInfo.Arguments = "/SourceDir="+PathAlgSource + " /DestinyDir="+PathAlgDestiny +" /FromCount="+sendPSO +" /ToCount="+sendBactery + " /RunIter="+interPSOtoSend.ToString();
            runner.Start();
            runner.WaitForExit();
           }


        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "роящиеся частицы + перемещение бактерии {";
                result += "Итераций= " + count_iteration.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c1= " + c1.ToString() + " ;" + Environment.NewLine;
                result += "Коэффициент_c2= " + c2.ToString() + " ;" + Environment.NewLine;
                result += "Особей в популяции= " + count_particle.ToString() + " ;" + Environment.NewLine;
                result += "Отправляемых решений от РЧ =" + sendPSO.ToString() + " ;" + Environment.NewLine;
                result += "Отправляемых решений от алгоритма Перемещения бактерии =" + sendBactery.ToString() + " ;" + Environment.NewLine;
                result += "Обмен решениями через каждые  =" + interPSOtoSend.ToString() + " ;" + Environment.NewLine;



                result += "}";
                return result;
            }
            return "роящиеся частицы + перемещение бактерии";

        }


       protected void savetoUFS(List <KnowlegeBasePCRules> Source, int startpos, int endPos, int TryNum) 
       {
           KnowlegeBasePCRules temp = result.RulesDatabaseSet[0];
           
           string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\toBactery\\";
           if (!Directory.Exists(PathAlg))
           { Directory.CreateDirectory(PathAlg); }
        string[] files=   Directory.GetFiles(PathAlg, "*.ufs");
        foreach (string file in files)
        {
            File.Delete(file); 
        }

           for (int i=startpos;i<=endPos;i++)
           {
               result.RulesDatabaseSet[0]=Source[i];

           PCFSUFSWriter.saveToUFS(result, PathAlg+ TryNum.ToString()+"_"+i.ToString()+".ufs");
           }
           result.RulesDatabaseSet[0] = temp;
       }



       protected KnowlegeBasePCRules[] loadDatabase()
       {
           KnowlegeBasePCRules temp = result.RulesDatabaseSet[0];
           string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\fromBactery\\";
           if (!Directory.Exists(PathAlg))
           { Directory.CreateDirectory(PathAlg); }

       string[] files=    Directory.GetFiles(PathAlg,"*.ufs",SearchOption.AllDirectories);
           KnowlegeBasePCRules[] tempResult = new KnowlegeBasePCRules[files.Count()];

           for (int i =0; i<files.Count();i++)
           {
             result=  PCFSUFSLoader.loadUFS(result, files[i]);
             tempResult[i] = result.RulesDatabaseSet[0];
             File.Delete(files[i]);
           }

           result.RulesDatabaseSet[0] = temp;

           return tempResult;
       }

       protected KnowlegeBasePCRules[] sortSolution(KnowlegeBasePCRules[] Source)
       {
           double[] keys = new double[Source.Count()];
           KnowlegeBasePCRules [] tempSol = Source.Clone() as KnowlegeBasePCRules[];
           KnowlegeBasePCRules tempKN = result.RulesDatabaseSet[0];
           for (int i = 0; i < Source.Count(); i++)
           {
               
               result.RulesDatabaseSet[0] = Source[i];
              keys[i]= result.ErrorLearnSamples(result.RulesDatabaseSet[0]);

           }
           result.RulesDatabaseSet[0] = tempKN;
           double[] tempError1 = keys.Clone() as double[];
           double[] tempError2 = keys.Clone() as double[];
           
           Array.Sort(keys, tempSol);
           Array.Sort(tempError1, Errors);
           Array.Sort(tempError2, OldErrors);
          
           return tempSol;
       }


        public class doubleReverse : IComparer<double>
        {
            Comparer<double> noReverse = Comparer<double>.Default;
            int IComparer<double>.Compare(double x, double y)
            {
                return noReverse.Compare(y, x);
            }
        }

        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new PSOBacterySearchConf();
            result.Init(CountFeatures);
            return result;
        }

    }
}
