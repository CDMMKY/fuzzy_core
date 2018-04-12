using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
//using Approximate_core.methods.termconfig.conf;
using System.ComponentModel;
using System.Threading;
using FuzzySystem.FuzzyAbstract.learn_algorithm.conf;
using FuzzySystem.FuzzyAbstract.conf;
using System.Windows.Forms;
using FuzzySystem.SingletoneApproximate.Mesure;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.FuzzyAbstract.learn_algorithm;
//using IFuzzySystem.SingletoneApproximate.LearnAlgorithm.ES;
using FuzzySystem.FuzzyAbstract;


namespace FuzzySystem.SingletoneApproximate.LearnAlgorithm
{
    public class MultiGoalOpimize : AbstractNotSafeLearnAlgorithm
    {
        double baseLearn;
        double baseComplexity;
        double baseIntebility;
        double sizePercent;
        double sizeComplexity;

        double sizeInteraply;
        int toMany;
        Dictionary<int, int> usedAsNext = new Dictionary<int, int>();
   
       
        bool isTermShrink;
        bool isRuleShrink;
        bool isUnionTerm;
        bool isLindBreakCross;

        bool isPSO;
        int countPSO;

        bool isANT;
        int countANT;

        bool isBEE;
        int countBEE;
       
        bool isGA;
        int countGA;

        bool isBFO;
        int countBFO;
        bool isES;
         int countES;
        
        Result_F showWhatHappen;
        string ResultShow = "";
        bool isEnd = false;
        string path;
        string dataSetName;
        int typeComplexity;
        int typeInterpreting;
        int countFuzzySystem;
        double allowSqare;
        double allowBorder;
        double diviver;
        int trysBeforeDivide;
        BackgroundWorker BW = new BackgroundWorker();


        public override List<FuzzySystemRelisedList.TypeSystem> SupportedFS
        {
            get
            {
                return new List<FuzzySystemRelisedList.TypeSystem>() { FuzzySystemRelisedList.TypeSystem.Singletone };
            }
        }

        [MTAThread]
        public override SAFuzzySystem TuneUpFuzzySystem(SAFuzzySystem Approx, ILearnAlgorithmConf conf)
        {
            
            Random rand = new Random(DateTime.Now.Millisecond);
            SAFuzzySystem result = Approx;

            BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_RunWorkerCompleted);
            BW.RunWorkerAsync();





            MultiGoalOptimaze_conf config = conf as MultiGoalOptimaze_conf;
            string PathAlg = (new FileInfo(Application.ExecutablePath)).DirectoryName + "\\FS\\";
            config.Init2(PathAlg, Approx.LearnSamplesSet.FileName);

            countFuzzySystem = config.Итераций_алгоритма;

            allowSqare = config.Допустимый_процент_перекрытия_по_площади_термов / 100;
            allowBorder = config.Допустимый_процент_перекрытия_по_границам / 100;
            int seedPath = rand.Next();
            sizePercent = config.Размер_шага_по_точности;
            sizeComplexity = config.Размер_шага_по_сложности;
            sizeInteraply = config.Размер_шага_по_интерпретируемости;
             diviver = config.Уменьшать_шаги_в;
            trysBeforeDivide = config.Уменьшать_шаг_после;
            path = config.path;
            dataSetName = config.dataSetName;
            toMany = config.Разрешено_похожих_систем;
            isPSO = config.toBool(config.Использовать_АРЧ);
          //  isBFO = config.toBool(config.Использовать_АПБ);
            isANT = config.toBool(config.Использовать_НАМК);
            isBEE = config.toBool(config.Использовать_САПК);
            isES = config.toBool(config.Использовать_ЕС);
            isGA = config.toBool(config.Использовать_ГА);
            isTermShrink = config.toBool(config.Удалять_термы);
            isRuleShrink = config.toBool(config.Удалять_правила);
            isUnionTerm = config.toBool(config.Объединять_термы);
            isLindBreakCross = config.toBool(config.Исключать_пересечение_лигвистически_далеких_термов);
            countANT = config.Использовать_НАМК_раз_за_такт;
           // countBFO = config.Использовать_за_такт_АПБ_раз;
            countPSO = config.Использовать_за_такт_АРЧ_раз;
            countBEE = config.Использовать_САПК_раз_за_такт;
            countES = config.Использовать_ЕС_раз_за_такт;
            countGA = config.Использовать_ГА_раз_за_такт;
            typeComplexity =(int) config.Критерий_сложности;
            typeInterpreting = (int)config.Критерий_интерпретируемости;

