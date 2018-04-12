using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using FuzzySystem.FuzzyAbstract;
using FuzzySystem.TakagiSugenoApproximate;
using System.Windows.Forms.DataVisualization.Charting;
using FuzzySystem.SingletoneApproximate;
using FuzzyCore.FuzzySystem.FuzzyAbstract;
using FuzzySystem.PittsburghClassifier;

namespace DrawMe
{
    public partial class StartF : Form
    {
        Color[] palete = new Color[20];
        int CurrentColor = 0;
        PCFuzzySystem PCFS;
        SAFuzzySystem SAFS;
        TSAFuzzySystem TSAFS;
        IFuzzySystem FSystem;
        FuzzySystem.FuzzyAbstract.FuzzySystemRelisedList.TypeSystem TSystem;

        SampleSet tempTable, temptestTable;
        string[][] NamesOfTerms;
        int[][] indexofTerm;
        public StartF()
        {

            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BrowseB_Click(object sender, EventArgs e)
        {
            if (openFSDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFSDialog.FileName;
                if (File.Exists(fileName))
                {
                    tempTable = BaseUFSLoader.LoadLearnFromUFS(fileName);

                    temptestTable = BaseUFSLoader.LoadTestFromUFS(fileName);

                    FSystem = BaseUFSLoader.LoadUFS(fileName, out TSystem);

                    switch (TSystem)
                    {
                        case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                            {
                                PCFS = FSystem as PCFuzzySystem;

                                break;
                            }
                        case FuzzySystemRelisedList.TypeSystem.Singletone:
                            {
                                SAFS = FSystem as SAFuzzySystem;
                                break;
                            }
                        case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                            {
                                TSAFS = FSystem as TSAFuzzySystem;

                                break;
                            }
                    }


                    NamesOfTerms = new String[getCountVars()][];
                    indexofTerm = new int[getCountVars()][];
                    makeNamesforAll();

                    FeatureCB.Items.Clear();
                    for (int i = 0; i < getCountVars(); i++)
                    {
                        FeatureCB.Items.Add(getNameAttribute(i));
                    }
                    FeatureCB.SelectedIndex = 0;
                    MakeRulesB.Enabled = true;
                    RulesRTB.Text = "";
                }

            }
        }

        private void FeatureCB_SelectedIndexChanged(object sender, EventArgs e)
        {
              ComboBox current = sender as ComboBox;
              int selectedIndex = current.SelectedIndex;

              FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Title = getNameAttribute(selectedIndex);
              FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Minimum = getMinAttribute(selectedIndex) - (getScatterAttribute(selectedIndex) / 20);
              FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Maximum = getMaxAttribute(selectedIndex) + (getScatterAttribute(selectedIndex) / 20);
              FuzyChart.ChartAreas["ChartFuzzy"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
              FuzyChart.Series.Clear();

              addMinLine(selectedIndex);
              addMaxLine(selectedIndex);


                  List<Term> sourceTerms =getTerm(selectedIndex);

                  for (int i = 0; i < sourceTerms.Count(); i++)
                  {
                      int indexRecognize = getIndexinTermSet(sourceTerms[i]) + 1;
                      double[] paramss = getTermParams(sourceTerms[i]);
                      int typeTerm = gettypeTerm(sourceTerms[i]);

                      drawTerm(sourceTerms[i], indexRecognize, typeTerm, NamesOfTerms[selectedIndex][i]);

                  }
              
              FuzyChart.Invalidate();
              FuzyChart.Visible = true;
              SaveToPngB.Enabled = true;
          //  MessageBox.Show(FuzyChart.Legends[0].CellColumns.Count.ToString());

        }

        private void FuzyChart_Click(object sender, EventArgs e)
        {

        }


        private void addMinLine(int selectedIndex)
        {

            FuzyChart.Series.Add("min");
            FuzyChart.Series["min"].ChartArea = "ChartFuzzy";
            FuzyChart.Series["min"].ChartType = SeriesChartType.FastLine;
            FuzyChart.Series["min"].Color = Color.Gray;
            FuzyChart.Series["min"].BorderDashStyle = ChartDashStyle.DashDotDot;

            FuzyChart.Series["min"].ToolTip = getMinAttribute(selectedIndex).ToString();
            FuzyChart.Series["min"].IsVisibleInLegend = true;
            FuzyChart.Series["min"].LegendText = "Минимум";
            FuzyChart.Series["min"].LegendToolTip = "Минимум входного признака";
            FuzyChart.Series["min"].Points.AddXY(getMinAttribute(selectedIndex), 0);
            FuzyChart.Series["min"].Points.AddXY(getMinAttribute(selectedIndex), 1);



        }
        private void addMaxLine(int selectedIndex)
        {
            
            FuzyChart.Series.Add("max");
            FuzyChart.Series["max"].ChartArea = "ChartFuzzy";
            FuzyChart.Series["max"].ChartType = SeriesChartType.FastLine;
            FuzyChart.Series["max"].Color = Color.Gray;
            FuzyChart.Series["max"].BorderDashStyle = ChartDashStyle.DashDotDot;

            FuzyChart.Series["max"].ToolTip = getMaxAttribute(selectedIndex).ToString();
            FuzyChart.Series["max"].IsVisibleInLegend = true;
            FuzyChart.Series["max"].LegendText = "Максимум";
            FuzyChart.Series["max"].LegendToolTip = "Максимум входного признака";
            FuzyChart.Series["max"].Points.AddXY(getMaxAttribute(selectedIndex), 0);
            FuzyChart.Series["max"].Points.AddXY(getMaxAttribute(selectedIndex), 1);
            
        }

        private void drawTerm(Term paramss, int indexRecognize, int typeTerm, string Name)
        {
            FuzyChart.Series.Add(Name);
            FuzyChart.Series[Name].ChartArea = "ChartFuzzy";
            FuzyChart.Series[Name].ChartType = SeriesChartType.FastLine;
            FuzyChart.Series[Name].IsVisibleInLegend = true;
            FuzyChart.Series[Name].LegendText = Name;
            FuzyChart.Series[Name].BorderWidth = 4;
            FuzyChart.Legends[0].LegendStyle = LegendStyle.Column;
            FuzyChart.Legends[0].Alignment = StringAlignment.Center;
          

          //  FuzyChart.Legends[0].BorderDashStyle = ChartDashStyle.Solid;
          //    FuzyChart.Legends[0].BorderWidth = 20;
            switch (typeTerm)
            {
                case 0: { drawTriangle(paramss, Name, indexRecognize); break; }
                case 3: { drawTrapecce(paramss, Name, indexRecognize); break; }
                case 2: { drawParabola(paramss, Name, typeTerm, indexRecognize); break; }
                case 1: { drawGauss(paramss, Name, typeTerm, indexRecognize); break; }
            }
                 }

        private void drawTriangle(Term paramss, string Name, int index)
        {
            FuzyChart.Series[Name].ToolTip = "Треугольный терм " + index.ToString();
            FuzyChart.Series[Name].LegendToolTip = "Треугольный терм " + index.ToString();
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[0], 0);
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[1], 1);
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[2], 0);

        }

