using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using FuzzySystem.PittsburghClassifier.UFS;
using FuzzySystem.SingletoneApproximate.UFS;
using FuzzySystem.SingletoneApproximate.Mesure;
using FuzzySystem.PittsburghClassifier.Mesure;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using FuzzySystem.PittsburghClassifier;
using FuzzySystem.SingletoneApproximate;
using FuzzySystem.FuzzyAbstract;
using FuzzyCore.FuzzySystem.FuzzyAbstract;



namespace DrawMeMultuGoal
{
    public partial class MainF : Form
    {
        List<double> ValueLGoodsRMSE;
        List<double> ValueLGoodsMSE;
        List<double> ValueLGoodsPercent;
        List<double> ValueLGoodsError;
        
        List<double> ValueTGoodsRMSE;
        List<double> ValueTGoodsMSE;
        List<double> ValueTGoodsPercent;
        List<double> ValueTGoodsError;

        List<double> ValueComplexityFull;
        List<double> ValueComplexityRules;

        List<double> ValueInterpretyNominal;
        List<double> ValueInterpretyReal;
        List<string> PathFilesUFS;
        bool isApprox = false;

        string rootDitectory;
        public MainF()
        {
            InitializeComponent();
        }

        private void InitList()
        {
            ValueLGoodsRMSE = new List<double>();
            ValueLGoodsMSE = new List<double>();
            ValueLGoodsError = new List<double>();
            ValueLGoodsPercent = new List<double>();

            ValueTGoodsRMSE = new List<double>();
            ValueTGoodsMSE = new List<double>();
            ValueTGoodsError = new List<double>();
            ValueTGoodsPercent = new List<double>();
     
            ValueComplexityFull = new List<double>();
            ValueComplexityRules = new List<double>();
     
            ValueInterpretyNominal = new List<double>();
            ValueInterpretyReal = new List<double>();

            PathFilesUFS = new List<string>();
              
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Properties.Settings.Default.LastSelected))
            {
                FuzzyUFSFOD.SelectedPath = Properties.Settings.Default.LastSelected;
            }