            List<IAbstractLearnAlgorithm> learnAlgorithms = initAlgoritms();
            List<ILearnAlgorithmConf> learnAlgorithmsconfig = initAlgoritmsConfigs(Approx.CountFeatures);
            List<double> ValueLPercent = new List<double>();
            List<double> ValueTPercent = new List<double>();
            List<double> ValueComplexity = new List<double>();
            List<double> ValueInterability = new List<double>();
            List<double> SummaryGoods = new List<double>();
            List<KnowlegeBaseSARules> Storage = new List<KnowlegeBaseSARules>();
            List<int> candidate = new List<int>();

            KnowlegeBaseSARules Best = result.RulesDatabaseSet[0];

           
            baseLearn = result.approxLearnSamples(result.RulesDatabaseSet[0]);
            ValueLPercent.Add(baseLearn);
            ValueTPercent.Add(result.approxTestSamples(result.RulesDatabaseSet[0]));
            baseComplexity = getComplexity(result);
            ValueComplexity.Add(baseComplexity);
            baseIntebility = getInterpreting( result, allowBorder, allowSqare);
            ValueInterability.Add(baseIntebility);


            Storage.Add(Best);
            int NSCount = 0;
            int deleted = 0;
            for (int numberStep = 0; numberStep < countFuzzySystem; numberStep++)
            {


                bool mustToDivide = true;
                int usedAlg = 0;
                for (int tr = 0; tr < trysBeforeDivide; tr++)
                {
                    deleted = 0;

                   // Parallel.For(0, learnAlgorithms.Count(), i =>
                   usedAlg = 0;
                    for(int i =0; i<learnAlgorithms.Count();i++)
                     {
                         Console.WriteLine("Fucked in Storage.Add(new a_Rules(Best))");
                         Storage.Add(new KnowlegeBaseSARules(Best));
                         Console.WriteLine("Fucked in result.RulesDatabaseSet.Clear()");
                         result.RulesDatabaseSet.Clear();
                         Console.WriteLine("Fucked in result.RulesDatabaseSet.Add( Storage[Storage.Count - 1])");
                         result.RulesDatabaseSet.Add(Storage[Storage.Count - 1]);
                         usedAlg++;
                         bool before_VAlue = true;
                         try
                         {
                             learnAlgorithms[i].TuneUpFuzzySystem(result, learnAlgorithmsconfig[i]);
                             GC.Collect();
                             before_VAlue = false;
                             ValueLPercent.Add(result.approxLearnSamples(result.RulesDatabaseSet[0]));
                             ValueTPercent.Add(result.approxTestSamples(result.RulesDatabaseSet[0]));
                             ValueComplexity.Add(getComplexity(result));
                             ValueInterability.Add(getInterpreting(result,allowBorder,allowSqare));
                             double temp = ValueLPercent[ValueLPercent.Count - 1] + ValueComplexity[ValueComplexity.Count() - 1] + ValueInterability[ValueInterability.Count() - 1];
                             Storage[Storage.Count - 1] = result.RulesDatabaseSet[0];

                             if (double.IsNaN(temp))
                             {
                                 Console.WriteLine("FuckAlarm " + i.ToString() + learnAlgorithms[i].ToString() + " is NAN");



                                 ValueLPercent.RemoveAt(ValueLPercent.Count() - 1);
                                 ValueTPercent.RemoveAt(ValueTPercent.Count() - 1);
                                 ValueComplexity.RemoveAt(ValueComplexity.Count() - 1);
                                 ValueInterability.RemoveAt(ValueInterability.Count() - 1);
                                 Storage.RemoveAt(Storage.Count() - 1);
                                 usedAlg--;
                             }
                         }
                         catch (Exception)
                         {
                             if (before_VAlue)
                             {
                                 Console.WriteLine("FuckAlarm " + i.ToString() + learnAlgorithms[i].ToString() + " before VAlue");
                             }
                             else
                             {
                                 Console.WriteLine("FuckAlarm " + i.ToString() + learnAlgorithms[i].ToString() + " after VAlue");

                                 ValueLPercent.RemoveAt(ValueLPercent.Count() - 1);
                                 ValueTPercent.RemoveAt(ValueTPercent.Count() - 1);
                                 ValueComplexity.RemoveAt(ValueComplexity.Count() - 1);
                                 ValueInterability.RemoveAt(ValueInterability.Count() - 1);
                                 Storage.RemoveAt(Storage.Count() - 1);
                             }
                         }

                         NSCount++;
                         Console.WriteLine("Fucked in ResultShow");
                         ResultShow += "[" + NSCount.ToString() + "]\t" + ValueLPercent[ValueLPercent.Count() - 1].ToString() + "\t" + ValueTPercent[ValueTPercent.Count() - 1].ToString() +
                            "\t" + ValueComplexity[ValueComplexity.Count() - 1].ToString() + "\t" + ValueInterability[ValueInterability.Count() - 1].ToString() + Environment.NewLine;
                    //     i++;
                     }
                    //);
                    Console.WriteLine("Fucked in deleted");
                   
                    deleted = removeDublicate(ValueLPercent, ValueComplexity, ValueInterability, ValueTPercent, Storage, rand);
                    usedAlg -= deleted;
                    Console.WriteLine("Fucked in candidate");
                   
                    candidate = canBeNext(ValueLPercent, ValueComplexity, ValueInterability);

                    if (candidate.Count() > 0) { mustToDivide = false; break; }

                }

                if (mustToDivide)
                {
                    MessageBox.Show("Divided happend ");

                    sizePercent = sizePercent / diviver;
                    sizeComplexity = sizeComplexity / diviver;
                    sizeInteraply = sizeInteraply / diviver;
                    continue;
                }

                Console.WriteLine("Fucked in SummaryGoods");
                  
                SummaryGoods = reCalcSummary(SummaryGoods, ValueLPercent, ValueComplexity, ValueInterability);

                Console.WriteLine("Fucked in indexofBest");
                int indexofBest = getNewBest(candidate, SummaryGoods);
                if (usedAsNext.ContainsKey(indexofBest))
                {
                    usedAsNext[indexofBest]++;
                }
                else { usedAsNext.Add(indexofBest, 1); }
               
                Console.WriteLine("Best");
               Best = Storage[indexofBest];

               Console.WriteLine("Fucked in for (int i = (Storage.Count - learnAlgorithms.Count); i < Storage.Count(); i++)");
               int toSaveCounter = NSCount - usedAlg;
               for (int i = (Storage.Count - usedAlg); i < Storage.Count(); i++)
                {
                    result.RulesDatabaseSet[0] = Storage[i];
                    saveFS(result, path, dataSetName, seedPath, numberStep,toSaveCounter, Best.Equals(result.RulesDatabaseSet[0]));
                    toSaveCounter++;
                }

               Console.WriteLine("Fucked in result.RulesDatabaseSet[0] = Best;");
                result.RulesDatabaseSet[0] = Best;

                Console.WriteLine("Fucked in End");
                baseLearn = result.approxLearnSamples(result.RulesDatabaseSet[0]);// ClassifyLearnSamples();
                baseComplexity = getComplexity(result);
                baseIntebility = getInterpreting(result,allowBorder, allowSqare);
                candidate.Clear();
                GC.Collect();

            } isEnd = true;
            Thread.Sleep(10000);
            result.RulesDatabaseSet[0].TermsSet.Trim();
            return result;
        }

