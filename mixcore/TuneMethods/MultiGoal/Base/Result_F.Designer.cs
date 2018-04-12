namespace FuzzySystem.FuzzyAbstract.learn_algorithm
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
            this.Result_RTB = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Result_RTB
            // 
            this.Result_RTB.AcceptsTab = true;
            this.Result_RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Result_RTB.Location = new System.Drawing.Point(0, 0);
            this.Result_RTB.Name = "Result_RTB";
            this.Result_RTB.ReadOnly = true;
            this.Result_RTB.Size = new System.Drawing.Size(662, 406);
            this.Result_RTB.TabIndex = 0;
            this.Result_RTB.Text = "";
            this.Result_RTB.TextChanged += new System.EventHandler(this.Result_RTB_TextChanged);
            // 
            // Result_F
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 406);
            this.Controls.Add(this.Result_RTB);
            this.Name = "Result_F";
            this.Text = "Результаты";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox Result_RTB;

    }
}