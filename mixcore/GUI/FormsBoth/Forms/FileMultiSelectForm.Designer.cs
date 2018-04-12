namespace Mix_core.Forms
{
    partial class FileMultiSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileMultiSelectForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TraView = new System.Windows.Forms.ListView();
            this.TstView = new System.Windows.Forms.ListView();
            this.FolderLabel = new System.Windows.Forms.Label();
            this.TraLabel = new System.Windows.Forms.Label();
            this.TstLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.FolderSelectButton = new System.Windows.Forms.Button();
            this.FolderTextBox = new System.Windows.Forms.TextBox();
            this.TstAddButton = new System.Windows.Forms.Button();
            this.TstRemoveButton = new System.Windows.Forms.Button();
            this.TstUpButton = new System.Windows.Forms.Button();
            this.TstDownButton = new System.Windows.Forms.Button();
            this.TraToTstButton = new System.Windows.Forms.Button();
            this.TstToTraButton = new System.Windows.Forms.Button();
            this.TraAddButton = new System.Windows.Forms.Button();
            this.TraRemoveButton = new System.Windows.Forms.Button();
            this.TraUpButton = new System.Windows.Forms.Button();
            this.TraDownButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Open_samples_FD = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.719862F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.2101F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.21011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.719862F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.21011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.21011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.719862F));
            this.tableLayoutPanel1.Controls.Add(this.TraView, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.TstView, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.FolderLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TraLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.TstLabel, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.OKButton, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.CancelButton, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.FolderSelectButton, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.FolderTextBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.TstAddButton, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.TstRemoveButton, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.TstUpButton, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.TstDownButton, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.TraToTstButton, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.TstToTraButton, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.TraAddButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.TraRemoveButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.TraUpButton, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.TraDownButton, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.07453F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.07903F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.21666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.21666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.21666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.21666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.21666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.763147F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(743, 501);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // TraView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.TraView, 2);
            this.TraView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraView.Location = new System.Drawing.Point(60, 103);
            this.TraView.Name = "TraView";
            this.tableLayoutPanel1.SetRowSpan(this.TraView, 5);
            this.TraView.Size = new System.Drawing.Size(278, 349);
            this.TraView.TabIndex = 1;
            this.TraView.UseCompatibleStateImageBehavior = false;
            this.TraView.View = System.Windows.Forms.View.List;
            // 
            // TstView
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.TstView, 2);
            this.TstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstView.Location = new System.Drawing.Point(401, 103);
            this.TstView.Name = "TstView";
            this.tableLayoutPanel1.SetRowSpan(this.TstView, 5);
            this.TstView.Size = new System.Drawing.Size(278, 349);
            this.TstView.TabIndex = 0;
            this.TstView.UseCompatibleStateImageBehavior = false;
            this.TstView.View = System.Windows.Forms.View.List;
            // 
            // FolderLabel
            // 
            this.FolderLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.FolderLabel, 2);
            this.FolderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FolderLabel.Location = new System.Drawing.Point(3, 0);
            this.FolderLabel.Name = "FolderLabel";
            this.FolderLabel.Size = new System.Drawing.Size(193, 50);
            this.FolderLabel.TabIndex = 2;
            this.FolderLabel.Text = "Папка";
            this.FolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TraLabel
            // 
            this.TraLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.TraLabel, 2);
            this.TraLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraLabel.Location = new System.Drawing.Point(60, 50);
            this.TraLabel.Name = "TraLabel";
            this.TraLabel.Size = new System.Drawing.Size(278, 50);
            this.TraLabel.TabIndex = 3;
            this.TraLabel.Text = "Обучающие выборки";
            this.TraLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TstLabel
            // 
            this.TstLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.TstLabel, 2);
            this.TstLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstLabel.Location = new System.Drawing.Point(401, 50);
            this.TstLabel.Name = "TstLabel";
            this.TstLabel.Size = new System.Drawing.Size(278, 50);
            this.TstLabel.TabIndex = 4;
            this.TstLabel.Text = "Тестовые выборки";
            this.TstLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OKButton
            // 
            this.OKButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel1.SetColumnSpan(this.OKButton, 3);
            this.OKButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.OKButton.Image = ((System.Drawing.Image)(resources.GetObject("OKButton.Image")));
            this.OKButton.Location = new System.Drawing.Point(3, 458);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(335, 40);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "Ок";
            this.OKButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.OKButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.CancelButton, 3);
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CancelButton.Image = ((System.Drawing.Image)(resources.GetObject("CancelButton.Image")));
            this.CancelButton.Location = new System.Drawing.Point(401, 458);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(339, 40);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // FolderSelectButton
            // 
            this.FolderSelectButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FolderSelectButton.BackgroundImage")));
            this.FolderSelectButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FolderSelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderSelectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FolderSelectButton.Location = new System.Drawing.Point(685, 3);
            this.FolderSelectButton.Name = "FolderSelectButton";
            this.FolderSelectButton.Size = new System.Drawing.Size(55, 44);
            this.FolderSelectButton.TabIndex = 7;
            this.FolderSelectButton.UseVisualStyleBackColor = true;
            this.FolderSelectButton.Click += new System.EventHandler(this.FolderSelectButton_Click);
            // 
            // FolderTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.FolderTextBox, 4);
            this.FolderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FolderTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FolderTextBox.Location = new System.Drawing.Point(202, 3);
            this.FolderTextBox.Name = "FolderTextBox";
            this.FolderTextBox.ReadOnly = true;
            this.FolderTextBox.Size = new System.Drawing.Size(477, 26);
            this.FolderTextBox.TabIndex = 8;
            // 
            // TstAddButton
            // 
            this.TstAddButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TstAddButton.BackgroundImage")));
            this.TstAddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TstAddButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstAddButton.Location = new System.Drawing.Point(685, 103);
            this.TstAddButton.Name = "TstAddButton";
            this.TstAddButton.Size = new System.Drawing.Size(55, 65);
            this.TstAddButton.TabIndex = 9;
            this.TstAddButton.UseVisualStyleBackColor = true;
            this.TstAddButton.Click += new System.EventHandler(this.TstAddButton_Click);
            // 
            // TstRemoveButton
            // 
            this.TstRemoveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TstRemoveButton.BackgroundImage")));
            this.TstRemoveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TstRemoveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstRemoveButton.Location = new System.Drawing.Point(685, 174);
            this.TstRemoveButton.Name = "TstRemoveButton";
            this.TstRemoveButton.Size = new System.Drawing.Size(55, 65);
            this.TstRemoveButton.TabIndex = 10;
            this.TstRemoveButton.UseVisualStyleBackColor = true;
            this.TstRemoveButton.Click += new System.EventHandler(this.TstRemoveButton_Click);
            // 
            // TstUpButton
            // 
            this.TstUpButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TstUpButton.BackgroundImage")));
            this.TstUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TstUpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstUpButton.Location = new System.Drawing.Point(685, 245);
            this.TstUpButton.Name = "TstUpButton";
            this.TstUpButton.Size = new System.Drawing.Size(55, 65);
            this.TstUpButton.TabIndex = 11;
            this.TstUpButton.UseVisualStyleBackColor = true;
            this.TstUpButton.Click += new System.EventHandler(this.TstUpButton_Click);
            // 
            // TstDownButton
            // 
            this.TstDownButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TstDownButton.BackgroundImage")));
            this.TstDownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TstDownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstDownButton.Location = new System.Drawing.Point(685, 316);
            this.TstDownButton.Name = "TstDownButton";
            this.TstDownButton.Size = new System.Drawing.Size(55, 65);
            this.TstDownButton.TabIndex = 12;
            this.TstDownButton.UseVisualStyleBackColor = true;
            this.TstDownButton.Click += new System.EventHandler(this.TstDownButton_Click);
            // 
            // TraToTstButton
            // 
            this.TraToTstButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TraToTstButton.BackgroundImage")));
            this.TraToTstButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TraToTstButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraToTstButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraToTstButton.Location = new System.Drawing.Point(344, 174);
            this.TraToTstButton.Name = "TraToTstButton";
            this.TraToTstButton.Size = new System.Drawing.Size(51, 65);
            this.TraToTstButton.TabIndex = 13;
            this.TraToTstButton.UseVisualStyleBackColor = true;
            this.TraToTstButton.Click += new System.EventHandler(this.TraToTstButton_Click);
            // 
            // TstToTraButton
            // 
            this.TstToTraButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TstToTraButton.BackgroundImage")));
            this.TstToTraButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TstToTraButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TstToTraButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TstToTraButton.Location = new System.Drawing.Point(344, 245);
            this.TstToTraButton.Name = "TstToTraButton";
            this.TstToTraButton.Size = new System.Drawing.Size(51, 65);
            this.TstToTraButton.TabIndex = 14;
            this.TstToTraButton.UseVisualStyleBackColor = true;
            this.TstToTraButton.Click += new System.EventHandler(this.TstToTraButton_Click);
            // 
            // TraAddButton
            // 
            this.TraAddButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TraAddButton.BackgroundImage")));
            this.TraAddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TraAddButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraAddButton.Location = new System.Drawing.Point(3, 103);
            this.TraAddButton.Name = "TraAddButton";
            this.TraAddButton.Size = new System.Drawing.Size(51, 65);
            this.TraAddButton.TabIndex = 15;
            this.TraAddButton.UseVisualStyleBackColor = true;
            this.TraAddButton.Click += new System.EventHandler(this.TraAddButton_Click);
            // 
            // TraRemoveButton
            // 
            this.TraRemoveButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TraRemoveButton.BackgroundImage")));
            this.TraRemoveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TraRemoveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraRemoveButton.Location = new System.Drawing.Point(3, 174);
            this.TraRemoveButton.Name = "TraRemoveButton";
            this.TraRemoveButton.Size = new System.Drawing.Size(51, 65);
            this.TraRemoveButton.TabIndex = 16;
            this.TraRemoveButton.UseVisualStyleBackColor = true;
            this.TraRemoveButton.Click += new System.EventHandler(this.TraRemoveButton_Click);
            // 
            // TraUpButton
            // 
            this.TraUpButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TraUpButton.BackgroundImage")));
            this.TraUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TraUpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraUpButton.Location = new System.Drawing.Point(3, 245);
            this.TraUpButton.Name = "TraUpButton";
            this.TraUpButton.Size = new System.Drawing.Size(51, 65);
            this.TraUpButton.TabIndex = 17;
            this.TraUpButton.UseVisualStyleBackColor = true;
            this.TraUpButton.Click += new System.EventHandler(this.TraUpButton_Click);
            // 
            // TraDownButton
            // 
            this.TraDownButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TraDownButton.BackgroundImage")));
            this.TraDownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TraDownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TraDownButton.Location = new System.Drawing.Point(3, 316);
            this.TraDownButton.Name = "TraDownButton";
            this.TraDownButton.Size = new System.Drawing.Size(51, 65);
            this.TraDownButton.TabIndex = 18;
            this.TraDownButton.UseVisualStyleBackColor = true;
            this.TraDownButton.Click += new System.EventHandler(this.TraDownButton_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.CheckFileExists = false;
            this.openFileDialog1.FileName = " ";
            this.openFileDialog1.Title = "Выберите папку";
            // 
            // Open_samples_FD
            // 
            this.Open_samples_FD.Filter = "Поддерживаемые файлы |*.dat;*.ufs|Keel data |*.dat|Union fuzzy system|*.ufs|Все ф" +
    "айлы|*.*";
            this.Open_samples_FD.Multiselect = true;
            this.Open_samples_FD.Title = "Выберите файл с данными KEEL или UFS";
            // 
            // FileMultiSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 501);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileMultiSelectForm";
            this.Text = "FileMultiSelectForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView TraView;
        private System.Windows.Forms.ListView TstView;
        private System.Windows.Forms.Label FolderLabel;
        private System.Windows.Forms.Label TraLabel;
        private System.Windows.Forms.Label TstLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button FolderSelectButton;
        private System.Windows.Forms.TextBox FolderTextBox;
        private System.Windows.Forms.Button TstAddButton;
        private System.Windows.Forms.Button TstRemoveButton;
        private System.Windows.Forms.Button TstUpButton;
        private System.Windows.Forms.Button TstDownButton;
        private System.Windows.Forms.Button TraToTstButton;
        private System.Windows.Forms.Button TstToTraButton;
        private System.Windows.Forms.Button TraAddButton;
        private System.Windows.Forms.Button TraRemoveButton;
        private System.Windows.Forms.Button TraUpButton;
        private System.Windows.Forms.Button TraDownButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog Open_samples_FD;
    }
}