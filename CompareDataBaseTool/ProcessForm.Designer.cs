namespace CompareDataBaseTool
{
    partial class ProcessForm
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
            this.labelInfor = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // labelInfor
            // 
            this.labelInfor.AutoSize = true;
            this.labelInfor.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfor.Location = new System.Drawing.Point(13, 13);
            this.labelInfor.Name = "labelInfor";
            this.labelInfor.Size = new System.Drawing.Size(199, 19);
            this.labelInfor.TabIndex = 0;
            this.labelInfor.Text = "正在进行升级，请稍后";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 39);
            this.progressBar1.MarqueeAnimationSpeed = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(790, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 74);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.labelInfor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProcessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "正在进行升级";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfor;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}