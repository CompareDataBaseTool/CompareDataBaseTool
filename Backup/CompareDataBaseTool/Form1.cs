using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace CompareDataBaseTool
{
    public partial class Form1 : Form
    {
        DBHelper oldDBHelper = null;
        DBHelper newDBHelper = null;
        XmlDocument xmlDoc = new XmlDocument();
        char directorySeparatorChar = Path.DirectorySeparatorChar;
        DataSet ds = new DataSet();
        DataRow resultRow = null;
        Dictionary<string, string[]> dicSql = new Dictionary<string, string[]>();
        private delegate void InvokeHandler();
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            dicSql.Clear();
            InitDataConfig();
            SetHeaderCheckboxIsChecked(false);
        }
        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void InitDataConfig()
        {
            string xmlConfigPath = Application.StartupPath + directorySeparatorChar + "Configs" + directorySeparatorChar + "config.xml";
            if (!File.Exists(xmlConfigPath))
            {
                LogHelper.WriteLog("缺少配置文件,文件路径为：" + xmlConfigPath);
                MessageBox.Show("缺少配置文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                dataGridView1.DataSource = null;
                AddDataGridColumsToDataGridView();
                xmlDoc.Load(xmlConfigPath);
                string dbProvider = GetXmlNodeValue("Configs/DataConfig/DbProvider");

                string oldConnectionStr = "server=" + GetXmlNodeValue("Configs/DataConfig/OldDbConfig/DbHost") + ";Initial Catalog=" + GetXmlNodeValue("Configs/DataConfig//OldDbConfig/DbName") + "; User ID= " + GetXmlNodeValue("Configs/DataConfig/OldDbConfig/UserID") + ";Password=" + GetXmlNodeValue("Configs/DataConfig/OldDbConfig/Password") + ";Connect Timeout=200";

                string newConnectionStr = "server=" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbHost") + ";Initial Catalog=" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbName") + "; User ID= " + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/UserID") + ";Password=" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/Password") + ";Connect Timeout=200";
                oldDBHelper = new DBHelper(oldConnectionStr, dbProvider);
                newDBHelper = new DBHelper(newConnectionStr, dbProvider);

                label1.Text = "当前升级的数据库为：" + GetXmlNodeValue("Configs/DataConfig//OldDbConfig/DbName");
                label1.Text += "(" + GetXmlNodeValue("Configs/DataConfig/OldDbConfig/DbHost") + ");";
                label1.Text += "新的数据库为:" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbName");
                label1.Text += "(" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbHost") + ");";

                string getPKSql = GetXmlNodeValue("Configs/DataConfig/GetPKSql");
                string getUQSql = GetXmlNodeValue("Configs/DataConfig/GetUQSql");
                string getFKSql = GetXmlNodeValue("Configs/DataConfig/GetFKSql");
                string getCKSql = GetXmlNodeValue("Configs/DataConfig/GetCKSql");
                string getDFSql = GetXmlNodeValue("Configs/DataConfig/GetDFSql");
                string getColunmSql = GetXmlNodeValue("Configs/DataConfig/GetColunmsSql");
                string getViewColumnSql = GetXmlNodeValue("Configs/DataConfig/GetViewColumnSql");
                string getViewSql = GetXmlNodeValue("Configs/DataConfig/GetViewSql");
                string getIndexSql = GetXmlNodeValue("Configs/DataConfig/GetIndexSql");
                string getTableSql = GetXmlNodeValue("Configs/DataConfig/GetTableSql");
                if (!dicSql.ContainsKey(getPKSql))
                    dicSql.Add(getPKSql, new string[] { TableNameClass.newPKDT, TableNameClass.oldPKDT });
                if (!dicSql.ContainsKey(getUQSql))
                    dicSql.Add(getUQSql, new string[] { TableNameClass.newUQDT, TableNameClass.oldUQDT });
                if (!dicSql.ContainsKey(getCKSql))
                    dicSql.Add(getCKSql, new string[] { TableNameClass.newCKDT, TableNameClass.oldCKDT });
                if (!dicSql.ContainsKey(getDFSql))
                    dicSql.Add(getDFSql, new string[] { TableNameClass.newDFDT, TableNameClass.oldDFDT });
                if (!dicSql.ContainsKey(getColunmSql))
                    dicSql.Add(getColunmSql, new string[] { TableNameClass.newColumnsDT, TableNameClass.oldColumnsDT });
                if (!dicSql.ContainsKey(getViewSql))
                    dicSql.Add(getViewSql, new string[] { TableNameClass.newViewDT, TableNameClass.oldViewDT });
                if (!dicSql.ContainsKey(getViewColumnSql))
                    dicSql.Add(getViewColumnSql, new string[] { TableNameClass.newViewColumnDT, TableNameClass.oldViewColumnDT });
                if (!dicSql.ContainsKey(getIndexSql))
                    dicSql.Add(getIndexSql, new string[] { TableNameClass.newIndexDT, TableNameClass.oldIndexDT });
                if (!dicSql.ContainsKey(getTableSql))
                    dicSql.Add(getTableSql, new string[] { TableNameClass.newTablesDT, TableNameClass.oldTablesDT });

            }
            catch (XmlException ex)
            {
                LogHelper.WriteLog("加载字典数据异常,异常信息为：" + ex.ToString());
                MessageBox.Show("出现异常" + ex.ToString(), "异常");
            }
        }
        /// <summary>
        /// 加载系统字典的表
        /// </summary>
        private void InitDictoryDataTable()
        {
            if (ds != null)
            {
                ds.Tables.Clear();
            }
            DataTable resultDT = new DataTable();
            resultDT.TableName = TableNameClass.resultDT;
            resultDT.Columns.Add(new DataColumn() { ColumnName = "type", DataType = typeof(string) });
            resultDT.Columns.Add(new DataColumn() { ColumnName = "name", DataType = typeof(string) });
            resultDT.Columns.Add(new DataColumn() { ColumnName = "oper", DataType = typeof(string) });
            resultDT.Columns.Add(new DataColumn() { ColumnName = "belongtable", DataType = typeof(string) });
            resultDT.Columns.Add(new DataColumn() { ColumnName = "detailInfo", DataType = typeof(string) });
            ds.Tables.Add(resultDT);
            foreach (KeyValuePair<string, string[]> kvp in dicSql)
            {
                ds.Tables.Add(newDBHelper.ExecuteDataTable(kvp.Key, kvp.Value[0]));
                ds.Tables.Add(oldDBHelper.ExecuteDataTable(kvp.Key, kvp.Value[1]));
            }

            if (ds.Tables[TableNameClass.oldViewDT] == null)
            {
                ds.Tables.Add(new DataTable() { TableName = TableNameClass.oldViewDT });
            }
            if (ds.Tables[TableNameClass.newViewDT] == null)
            {
                ds.Tables.Add(new DataTable() { TableName = TableNameClass.newViewDT });
            }
        }
        /// <summary>
        /// 根据Xpath获取节点的值
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        private string GetXmlNodeValue(string xPath)
        {
            string nodeValue = "";
            try
            {
                nodeValue = xmlDoc.SelectSingleNode(xPath).InnerText;
            }
            catch
            {
                nodeValue = "";
            }

            return nodeValue;
        }

        private void buttonStartUpdateDatabase_Click(object sender, EventArgs e)
        {
            if (oldDBHelper == null || newDBHelper == null)
                return;
            ProcessOperator process = new ProcessOperator();
            process.MessageInfo = "正在进行升级,请稍后...";
            process.BackgroundWork = this.StartUpdateDataBase;
            process.BackgroundWorkerCompleted += new EventHandler<BackgroundWorkerEventArgs>(process_BackgroundWorkerCompleted);
            process.Start();
        }

        void process_BackgroundWorkerCompleted(object sender, BackgroundWorkerEventArgs e)
        {
            if (e.BackGroundException != null)
            {
                MessageBox.Show("异常:" + e.BackGroundException.Message);
            }
        }
        /// <summary>
        /// 开始升级数据库
        /// </summary>
        private void StartUpdateDataBase()
        {
            List<string> sqlList = new List<string>();
            List<int> rowList = new List<int>();
            StringBuilder sb = new StringBuilder();
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                if ((bool)dataGridView1.Rows[rows].Cells[0].EditedFormattedValue == true)
                {
                    rowList.Add(rows);
                    switch (Convert.ToString(dataGridView1.Rows[rows].Cells["oper"].Value))
                    {
                        case LogType.addType:
                            GetAddSql(dataGridView1.Rows[rows], sqlList);
                            break;
                        case LogType.delType:
                            GetDelSql(dataGridView1.Rows[rows], sqlList);
                            break;
                        case LogType.editType:
                            GetModifySql(dataGridView1.Rows[rows], sqlList);
                            break;
                    }
                }
            }
            if (sqlList.Count > 0)
            {
                string errorMsg = string.Empty;
                if (oldDBHelper.ExecuteTrans(sqlList, ref errorMsg))
                {
                    try
                    {
                        sqlList.Clear();
                        InitDifferentDataBaseData();
                        MessageBox.Show(this, "升级成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "升级成功,但重新加载字典数据异常,异常信息为:" + ex.ToString());
                    }
                    finally
                    {
                        //ClearDataTable();
                    }
                }
                else
                {
                    MessageBox.Show(this, "升级失败,错误信息为:" + errorMsg);
                }
            }
            else
            {
                MessageBox.Show(this, "请选择升级项");
            }
        }
        /// <summary>
        /// 获取修改的相关SQL语句
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        /// <param name="sqlList"></param>
        private void GetModifySql(DataGridViewRow dataGridViewRow, List<string> sqlList)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = Convert.ToString(dataGridViewRow.Cells["belongtable"].Value);
            string columnName = Convert.ToString(dataGridViewRow.Cells["name"].Value);
            DataRow[] tmpDr = null;
            switch (Convert.ToString(dataGridViewRow.Cells["type"].Value))
            {
                case LogType.colunmType://列
                    tmpDr = ds.Tables[TableNameClass.oldColumnsDT].Select("表名='" + tableName + "' and 字段名='" + columnName + "'");
                    if (Convert.ToString(tmpDr[0]["主键"]) == "是")
                    {
                        tmpDr = ds.Tables[TableNameClass.oldPKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        if (tmpDr.Length > 0)
                        {
                            CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.pkType, sqlList);
                        }
                    }
                    tmpDr = ds.Tables[TableNameClass.oldUQDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.uqType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldDFDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.dfType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldCKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.ckType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldIndexDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.indexType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.newColumnsDT].Select("表名='" + tableName + "' and 字段名='" + columnName + "'");
                    sb.Remove(0, sb.Length);
                    sb.Append(" alter table " + tableName + " alter column " + columnName + " " + tmpDr[0]["类型"] + " ");
                    sb.Append(CreateFieldType(tmpDr[0], true));
                    sb.Append(" ;");
                    sqlList.Add(sb.ToString());
                    sb.Remove(0, sb.Length);
                    if (ds.Tables[TableNameClass.oldColumnsDT].Select("表名='" + tableName + "' and 字段名='" + columnName + "'")[0]["说明"].ToString().Trim().Length == 0)
                    {
                        CreateCommentSql(tableName, columnName, tmpDr[0]["说明"].ToString(), sqlList);
                    }
                    else
                    {
                        UpdateCommentSql(tableName, columnName, tmpDr[0]["说明"].ToString(), sqlList);
                    }
                    tmpDr = null;
                    if (ds.Tables[TableNameClass.oldTablesDT].Select("表名='" + tableName + "'").Length > 0)
                    {
                        tmpDr = ds.Tables[TableNameClass.newCKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        foreach (DataRow ck in tmpDr)
                        {
                            CreateAddConstraintSql(tableName, ck["约束名称"].ToString(), LogType.ckType, sqlList);
                        }
                        tmpDr = ds.Tables[TableNameClass.newDFDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        foreach (DataRow df in tmpDr)
                        {
                            CreateAddConstraintSql(tableName, df["约束名称"].ToString(), LogType.dfType, sqlList);
                        }
                        sb.Remove(0, sb.Length);
                        tmpDr = ds.Tables[TableNameClass.newPKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        if (tmpDr.Length > 0)
                        {
                            CreateAddConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.pkType, sqlList);
                        }
                        sb.Remove(0, sb.Length);
                        tmpDr = ds.Tables[TableNameClass.newUQDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        foreach (DataRow uq in tmpDr)
                        {
                            CreateAddConstraintSql(tableName, uq["约束名称"].ToString(), LogType.uqType, sqlList);
                        }
                        tmpDr = ds.Tables[TableNameClass.newIndexDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                        foreach (DataRow indx in tmpDr)
                        {
                            CreateAddConstraintSql(tableName, indx["约束名称"].ToString(), LogType.indexType, sqlList);
                        }

                    }
                    break;
                case LogType.ckType:
                case LogType.dfType:
                case LogType.pkType:
                case LogType.uqType:
                case LogType.indexType:
                case LogType.viewType:
                    CreateDelConstraintSql(tableName, columnName, Convert.ToString(dataGridViewRow.Cells["type"].Value), sqlList);
                    CreateAddConstraintSql(tableName, columnName, Convert.ToString(dataGridViewRow.Cells["type"].Value), sqlList);
                    break;

            }
            sb.Remove(0, sb.Length);
            tmpDr = null;
        }
        /// <summary>
        /// 获取增加的相关SQL语句
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        /// <param name="sqlList"></param>
        private void GetAddSql(DataGridViewRow dataGridViewRow, List<string> sqlList)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = Convert.ToString(dataGridViewRow.Cells["belongtable"].Value);
            string columnName = Convert.ToString(dataGridViewRow.Cells["name"].Value);
            switch (Convert.ToString(dataGridViewRow.Cells["type"].Value))
            {
                case LogType.tableType://表
                    CreateTableToDatabase(ds.Tables[TableNameClass.newColumnsDT].Select("表名='" + tableName + "'"), sqlList);
                    break;
                case LogType.colunmType://列
                    CreateColumnToDataTable(tableName, columnName, sqlList);
                    break;
                //case LogType.fkType:
                case LogType.ckType:
                case LogType.dfType:
                case LogType.pkType:
                case LogType.uqType:
                case LogType.indexType:
                case LogType.viewType:
                    CreateAddConstraintSql(tableName, columnName, Convert.ToString(dataGridViewRow.Cells["type"].Value), sqlList);
                    break;
            }
            sb.Remove(0, sb.Length);
        }
        /// <summary>
        /// 获取删除的SQL语句
        /// </summary>
        /// <param name="dataGridViewRow"></param>
        /// <param name="tmpSqlList">sql的集合</param>
        private void GetDelSql(DataGridViewRow dataGridViewRow, List<string> sqlList)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = Convert.ToString(dataGridViewRow.Cells["belongtable"].Value);
            string columnName = Convert.ToString(dataGridViewRow.Cells["name"].Value);
            DataRow[] tmpDr = null;
            switch (Convert.ToString(dataGridViewRow.Cells["type"].Value))
            {
                case LogType.tableType://表
                    sb.Remove(0, sb.Length);
                    /* tmpDr = oldFKDT.Select("主表名称='" + tableName + "'");
                     if (tmpDr.Length > 0)
                     {
                         foreach (DataRow dr in tmpDr)
                         {
                             CreateDelConstraintSql(dr["子表名称"] + "," + dr["主表名称"], dr["外键名称"].ToString(), LogType.fkType, sqlList);
                         }
                     }*/
                    sb.Append("drop table " + tableName + ";");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.colunmType://列                  
                    tmpDr = ds.Tables[TableNameClass.oldPKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.pkType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldUQDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.uqType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldDFDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.dfType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldCKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.ckType, sqlList);
                    }
                    tmpDr = ds.Tables[TableNameClass.oldIndexDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
                    if (tmpDr.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr[0]["约束名称"].ToString(), LogType.indexType, sqlList);
                    }
                    sb.Remove(0, sb.Length);
                    sb.Append("alter table " + tableName + " drop column " + columnName + ";");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.fkType:
                case LogType.ckType:
                case LogType.dfType:
                case LogType.pkType:
                case LogType.uqType:
                case LogType.indexType:
                case LogType.viewType:
                    CreateDelConstraintSql(tableName, columnName, Convert.ToString(dataGridViewRow.Cells["type"].Value), sqlList);
                    break;
            }
            sb.Remove(0, sb.Length);
            tmpDr = null;
        }

        private void buttonDiffent_Click(object sender, EventArgs e)
        {
            if (oldDBHelper == null || newDBHelper == null)
                return;
            this.Invoke(new InvokeHandler(delegate()
            {
                dataGridView1.DataSource = null;
                if (ds.Tables[TableNameClass.resultDT] != null)
                {
                    ds.Tables[TableNameClass.resultDT].Rows.Clear();
                }
            }));
            ProcessOperator process = new ProcessOperator();
            process.MessageInfo = "正在进行比对,请稍后...";
            process.BackgroundWork = this.StartCompareDataBase;
            process.BackgroundWorkerCompleted += new EventHandler<BackgroundWorkerEventArgs>(process_BackgroundWorkerCompleted);
            process.Start();

        }
        /// <summary>
        /// 开始比对数据库
        /// </summary>
        private void StartCompareDataBase()
        {
            InitDifferentDataBaseData();
            if (ds.Tables[TableNameClass.resultDT].Rows.Count == 0)
            {
                ClearDataTable();
                MessageBox.Show(this, "数据库无变化,不用进行升级");
                return;
            }
        }
        /// <summary>
        /// 加载不同
        /// </summary>
        private void InitDifferentDataBaseData()
        {
            InitDictoryDataTable();
            StringBuilder sb = new StringBuilder();
            List<DataRow> delTableList = GetDifferenceTableList(ds.Tables[TableNameClass.oldTablesDT], ds.Tables[TableNameClass.newTablesDT]);
            foreach (DataRow delTable in delTableList)
            {
                AddDataRowToResultDT(LogType.tableType, Convert.ToString(delTable["表名"]), LogType.delType, Convert.ToString(delTable["表名"]));
            }
            List<DataRow> createTableList = GetDifferenceTableList(ds.Tables[TableNameClass.newTablesDT], ds.Tables[TableNameClass.oldTablesDT]);
            foreach (DataRow createTable in createTableList)
            {
                AddDataRowToResultDT(LogType.tableType, Convert.ToString(createTable["表名"]), LogType.addType, Convert.ToString(createTable["表名"]));
            }
            GetAddDelModifyColumn();
            GetDifferentConstraintFromDataTable();
            GetDefferentView();
            RankResultDt();
            this.Invoke(new InvokeHandler(delegate()
            {
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.ScrollBars = ScrollBars.Vertical;
                if (dataGridView1.Columns.Count == 0)
                {
                    AddDataGridColumsToDataGridView();
                }
                dataGridView1.DataSource = ds.Tables[TableNameClass.resultDT];
                SetHeaderCheckboxIsChecked(false);
            }));
        }
        /// <summary>
        /// 向DataGridView里面添加列
        /// </summary>
        private void AddDataGridColumsToDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            DataGridViewCheckBoxColumn dgcc = new DataGridViewCheckBoxColumn();
            dataGridView1.Columns.Add(dgcc);
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "type", DataPropertyName = "type", HeaderText = "类型", Width = 207, SortMode = DataGridViewColumnSortMode.NotSortable });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "name", DataPropertyName = "name", HeaderText = "名称", Width = 207, SortMode = DataGridViewColumnSortMode.NotSortable });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "oper", DataPropertyName = "oper", HeaderText = "操作", Width = 207, SortMode = DataGridViewColumnSortMode.NotSortable });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "belongtable", DataPropertyName = "belongtable", HeaderText = "所属表", Width = 207, SortMode = DataGridViewColumnSortMode.NotSortable });
            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            cbHeader.OnHeaderCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnHeaderCheckBoxClicked);
            dataGridView1.Columns[0].HeaderCell = cbHeader;//设置checkbox列列头cell为我们画的那个checkbox列头
        }

        void cbHeader_OnHeaderCheckBoxClicked(bool state)
        {
            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                dataGridView1.Rows[rows].Cells[0].Value = state;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = false;
                    SetHeaderCheckboxIsChecked(false);
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
                }
            }
        }
        /// <summary>
        ///创建新表
        /// </summary>
        ///<param name="dataRow">列的集合</param>
        ///<param name="sqlList">sql语句的集合</param>
        /// <returns></returns>
        private void CreateTableToDatabase(DataRow[] dataRow, List<string> sqlList)
        {
            if (dataRow == null || dataRow.Length == 0)
                return;
            StringBuilder sb = new StringBuilder();
            string createTableName = Convert.ToString(dataRow[0][0]);
            sb.Append("create table " + createTableName + "( ");
            DataRow[] drPK = ds.Tables[TableNameClass.newPKDT].Select("表名='" + createTableName + "'");
            foreach (DataRow dr in dataRow)
            {
                sb.Append(dr["字段名"] + " ");
                sb.Append(dr["类型"]);
                sb.Append(CreateFieldType(dr, false));
                sb.Append(",");
            }
            if (drPK.Length > 0)
            {
                sb.Append("CONSTRAINT [" + drPK[0]["约束名称"] + "] PRIMARY KEY CLUSTERED ( ");
                foreach (DataRow dr in drPK)
                {
                    sb.Append(dr["约束列名"] + " ASC ,");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
                sb.Append(") ");
                sb.Append(",");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ),");
            }
            sb.Remove(sb.Length - 1, 1);
            sqlList.Add(sb.ToString());
            sb.Remove(0, sb.Length);
            foreach (DataRow ck in ds.Tables[TableNameClass.newCKDT].Select("表名='" + createTableName + "'"))
            {
                CreateAddConstraintSql(createTableName, ck["约束名称"].ToString(), LogType.ckType, sqlList);
            }
            foreach (DataRow df in ds.Tables[TableNameClass.newDFDT].Select("表名='" + createTableName + "'"))
            {
                CreateAddConstraintSql(createTableName, df["约束名称"].ToString(), LogType.dfType, sqlList);
            }
            foreach (DataRow uq in ds.Tables[TableNameClass.newUQDT].Select("表名='" + createTableName + "'"))
            {
                CreateAddConstraintSql(createTableName, uq["约束名称"].ToString(), LogType.uqType, sqlList);
            }
            foreach (DataRow indx in ds.Tables[TableNameClass.newIndexDT].Select("表名='" + createTableName + "'"))
            {
                CreateAddConstraintSql(createTableName, indx["约束名称"].ToString(), LogType.indexType, sqlList);
            }
            foreach (DataRow dr in dataRow)
            {
                if (dr["说明"].ToString().Length > 0)
                    CreateCommentSql(createTableName, dr["字段名"].ToString(), dr["说明"].ToString(), sqlList);
            }
            sb.Remove(0, sb.Length);
        }
        /// <summary>
        /// 创建添加约束Sql语句
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="constraintName">约束名称</param>
        /// <param name="constrintType">约束类型</param>
        /// <param name="sqlList">sql语句集合</param>
        private void CreateAddConstraintSql(string tableName, string constraintName, string constrintType, List<string> sqlList)
        {
            StringBuilder sb = new StringBuilder();
            DataRow[] tmpDr = null;
            DataRow[] tmpDr2 = null;
            switch (constrintType)
            {
                case LogType.fkType:
                    StringBuilder sb2 = new StringBuilder();
                    tmpDr = ds.Tables[TableNameClass.newFKDT].Select("外键名称='" + constraintName + "'");
                    sb.Remove(0, sb.Length);
                    sb2.Remove(0, sb2.Length);
                    string tmpSql = "alter table " + tmpDr[0]["子表名称"] + " with check add constraint " + constraintName + " foreign key ({0}) references " + tmpDr[0]["主表名称"] + " ({1});";
                    foreach (DataRow fk in tmpDr)
                    {
                        sb.Append(fk["子表列名"] + ",");
                        sb2.Append(fk["主表列名"] + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                    tmpSql = string.Format(tmpSql, sb.ToString(), sb2.ToString());
                    if (!sqlList.Contains(tmpSql))
                    {
                        sqlList.Add(tmpSql);
                    }
                    sb.Remove(0, sb.Length);
                    sb2.Remove(0, sb2.Length);
                    break;
                case LogType.ckType:
                    tmpDr = ds.Tables[TableNameClass.newCKDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'");
                    tmpDr2 = ds.Tables[TableNameClass.oldCKDT].Select("表名='" + tableName + "' and 约束列名='" + tmpDr[0]["约束列名"] + "'");
                    if (tmpDr2.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr2[0]["约束名称"].ToString(), LogType.ckType, sqlList);
                    }
                    sb.Remove(0, sb.Length);
                    sb.Append("alter table " + tableName + " with check add constraint [" + constraintName + "] CHECK  (" + tmpDr[0]["约束定义"] + ");");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.dfType:
                    tmpDr = ds.Tables[TableNameClass.newDFDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'");
                    tmpDr2 = ds.Tables[TableNameClass.oldDFDT].Select("表名='" + tableName + "' and 约束列名='" + tmpDr[0]["约束列名"] + "'");
                    sb.Remove(0, sb.Length);
                    if (tmpDr2.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr2[0]["约束名称"].ToString(), LogType.dfType, sqlList);
                    }
                    sb.Append("alter table " + tableName + "   add constraint [" + constraintName + "] default  (" + tmpDr[0]["约束定义"] + ") FOR [" + tmpDr[0]["约束列名"] + "];");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.pkType:
                    sb.Remove(0, sb.Length);
                    tmpDr = ds.Tables[TableNameClass.newPKDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'");
                    tmpDr2 = ds.Tables[TableNameClass.oldUQDT].Select("表名='" + tableName + "' and 约束列名='" + tmpDr[0]["约束列名"] + "'");
                    if (tmpDr2.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr2[0]["约束名称"].ToString(), LogType.pkType, sqlList);
                    }
                    sb.Append("alter table " + tableName + " add CONSTRAINT [" + tmpDr[0]["约束名称"] + "] PRIMARY KEY " + tmpDr[0]["约束类型"] + " ( ");
                    foreach (DataRow dr in ds.Tables[TableNameClass.newPKDT].Select("表名='" + tableName + "' and 约束名称='" + tmpDr[0]["约束名称"] + "'"))
                    {
                        sb.Append(dr["约束列名"]);
                        if (Convert.ToString(dr["是否降序"]).ToLower() == "true")
                        {
                            sb.Append(" desc  ");
                        }
                        else
                        {
                            sb.Append(" asc  ");
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY];");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    tmpDr = null;
                    sb.Remove(0, sb.Length);
                    break;
                case LogType.uqType:
                    tmpDr = ds.Tables[TableNameClass.newUQDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'");
                    tmpDr2 = ds.Tables[TableNameClass.oldUQDT].Select("表名='" + tableName + "' and 约束列名='" + tmpDr[0]["约束列名"] + "'");
                    if (tmpDr2.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr2[0]["约束名称"].ToString(), LogType.uqType, sqlList);
                    }
                    sb.Remove(0, sb.Length);
                    sb.Append("alter table " + tableName + " add constraint " + constraintName + " unique " + tmpDr[0]["约束类型"] + "( ");
                    foreach (DataRow dr2 in tmpDr)
                    {
                        sb.Append(dr2["约束列名"] + " ");
                        if (Convert.ToString(dr2["是否降序"]).ToLower() == "true")
                        {
                            sb.Append(" desc");
                        }
                        else
                        {
                            sb.Append(" asc ");
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(");");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.indexType:
                    tmpDr = ds.Tables[TableNameClass.newIndexDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'");
                    tmpDr2 = ds.Tables[TableNameClass.oldIndexDT].Select("表名='" + tableName + "' and 约束列名='" + tmpDr[0]["约束列名"] + "'");
                    if (tmpDr2.Length > 0)
                    {
                        CreateDelConstraintSql(tableName, tmpDr2[0]["约束名称"].ToString(), LogType.indexType, sqlList);
                    }
                    sb.Remove(0, sb.Length);
                    sb.Append("create ");
                    if (Convert.ToString(tmpDr[0]["是否唯一索引"]).ToLower() == "true")
                    {
                        sb.Append(" unique ");
                    }
                    sb.Append(" " + tmpDr[0]["约束类型"] + "  index " + " " + constraintName + " on " + tableName + " (");
                    foreach (DataRow dr2 in ds.Tables[TableNameClass.newIndexDT].Select("表名='" + tableName + "' and 约束名称='" + constraintName + "'"))
                    {
                        sb.Append(dr2["约束列名"]);
                        if (Convert.ToString(dr2["是否降序"]).ToLower() == "true")
                        {
                            sb.Append(" desc");
                        }
                        else
                        {
                            sb.Append(" asc ");
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(");");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                case LogType.viewType:
                    tmpDr = ds.Tables[TableNameClass.newViewDT].Select("视图名称='" + constraintName + "'");
                    if (!sqlList.Contains(constraintName))
                    {
                        sqlList.Add(tmpDr[0]["视图脚本"].ToString());
                    }
                    break;
            }
            tmpDr = null;
            sb.Remove(0, sb.Length);
        }

        /// <summary>
        /// 创建添加约束Sql语句
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="constraintName">约束名称</param>
        /// <param name="constrintType">约束类型</param>
        /// <param name="sqlList">sql语句集合</param>
        private void CreateDelConstraintSql(string tableName, string constraintName, string constrintType, List<string> sqlList)
        {
            StringBuilder sb = new StringBuilder();
            switch (constrintType)
            {
                case LogType.fkType:
                    sb.Remove(0, sb.Length);
                    sb.Append("alter table " + ds.Tables[TableNameClass.oldFKDT].Select("外键名称='" + constraintName + "'")[0]["子表名称"] + " drop constraint " + constraintName + ";");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
                /*case LogType.pkType:
                    if (dtNewTable.Select("表名='" + tableName + "'").Length > 0)
                    {
                        DataRow tmpDr = oldFKDT.Select("约束名称='"+constraintName+"'")[0];
                        DataRow[] tmpDr2 = oldFKDT.Select("主表名称='" + tmpDr["表名"] + "' and 主表列名='" + tmpDr["约束列名"] + "'");
                        if (tmpDr2.Length > 0)
                        {
                            CreateDelConstraintSql(tmpDr2[0]["子表名称"] + "," + tmpDr2[0]["主表名称"], tmpDr2[0]["外键名称"].ToString(), LogType.fkType, sqlList);
                        }
                        sb.Remove(0, sb.Length);
                        sb.Append("alter table " + tableName + " drop constraint " + constraintName + ";");
                        if (!sqlList.Contains(sb.ToString()))
                        {
                            sqlList.Add(sb.ToString());
                        }
                    }
                    break;*/
                case LogType.pkType:
                case LogType.ckType:
                case LogType.dfType:
                case LogType.uqType:
                    if (ds.Tables[TableNameClass.newTablesDT].Select("表名='" + tableName + "'").Length > 0)
                    {
                        sb.Remove(0, sb.Length);
                        sb.Append("alter table " + tableName + " drop constraint " + constraintName + ";");
                        if (!sqlList.Contains(sb.ToString()))
                        {
                            sqlList.Add(sb.ToString());
                        }
                    }
                    break;
                case LogType.indexType:
                    if (ds.Tables[TableNameClass.newTablesDT].Select("表名='" + tableName + "'").Length > 0)
                    {
                        sb.Remove(0, sb.Length);
                        sb.Append("drop index " + constraintName + " on " + tableName + ";");
                        if (!sqlList.Contains(sb.ToString()))
                        {
                            sqlList.Add(sb.ToString());
                        }
                    }
                    break;
                case LogType.viewType:
                    sb.Remove(0, sb.Length);
                    sb.Append("IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'" + constraintName + "'))");
                    sb.Append(" drop view " + constraintName + ";");
                    if (!sqlList.Contains(sb.ToString()))
                    {
                        sqlList.Add(sb.ToString());
                    }
                    break;
            }
            sb.Remove(0, sb.Length);
        }
        /// <summary>
        /// 添加列注释
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <param name="comment">注释</param>
        /// <param name="sqlList"></param>
        private void CreateCommentSql(string tableName, string columnName, string comment, List<string> sqlList)
        {
            sqlList.Add("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + comment + "' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + tableName + "', @level2type=N'COLUMN',@level2name=N'" + columnName + "'");
        }
        /// <summary>
        /// 更新列注释
        /// <param name="tableName">表名称</param>
        /// <param name="columnName">列名称</param>
        /// <param name="comment">注释</param>
        /// <param name="sqlList"></param>
        private void UpdateCommentSql(string tableName, string columnName, string comment, List<string> sqlList)
        {
            sqlList.Add("EXEC sp_updateextendedproperty 'MS_Description','" + comment + "','SCHEMA',dbo,'TABLE','" + tableName + "','column','" + columnName + "'");
        }

        /// <summary>
        /// 新老表名进行比对(返回的是dt1减去dt2中相同的元素的结果)
        /// </summary>
        /// <param name="newTable">新表集合</param>
        /// <param name="oldTable">老表集合</param>
        /// <returns>返回的是dt1减去dt2中相同的元素的结果</returns>
        private List<DataRow> GetDifferenceTableList(DataTable dt1, DataTable dt2)
        {
            List<DataRow> list = new List<DataRow>();
            if (dt1 == null || dt2 == null)
                return list;
            IEnumerable<DataRow> exceptTable = dt1.AsEnumerable().Except(dt2.AsEnumerable(), DataRowComparer.Default);

            if (exceptTable == null || exceptTable.Count() == 0)
                return list;
            list = exceptTable.ToList<DataRow>();
            return list;
        }

        /// <summary>
        /// 比较两个datarow是否相同
        /// </summary>
        /// <param name="dr1"></param>
        /// <param name="dr2"></param>
        /// <returns>true表示相同,false表示不同</returns>
        private List<DataRow> GetDifferenceColumnList(DataRow[] dr1, DataRow[] dr2, ref string diffMsg)
        {
            List<DataRow> list = new List<DataRow>();
            if (dr1 == null || dr2 == null)
                return list;
            for (int rows = 0; rows < dr1.Length; rows++)
            {
                for (int cols = 0; cols < dr1[rows].ItemArray.Length; cols++)
                {
                    if (cols == 2 || cols == 3 || cols == 4 || cols == 5 || cols == 7 || cols == 11)
                        continue;
                    if (Convert.ToString(dr1[rows].ItemArray[cols]) != Convert.ToString(dr2[rows].ItemArray[cols]))
                    {
                        list.Add(dr1[rows]);
                        diffMsg = "表" + Convert.ToString(dr1[rows].ItemArray[0]) + "中列\"" + Convert.ToString(dr1[rows].ItemArray[1]) + "\":" + ds.Tables[TableNameClass.oldColumnsDT].Columns[cols].ColumnName + "由" + Convert.ToString(dr1[rows].ItemArray[cols]) + "更改为：" + Convert.ToString(dr2[rows].ItemArray[cols]);
                        break;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取新增、删除、修改列
        /// </summary>
        private void GetAddDelModifyColumn()
        {
            foreach (DataRow tableName in ds.Tables[TableNameClass.newTablesDT].Rows)
            {
                if (ds.Tables[TableNameClass.oldTablesDT].Select("表名='" + tableName["表名"] + "'").Length > 0)
                {
                    foreach (DataRow creatColunm in ds.Tables[TableNameClass.newColumnsDT].Select("表名='" + tableName["表名"] + "'"))
                    {
                        if (ds.Tables[TableNameClass.oldColumnsDT].Select("表名='" + tableName["表名"] + "' and 字段名='" + creatColunm["字段名"] + "'").Length == 0)
                        {
                            AddDataRowToResultDT(LogType.colunmType, Convert.ToString(creatColunm["字段名"]), LogType.addType, Convert.ToString(creatColunm["表名"]));
                        }
                    }
                }
            }
            List<DataRow> modifyColumnList = new List<DataRow>();
            DataRow[] modifyColumn = null;
            DataRow[] drArr = null;
            string msg = string.Empty;
            foreach (DataRow tableName in ds.Tables[TableNameClass.oldTablesDT].Rows)
            {
                if (ds.Tables[TableNameClass.newTablesDT].Select("表名='" + tableName["表名"] + "'").Length > 0)
                {
                    drArr = ds.Tables[TableNameClass.oldColumnsDT].Select("表名='" + tableName["表名"] + "'");
                    foreach (DataRow dr in drArr)
                    {
                        modifyColumn = ds.Tables[TableNameClass.newColumnsDT].Select("表名='" + tableName["表名"] + "' and 字段名='" + dr["字段名"] + "'");
                        if (modifyColumn.Length == 0)//删除列
                        {
                            AddDataRowToResultDT(LogType.colunmType, Convert.ToString(dr["字段名"]), LogType.delType, Convert.ToString(dr["表名"]));
                        }
                        else  //修改列
                        {
                            modifyColumnList = GetDifferenceColumnList(new DataRow[] { dr }, modifyColumn, ref msg);
                            if (modifyColumnList.Count > 0)
                            {
                                AddDataRowToResultDT(LogType.colunmType, Convert.ToString(dr["字段名"]), LogType.editType, Convert.ToString(dr["表名"]), msg);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 新增列
        /// <param name="dt">表结构</param>
        /// <param name="columnName">列名</param>
        /// <param name="sqlList">sql语句</param>
        /// </summary>
        private void CreateColumnToDataTable(string tableName, string columnName, List<string> sqlList)
        {
            if (sqlList == null)
                sqlList = new List<string>();
            StringBuilder sb = new StringBuilder();
            DataRow[] drArr = ds.Tables[TableNameClass.newColumnsDT].Select("表名='" + tableName + "' and 字段名='" + columnName + "'");
            DataRow[] drPK = ds.Tables[TableNameClass.newPKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
            DataRow[] drCK = ds.Tables[TableNameClass.newCKDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
            DataRow[] drUQ = ds.Tables[TableNameClass.newUQDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
            DataRow[] drDF = ds.Tables[TableNameClass.newDFDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");
            DataRow[] drIndex = ds.Tables[TableNameClass.newIndexDT].Select("表名='" + tableName + "' and 约束列名='" + columnName + "'");

            foreach (DataRow creatColunm in drArr)
            {
                sb.Remove(0, sb.Length);
                sb.Append("alter table " + creatColunm["表名"] + "  add " + creatColunm["字段名"] + " " + creatColunm["类型"]);
                sb.Append(CreateFieldType(creatColunm, false));
                sb.Append(";");
                sqlList.Add(sb.ToString());
                if (creatColunm["说明"].ToString().Length > 0)
                {
                    CreateCommentSql(tableName, columnName, creatColunm["说明"].ToString(), sqlList);
                }
                sb.Remove(0, sb.Length);
                if (drPK.Length > 0)
                {
                    CreateAddConstraintSql(tableName, drPK[0]["约束名称"].ToString(), LogType.pkType, sqlList);
                }
                sb.Remove(0, sb.Length);
                foreach (DataRow ck in drCK)
                {
                    CreateAddConstraintSql(tableName, ck["约束名称"].ToString(), LogType.ckType, sqlList);
                }
                foreach (DataRow df in drDF)
                {
                    CreateAddConstraintSql(tableName, df["约束名称"].ToString(), LogType.dfType, sqlList);
                }
                foreach (DataRow uq in drUQ)
                {
                    CreateAddConstraintSql(tableName, uq["约束名称"].ToString(), LogType.uqType, sqlList);
                }
                foreach (DataRow indx in drIndex)
                {
                    CreateAddConstraintSql(tableName, indx["约束名称"].ToString(), LogType.indexType, sqlList);
                }

                sb.Remove(0, sb.Length);
            }
        }
        /// <summary>
        /// 创建字段类型
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string CreateFieldType(DataRow dr, bool isEdit)
        {
            if (dr == null)
                return "";
            StringBuilder sb = new StringBuilder();
            switch (Convert.ToString(dr["类型"]))
            {
                case "nvarchar":
                case "varchar":
                case "nchar":
                case "char":
                case "binary":
                case "time":
                case "varbinary":
                    if (Convert.ToString(dr["长度"]) == "-1")
                    {
                        sb.Append("(max)");
                    }
                    else
                    {
                        sb.Append("(" + dr["长度"] + ")");
                    }
                    break;
                case "decimal":
                case "numeric":
                    sb.Append("(" + dr["长度"] + "," + dr["小数位数"] + ")");
                    break;
            }
            if (!isEdit)
            {
                if (Convert.ToString(dr["标识"]) == "是")
                {
                    sb.Append(" identity(" + dr["标识种子"] + "," + dr["标识增长量"] + ")");
                }
            }
            if (Convert.ToString(dr["允许空"]) == "否")
            {
                sb.Append(" not null");
            }
            else
            {
                sb.Append(" null ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把DataRow添加到ResultDT
        /// </summary>
        /// <param name="type">类型(表,列,主键等)</param>
        /// <param name="name">类型的名称</param>
        /// <param name="operContent">操作内容(新增,删除,修改)</param>
        /// <param name="belongTable">所属表</param>
        private void AddDataRowToResultDT(string type, string name, string operContent, string belongTable)
        {
            if (ds.Tables[TableNameClass.resultDT] == null)
                return;
            resultRow = ds.Tables[TableNameClass.resultDT].NewRow();
            resultRow["type"] = type;
            resultRow["name"] = name;
            resultRow["oper"] = operContent;
            resultRow["belongtable"] = (belongTable + "").Length == 0 ? "--" : belongTable;
            resultRow["detailInfo"] = "--";
            ds.Tables[TableNameClass.resultDT].Rows.Add(resultRow);
        }
        /// <summary>
        /// 把DataRow添加到ResultDT
        /// </summary>
        /// <param name="type">类型(表,列,主键等)</param>
        /// <param name="name">类型的名称</param>
        /// <param name="operContent">操作内容(新增,删除,修改)</param>
        /// <param name="belongTable">所属表</param>
        /// <param name="detailInfo">详细信息</param>
        private void AddDataRowToResultDT(string type, string name, string operContent, string belongTable, string detailInfo)
        {
            if (ds.Tables[TableNameClass.resultDT] == null)
                return;
            resultRow = ds.Tables[TableNameClass.resultDT].NewRow();
            resultRow["type"] = type;
            resultRow["name"] = name;
            resultRow["oper"] = operContent;
            resultRow["belongtable"] = (belongTable + "").Length == 0 ? "--" : belongTable;
            resultRow["detailInfo"] = (detailInfo + "").Length == 0 ? "--" : detailInfo;
            ds.Tables[TableNameClass.resultDT].Rows.Add(resultRow);
        }
        /// <summary>
        /// 清空内存
        /// </summary>
        private void ClearDataTable()
        {
            ds.Tables.Clear();
            dicSql.Clear();
            resultRow = null;
        }
        /// <summary>
        /// 得到差异的约束
        /// </summary>
        private void GetDifferentConstraintFromDataTable()
        {
            StringBuilder sb = new StringBuilder();
            List<string> constraintList = new List<string>();
            GetDifferentContraint(ds.Tables[TableNameClass.newPKDT], ds.Tables[TableNameClass.oldPKDT], LogType.pkType, constraintList, sb);
            GetDifferentContraint(ds.Tables[TableNameClass.newUQDT], ds.Tables[TableNameClass.oldUQDT], LogType.uqType, constraintList, sb);
            GetDifferentContraint(ds.Tables[TableNameClass.newCKDT], ds.Tables[TableNameClass.oldCKDT], LogType.ckType, constraintList, sb);
            GetDifferentContraint(ds.Tables[TableNameClass.newDFDT], ds.Tables[TableNameClass.oldDFDT], LogType.dfType, constraintList, sb);
            GetDifferentContraint(ds.Tables[TableNameClass.newIndexDT], ds.Tables[TableNameClass.oldIndexDT], LogType.indexType, constraintList, sb);
            #region 外键情况暂不考虑
            /*foreach (DataRow dr in oldFKDT.Rows)
            {
                if (newFKDT.Select("外键名称='" + dr["外键名称"] + "'").Length == 0)
                {
                    if (!constraintList.Contains(Convert.ToString(dr["外键名称"])))
                    {
                        AddDataRowToResultDT(LogType.fkType, Convert.ToString(dr["外键名称"]), LogType.delType, Convert.ToString(dr["子表名称"]));
                        constraintList.Add(Convert.ToString(dr["外键名称"]));
                    }
                }
            }
            foreach (DataRow dr in newFKDT.Rows)
            {
                if (oldFKDT.Select("外键名称='" + dr["外键名称"] + "'").Length == 0)
                {
                    if (!constraintList.Contains(Convert.ToString(dr["外键名称"])))
                    {
                        AddDataRowToResultDT(LogType.fkType, Convert.ToString(dr["外键名称"]), LogType.addType, Convert.ToString(dr["子表名称"]) + "," + Convert.ToString(dr["主表名称"]));
                        constraintList.Add(Convert.ToString(dr["外键名称"]));
                    }
                }
                else
                {
                    if (oldFKDT.Select("子表名称='" + dr["子表名称"] + "' and 外键名称='" + dr["外键名称"] + "' and 子表列名='" + dr["子表列名"] + "' and 主表名称='" + dr["主表名称"] + "' and 主表列名='" + dr["主表列名"] + "'").Length == 0)
                    {
                        if (!constraintList.Contains(Convert.ToString(dr["外键名称"])))
                        {
                            AddDataRowToResultDT(LogType.fkType, Convert.ToString(dr["外键名称"]), LogType.editType, Convert.ToString(dr["子表名称"]) + "," + Convert.ToString(dr["主表名称"]));
                            constraintList.Add(Convert.ToString(dr["外键名称"]));
                        }
                    }
                }
            }*/
            #endregion

        }
        /// <summary>
        /// 获取不同的视图
        /// </summary>
        private void GetDefferentView()
        {
            var constraintList = new List<string>();
            foreach (DataRow drView in ds.Tables[TableNameClass.oldViewDT].Rows)
            {
                if (ds.Tables[TableNameClass.newViewDT].Select("视图名称='" + drView["视图名称"] + "'").Length == 0)
                {
                    if (!constraintList.Contains(Convert.ToString(drView["视图名称"])))
                    {
                        AddDataRowToResultDT(LogType.viewType, Convert.ToString(drView["视图名称"]), LogType.delType, "---");
                        constraintList.Add(Convert.ToString(drView["视图名称"]));
                    }
                }
            }
            foreach (DataRow drView in ds.Tables[TableNameClass.newViewDT].Rows)
            {
                var viewName = drView["视图名称"].ToString();
                if (ds.Tables[TableNameClass.oldViewDT].Select("视图名称='" + viewName + "'").Length == 0)
                {
                    if (!constraintList.Contains(viewName))
                    {
                        AddDataRowToResultDT(LogType.viewType, viewName, LogType.addType, "---");
                        constraintList.Add(viewName);
                    }
                }
                else
                {
                    var newColumnArr = ds.Tables[TableNameClass.newViewColumnDT].Select("视图名称='" + viewName + "'");
                    var oldColumnArr = ds.Tables[TableNameClass.oldViewColumnDT].Select("视图名称='" + viewName + "'");
                    var msg = "";
                    var isDifferent = (newColumnArr.Length != oldColumnArr.Length) || GetDifferenceColumnList(newColumnArr, oldColumnArr, ref msg).Count > 0;
                    if (isDifferent)
                    {
                        if (!constraintList.Contains(viewName))
                        {
                            AddDataRowToResultDT(LogType.viewType, viewName, LogType.editType, "---");
                            constraintList.Add(viewName);
                        }
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 获取不同的约束
        /// </summary>
        /// <param name="newDT">新的约束表集合</param>
        /// <param name="oldDT">老的约束表集合</param>
        /// <param name="logType">日志类型</param>
        /// <param name="constraintList">约束集合</param>
        private void GetDifferentContraint(DataTable newDT, DataTable oldDT, string logType, List<string> constraintList, StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();
            foreach (DataRow dr in oldDT.Rows)
            {
                sb.Remove(0, sb.Length);
                if (newDT.Select("表名='" + dr["表名"] + "' and 约束名称='" + dr["约束名称"] + "'").Length == 0 && ds.Tables[TableNameClass.newTablesDT].Select("表名='" + dr["表名"] + "'").Length > 0)
                {
                    sb.Append(Convert.ToString(dr["约束名称"]) + "," + dr["表名"]);
                    if (!constraintList.Contains(sb.ToString()))
                    {
                        AddDataRowToResultDT(logType, Convert.ToString(dr["约束名称"]), LogType.delType, Convert.ToString(dr["表名"]));
                        constraintList.Add(sb.ToString());
                    }
                }
            }
            foreach (DataRow dr in newDT.Rows)
            {
                sb.Remove(0, sb.Length);
                if (ds.Tables[TableNameClass.oldTablesDT].Select("表名='" + dr["表名"] + "' ").Length > 0 && ds.Tables[TableNameClass.newTablesDT].Select("表名='" + dr["表名"] + "' ").Length > 0)
                {
                    if (oldDT.Select("表名='" + dr["表名"] + "' and 约束名称='" + dr["约束名称"] + "'").Length == 0)
                    {
                        sb.Append(Convert.ToString(dr["约束名称"]) + "," + dr["表名"]);
                        if (!constraintList.Contains(sb.ToString()))
                        {
                            AddDataRowToResultDT(logType, Convert.ToString(dr["约束名称"]), LogType.addType, Convert.ToString(dr["表名"]));
                            constraintList.Add(sb.ToString());
                        }
                    }
                    else
                    {
                        sb.Remove(0, sb.Length);
                        foreach (DataColumn dc in oldDT.Columns)
                        {
                            sb.Append(" and " + dc.ColumnName + "='" + Convert.ToString(dr[dc.ColumnName]).Replace("'", "''") + "'");
                        }
                        if (sb.Length > 0)
                        {
                            sb.Remove(0, 4);
                        }
                        if (oldDT.Select("" + sb.ToString() + "").Length == 0)
                        {
                            sb.Remove(0, sb.Length);
                            sb.Append(Convert.ToString(dr["约束名称"]) + "," + dr["表名"]);
                            if (!constraintList.Contains(sb.ToString()))
                            {
                                AddDataRowToResultDT(logType, Convert.ToString(dr["约束名称"]), LogType.editType, Convert.ToString(dr["表名"]));
                                constraintList.Add(sb.ToString());
                            }
                        }
                    }
                    sb.Remove(0, sb.Length);
                }
            }
        }

        /// <summary>
        /// 设置表头复选框是否选中
        /// </summary>
        /// <param name="isCheck">true表示选中,false表示不选中</param>
        private void SetHeaderCheckboxIsChecked(bool isCheck)
        {
            if (dataGridView1.Columns.Count > 0)
            {
                DatagridViewCheckBoxHeaderCell cbHeader = dataGridView1.Columns[0].HeaderCell as DatagridViewCheckBoxHeaderCell;
                if (cbHeader != null)
                    cbHeader.Checked = isCheck;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (ds.Tables[TableNameClass.resultDT].Rows.Count > 0 && ds.Tables[TableNameClass.resultDT].Rows[e.RowIndex]["type"].ToString() == LogType.colunmType && ds.Tables[TableNameClass.resultDT].Rows[e.RowIndex]["oper"].ToString() == LogType.editType)
                {
                    MessageBox.Show(ds.Tables[TableNameClass.resultDT].Rows[e.RowIndex]["detailInfo"].ToString());
                }
            }
        }
        /// <summary>
        /// 对结果集进行重新排列
        /// </summary>
        private void RankResultDt()
        {
            if (ds == null || !ds.Tables.Contains(TableNameClass.resultDT))
                return;
            DataTable tmpDt = ds.Tables[TableNameClass.resultDT].Copy();
            ds.Tables[TableNameClass.resultDT].Rows.Clear();
            DataRow[] dr = tmpDt.Select("oper='" + LogType.delType + "' and type <>'" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            dr = tmpDt.Select("oper='" + LogType.delType + "' and type ='" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            dr = tmpDt.Select("oper='" + LogType.addType + "' and type='" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            dr = tmpDt.Select("oper='" + LogType.editType + "'  and type='" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            dr = tmpDt.Select("oper='" + LogType.addType + "' and type <>'" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            dr = tmpDt.Select("oper='" + LogType.editType + "'  and type <>'" + LogType.colunmType + "'");
            foreach (DataRow tmpDr in dr)
            {
                ds.Tables[TableNameClass.resultDT].ImportRow(tmpDr);
            }
            tmpDt.Clear();
        }

        private void btnSetEnvirmentVariables_Click(object sender, EventArgs e)
        {
            this.Hide();
            SetEnvirmentVarible.OldDBInfo = GetXmlNodeValue("Configs/DataConfig//OldDbConfig/DbName") + "----(" + GetXmlNodeValue("Configs/DataConfig/OldDbConfig/DbHost") + ")";
            SetEnvirmentVarible.NewDBInfo = GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbName") + "----(" + GetXmlNodeValue("Configs/DataConfig/NewDbConfig/DbHost") + ")";
            SetEnvirmentVarible set = new SetEnvirmentVarible();
            set.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
