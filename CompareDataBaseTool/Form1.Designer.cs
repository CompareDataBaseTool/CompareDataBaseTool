namespace CompareDataBaseTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonStartUpdateDatabase = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonDiffent = new System.Windows.Forms.Button();
            this.btnSetEnvirmentVariables = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.belongtable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStartUpdateDatabase
            // 
            this.buttonStartUpdateDatabase.Location = new System.Drawing.Point(604, 548);
            this.buttonStartUpdateDatabase.Name = "buttonStartUpdateDatabase";
            this.buttonStartUpdateDatabase.Size = new System.Drawing.Size(142, 23);
            this.buttonStartUpdateDatabase.TabIndex = 0;
            this.buttonStartUpdateDatabase.Text = "开始升级";
            this.buttonStartUpdateDatabase.UseVisualStyleBackColor = true;
            this.buttonStartUpdateDatabase.Click += new System.EventHandler(this.buttonStartUpdateDatabase_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.type,
            this.name,
            this.oper,
            this.belongtable});
            this.dataGridView1.Location = new System.Drawing.Point(2, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(893, 511);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // buttonDiffent
            // 
            this.buttonDiffent.Location = new System.Drawing.Point(393, 548);
            this.buttonDiffent.Name = "buttonDiffent";
            this.buttonDiffent.Size = new System.Drawing.Size(114, 23);
            this.buttonDiffent.TabIndex = 2;
            this.buttonDiffent.Text = "比较差异";
            this.buttonDiffent.UseVisualStyleBackColor = true;
            this.buttonDiffent.Click += new System.EventHandler(this.buttonDiffent_Click);
            // 
            // btnSetEnvirmentVariables
            // 
            this.btnSetEnvirmentVariables.Location = new System.Drawing.Point(231, 548);
            this.btnSetEnvirmentVariables.Name = "btnSetEnvirmentVariables";
            this.btnSetEnvirmentVariables.Size = new System.Drawing.Size(98, 23);
            this.btnSetEnvirmentVariables.TabIndex = 3;
            this.btnSetEnvirmentVariables.Text = "设置环境变量";
            this.btnSetEnvirmentVariables.UseVisualStyleBackColor = true;
            this.btnSetEnvirmentVariables.Click += new System.EventHandler(this.btnSetEnvirmentVariables_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "type";
            this.dataGridViewTextBoxColumn1.HeaderText = "类型";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 207;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "名称";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 207;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "oper";
            this.dataGridViewTextBoxColumn3.HeaderText = "操作";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 207;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "belongtable";
            this.dataGridViewTextBoxColumn4.HeaderText = "所属表";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 207;
            // 
            // type
            // 
            this.type.DataPropertyName = "type";
            this.type.HeaderText = "类型";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Width = 207;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "名称";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 207;
            // 
            // oper
            // 
            this.oper.DataPropertyName = "oper";
            this.oper.HeaderText = "操作";
            this.oper.Name = "oper";
            this.oper.ReadOnly = true;
            this.oper.Width = 207;
            // 
            // belongtable
            // 
            this.belongtable.DataPropertyName = "belongtable";
            this.belongtable.HeaderText = "所属表";
            this.belongtable.Name = "belongtable";
            this.belongtable.ReadOnly = true;
            this.belongtable.Width = 207;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 583);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetEnvirmentVariables);
            this.Controls.Add(this.buttonDiffent);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonStartUpdateDatabase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库自动升级程序";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartUpdateDatabase;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonDiffent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn oper;
        private System.Windows.Forms.DataGridViewTextBoxColumn belongtable;
        private System.Windows.Forms.Button btnSetEnvirmentVariables;
        private System.Windows.Forms.Label label1;
    }
}

