namespace DrawMe
{
    partial class StartF
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
            System.Windows.Forms.DataVisualization.Charting.LineAnnotation lineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.LineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 0D);
            this.BrowseB = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SaveToPngB = new System.Windows.Forms.Button();
            this.FeatureCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FuzyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.openFSDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.FuzzyTC = new System.Windows.Forms.TabControl();
            this.FuzzyTermTP = new System.Windows.Forms.TabPage();
            this.FuzzyRulesTP = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.MakeRulesB = new System.Windows.Forms.Button();
            this.RulesRTB = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FuzyChart)).BeginInit();
            this.FuzzyTC.SuspendLayout();
            this.FuzzyTermTP.SuspendLayout();
            this.FuzzyRulesTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrowseB
            // 
            this.BrowseB.Dock = System.Windows.Forms.DockStyle.Top;
            this.BrowseB.Location = new System.Drawing.Point(0, 0);
            this.BrowseB.Name = "BrowseB";
            this.BrowseB.Size = new System.Drawing.Size(967, 23);
            this.BrowseB.TabIndex = 1;
            this.BrowseB.Text = "Обзор";
            this.BrowseB.UseVisualStyleBackColor = true;
            this.BrowseB.Click += new System.EventHandler(this.BrowseB_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(953, 369);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SaveToPngB);
            this.splitContainer1.Panel1.Controls.Add(this.FeatureCB);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FuzyChart);
            this.splitContainer1.Size = new System.Drawing.Size(953, 369);
            this.splitContainer1.SplitterDistance = 149;
            this.splitContainer1.TabIndex = 1;
            // 
            // SaveToPngB
            // 
            this.SaveToPngB.Enabled = false;
            this.SaveToPngB.Location = new System.Drawing.Point(6, 111);
            this.SaveToPngB.Name = "SaveToPngB";
            this.SaveToPngB.Size = new System.Drawing.Size(140, 72);
            this.SaveToPngB.TabIndex = 2;
            this.SaveToPngB.Text = "Сохранить";
            this.SaveToPngB.UseVisualStyleBackColor = true;
            this.SaveToPngB.Click += new System.EventHandler(this.SaveToPngB_Click);
            // 
            // FeatureCB
            // 
            this.FeatureCB.FormattingEnabled = true;
            this.FeatureCB.Location = new System.Drawing.Point(6, 47);
            this.FeatureCB.Name = "FeatureCB";
            this.FeatureCB.Size = new System.Drawing.Size(140, 21);
            this.FeatureCB.TabIndex = 1;
            this.FeatureCB.SelectedIndexChanged += new System.EventHandler(this.FeatureCB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Входной признак";
            // 
            // FuzyChart
            // 
            lineAnnotation1.Name = "LineAnnotation1";
            this.FuzyChart.Annotations.Add(lineAnnotation1);
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));

            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "Text 12";
            chartArea1.AxisY.Interval = 0.5D;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Maximum = 1D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.Title = "m";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Symbol", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            chartArea1.Name = "ChartFuzzy";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 82F;
            this.FuzyChart.ChartAreas.Add(chartArea1);
            this.FuzyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            
            this.FuzyChart.Legends.Add(legend1);
            this.FuzyChart.Location = new System.Drawing.Point(0, 0);
            this.FuzyChart.Name = "FuzyChart";
            this.FuzyChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones;
            series1.ChartArea = "ChartFuzzy";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            this.FuzyChart.Series.Add(series1);
            this.FuzyChart.Size = new System.Drawing.Size(800, 369);
            this.FuzyChart.TabIndex = 0;
            this.FuzyChart.Text = "FuzzySystem";
            this.FuzyChart.Visible = false;
            this.FuzyChart.Click += new System.EventHandler(this.FuzyChart_Click);
            // 
            // openFSDialog
            // 
            this.openFSDialog.Filter = "Union Fuzzy System file|*.ufs|All files|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.emf";
            this.saveFileDialog1.FileName = "Fuzzy.emf";
            this.saveFileDialog1.Filter = "Enhanced MetaFile|*.emf";
            // 
            // FuzzyTC
            // 
            this.FuzzyTC.Controls.Add(this.FuzzyTermTP);
            this.FuzzyTC.Controls.Add(this.FuzzyRulesTP);
            this.FuzzyTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FuzzyTC.Location = new System.Drawing.Point(0, 23);
            this.FuzzyTC.Name = "FuzzyTC";
            this.FuzzyTC.SelectedIndex = 0;
            this.FuzzyTC.Size = new System.Drawing.Size(967, 401);
            this.FuzzyTC.TabIndex = 3;
            // 
            // FuzzyTermTP
            // 
            this.FuzzyTermTP.BackColor = System.Drawing.Color.Silver;
            this.FuzzyTermTP.Controls.Add(this.panel1);
            this.FuzzyTermTP.Location = new System.Drawing.Point(4, 22);
            this.FuzzyTermTP.Name = "FuzzyTermTP";
            this.FuzzyTermTP.Padding = new System.Windows.Forms.Padding(3);
            this.FuzzyTermTP.Size = new System.Drawing.Size(959, 375);
            this.FuzzyTermTP.TabIndex = 0;
            this.FuzzyTermTP.Text = "Термы";
            // 
            // FuzzyRulesTP
            // 
            this.FuzzyRulesTP.Controls.Add(this.splitContainer2);
            this.FuzzyRulesTP.Location = new System.Drawing.Point(4, 22);
            this.FuzzyRulesTP.Name = "FuzzyRulesTP";
            this.FuzzyRulesTP.Padding = new System.Windows.Forms.Padding(3);
            this.FuzzyRulesTP.Size = new System.Drawing.Size(959, 375);
            this.FuzzyRulesTP.TabIndex = 1;
            this.FuzzyRulesTP.Text = "Правила";
            this.FuzzyRulesTP.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer2.Panel1.Controls.Add(this.MakeRulesB);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.RulesRTB);
            this.splitContainer2.Size = new System.Drawing.Size(953, 369);
            this.splitContainer2.SplitterDistance = 148;
            this.splitContainer2.TabIndex = 0;
            // 
            // MakeRulesB
            // 
            this.MakeRulesB.Enabled = false;
            this.MakeRulesB.Location = new System.Drawing.Point(3, 12);
            this.MakeRulesB.Name = "MakeRulesB";
            this.MakeRulesB.Size = new System.Drawing.Size(142, 44);
            this.MakeRulesB.TabIndex = 0;
            this.MakeRulesB.Text = "Показать правила";
            this.MakeRulesB.UseVisualStyleBackColor = true;
            this.MakeRulesB.Click += new System.EventHandler(this.MakeRulesB_Click);
            // 
            // RulesRTB
            // 
            this.RulesRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RulesRTB.Location = new System.Drawing.Point(0, 0);
            this.RulesRTB.Name = "RulesRTB";
            this.RulesRTB.ReadOnly = true;
            this.RulesRTB.Size = new System.Drawing.Size(801, 369);
            this.RulesRTB.TabIndex = 0;
            this.RulesRTB.Text = "";
            // 
            // StartF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 424);
            this.Controls.Add(this.FuzzyTC);
            this.Controls.Add(this.BrowseB);
            this.Name = "StartF";
            this.Text = "Отобразить Нечеткую Систему";
            this.Load += new System.EventHandler(this.StartF_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FuzyChart)).EndInit();
            this.FuzzyTC.ResumeLayout(false);
            this.FuzzyTermTP.ResumeLayout(false);
            this.FuzzyRulesTP.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BrowseB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart FuzyChart;
        private System.Windows.Forms.ComboBox FeatureCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFSDialog;
        private System.Windows.Forms.Button SaveToPngB;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl FuzzyTC;
        private System.Windows.Forms.TabPage FuzzyTermTP;
        private System.Windows.Forms.TabPage FuzzyRulesTP;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button MakeRulesB;
        private System.Windows.Forms.RichTextBox RulesRTB;

    }
}

