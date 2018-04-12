namespace Mix_core.Forms
{
    partial class universal_conf_form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(universal_conf_form));
            this.conf_algorithm_params_PG = new System.Windows.Forms.PropertyGrid();
            this.OK_B = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // conf_algorithm_params_PG
            // 
            this.conf_algorithm_params_PG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conf_algorithm_params_PG.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.conf_algorithm_params_PG.Location = new System.Drawing.Point(0, 0);
            this.conf_algorithm_params_PG.Name = "conf_algorithm_params_PG";
            this.conf_algorithm_params_PG.Size = new System.Drawing.Size(765, 375);
            this.conf_algorithm_params_PG.TabIndex = 0;
            this.conf_algorithm_params_PG.ToolbarVisible = false;
            this.conf_algorithm_params_PG.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.conf_algorithm_params_PG_PropertyValueChanged);
            // 
            // OK_B
            // 
            this.OK_B.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_B.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OK_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OK_B.Location = new System.Drawing.Point(0, 344);
            this.OK_B.Name = "OK_B";
            this.OK_B.Size = new System.Drawing.Size(765, 31);
            this.OK_B.TabIndex = 1;
            this.OK_B.Text = "OK";
            this.OK_B.UseVisualStyleBackColor = true;
            this.OK_B.Click += new System.EventHandler(this.OK_B_Click);
            // 
            // universal_conf_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(765, 375);
            this.Controls.Add(this.OK_B);
            this.Controls.Add(this.conf_algorithm_params_PG);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "universal_conf_form";
            this.Text = "universal_conf_F";
            this.Load += new System.EventHandler(this.universal_conf_form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK_B;
        public System.Windows.Forms.PropertyGrid conf_algorithm_params_PG;
    }
}