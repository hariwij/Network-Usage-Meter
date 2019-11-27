namespace NetMeter
{
    partial class Form1
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
            this.TxtTotal = new System.Windows.Forms.Label();
            this.TxtUse = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // TxtTotal
            // 
            this.TxtTotal.AutoSize = true;
            this.TxtTotal.Font = new System.Drawing.Font("Arial Black", 17F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTotal.ForeColor = System.Drawing.Color.Red;
            this.TxtTotal.Location = new System.Drawing.Point(-2, -3);
            this.TxtTotal.Name = "TxtTotal";
            this.TxtTotal.Size = new System.Drawing.Size(94, 32);
            this.TxtTotal.TabIndex = 0;
            this.TxtTotal.Text = "0.00 B";
            this.TxtTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TxtTotal.SizeChanged += new System.EventHandler(this.TxtTotal_SizeChanged);
            // 
            // TxtUse
            // 
            this.TxtUse.AutoSize = true;
            this.TxtUse.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtUse.ForeColor = System.Drawing.Color.Red;
            this.TxtUse.Location = new System.Drawing.Point(98, -3);
            this.TxtUse.Name = "TxtUse";
            this.TxtUse.Size = new System.Drawing.Size(28, 16);
            this.TxtUse.TabIndex = 1;
            this.TxtUse.Text = "0 B";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 37);
            this.Controls.Add(this.TxtUse);
            this.Controls.Add(this.TxtTotal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label TxtTotal;
        private System.Windows.Forms.Label TxtUse;
        private System.Windows.Forms.Timer timer1;
    }
}