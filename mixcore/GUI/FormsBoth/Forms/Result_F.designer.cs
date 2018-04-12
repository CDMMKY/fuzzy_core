namespace Mix_core.Forms
{
    partial class Result_F
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Result_F));
            this.Result_RTB = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.копироватьОшибкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьПравильныйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьMSE2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EachNumTB = new System.Windows.Forms.ToolStripTextBox();
            this.timerSaveTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Result_RTB
            // 
            this.Result_RTB.AcceptsTab = true;
            this.Result_RTB.AutoWordSelection = true;
            this.Result_RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Result_RTB.Location = new System.Drawing.Point(0, 27);
            this.Result_RTB.Name = "Result_RTB";
            this.Result_RTB.ReadOnly = true;
            this.Result_RTB.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.Result_RTB.Size = new System.Drawing.Size(575, 365);
            this.Result_RTB.TabIndex = 0;
            this.Result_RTB.Text = "";
            this.Result_RTB.TextChanged += new System.EventHandler(this.Result_RTB_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.копироватьОшибкиToolStripMenuItem,
            this.копироватьПравильныйToolStripMenuItem,
            this.копироватьMSE2ToolStripMenuItem,
            this.EachNumTB});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(575, 27);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // копироватьОшибкиToolStripMenuItem
            // 
            this.копироватьОшибкиToolStripMenuItem.Name = "копироватьОшибкиToolStripMenuItem";
            this.копироватьОшибкиToolStripMenuItem.Size = new System.Drawing.Size(132, 23);
            this.копироватьОшибкиToolStripMenuItem.Text = "Копировать ошибки";
            // 
            // копироватьПравильныйToolStripMenuItem
            // 
            this.копироватьПравильныйToolStripMenuItem.Name = "копироватьПравильныйToolStripMenuItem";
            this.копироватьПравильныйToolStripMenuItem.Size = new System.Drawing.Size(169, 23);
            this.копироватьПравильныйToolStripMenuItem.Text = "Копировать % правильный";
            // 
            // копироватьMSE2ToolStripMenuItem
            // 
            this.копироватьMSE2ToolStripMenuItem.Name = "копироватьMSE2ToolStripMenuItem";
            this.копироватьMSE2ToolStripMenuItem.Size = new System.Drawing.Size(121, 23);
            this.копироватьMSE2ToolStripMenuItem.Text = "Копировать MSE/2";
            // 
            // EachNumTB
            // 
            this.EachNumTB.Name = "EachNumTB";
            this.EachNumTB.Size = new System.Drawing.Size(25, 23);
            this.EachNumTB.Text = "1";
            // 
            // timerSaveTimer
            // 
            this.timerSaveTimer.Enabled = true;
            this.timerSaveTimer.Interval = 300000;
            this.timerSaveTimer.Tick += new System.EventHandler(this.timerSaveTimer_Tick);
            // 
            // Result_F
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(575, 392);
            this.Controls.Add(this.Result_RTB);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "Result_F";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Результаты";
            this.Shown += new System.EventHandler(this.Result_F_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox Result_RTB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem копироватьОшибкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьПравильныйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem копироватьMSE2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox EachNumTB;
        private System.Windows.Forms.Timer timerSaveTimer;
    }
}