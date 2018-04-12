using System;
using System.Collections.Generic;
using System.Linq;
using FuzzySystem.SingletoneApproximate.LearnAlgorithm;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract.conf;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using System.IO;
using System.Windows.Forms;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.FuzzyAbstract;

namespace PSOMethods.Approx
{
    public class Term_config_PSO_Bactery : Term_Config_PSO
    {
        int sendPSO = 0;
        int sendBactery = 0;
        int interPSOtoSend = 0;
        doubleReverse ReverseSorter = new doubleReverse();
        SAFuzzySystem result;

        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approximate, ILearnAlgorithmConf conf)
        {
            PSOBacterySearchConf CurrentConf = conf as PSOBacterySearchConf;
            count_iteration = CurrentConf.PSOSCCountIteration;
            c1 = CurrentConf.PSOSCC1;
            c2 = CurrentConf.PSOSCC2;
            w = 1;
            count_particle = CurrentConf.PSOSCPopulationSize;
            sendBactery = CurrentConf.PSOBacteryHOCountGet;
            sendPSO = CurrentConf.PSOBacteryHOCountSend;
            interPSOtoSend = CurrentConf.PSOBacteryHOCountChange;
            result = Approximate;

            X = new KnowlegeBaseSARules[count_particle];
            V = new KnowlegeBaseSARules[count_particle];
            Pi = new KnowlegeBaseSARules[count_particle];
            Pg = new KnowlegeBaseSARules();
            Errors = new double[count_particle];
            OldErrors = new double[count_particle];
            rnd = new Random();


            preIterate(result);
            int trySend = 0;
            int counterIter = 0;
            for (int i = 0; i < count_iteration; i++)
            {
                oneIterate(result);
                counterIter++;
                if (counterIter == interPSOtoSend)
                {
                    Pi = sortSolution(Pi);
                    savetoUFS(Pi.ToList(), 0, sendPSO, trySend);
                    BacteryRunner();
                    trySend++;
                   
                    List<KnowlegeBaseSARules> tempRes = loadDatabase().ToList();

                    int size = tempRes.Count;
                    for (int p = tempRes.Count - 1; p >= 0; p--)
                    {
                        X[p] = tempRes[0];

                        double newError = Approximate.approxLearnSamples(X[p]);

                        if (newError < Errors[p])
                        {
                            Pi[p] = new KnowlegeBaseSARules(X[p]);
                            OldErrors[p] = Errors[p];
                            Errors[p] = newError;

                            if (minError > newError)
                            {
                                minError = newError;
                                Pg = new KnowlegeBaseSARules(X[p]);
                            }


                        }

                        tempRes.RemoveAt(0);
                    }

                    counterIter = 0;
                }
            }
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }


        public void BacteryRunner()
        {
            string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\BacteryAlg\\";
            string PathAlgSource = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\toBactery\\";
            string PathAlgDestiny = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\fromBactery\\";

            System.Diagnostics.Process runner = new System.Diagnostics.Process();
            runner.StartInfo.WorkingDirectory = PathAlg;
            runner.StartInfo.FileName = PathAlg + "bactria.bat";
            runner.StartInfo.Arguments = "/SourceDir=" + PathAlgSource + " /DestinyDir=" + PathAlgDestiny + " /FromCount=" + sendPSO + " /ToCount=" + sendBactery + " /RunIter=" + interPSOtoSend.ToString();
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


        protected void savetoUFS(List<KnowlegeBaseSARules> Source, int startpos, int endPos, int TryNum)
        {
            KnowlegeBaseSARules temp = result.RulesDatabaseSet[0];

            string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\toBactery\\";
            if (!Directory.Exists(PathAlg))
            { Directory.CreateDirectory(PathAlg); }
            string[] files = Directory.GetFiles(PathAlg, "*.ufs");
            foreach (string file in files)
            {
                File.Delete(file);
            }

            for (int i = startpos; i <= endPos; i++)
            {
                result.RulesDatabaseSet[0] = Source[i];

                SAFSUFSWriter.saveToUFS(result, PathAlg + TryNum.ToString() + "_" + i.ToString() + ".ufs");
            }
            result.RulesDatabaseSet[0] = temp;
        }



        protected KnowlegeBaseSARules[] loadDatabase()
        {
            KnowlegeBaseSARules temp = result.RulesDatabaseSet[0];
            string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\fromBactery\\";
            if (!Directory.Exists(PathAlg))
            { Directory.CreateDirectory(PathAlg); }

            string[] files = Directory.GetFiles(PathAlg, "*.ufs", SearchOption.AllDirectories);
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
            double[] keys = new double[Source.Count()];
            KnowlegeBaseSARules[] tempSol = Source.Clone() as KnowlegeBaseSARules[];
            for (int i = 0; i < Source.Count(); i++)
            {
                result.RulesDatabaseSet[0] = Source[i];
                keys[i] = result.approxLearnSamples(result.RulesDatabaseSet[0]);

            }

            Array.Sort(keys, tempSol);
            double[] tempError = keys.Clone() as double[];
            double[] tempError1 = keys.Clone() as double[];

            Array.Sort(tempError, Errors);
            Array.Sort(tempError1, OldErrors);


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