        void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        void BW_DoWork(object sender, DoWorkEventArgs e)
        {

            showWhatHappen = new Result_F();



            showWhatHappen.Text = "Подробности трехкритериальной оптимизации";
            showWhatHappen.Result_RTB.Text = "№\tОшибка Обучения\tОшибка Тестовая\tСложность\tИнтерпретируемость" + Environment.NewLine;
            showWhatHappen.Show();
            while (isEnd == false)
            {
                Thread.Sleep(2000);
                showWhatHappen.Result_RTB.Text = "№\tОшибка Обучающая\tОшибка Тестовая\tСложность\tИнтерпретируемость" + Environment.NewLine;
                showWhatHappen.Result_RTB.Text += ResultShow.ToString();
                showWhatHappen.Result_RTB.SelectionStart = showWhatHappen.Result_RTB.Text.Length - 1;
                showWhatHappen.Result_RTB.ScrollToCaret();
                 showWhatHappen.Refresh();
                 Thread.Sleep(2000);


            }
            showWhatHappen.Result_RTB.SaveFile(path + "\\" + dataSetName + "\\REPORT.TXT", RichTextBoxStreamType.PlainText);


        }


        private List<IAbstractLearnAlgorithm> initAlgoritms()
        {
            List<IAbstractLearnAlgorithm> result = new List<IAbstractLearnAlgorithm>();
            if (isTermShrink)
            {
                result.Add(new OptimizeTermShrinkAndRotate());
            }
            if (isRuleShrink)
            {
                result.Add(new OptimizeRullesShrink());
            }
            if (isUnionTerm)
            {
                result.Add(new UnionTerms());
            }
            if (isLindBreakCross)
            {
                result.Add(new BreakTheCrossByLinds());
            }
            if (isPSO)
            {
                for (int i = 0; i < countPSO; i++)
                {
                    result.Add(new Term_Config_PSO());
                }
            }
            if (isANT)
            {
                for (int i = 0; i < countANT; i++)
                {
                    result.Add(new Term_config_Aco.Modified_ACO());
                }
            }

            if (isBEE)
            {
                for (int i = 0; i < countBEE; i++)
                {

                    result.Add(new Bee.BeeStructureAlgorithm());
                }
             }
            if (isES)

                for (int i = 0; i < countES; i++)
                {

                    result.Add(new ESMethod());
                }

            if (isGA)

                for (int i = 0; i < countGA; i++)
                {

                    result.Add(new GeneticAlgorithmTune.GeneticApprox());
                }

       


            return result;
        }

