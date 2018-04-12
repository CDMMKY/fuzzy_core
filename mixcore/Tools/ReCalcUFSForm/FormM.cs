using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ReCalcUFSForm
{
    public partial class MainF : Form
    {
        List<string> UFSStorage = new List<string>();
     //   int completedProgress = 0;
        ListOfParams Saver;
        string sourceOutText = "";
        int status = -1;
   //     int maxCountItems = 0;
        public MainF()
        {
            InitializeComponent();
        }

        private void BrowseB_Click(object sender, EventArgs e)
        {
            UFSBrowseDirectory.SelectedPath = Properties.Settings.Default.LastPath;
            if (!Directory.Exists(UFSBrowseDirectory.SelectedPath))
            {
                UFSBrowseDirectory.SelectedPath = "";
            }

            if (UFSBrowseDirectory.ShowDialog() == DialogResult.OK)
            {
                UFSStorage = Directory.GetFiles(UFSBrowseDirectory.SelectedPath, "*.ufs", SearchOption.AllDirectories).ToList();
                Properties.Settings.Default.LastPath = UFSBrowseDirectory.SelectedPath;
                Properties.Settings.Default.Save();
                if (UFSStorage.Count < 1) return;
                ProgressChecker_T.Enabled = true;

                ProgressL.Visible = true;
                ShowProgressPB.Visible = true;
                CompletedL.Visible = true;
                CompleteStatusL.Visible = true;

                backgroundSunShine.RunWorkerAsync();

            }

        }

        private void backgroundSunShine_DoWork(object sender, DoWorkEventArgs e)
        {
            Saver = new ListOfParams();

            backgroundSunShine.ReportProgress(0);

            Saver.loadData(UFSStorage);
            backgroundSunShine.ReportProgress(1);

            Saver.Init();
            backgroundSunShine.ReportProgress(2);

            Task[] Tasks = new Task[2];

            Tasks[0] = Saver.savetoXLS(UFSBrowseDirectory.SelectedPath, "InterpretyXLS.xlsx");

       
            Tasks[1] =  Saver.savetoTXT(UFSBrowseDirectory.SelectedPath, "InterpretyNormal.txt", "InterpretySumStraigth.txt", "InterpretySumReverce.txt");
            Tasks[0].Start();
            Tasks[1].Start();
           Task.WaitAll(Tasks);
            
         

        }


        private void makeStatusString(int status, int completed, int Count,int comleted1= 0)
        {
            switch (status)
            {
                case 0: sourceOutText = "Проводиться загрузка файлов: " + completed + "/" + Count; break;
                case 1: sourceOutText = "Получаются свойства нечетких систем: " + completed + "/" + Count; break;
                case 2: sourceOutText = "Данные о нечетких системах сохраняются в XLSX: " + comleted1 + "/18" + Environment.NewLine;
                        sourceOutText+="Данные сохраняются в текстовые файлы: " + completed + "/" + (Count*6).ToString(); break;
           
                default: sourceOutText = ""; break;
            }
        }

        private void backgroundSunShine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            status = e.ProgressPercentage;

        }

        private void backgroundSunShine_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressChecker_T.Enabled = false;
            ProgressL.Visible = false;
            ShowProgressPB.Visible = false;
            CompleteStatusL.Text = "Завершено";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int completedProgress = 0;
            int completedColums = 0;
            if (Saver != null)
            {
                completedProgress = Saver.CompletedFile;
                completedColums = Saver.CompletedColums;
            }

            makeStatusString(status, completedProgress, UFSStorage.Count,completedColums);
            CompleteStatusL.Text = sourceOutText;
        }
    }
}
