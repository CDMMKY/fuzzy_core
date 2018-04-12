namespace DrawMeMultuGoal
{
    partial class MainF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.DrawAndOptionsP = new System.Windows.Forms.Panel();
            this.OptionsorDrawSC = new System.Windows.Forms.SplitContainer();
            this.YlogBaseMTB = new System.Windows.Forms.MaskedTextBox();
            this.YlogorirhmicCB = new System.Windows.Forms.CheckBox();
            this.XlogBaseMTB = new System.Windows.Forms.MaskedTextBox();
            this.XLogaroithmCB = new System.Windows.Forms.CheckBox();
            this.SavetoXLS = new System.Windows.Forms.Button();
            this.SaveImgB = new System.Windows.Forms.Button();
            this.CleanRepeatesAndNullB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TypeGraphCB = new System.Windows.Forms.ComboBox();
            this.MultuFuzzyC = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.FuzzyUFSFOD = new System.Windows.Forms.FolderBrowserDialog();
            this.ExcelSaveD = new System.Windows.Forms.SaveFileDialog();
            this.LoadingFuzzyBarPB = new System.Windows.Forms.ProgressBar();
            this.FuzzyLoadingBW = new System.ComponentModel.BackgroundWorker();
            this.DrawAndOptionsP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OptionsorDrawSC)).BeginInit();
            this.OptionsorDrawSC.Panel1.SuspendLayout();
            this.OptionsorDrawSC.Panel2.SuspendLayout();
            this.OptionsorDrawSC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MultuFuzzyC)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(804, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Обзор папки с UFS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DrawAndOptionsP
            // 
            this.DrawAndOptionsP.Controls.Add(this.OptionsorDrawSC);
            this.DrawAndOptionsP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawAndOptionsP.Location = new System.Drawing.Point(0, 42);
            this.DrawAndOptionsP.Name = "DrawAndOptionsP";
            this.DrawAndOptionsP.Size = new System.Drawing.Size(804, 495);
            this.DrawAndOptionsP.TabIndex = 1;
            // 
            // OptionsorDrawSC
            // 
            this.OptionsorDrawSC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OptionsorDrawSC.Location = new System.Drawing.Point(0, 0);
            this.OptionsorDrawSC.Name = "OptionsorDrawSC";
            // 
            // OptionsorDrawSC.Panel1
            // 
            this.OptionsorDrawSC.Panel1.Controls.Add(this.YlogBaseMTB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.YlogorirhmicCB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.XlogBaseMTB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.XLogaroithmCB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.SavetoXLS);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.SaveImgB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.CleanRepeatesAndNullB);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.label1);
            this.OptionsorDrawSC.Panel1.Controls.Add(this.TypeGraphCB);
            // 
            // OptionsorDrawSC.Panel2
            // 
            this.OptionsorDrawSC.Panel2.Controls.Add(this.MultuFuzzyC);
            this.OptionsorDrawSC.Size = new System.Drawing.Size(804, 495);
            this.OptionsorDrawSC.SplitterDistance = 186;
            this.OptionsorDrawSC.TabIndex = 0;
            // 
            // YlogBaseMTB
            // 
            this.YlogBaseMTB.Enabled = false;
            this.YlogBaseMTB.Location = new System.Drawing.Point(121, 133);
            this.YlogBaseMTB.Mask = "00";
            this.YlogBaseMTB.Name = "YlogBaseMTB";
            this.YlogBaseMTB.Size = new System.Drawing.Size(57, 20);
            this.YlogBaseMTB.TabIndex = 8;
            this.YlogBaseMTB.Text = "10";
            this.YlogBaseMTB.MaskChanged += new System.EventHandler(this.YlogBaseMTB_MaskChanged);
            // 
            // YlogorirhmicCB
            // 
            this.YlogorirhmicCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.YlogorirhmicCB.AutoSize = true;
            this.YlogorirhmicCB.Enabled = false;
            this.YlogorirhmicCB.Location = new System.Drawing.Point(3, 133);
            this.YlogorirhmicCB.Name = "YlogorirhmicCB";
            this.YlogorirhmicCB.Size = new System.Drawing.Size(91, 17);
            this.YlogorirhmicCB.TabIndex = 7;
            this.YlogorirhmicCB.Text = "Y лог. шкала";
            this.YlogorirhmicCB.UseVisualStyleBackColor = true;
            this.YlogorirhmicCB.CheckedChanged += new System.EventHandler(this.YlogorirhmicCB_CheckedChanged);
            // 
            // XlogBaseMTB
            // 
            this.XlogBaseMTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XlogBaseMTB.Enabled = false;
            this.XlogBaseMTB.Location = new System.Drawing.Point(121, 83);
            this.XlogBaseMTB.Mask = "00";
            this.XlogBaseMTB.Name = "XlogBaseMTB";
            this.XlogBaseMTB.Size = new System.Drawing.Size(55, 20);
            this.XlogBaseMTB.TabIndex = 6;
            this.XlogBaseMTB.Text = "10";
            this.XlogBaseMTB.MaskChanged += new System.EventHandler(this.XlogBaseMTB_MaskChanged);
            // 
            // XLogaroithmCB
            // 
            this.XLogaroithmCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XLogaroithmCB.AutoSize = true;
            this.XLogaroithmCB.Enabled = false;
            this.XLogaroithmCB.Location = new System.Drawing.Point(3, 83);
            this.XLogaroithmCB.Name = "XLogaroithmCB";
            this.XLogaroithmCB.Size = new System.Drawing.Size(91, 17);
            this.XLogaroithmCB.TabIndex = 5;
            this.XLogaroithmCB.Text = "X лог. шкала";
            this.XLogaroithmCB.UseVisualStyleBackColor = true;
            this.XLogaroithmCB.CheckedChanged += new System.EventHandler(this.XLogaroithmCB_CheckedChanged);
            // 
            // SavetoXLS
            // 
            this.SavetoXLS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SavetoXLS.Location = new System.Drawing.Point(4, 282);
            this.SavetoXLS.Name = "SavetoXLS";
            this.SavetoXLS.Size = new System.Drawing.Size(172, 48);
            this.SavetoXLS.TabIndex = 4;
            this.SavetoXLS.Text = "Сохранить таблицу";
            this.SavetoXLS.UseVisualStyleBackColor = true;
            this.SavetoXLS.Click += new System.EventHandler(this.SavetoXLS_Click);
            // 
            // SaveImgB
            // 
            this.SaveImgB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveImgB.Enabled = false;
            this.SaveImgB.Location = new System.Drawing.Point(4, 225);
            this.SaveImgB.Name = "SaveImgB";
            this.SaveImgB.Size = new System.Drawing.Size(172, 50);
            this.SaveImgB.TabIndex = 3;
            this.SaveImgB.Text = "Сохранить графики";
            this.SaveImgB.UseVisualStyleBackColor = true;
            this.SaveImgB.Click += new System.EventHandler(this.SaveImgB_Click);
            // 
            // CleanRepeatesAndNullB
            // 
            this.CleanRepeatesAndNullB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CleanRepeatesAndNullB.Enabled = false;
            this.CleanRepeatesAndNullB.Location = new System.Drawing.Point(4, 166);
            this.CleanRepeatesAndNullB.Name = "CleanRepeatesAndNullB";
            this.CleanRepeatesAndNullB.Size = new System.Drawing.Size(172, 53);
            this.CleanRepeatesAndNullB.TabIndex = 2;
            this.CleanRepeatesAndNullB.Text = "Удалить нулевые системы";
            this.CleanRepeatesAndNullB.UseVisualStyleBackColor = true;
            this.CleanRepeatesAndNullB.Click += new System.EventHandler(this.CleanRepeatesAndNullB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Тип графика";
            // 
            // TypeGraphCB
            // 
            this.TypeGraphCB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypeGraphCB.Enabled = false;
            this.TypeGraphCB.FormattingEnabled = true;
            this.TypeGraphCB.Items.AddRange(new object[] {
            "Сложность-ТочностьО",
            "Интерпретируемость-ТочностьО",
            "Сложность-Интерпретируемость",
            "ТочностьО-ТочностьТ"});
            this.TypeGraphCB.Location = new System.Drawing.Point(3, 34);
            this.TypeGraphCB.Name = "TypeGraphCB";
            this.TypeGraphCB.Size = new System.Drawing.Size(169, 21);
            this.TypeGraphCB.TabIndex = 0;
            this.TypeGraphCB.SelectedIndexChanged += new System.EventHandler(this.TypeGraphCB_SelectedIndexChanged);
            // 
            // MultuFuzzyC
            // 
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.Name = "FuzzisChart";
            this.MultuFuzzyC.ChartAreas.Add(chartArea1);
            this.MultuFuzzyC.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.MultuFuzzyC.Legends.Add(legend1);
            this.MultuFuzzyC.Location = new System.Drawing.Point(0, 0);
            this.MultuFuzzyC.Name = "MultuFuzzyC";
            this.MultuFuzzyC.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series1.ChartArea = "FuzzisChart";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.Name = "MultySerries";
            this.MultuFuzzyC.Series.Add(series1);
            this.MultuFuzzyC.Size = new System.Drawing.Size(614, 495);
            this.MultuFuzzyC.TabIndex = 0;
            this.MultuFuzzyC.Text = "chart1";
            // 
            // FuzzyUFSFOD
            // 
            this.FuzzyUFSFOD.Description = "Выберите папку содержащую UFS";
            // 
            // LoadingFuzzyBarPB
            // 
            this.LoadingFuzzyBarPB.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoadingFuzzyBarPB.Location = new System.Drawing.Point(0, 23);
            this.LoadingFuzzyBarPB.MarqueeAnimationSpeed = 0;
            this.LoadingFuzzyBarPB.Name = "LoadingFuzzyBarPB";
            this.LoadingFuzzyBarPB.Size = new System.Drawing.Size(804, 19);
            this.LoadingFuzzyBarPB.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.LoadingFuzzyBarPB.TabIndex = 2;
            // 
            // FuzzyLoadingBW
            // 
            this.FuzzyLoadingBW.WorkerReportsProgress = true;
            this.FuzzyLoadingBW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.FuzzyLoadingBW_DoWork);
            this.FuzzyLoadingBW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.FuzzyLoadingBW_RunWorkerCompleted);
            // 
            // MainF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(804, 537);
            this.Controls.Add(this.DrawAndOptionsP);
            this.Controls.Add(this.LoadingFuzzyBarPB);
            this.Controls.Add(this.button1);
            this.Name = "MainF";
            this.Text = "Построение графиков Парето";
            this.DrawAndOptionsP.ResumeLayout(false);
            this.OptionsorDrawSC.Panel1.ResumeLayout(false);
            this.OptionsorDrawSC.Panel1.PerformLayout();
            this.OptionsorDrawSC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OptionsorDrawSC)).EndInit();
            this.OptionsorDrawSC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MultuFuzzyC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel DrawAndOptionsP;
        private System.Windows.Forms.SplitContainer OptionsorDrawSC;
        private System.Windows.Forms.ComboBox TypeGraphCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SavetoXLS;
        private System.Windows.Forms.Button SaveImgB;
        private System.Windows.Forms.Button CleanRepeatesAndNullB;
        private System.Windows.Forms.DataVisualization.Charting.Chart MultuFuzzyC;
        private System.Windows.Forms.MaskedTextBox XlogBaseMTB;
        private System.Windows.Forms.CheckBox XLogaroithmCB;
        private System.Windows.Forms.FolderBrowserDialog FuzzyUFSFOD;
        private System.Windows.Forms.SaveFileDialog ExcelSaveD;
        private System.Windows.Forms.MaskedTextBox YlogBaseMTB;
        private System.Windows.Forms.CheckBox YlogorirhmicCB;
        private System.Windows.Forms.ProgressBar LoadingFuzzyBarPB;
        private System.ComponentModel.BackgroundWorker FuzzyLoadingBW;
    }
}