        private List<ILearnAlgorithmConf> initAlgoritmsConfigs(int CountFeature)
        {

            List<ILearnAlgorithmConf> result = new List<ILearnAlgorithmConf>();

            if (isTermShrink)
            {

                result.Add(new OptimizeTermShrinkAndRotateConf());
            }

            if (isRuleShrink)
            {

                result.Add(new OptimizeRullesShrinkConf());
            }
            if (isUnionTerm)
            {
                result.Add(new UnionTermsConf());
            }
            if (isLindBreakCross)
            {
                result.Add(null);
            }
            if (isPSO)
            {
                for (int i = 0; i < countPSO; i++)
                {
                    result.Add(new PSOSearchConf());
                }
            }
            if (isANT)
            {
                for (int i = 0; i < countANT; i++)
                {
                    result.Add(new MACOSearchConf());
                }
            }

            if (isBEE)
            {
                for (int i = 0; i < countBEE; i++)
                {

                    result.Add(new BeeStructureConf());
                }
            }

            if (isES)
            {
                for (int i = 0; i < countES; i++)
                {ESConfig method = new ESConfig();
                    method.Init(CountFeature);
                    result.Add(method); 
                }
            }

            if (isGA)
            {
                for (int i = 0; i < countGA; i++)
                {
                   GeneticConf method = new GeneticConf();
                    method.Init(CountFeature);
                    result.Add(method);
                }
            }


         /*   if (isBFO)
            {
                for (int i = 0; i < countBFO; i++)
                {
                    BacterialForagingOptimization.Base.BacteryAlgorithmConfig method = new BacterialForagingOptimization.Base.BacteryAlgorithmConfig();
                    method.Init(CountFeature);
                    result.Add(method);
                }
            }
            */

            return result;
        }