            if (FuzzyUFSFOD.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LastSelected = FuzzyUFSFOD.SelectedPath;
                Properties.Settings.Default.Save();
                rootDitectory = FuzzyUFSFOD.SelectedPath;
                InitList();
                LoadingFuzzyBarPB.Visible = true;
                LoadingFuzzyBarPB.MarqueeAnimationSpeed = 100;
                LoadingFuzzyBarPB.Refresh();
                FuzzyLoadingBW.RunWorkerAsync();

            }


        }

        private void addApproxValue(SAFuzzySystem Approx)
        {
            double Value = Approx.approxLearnSamples(Approx.RulesDatabaseSet[0]);
            ValueLGoodsRMSE.Add(Value);
            ValueLGoodsMSE.Add(Approx.RMSEtoMSEforLearn(Value));

            Value = Approx.approxTestSamples(Approx.RulesDatabaseSet[0]);
            ValueTGoodsRMSE.Add(Value);
            ValueTGoodsMSE.Add(Approx.RMSEtoMSEforTest(Value));

            Value = Approx.getComplexit();
            ValueComplexityFull.Add(Value);
            Value = Approx.getRulesCount();
            ValueComplexityRules.Add(Value);

            Value = Approx.getNormalIndex();
            ValueInterpretyNominal.Add(Value);
            Value = Approx.getIndexReal();
            ValueInterpretyReal.Add(Value);
        }


          private void addClassifierValue(PCFuzzySystem Classifier)
        {
            double Value = Classifier.ClassifyLearnSamples(Classifier.RulesDatabaseSet[0]);
            ValueLGoodsPercent.Add(Value);
            ValueLGoodsError.Add(100-Value);

             Value = Classifier.ClassifyTestSamples(Classifier.RulesDatabaseSet[0]);
            ValueTGoodsPercent.Add(Value);
            ValueTGoodsError.Add(100 - Value);


            Value = Classifier.getComplexit();
            ValueComplexityFull.Add(Value);
            Value = Classifier.getRulesCount();
            ValueComplexityRules.Add(Value);

            Value = Classifier.getNormalIndex();
            ValueInterpretyNominal.Add(Value);
            Value = Classifier.getIndexReal();
            ValueInterpretyReal.Add(Value);
        }



        private void LoadAllFS()
        {

            PathFilesUFS = Directory.GetFiles(rootDitectory, "*.UFS", SearchOption.AllDirectories).ToList();
            PCFuzzySystem Classifier = null;
            SampleSet tempcTable = null, temptestcTable = null;

            SAFuzzySystem Approx = null;
            SampleSet tempaTable = null, temptestaTable = null;

            for (int i = 0; i < PathFilesUFS.Count(); i++)
            {

                try
                {

                    if (isApprox)
                    {
                        tempaTable = BaseUFSLoader.LoadLearnFromUFS(PathFilesUFS[i]);
                        temptestaTable = BaseUFSLoader.LoadTestFromUFS(PathFilesUFS[i]);
                        Approx = new SAFuzzySystem(tempaTable, temptestaTable);
                       Approx= Approx.loadUFS(PathFilesUFS[i]);
       
                    }
                    else
                    {
                        tempcTable = BaseUFSLoader.LoadLearnFromUFS(PathFilesUFS[i]);
                        temptestcTable = BaseUFSLoader.LoadTestFromUFS(PathFilesUFS[i]);
                        Classifier = new PCFuzzySystem(tempcTable, temptestcTable);
                       Classifier= Classifier.loadUFS(PathFilesUFS[i]);
                    }
                }
                catch (Exception ex)
                {
                    isApprox = !isApprox;
                    try
                    {
                        if (isApprox)
                        {

                            tempaTable = BaseUFSLoader.LoadLearnFromUFS(PathFilesUFS[i]);
                            temptestaTable = BaseUFSLoader.LoadTestFromUFS(PathFilesUFS[i]);
                            Approx = new SAFuzzySystem(tempaTable, temptestaTable);
                            Approx = Approx.loadUFS(PathFilesUFS[i]);
      
                        }
                        else
                        {
                            tempcTable = BaseUFSLoader.LoadLearnFromUFS(PathFilesUFS[i]);
                            temptestcTable = BaseUFSLoader.LoadTestFromUFS(PathFilesUFS[i]);
                            Classifier = new PCFuzzySystem(tempcTable, temptestcTable);
                            Classifier = Classifier.loadUFS(PathFilesUFS[i]);
                  
                        }
                    }
                    catch (Exception ex1)
                    {
                        Console.Write("{0} \n{1} \n{2}", ex.Data.ToString(), ex.Message, ex.Source);
                        Console.Write("{0} \n{1} \n{2}", ex1.Data.ToString(), ex1.Message, ex.Source);
                        continue;
                 
                    }
                }

                if (isApprox)
                {

               addApproxValue(Approx);
           
                }
                else
                {
                    addClassifierValue(Classifier);
                }
                GC.Collect();

            }

            CleanRepeatesAndNullB_Click(this,null);
        }

        private void PaintAxies(List<double> SourceX, List<double> SourceY, string nameX, string nameY)
        {

            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.Title = nameX;
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.Title = nameY;
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.Minimum = SourceX.Min();
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.Maximum = SourceX.Max();
       
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.Minimum = SourceY.Min();
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.Maximum = SourceY.Max();
            MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;


            MultuFuzzyC.Series.Clear();

    
        }
        private void Drawpoints(List<double> SourceX, List<double> SourceY, string Name, List<double> SourceZ = null, string Name2 = null)
        {
            MultuFuzzyC.Series.Add(Name);
            MultuFuzzyC.Series[Name].ChartArea = "FuzzisChart";
            MultuFuzzyC.Series[Name].ChartType = SeriesChartType.FastPoint;
            MultuFuzzyC.Series[Name].IsVisibleInLegend = true;
            MultuFuzzyC.Series[Name].LegendText = Name;
            if (SourceZ != null)
            {
                MultuFuzzyC.Series.Add(Name2);
                MultuFuzzyC.Series[Name2].ChartArea = "FuzzisChart";
                MultuFuzzyC.Series[Name2].ChartType = SeriesChartType.FastPoint;
                MultuFuzzyC.Series[Name2].IsVisibleInLegend = true;
                MultuFuzzyC.Series[Name2].LegendText = Name2;

            }
            int count = SourceX.Count();
            if (count > SourceY.Count) { count = SourceY.Count; }
            for (int i = 0; i < count; i++)
            {

                MultuFuzzyC.Series[Name].Points.AddXY(SourceX[i], SourceY[i]);
                if (SourceZ != null)
                {
                    MultuFuzzyC.Series[Name2].Points.AddXY(SourceX[i], SourceZ[i]);

                }

            }
        }

        private void TypeGraphCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            XLogaroithmCB.Checked = false;
            YlogorirhmicCB.Checked = false;
          /*
            switch (TypeGraphCB.SelectedIndex)
            {
                case 0:
                    {
                        if (isApprox)
                        {
                            PaintAxies(ValueComplexity, ValueLGoods, "Сложность", "Ошибка RMSE");
                        }
                        else {
                            PaintAxies(ValueComplexity, ValueLGoods, "Сложность", "Точность %");
                
                        }
                        Drawpoints(ValueComplexity, ValueLGoods, "Обучающая выборка", ValueTGoods, "Тестовая выборка");

                    } break;
                case 1:
                    {
                        if (isApprox)
                        {
                            PaintAxies(ValueInterprety, ValueLGoods, "Интерпретируемость", "Ошибка RMSE");
                        }
                        else
                        {
                            PaintAxies(ValueInterprety, ValueLGoods, "Интерпретируемость", "Точность %");
                        
                        }

                        Drawpoints(ValueInterprety, ValueLGoods, "Обучающая выборка", ValueTGoods, "Тестовая выборка");

                    } break;
                case 2:
                    {

                        PaintAxies(ValueInterprety, ValueComplexity, "Интерпретируемость", "Сложность");
                        Drawpoints(ValueInterprety, ValueComplexity, "Нечеткие системы");

                    } break;
                case 3:
                    {

                        if (isApprox)
                        {
                            PaintAxies(ValueLGoods, ValueTGoods, "Ошибка RMSE обучающей", "Ошибка RMSE тестовой");
                        }
                        else
                        {
                            PaintAxies(ValueLGoods, ValueTGoods, "Точность % обучающей", "Точность % тестовой");
                     
                        }                        
                        Drawpoints(ValueLGoods, ValueTGoods, "Нечеткие системы");

                    } break;
            }*/
        }

        private void FuzzyLoadingBW_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadAllFS();
        }

        private void FuzzyLoadingBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadingFuzzyBarPB.MarqueeAnimationSpeed = 0;
            LoadingFuzzyBarPB.Visible = false;
        }

        private void removeByIndex(int i )
        {
            if (isApprox)
            { ValueLGoodsRMSE.RemoveAt(i);
            ValueTGoodsRMSE.RemoveAt(i);
            ValueLGoodsMSE.RemoveAt(i);
            ValueTGoodsMSE.RemoveAt(i);
            
            }
            else
            {
                ValueLGoodsPercent.RemoveAt(i);
                ValueTGoodsPercent.RemoveAt(i);
                ValueLGoodsError.RemoveAt(i);
                ValueTGoodsError.RemoveAt(i);
            }
            ValueComplexityFull.RemoveAt(i);
            ValueComplexityRules.RemoveAt(i);
            ValueInterpretyNominal.RemoveAt(i);
            ValueInterpretyReal.RemoveAt(i);
        }

        private void CleanRepeatesAndNullB_Click(object sender, EventArgs e)
        {
            for (int i = (ValueComplexityRules.Count - 1); i >= 0; i--)
            {

                if (ValueComplexityRules[i] == 0)
                {
                    removeByIndex(i);
                    PathFilesUFS.RemoveAt(i);
                }


                if (isApprox)
                {
                    if (double.IsInfinity(ValueLGoodsRMSE[i]) || double.IsInfinity(ValueTGoodsRMSE[i]))
                    {
                        removeByIndex(i);
                        PathFilesUFS.RemoveAt(i);
                    }
                }
                else
                {
                    if (double.IsInfinity(ValueTGoodsError[i]) || double.IsInfinity(ValueLGoodsError[i]))
                    {
                        removeByIndex(i);
                        PathFilesUFS.RemoveAt(i);
                    }
           
                }



            }
        //    int Selected = TypeGraphCB.SelectedIndex;
       //     TypeGraphCB.SelectedIndex = -1;
       //     TypeGraphCB.SelectedIndex = Selected;

        }

        private void XLogaroithmCB_CheckedChanged(object sender, EventArgs e)
        {
 /*           CheckBox CB = sender as CheckBox;
            if (CB.Checked)
            {
                XlogBaseMTB.Enabled = true;
                MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.IsLogarithmic = true;
                int logBase = 0;
                if (int.TryParse(XlogBaseMTB.Text, out logBase))
                {
                    MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.LogarithmBase = logBase;
                    MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.IsLogarithmic = true;
                }

            }
            else
            {
            
                MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.IsLogarithmic = false;
                XlogBaseMTB.Enabled = false;

                int Selected = TypeGraphCB.SelectedIndex;
                TypeGraphCB.SelectedIndex = -1;
                TypeGraphCB.SelectedIndex = Selected;

            }
            */
        }

        private void YlogorirhmicCB_CheckedChanged(object sender, EventArgs e)
        {
        /*    CheckBox CB = sender as CheckBox;
            if (CB.Checked)
            {
                YlogBaseMTB.Enabled = true;
                MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.IsLogarithmic = true;
                int logBase = 0;
                if (int.TryParse(YlogBaseMTB.Text, out logBase))
                {
                    MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.LogarithmBase = logBase;
                    MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.IsLogarithmic = true;
           
                }

            }
            else
            {
             
                MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.IsLogarithmic = false;
                YlogBaseMTB.Enabled = false;

                int Selected = TypeGraphCB.SelectedIndex;
                TypeGraphCB.SelectedIndex = -1;
                TypeGraphCB.SelectedIndex = Selected;

            }
            */
        }

      
    

        private void YlogBaseMTB_MaskChanged(object sender, EventArgs e)
        {/*
            if (YlogorirhmicCB.Checked)
            {
                int logBase = 0;
                if (int.TryParse(YlogBaseMTB.Text, out logBase))
                {
                    if (logBase > 0)
                    {
                        MultuFuzzyC.ChartAreas["FuzzisChart"].AxisY.LogarithmBase = logBase;

                    }
                }

            }
            */
        }

        private void XlogBaseMTB_MaskChanged(object sender, EventArgs e)
        {
            /*
            if (XLogaroithmCB.Checked)
            {
                int logBase = 0;
                if (int.TryParse(XlogBaseMTB.Text, out logBase))
                {
                    if (logBase > 0)
                    {
                        MultuFuzzyC.ChartAreas["FuzzisChart"].AxisX.LogarithmBase = logBase;

                    }
                }

            }
            */
        }

        private void SaveImgB_Click(object sender, EventArgs e)
        {
         /*  FormWindowState Current =this.WindowState ;
           this.WindowState = FormWindowState.Maximized;

           int Selected = TypeGraphCB.SelectedIndex;
           TypeGraphCB.SelectedIndex = 0;
           MultuFuzzyC.SaveImage(rootDitectory+"\\AllPresiccionComplexity.emf", ChartImageFormat.EmfDual);

           TypeGraphCB.SelectedIndex = 1;
           MultuFuzzyC.SaveImage(rootDitectory + "\\AllPresiccionInterprety.emf", ChartImageFormat.EmfDual);


           TypeGraphCB.SelectedIndex = 2;
           MultuFuzzyC.SaveImage(rootDitectory + "\\ComplexityInterprety.emf", ChartImageFormat.EmfDual);

           TypeGraphCB.SelectedIndex = 3;
           MultuFuzzyC.SaveImage(rootDitectory + "\\LearnTest.emf", ChartImageFormat.EmfDual);


           TypeGraphCB.SelectedIndex = Selected;
           this.WindowState = Current;
        */
        }

        private void SavetoXLS_Click(object sender, EventArgs e)
        {
               Excel.Application exapp = new Excel.Application();
               exapp.Workbooks.Add();
               Excel.Workbook exworkbook = exapp.Workbooks[1];
               Excel.Worksheet sheet = exapp.Sheets[1];
          
            Excel.Range temp = sheet.get_Range("A1");
               temp.Value2 = "Имя файла";

                temp = sheet.get_Range("B1");
            
            if (isApprox)
                {
                    temp.Value2 = "RMSE на обучающей";
                            }
                else
                {
                    temp.Value2 = "% правильный на обучающей";
                }

               temp = sheet.get_Range("C1");

               if (isApprox)
               {
                   temp.Value2 = "RMSE на тестовой";
     
               }
               else
               {
                   temp.Value2 = "% правильный на тестовой";
     
               }

               temp = sheet.get_Range("D1");

               if (isApprox)
               {
                   temp.Value2 = "MSE на обучающей";
               }
               else
               {
                   temp.Value2 = "% ошибки на обучающей";
               }

               temp = sheet.get_Range("E1");

               if (isApprox)
               {
                   temp.Value2 = "MSE на тестовой";

               }
               else
               {
                   temp.Value2 = "% ошибки на тестовой";

               }
            
            temp = sheet.get_Range("F1");
            temp.Value2 = "Сложность термы + правила";

            temp = sheet.get_Range("G1");
            temp.Value2 = "Интерпретируемость нормированный индекс";

            temp = sheet.get_Range("H1");
            temp.Value2 = "Сложность, количество правил";

            temp = sheet.get_Range("I1");
            temp.Value2 = "Интерпретируемость вещественный индекс";


            if (isApprox)
            {
                write_to_columm(sheet, PathFilesUFS, ValueLGoodsRMSE, ValueTGoodsRMSE, ValueLGoodsMSE, ValueTGoodsMSE, ValueComplexityFull, ValueInterpretyNominal, ValueComplexityRules, ValueInterpretyReal, "A", 2);
            }
            else
            {
                write_to_columm(sheet, PathFilesUFS, ValueLGoodsPercent, ValueTGoodsPercent, ValueLGoodsError, ValueTGoodsError, ValueComplexityFull, ValueInterpretyNominal, ValueComplexityRules, ValueInterpretyReal, "A", 2);
          
            }
                if (File.Exists(rootDitectory + "\\Report.xlsx"))
            {
                File.Delete(rootDitectory + "\\Report.xlsx");
            }
            exworkbook.SaveAs(rootDitectory +"\\Report.xlsx");
            exapp.Quit();


        }




        static public void write_to_columm(Excel.Worksheet sheet, List<string> Names, List<double> LearnsR, List<double> TestsR, List<double> LearnsM, List<double> TestsM, List<double> ComplexityF, List<double> InterpretyF, List<double> ComplexityR, List<double> InterpretyR, string liter_dest, int Count_dest)
        {
            string liter = liter_dest;
            writeOneColumn(sheet, liter, Count_dest, Names);
         
            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, LearnsR);
        
      
            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, TestsR);

            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, LearnsM);

            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, TestsM);

            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, ComplexityF);

            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, InterpretyF);


            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, ComplexityR);

            liter = inc_literal(liter);
            writeOneColumn(sheet, liter, Count_dest, InterpretyR);
          }



        static public string inc_literal(string source)
        {
            string result;

            char[] source_char = source.ToArray();
            if (source_char[source_char.Count() - 1] == 'Z')
            {

                source_char[source_char.Count() - 1] = 'A';

                if ((source_char.Count()) - 1 > 0)
                {
                    source_char[source_char.Count() - 2] = (char)((int)(source_char[source_char.Count() - 2]) + 1);
                }
                else
                {
                    string temp = "A";

                    temp += new string(source_char);
                    source_char = temp.ToCharArray();
                }
            }
            else
            {
                source_char[source_char.Count() - 1] = (char)((int)(source_char[source_char.Count() - 1]) + 1);
            }


            result = new string(source_char);
            return result;
        }



        private static void writeOneColumn(Excel.Worksheet Sheet, string Literal, int CountDest, List<string> Source)
      {
          for (int i = 0; i < Source.Count; i++)
          {
              Excel.Range temp = Sheet.get_Range(Literal + (CountDest + i).ToString());
              temp.Value2 = Source[i];

          }
      }

      private static void writeOneColumn(Excel.Worksheet Sheet, string Literal, int CountDest, List<double> Source)
      {
          for (int i = 0; i < Source.Count; i++)
          {
              Excel.Range temp = Sheet.get_Range(Literal + (CountDest + i).ToString());
              temp.Value2 = Source[i];

          }
      }



        private void TypeAccuracyCB_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Properties.Settings.Default.SelectedAccuracy = TypeAccuracyCB.SelectedIndex;
           // Properties.Settings.Default.Save();
        }

        private void TypeComlexCB_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Properties.Settings.Default.SelectedComlexity = TypeComlexCB.SelectedIndex;
           // Properties.Settings.Default.Save();
      
        }

        private void TypeInterpretationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
           // Properties.Settings.Default.SelectedInterpretation = TypeInterpretationCB.SelectedIndex;
           // Properties.Settings.Default.Save();
      
        }



    }
}
