namespace ReCalcUFSForm
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
            this.components = new System.ComponentModel.Container();
            this.TableFullForm = new System.Windows.Forms.TableLayoutPanel();
            this.BrowseL = new System.Windows.Forms.Label();
            this.BrowseB = new System.Windows.Forms.Button();
            this.ProgressL = new System.Windows.Forms.Label();
            this.ResultL = new System.Windows.Forms.Label();
            this.ShowProgressPB = new System.Windows.Forms.ProgressBar();
            this.CompletedL = new System.Windows.Forms.Label();
            this.CompleteStatusL = new System.Windows.Forms.Label();
            this.UFSBrowseDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundSunShine = new System.ComponentModel.BackgroundWorker();
            this.ProgressChecker_T = new System.Windows.Forms.Timer(this.components);
            this.TableFullForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableFullForm
            // 
            this.TableFullForm.ColumnCount = 2;
            this.TableFullForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableFullForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableFullForm.Controls.Add(this.BrowseL, 0, 0);
            this.TableFullForm.Controls.Add(this.BrowseB, 1, 0);
            this.TableFullForm.Controls.Add(this.ProgressL, 0, 1);
            this.TableFullForm.Controls.Add(this.ResultL, 1, 1);
            this.TableFullForm.Controls.Add(this.ShowProgressPB, 1, 1);
            this.TableFullForm.Controls.Add(this.CompletedL, 0, 3);
            this.TableFullForm.Controls.Add(this.CompleteStatusL, 1, 3);
            this.TableFullForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableFullForm.Location = new System.Drawing.Point(0, 0);
            this.TableFullForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TableFullForm.Name = "TableFullForm";
            this.TableFullForm.RowCount = 2;
            this.TableFullForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.11258F));
            this.TableFullForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.11258F));
            this.TableFullForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 0.9933776F));
            this.TableFullForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.78146F));
            this.TableFullForm.Size = new System.Drawing.Size(770, 345);
            this.TableFullForm.TabIndex = 0;
            // 
            // BrowseL
            // 
            this.BrowseL.AutoSize = true;
            this.BrowseL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseL.Location = new System.Drawing.Point(3, 0);
            this.BrowseL.Name = "BrowseL";
            this.BrowseL.Size = new System.Drawing.Size(379, 114);
            this.BrowseL.TabIndex = 0;
            this.BrowseL.Text = "Загрузка папки с UFS";
            // 
            // BrowseB
            // 
            this.BrowseB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseB.Location = new System.Drawing.Point(388, 4);
            this.BrowseB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BrowseB.Name = "BrowseB";
            this.BrowseB.Size = new System.Drawing.Size(379, 106);
            this.BrowseB.TabIndex = 1;
            this.BrowseB.Text = "Обзор UFS";
            this.BrowseB.UseVisualStyleBackColor = true;
            this.BrowseB.Click += new System.EventHandler(this.BrowseB_Click);
            // 
            // ProgressL
            // 
            this.ProgressL.AutoSize = true;
            this.ProgressL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressL.Location = new System.Drawing.Point(3, 114);
            this.ProgressL.Name = "ProgressL";
            this.ProgressL.Size = new System.Drawing.Size(379, 114);
            this.ProgressL.TabIndex = 2;
            this.ProgressL.Text = "Состояние";
            this.ProgressL.Visible = false;
            // 
            // ResultL
            // 
            this.ResultL.AutoSize = true;
            this.ResultL.Location = new System.Drawing.Point(3, 228);
            this.ResultL.Name = "ResultL";
            this.ResultL.Size = new System.Drawing.Size(0, 3);
            this.ResultL.TabIndex = 3;
            // 
            // ShowProgressPB
            // 
            this.ShowProgressPB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowProgressPB.Location = new System.Drawing.Point(388, 117);
            this.ShowProgressPB.Name = "ShowProgressPB";
            this.ShowProgressPB.Size = new System.Drawing.Size(379, 108);
            this.ShowProgressPB.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ShowProgressPB.TabIndex = 4;
            this.ShowProgressPB.Visible = false;
            // 
            // CompletedL
            // 
            this.CompletedL.AutoSize = true;
            this.CompletedL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompletedL.Location = new System.Drawing.Point(3, 231);
            this.CompletedL.Name = "CompletedL";
            this.CompletedL.Size = new System.Drawing.Size(379, 114);
            this.CompletedL.TabIndex = 2;
            this.CompletedL.Text = "Выполнено";
            this.CompletedL.Visible = false;
            // 
            // CompleteStatusL
            // 
            this.CompleteStatusL.AutoSize = true;
            this.CompleteStatusL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompleteStatusL.Location = new System.Drawing.Point(388, 231);
            this.CompleteStatusL.Name = "CompleteStatusL";
            this.CompleteStatusL.Size = new System.Drawing.Size(379, 114);
            this.CompleteStatusL.TabIndex = 6;
            this.CompleteStatusL.Visible = false;
            // 
            // UFSBrowseDirectory
            // 
            this.UFSBrowseDirectory.SelectedPath = global::ReCalcUFSForm.Properties.Settings.Default.LastPath;
            // 
            // backgroundSunShine
            // 
            this.backgroundSunShine.WorkerReportsProgress = true;
            this.backgroundSunShine.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundSunShine_DoWork);
            this.backgroundSunShine.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundSunShine_ProgressChanged);
            this.backgroundSunShine.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundSunShine_RunWorkerCompleted);
            // 
            // ProgressChecker_T
            // 
            this.ProgressChecker_T.Interval = 500;
            this.ProgressChecker_T.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 345);
            this.Controls.Add(this.TableFullForm);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainF";
            this.Text = "ReCalcUFS";
            this.TableFullForm.ResumeLayout(false);
            this.TableFullForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableFullForm;
        private System.Windows.Forms.Label BrowseL;
        private System.Windows.Forms.Button BrowseB;
        private System.Windows.Forms.Label ProgressL;
        private System.Windows.Forms.Label ResultL;
        private System.Windows.Forms.FolderBrowserDialog UFSBrowseDirectory;
        private System.Windows.Forms.ProgressBar ShowProgressPB;
        private System.Windows.Forms.Label CompletedL;
        private System.Windows.Forms.Label CompleteStatusL;
        private System.ComponentModel.BackgroundWorker backgroundSunShine;
        private System.Windows.Forms.Timer ProgressChecker_T;
    }
}

