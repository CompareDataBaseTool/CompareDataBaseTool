namespace CompareDataBaseTool
{
    partial class SetEnvirmentVarible
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
            this.cbOldDBList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbNewDBList = new System.Windows.Forms.ComboBox();
            this.btnChoice = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtDBPwd = new System.Windows.Forms.TextBox();
            this.txtDBUserName = new System.Windows.Forms.TextBox();
            this.txtDBIP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbOldDBList
            // 
            this.cbOldDBList.FormattingEnabled = true;
            this.cbOldDBList.Location = new System.Drawing.Point(6, 20);
            this.cbOldDBList.Name = "cbOldDBList";
            this.cbOldDBList.Size = new System.Drawing.Size(279, 20);
            this.cbOldDBList.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbOldDBList);
            this.groupBox1.Location = new System.Drawing.Point(12, 287);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 55);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "老的数据(即将升级)库列表";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbNewDBList);
            this.groupBox2.Location = new System.Drawing.Point(309, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 55);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新数据库列表";
            // 
            // cbNewDBList
            // 
            this.cbNewDBList.FormattingEnabled = true;
            this.cbNewDBList.Location = new System.Drawing.Point(6, 20);
            this.cbNewDBList.Name = "cbNewDBList";
            this.cbNewDBList.Size = new System.Drawing.Size(312, 20);
            this.cbNewDBList.TabIndex = 6;
            // 
            // btnChoice
            // 
            this.btnChoice.Location = new System.Drawing.Point(252, 366);
            this.btnChoice.Name = "btnChoice";
            this.btnChoice.Size = new System.Drawing.Size(75, 23);
            this.btnChoice.TabIndex = 7;
            this.btnChoice.Text = "选择";
            this.btnChoice.UseVisualStyleBackColor = true;
            this.btnChoice.Click += new System.EventHandler(this.btnChoice_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDel);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.txtDBPwd);
            this.groupBox3.Controls.Add(this.txtDBUserName);
            this.groupBox3.Controls.Add(this.txtDBIP);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtDBName);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(632, 246);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "添加新的数据库";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(338, 81);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 5;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(338, 41);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtDBPwd
            // 
            this.txtDBPwd.Location = new System.Drawing.Point(107, 191);
            this.txtDBPwd.Name = "txtDBPwd";
            this.txtDBPwd.Size = new System.Drawing.Size(178, 21);
            this.txtDBPwd.TabIndex = 3;
            // 
            // txtDBUserName
            // 
            this.txtDBUserName.Location = new System.Drawing.Point(107, 132);
            this.txtDBUserName.Name = "txtDBUserName";
            this.txtDBUserName.Size = new System.Drawing.Size(178, 21);
            this.txtDBUserName.TabIndex = 2;
            // 
            // txtDBIP
            // 
            this.txtDBIP.Location = new System.Drawing.Point(107, 84);
            this.txtDBIP.Name = "txtDBIP";
            this.txtDBIP.Size = new System.Drawing.Size(178, 21);
            this.txtDBIP.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "数据库用户名:";
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(107, 36);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(178, 21);
            this.txtDBName.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "数据库IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "数据库名称:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 191);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "数据库密码:";
            // 
            // SetEnvirmentVarible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 401);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnChoice);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetEnvirmentVarible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置系统环境变量";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetEnvirmentVarible_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbOldDBList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbNewDBList;
        private System.Windows.Forms.Button btnChoice;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtDBPwd;
        private System.Windows.Forms.TextBox txtDBUserName;
        private System.Windows.Forms.TextBox txtDBIP;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;

    }
}