        private void saveFS(SAFuzzySystem Classifier, string path, string dataSet, int seed, int numstep, int globalCounter, bool isPath)
        {
            string RootPath = path + "\\" + dataSet + "\\";
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }
            string name = globalCounter.ToString() + "_s" + seed.ToString();
            if (isPath)
            {
                name += "_p" + numstep.ToString();
            }
            name += ".ufs";
            SAFSUFSWriter.saveToUFS(Classifier, RootPath + name);

        }

        private double getGoodsMesuare(double learnGoods, double complexityGoods, double interabilityGoods)
        {
            double result = 0;
            result += Math.Floor((baseLearn - learnGoods) / sizePercent) + Math.Floor((baseComplexity - complexityGoods) / sizeComplexity) + Math.Floor((interabilityGoods - baseIntebility) / sizeInteraply);

            return result;
        }


        private List<double> reCalcSummary(List<double> Summary, List<double> ValueLGoods, List<double> ValueComplexityGoods, List<double> ValueInterapibility)
        {
            Summary.Clear();

            for (int i = 0; i < ValueLGoods.Count; i++)
            {
                Summary.Add(getGoodsMesuare(ValueLGoods[i], ValueComplexityGoods[i], ValueInterapibility[i]));
            }
            return Summary;

        }

        private List<int> canBeNext(List<double> ValueLGoods, List<double> ValueComplexityGoods, List<double> ValueInterapibility)
        {
            List<int> candidate = new List<int>();
            double Ldistance;
            double Cdistance;
            double Idistance;
            for (int i = 0; i < ValueLGoods.Count(); i++)
            {
                bool isCandidate = true;
                int countNearestPoint = 0;
                for (int j = i + 1; j < ValueLGoods.Count(); j++)
                {
                    Ldistance = Math.Abs(ValueLGoods[i] - ValueLGoods[j]);
                    Cdistance = Math.Abs(ValueComplexityGoods[i] - ValueComplexityGoods[j]);
                    Idistance = Math.Abs(ValueInterapibility[i] - ValueInterapibility[j]);
                    if ((Ldistance < sizePercent) && (Cdistance < sizeComplexity) && (Idistance < sizeInteraply))
                    {
                        countNearestPoint++;
                    }
                    if (countNearestPoint >= toMany)
                    {
                        isCandidate = false;
                        break;
                    }
                }
                if (usedAsNext.ContainsKey(i))
                {
                    if (usedAsNext[i] > toMany)
                    {
                        isCandidate = false;
                    }
                }

                if (isCandidate)
                {
                    candidate.Add(i);
                }
            }

            return candidate;
        }

        private int getNewBest(List<int> candidate, List<double> Summary)
        {
            int indexofBest = -1;
            double max = double.MinValue;
            for (int i = 0; i < candidate.Count(); i++)
            {
                if (max < Summary[candidate[i]])
                {
                    max = Summary[candidate[i]];
                    indexofBest = candidate[i];
                }
            }

            return indexofBest;
        }



        private int removeDublicate(List<double> ValueLGoods, List<double> ValueComplexityGoods, List<double> ValueInterapibility, List<double> ValueTGoods, List<KnowlegeBaseSARules> Storage, Random rand)
        {
            int result = 0;
           int indexforDel = 0;
            List<int> ClonesforI = new List<int>();
            for (int i = ValueLGoods.Count()-1; i >=0; i--)
            {

                for (int j = i + 1; j < ValueLGoods.Count(); j++)
                {
                    if (ValueLGoods[i] == ValueLGoods[j])
                    {
                        if (ValueComplexityGoods[i] == ValueComplexityGoods[j])
                        {
                            if (ValueInterapibility[i] == ValueInterapibility[j])
                            {
                                if (ValueTGoods[i] == ValueTGoods[j])
                                {

                                    ClonesforI.Add(j);

                                }
                            }
                        }
                    }
                }
                if (ClonesforI.Count > 0)
                {
                    ClonesforI.Sort();
                   
                    while (ClonesforI.Count > 0)
                    {
                        indexforDel = ClonesforI.Count-1;

                        Storage.RemoveAt(ClonesforI[indexforDel]);
                        ValueTGoods.RemoveAt(ClonesforI[indexforDel]);
                        ValueInterapibility.RemoveAt(ClonesforI[indexforDel]);
                        ValueComplexityGoods.RemoveAt(ClonesforI[indexforDel]);
                        ValueLGoods.RemoveAt(ClonesforI[indexforDel]);
                        ClonesforI.RemoveAt(indexforDel);
                        result++;
                    }
                }
                ClonesforI.Clear();

            }
            return result;
        }

        private double getComplexity(SAFuzzySystem Approx)

        {
            switch (typeComplexity)
            {
                case 0: return Approx.getComplexit();
                case 1: return Approx.getRulesCount();
                default: return Approx.getComplexit();
            }
        }

        private double getInterpreting(SAFuzzySystem Approx, double allowSquare, double allowBorder)
        {
            switch (typeInterpreting)
            {
                case 0: return Approx.getNormalIndex(allowBorder,allowSquare);
                case 1: return Approx.getIndexReal(allowBorder, allowBorder);
                default: return Approx.getNormalIndex(allowBorder, allowBorder);
            }
        }

        public void loadParams(string param)
        {
            throw (new NotImplementedException());
        }

        public override string ToString(bool with_param = false)
        {
            if (with_param)
            {
                string result = "Трехкритериальная оптимизация {";
                result += "Итераций= " + countFuzzySystem.ToString() + " ;" + Environment.NewLine;
                result += "Допустимый процент перекрытия по площади термов= " + (allowSqare*100).ToString() + " ;" + Environment.NewLine;
                result += "Допустимый процент перекрытия по границам= " + (allowBorder * 100).ToString() + " ;" + Environment.NewLine;
                result += "Размер шага по точности= " + (sizePercent).ToString() + " ;" + Environment.NewLine;
                result += "Размер шага по сложности= " + (sizeComplexity).ToString() + " ;" + Environment.NewLine;
                result += "Размер шага по интерпретируемости= " + (sizeInteraply).ToString() + " ;" + Environment.NewLine;
                result += "Уменьшать шаги в= " + (diviver).ToString() + " ;" + Environment.NewLine;
                result += "Уменьшать шаг после= " + (trysBeforeDivide).ToString() + " ;" + Environment.NewLine;
                result += "Разрешено похожих систем= " + (toMany).ToString() + " ;" + Environment.NewLine;
                result += "Использовать АРЧ= " + (isPSO).ToString() + " ;" + Environment.NewLine;
                result += "Использовать за_такт АРЧ_раз= " + (countPSO).ToString() + " ;" + Environment.NewLine;
                result += "Использовать НАМК= " + (isANT).ToString() + " ;" + Environment.NewLine;
                result += "Использовать НАМК раз за такт= " + (countANT).ToString() + " ;" + Environment.NewLine;
                result += "Использовать САПК= " + (isBEE).ToString() + " ;" + Environment.NewLine;
                result += "Использовать_САПК_раз_за_такт= " + (countBEE).ToString() + " ;" + Environment.NewLine;
           
                result += "Использовать ЕС= " + (isES).ToString() + " ;" + Environment.NewLine;

                result += "Использовать ЕС раз за такт= " + (countES).ToString() + " ;" + Environment.NewLine;
                result += "Удалять термы= " + (isTermShrink).ToString() + " ;" + Environment.NewLine;
                result += "Удалять правила= " + (isRuleShrink).ToString() + " ;" + Environment.NewLine;
                result += "Объединять термы= " + (isUnionTerm).ToString() + " ;" + Environment.NewLine;
                result += "Исключать пересечение лигвистически далеких термов= " + (isLindBreakCross).ToString() + " ;" + Environment.NewLine;

                switch (typeComplexity)
                {
                    case 0:
                        {
                            result += "Критерий сложности= Правила и Термы;" + Environment.NewLine;
                        } break;
                    case 1:
                        {
                            result += "Критерий сложности= Правила;" + Environment.NewLine;
                        
                    } break;
                }

                switch (typeInterpreting)
                {
                    case 0:
                        {
                            result += "Критерий интерпретируемости= Нормированный индекс;" + Environment.NewLine;
                        } break;
                    case 1:
                        {
                            result += "Критерий интерпретируемости= Вещественный индекс;" + Environment.NewLine;

                        } break;
                }



                result += "}";
                return result;
            }
            return "Трехкритериальная оптимизация";
        }


        public override ILearnAlgorithmConf getConf(int CountFeatures)
        {
            ILearnAlgorithmConf result = new MultiGoalOptimaze_conf();
            result.Init(CountFeatures);
            return result;
        }
        
    }
}
