using FuzzySystem.FuzzyAbstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Mix_core.Forms
{
    public partial class Result_F : Form
    {
        public List<double> ApproxLearnResult;
        public List<double> ApproxTestResult;
        public List<double> ApproxLearnResultMSE;
        public List<double> ApproxTestResultMSE;
        public List<double> ApproxLearnResultMSEdiv2; 
        public List<double> ApproxTestResultMSEdiv2; 
        public List<double> ClassLearnResult; 
        public List<double> ClassTestResult; 
        public List<double> ClassErLearn; 
        public List<double> ClassErTest; 

        FuzzySystemRelisedList.TypeSystem TypeFS;
        bool isMultiChoose;
        int countMultiChoosen;
        public string pathtoSave { get; set; }
        public Result_F(FuzzySystemRelisedList.TypeSystem tFS, bool IsMultiChoose, int CountMultiChoosen)
        {
            InitializeComponent();
            TypeFS = tFS;
            isMultiChoose = IsMultiChoose;
            countMultiChoosen = CountMultiChoosen;

            ApproxLearnResult = new List<double>();
      ApproxTestResult = new List<double>();
        ApproxLearnResultMSE = new List<double>();
        ApproxTestResultMSE = new List<double>();
        ApproxLearnResultMSEdiv2 = new List<double>();
         ApproxTestResultMSEdiv2 = new List<double>();
        ClassLearnResult = new List<double>();
        ClassTestResult = new List<double>();
       ClassErLearn = new List<double>();
        ClassErTest = new List<double>();
        }

        private void Result_RTB_TextChanged(object sender, EventArgs e)
        {
            /*   int possition = 0;
               try
               {
                   int firstIndex = Result_RTB.Text.IndexOf(" ", possition, StringComparison.OrdinalIgnoreCase);
                   while (firstIndex >= 0)
                   {
                       int lastindex = Result_RTB.Text.IndexOf(" ", firstIndex + 1, StringComparison.OrdinalIgnoreCase);
                       Result_RTB.Select(firstIndex + 1, lastindex - firstIndex - 1);
                       Result_RTB.SelectionColor = Color.Blue;
                       Result_RTB.SelectionFont = Result_RTB.Font;
                       firstIndex = Result_RTB.Text.IndexOf(" ", lastindex + 1, StringComparison.OrdinalIgnoreCase);
                   }
               }
               catch
               {
               }

               Result_RTB.Select(0, 0);*/
        }

        private void Result_F_Shown(object sender, EventArgs e)
        {

            if ((TypeFS == FuzzySystemRelisedList.TypeSystem.Singletone) || (TypeFS == FuzzySystemRelisedList.TypeSystem.TakagiSugenoApproximate))
            {


                копироватьПравильныйToolStripMenuItem.Text = "Копировать RMSE";
                копироватьПравильныйToolStripMenuItem.Click += (object sen, EventArgs eve) =>
                {
                    PutText(ApproxLearnResult, ApproxTestResult);
                };

                копироватьОшибкиToolStripMenuItem.Text = "Копировать MSE";
                копироватьОшибкиToolStripMenuItem.Click += (object sen, EventArgs eve) =>
                {
                    PutText(ApproxLearnResultMSE, ApproxTestResultMSE);
                };

                копироватьMSE2ToolStripMenuItem.Text = "Копировать MSE/2";
                копироватьMSE2ToolStripMenuItem.Click += (object sen, EventArgs eve) =>
                {
                    PutText(ApproxLearnResultMSEdiv2, ApproxTestResultMSEdiv2);
                };
            }
            if (TypeFS == FuzzySystemRelisedList.TypeSystem.PittsburghClassifier)
            {
                копироватьПравильныйToolStripMenuItem.Text = "Копировать точности";
                копироватьПравильныйToolStripMenuItem.Click += (object sen, EventArgs eve) =>
                {
                    PutText(ClassLearnResult, ClassTestResult);
                };

                копироватьОшибкиToolStripMenuItem.Text = "Копировать ошибки";
                копироватьОшибкиToolStripMenuItem.Click += (object sen, EventArgs eve) =>
                {
                    PutText(ClassErLearn, ClassErTest);
                };
                копироватьMSE2ToolStripMenuItem.Visible = false;

            }

        }


        void PutText(List<double> Source, List<double> Source2)
        {
            string s = "";
            int EachNum = int.Parse(EachNumTB.Text);
            int countNum = 1;
            if (isMultiChoose)
            {
                countNum = Source.Count / countMultiChoosen;
            }
            double meanLearn = 0.0;
            double meanTest = 0.0;
            int counter = 0;
            int result;
            for (int i = 0; i < Source.Count; i++)
            {
                Math.DivRem(i + 1, EachNum, out result);
                if (result == 0)

                {
                    s += (Source[i].ToString() + "\t" + Source2[i].ToString() + Environment.NewLine);
                    meanLearn += Source[i];
                    meanTest += Source2[i];
                    counter++;
                }
                if (isMultiChoose)
                {


                    Math.DivRem(i + 1, countNum, out result);
                    if (result == 0)
                    {
                        s += Environment.NewLine;
                        s += ((meanLearn / (double)counter).ToString() + "\t" + (meanTest / (double)counter).ToString() + Environment.NewLine);
                        s += Environment.NewLine;
                        meanLearn = 0.0;
                        meanTest = 0.0;
                        counter = 0;
                    }
                }
            }
            if (!String.IsNullOrWhiteSpace(s))
                Clipboard.SetText(s);
        }

        private void timerSaveTimer_Tick(object sender, EventArgs e)
        {
            if (pathtoSave != null)
            {
                Result_RTB.SaveFile(pathtoSave, RichTextBoxStreamType.UnicodePlainText);
            }
        }
    }
}