        private void drawTrapecce(Term paramss, string Name, int index)
        {
            FuzyChart.Series[Name].ToolTip = "Трапецивидный терм " + index.ToString();
            FuzyChart.Series[Name].LegendToolTip = "Трапецивидный терм " + index.ToString();
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[0], 0);
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[1], 1);
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[2], 1);
            FuzyChart.Series[Name].Points.AddXY(paramss.Parametrs[3], 0);

        }
        private void drawParabola(Term paramss, string Name, int typeTerm, int index)
        {
            FuzyChart.Series[Name].ToolTip = "Параболический терм " + index.ToString();
            FuzyChart.Series[Name].LegendToolTip = "Параболический терм " + index.ToString();

            double left = paramss.Parametrs[0];
            double spin = (paramss.Parametrs[1] - paramss.Parametrs[0]) / 99;
          
            for (int i = 0; i < 100; i++)
            {
                FuzyChart.Series[Name].Points.AddXY(left, paramss.LevelOfMembership(left));
                left += spin;
            }
            
        }
        private void drawGauss(Term paramss, string Name, int typeTerm, int index)
        {
             FuzyChart.Series[Name].ToolTip = "Гауссовый терм " + index.ToString();
             FuzyChart.Series[Name].LegendToolTip = "Гауссовый терм " + index.ToString();

             double left = FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Minimum;
             double spin = (FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Maximum - FuzyChart.ChartAreas["ChartFuzzy"].AxisX.Minimum) / 499;
             
             for (int i = 0; i < 500; i++)
             {
              
                 FuzyChart.Series[Name].Points.AddXY(left, paramss.LevelOfMembership(left));
                 left += spin;
             } 
        }


        private string NumtoLetter(int Value)
        {
            string result = "A";
            char[] source_char = result.ToArray();
            source_char[source_char.Count() - 1] = (char)((int)(source_char[source_char.Count() - 1]) + Value);
            result = new string(source_char);
            return result;
        }


        public class TermComp : IComparer<Term>
        {
          
            public int Compare(Term x, Term y)
            {
                double xCenter = getValueofCenter(x);
                double yCenter = getValueofCenter(y);
                if (xCenter == yCenter) { return 0; }
                if (xCenter < yCenter) { return -1; }
                if (xCenter > yCenter) { return 1; }

                return 0;
            }

            private double getValueofCenter(Term Term)
            {
                double Center = 0;
                switch (Term.TermFuncType)
                {
                    case  TypeTermFuncEnum.Треугольник: { Center = Term.Parametrs[1]; break; }
                    case TypeTermFuncEnum.Трапеция: { Center = (Term.Parametrs[1] + Term.Parametrs[2]) / 2; break; }
                    case TypeTermFuncEnum.Парабола: { Center = (Term.Parametrs[1] + Term.Parametrs[2]) / 2; break; }
                    case TypeTermFuncEnum.Гауссоида: { Center = Term.Parametrs[0]; break; }
                }
                return Center;

            }

        }

      




        private void StartF_Load(object sender, EventArgs e)
        {
            palete[0] = Color.AliceBlue;
            palete[1] = Color.AntiqueWhite;
            palete[2] = Color.Aqua;
            palete[3] = Color.Aquamarine;
            palete[4] = Color.Azure;
            palete[5] = Color.Beige;
            palete[6] = Color.Bisque;
            palete[7] = Color.Black;
            palete[8] = Color.BlanchedAlmond;
            palete[9] = Color.Blue;
            palete[10] = Color.BlueViolet;
            palete[11] = Color.Brown;
            palete[12] = Color.BurlyWood;
            palete[13] = Color.CadetBlue;
            palete[14] = Color.Chartreuse;
            palete[15] = Color.Chocolate;
            palete[16] = Color.Coral;
            palete[17] = Color.CornflowerBlue;
            palete[18] = Color.Cornsilk;
            palete[19] = Color.Crimson;
            //  palete[20] = Color.Beige;
            //  palete[21] = Color.Beige;
            //  palete[22] = Color.Beige;

        }



        private Color getColor()
        {
            Color result = palete[CurrentColor];
            CurrentColor++;
            if (CurrentColor >= 20) { CurrentColor = 0; }
            return result;
        }

        private string indexofColor()
        {
            return CurrentColor.ToString();
        }

        private void SaveToPngB_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FuzyChart.SaveImage(saveFileDialog1.FileName, ChartImageFormat.EmfDual);
            }
        }

        private void MakeRulesB_Click(object sender, EventArgs e)
        { 
            RulesRTB.Text = "";
            for (int j = 0; j < getcountRulesinDataBase(); j++)
            {
                RulesRTB.Text += "Правило " + (j + 1).ToString() + ": ЕСЛИ  ";
                for (int i = 0; i < getcountTermsinRule(j); i++)
                {
                       Term currentTerm = getTerminRulesDataBase(j,i);
                        List<Term> sourceTerms = getTerm(currentTerm.NumVar);
                        int currentindex = sourceTerms.IndexOf(currentTerm);

                        RulesRTB.Text += getNameAttribute(currentTerm.NumVar).ToString() + " = " + NamesOfTerms[currentTerm.NumVar][currentindex];
                        if (i < (getcountTermsinRule(j) - 1))
                        {
                            RulesRTB.Text += " И ";
                        }
          
                }
                switch (TSystem)
                { 
                    case  FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                {
                    RulesRTB.Text += " ТО Класс = " + PCFS.RulesDatabaseSet[0].RulesDatabase[j].LabelOfClass;
                    RulesRTB.Text += ", Вес = " + PCFS.RulesDatabaseSet[0].RulesDatabase[j].CF.ToString() + Environment.NewLine;
                    break;
                }

                    case FuzzySystemRelisedList.TypeSystem.Singletone:
                {
                    RulesRTB.Text += " ТО Y = " + SAFS.RulesDatabaseSet[0].RulesDatabase[j].IndependentConstantConsequent.ToString();
                    RulesRTB.Text +=  Environment.NewLine;
                    break;
                
                }
                    case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                {

                    RulesRTB.Text += " ТО Y = " + TSAFS.RulesDatabaseSet[0].RulesDatabase[j].IndependentConstantConsequent.ToString();
                    for (int i = 0; i < getCountVars(); i++)
                    {
                                if (TSAFS.RulesDatabaseSet[0].RulesDatabase[j].RegressionConstantConsequent[i] >= 0)
                                { RulesRTB.Text += " +"; }
                                RulesRTB.Text += " " + TSAFS.RulesDatabaseSet[0].RulesDatabase[j].RegressionConstantConsequent[i].ToString() + " * " + getNameAttribute(i);
                    }
                    RulesRTB.Text += Environment.NewLine;
                    break;
                }
            }
            }
            
        }



        private void makeNamesforAll()
        {
            for (int i = 0; i < getCountVars(); i++)
            {
                makeforfeature(i);
            }
        }

        private void makeforfeature(int feature)
        {

                var sourceTerms = getTerm(feature);

                NamesOfTerms[feature] = new string[TermListCount(sourceTerms)];
                indexofTerm[feature] = new int[TermListCount(sourceTerms)];
                for (int j = 0; j < sourceTerms.Count; j++)
                {
                    indexofTerm[feature][j] = getIndexinTermSet(getTerminList(sourceTerms, j));
                    NamesOfTerms[feature][j] = NumtoLetter(feature) + (j + 1).ToString();
                }
            
           
        }

        private int getCountVars()
        {
            return FSystem.CountFeatures;
        }

       
        private List<Term> getTerm(int feature)
        {
            try
            {
                List<Term> sourceTerms=null;
                 switch (TSystem)
            {
                case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                    {
                        sourceTerms= PCFS.RulesDatabaseSet[0].TermsSet.Where(x=>x.NumVar==feature).ToList();
                        break;
                    }
                case FuzzySystemRelisedList.TypeSystem.Singletone:
                    {
                        sourceTerms =SAFS.RulesDatabaseSet[0].TermsSet.Where(x=>x.NumVar==feature).ToList();
                        break;
                    }
                case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                    {
                        sourceTerms =TSAFS.RulesDatabaseSet[0].TermsSet.Where(x=>x.NumVar==feature).ToList();
                        break;
                    }
            }
                
                
                IComparer<Term> toSort = new TermComp();
                sourceTerms.Sort(toSort);
                return sourceTerms;
            }
            catch (Exception) { return null; }
        }

        private int getIndexinTermSet(Term Term)
        {
            switch (TSystem)
            {
                case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                    {
                        return PCFS.RulesDatabaseSet[0].TermsSet.IndexOf(Term);
                    
                    }
                case FuzzySystemRelisedList.TypeSystem.Singletone:
                    {
                        return SAFS.RulesDatabaseSet[0].TermsSet.IndexOf(Term);
                    }
                case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                    {
                        return TSAFS.RulesDatabaseSet[0].TermsSet.IndexOf(Term);
                     
                    }
            }

            return -1;
        }



        private int TermListCount(List<Term> sourceTerm)
        {
            return sourceTerm.Count();
        }

        private Term getTerminList(List<Term> sourceList, int j)
        {
            return sourceList[j];
        }
        

        private string getNameAttribute(int i)
        {
            return FSystem.LearnSamplesSet.InputAttributes[i].Name;

        }

        private double getMinAttribute(int i)
        {
            return FSystem.LearnSamplesSet.InputAttributes[i].Min;

        }

        private double getMaxAttribute(int i)
        {
            return FSystem.LearnSamplesSet.InputAttributes[i].Max;

        }

        private double getScatterAttribute(int i)
        {
            return FSystem.LearnSamplesSet.InputAttributes[i].Scatter;
        }

        private double[] getTermParams(Term Term)
        {
            return Term.Parametrs;
        }
        private int gettypeTerm(Term Term)
        {
            return (int)Term.TermFuncType;
        }



        private Term getTerminRulesDataBase(int numberofRule, int numberofTerm)
        {
            switch (TSystem)
            {
                case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                    {
                        return PCFS.RulesDatabaseSet[0].RulesDatabase[numberofRule].ListTermsInRule[numberofTerm];
                       
                    }
                case FuzzySystemRelisedList.TypeSystem.Singletone:
                    {
                        return SAFS.RulesDatabaseSet[0].RulesDatabase[numberofRule].ListTermsInRule[numberofTerm];
                    }
                case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                    {
                        return TSAFS.RulesDatabaseSet[0].RulesDatabase[numberofRule].ListTermsInRule[numberofTerm];
                     
                    }
            }


            return null;
        }

        private int getcountRulesinDataBase()
        {
            switch (TSystem)
            {
                case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                    {
                        return PCFS.RulesDatabaseSet[0].RulesDatabase.Count;
                    
                    }
                case FuzzySystemRelisedList.TypeSystem.Singletone:
                    {
                        return SAFS.RulesDatabaseSet[0].RulesDatabase.Count;
                    }
                case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                    {
                        return TSAFS.RulesDatabaseSet[0].RulesDatabase.Count();
                      
                    }
            }


            return 0;
        }

        private int getcountTermsinRule(int i)
        {
            switch (TSystem)
            {
                case FuzzySystemRelisedList.TypeSystem.PittsburghClassifier:
                    {
                        return PCFS.RulesDatabaseSet[0].RulesDatabase[i].ListTermsInRule.Count;
                      
                    }
                case FuzzySystemRelisedList.TypeSystem.Singletone:
                    {
                        return SAFS.RulesDatabaseSet[0].RulesDatabase[i].ListTermsInRule.Count;
                    }
                case FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate:
                    {
                        return TSAFS.RulesDatabaseSet[0].RulesDatabase[i].ListTermsInRule.Count;
                       
                    }
            }

            return 0;

        }
    }
}
