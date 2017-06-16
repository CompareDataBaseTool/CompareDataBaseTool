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
using System.Text.RegularExpressions;

namespace CompareDataBaseTool
{
    public partial class SetEnvirmentVarible : Form
    {
        static readonly char directorySeparatorChar = Path.DirectorySeparatorChar;
        readonly string commUseDDBXmlConfigPath = Application.StartupPath + directorySeparatorChar + "Configs" + directorySeparatorChar + "CommonUsedDBList.xml";
        readonly string dbXmlConfig = Application.StartupPath + directorySeparatorChar + "Configs" + directorySeparatorChar + "config.xml";

        public static string NewDBInfo { get; set; }
        public static string OldDBInfo { get; set; }
        public SetEnvirmentVarible()
        {
            InitializeComponent();
            InitCommonUsedDBList();
            ClearDBConfigTxt();
        }

        private void InitCommonUsedDBList()
        {
            cbOldDBList.Items.Clear();
            cbNewDBList.Items.Clear();
            if (!IsXmlFileExists())
            {
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(commUseDDBXmlConfigPath);
            XmlNodeList nodeList = xmlDoc.SelectNodes("dbList/db");
            foreach (XmlNode node in nodeList)
            {
                string item = "";
                if (node.Attributes["name"] != null)
                    item += node.Attributes["name"].Value;
                if (item.Length > 0)
                {
                    item += "----(" + node.Attributes["ip"].Value + ")";
                    cbOldDBList.Items.Add(item);
                    cbNewDBList.Items.Add(item);
                }
            }
            if (cbNewDBList.Items.Count > 0 || cbOldDBList.Items.Count > 0)
            {
                cbOldDBList.SelectedItem = OldDBInfo;
                cbNewDBList.SelectedItem = NewDBInfo;
                if (cbOldDBList.SelectedItem == null)
                    cbOldDBList.SelectedIndex = 0;
                if (cbNewDBList.SelectedItem == null)
                    cbNewDBList.SelectedIndex = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!IsXmlFileExists())
            {
                return;
            }
            if (txtDBIP.Text.Length == 0 || txtDBName.Text.Length == 0)
            {
                MessageBox.Show("数据库IP和名称不能为空");
                return;
            }
            Regex rg = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
            if (!rg.IsMatch(txtDBIP.Text))
            {
                MessageBox.Show("请输入有效的IP地址");
                return;
            }
            if (txtDBName.Text.Length == 0 || txtDBPwd.Text.Length == 0)
            {
                MessageBox.Show("数据库用户名和密码不能为空");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(commUseDDBXmlConfigPath);
            XmlNode node = xmlDoc.SelectSingleNode("dbList");
            XmlNodeList nodeList = node.SelectNodes("db[@name='" + txtDBName.Text + "'][@ip='" + txtDBIP.Text + "']");
            if (nodeList.Count > 0)
            {
                MessageBox.Show("数据库已经存在列表");
                return;
            }
            XmlElement element = xmlDoc.CreateElement("db");
            element.SetAttribute("name", txtDBName.Text);
            element.SetAttribute("ip", txtDBIP.Text);
            element.SetAttribute("userName", txtDBUserName.Text);
            element.SetAttribute("pwd", txtDBPwd.Text);
            node.InsertBefore(element, node.FirstChild);
            xmlDoc.Save(commUseDDBXmlConfigPath);
            ClearDBConfigTxt();
            InitCommonUsedDBList();
        }
        bool IsXmlFileExists()
        {
            if (!File.Exists(commUseDDBXmlConfigPath))
            {
                LogHelper.WriteLog("缺少常用数据库配置文件,文件路径为：" + commUseDDBXmlConfigPath);
                MessageBox.Show("缺少常用数据库配置文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        void ClearDBConfigTxt()
        {
            txtDBIP.Text = txtDBName.Text = "";
            txtDBUserName.Text = "";
            txtDBPwd.Text = "";
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            if (cbOldDBList.SelectedItem == null || cbNewDBList.SelectedItem == null)
            {
                MessageBox.Show("必须选择升级的新老数据库");
                return;
            }
            string oldDB = cbOldDBList.SelectedItem as string;
            string newDB = cbNewDBList.SelectedItem as string;
            if (oldDB.Equals(newDB))
            {
                MessageBox.Show("新老数据库不能一样");
                return;
            }
            string oldDBName = oldDB.Substring(0, oldDB.IndexOf("-"));
            string oldDBIP = oldDB.Substring(oldDB.IndexOf("(") + 1, oldDB.IndexOf(")") - oldDB.IndexOf("(") - 1);
            string newDBName = newDB.Substring(0, newDB.IndexOf("-"));
            string newDBIP = newDB.Substring(newDB.IndexOf("(") + 1, newDB.IndexOf(")") - newDB.IndexOf("(") - 1);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(commUseDDBXmlConfigPath);
            XmlNode oldDBNode = xmlDoc.SelectSingleNode("dbList/db[@name='" + oldDBName + "'][@ip='" + oldDBIP + "']");
            XmlNode newDBNode = xmlDoc.SelectSingleNode("dbList/db[@name='" + newDBName + "'][@ip='" + newDBIP + "']");
            ModifyDBConfigXmlFile(oldDBNode, newDBNode);
            this.Close();
            ShowMainFormDialog();

        }

        private void ModifyDBConfigXmlFile(XmlNode oldDBNode, XmlNode newDBNode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(dbXmlConfig);
            XmlNode node = xmlDoc.SelectSingleNode("Configs/DataConfig/OldDbConfig");
            EditDBConfigNode(node, oldDBNode);
            node = xmlDoc.SelectSingleNode("Configs/DataConfig/NewDbConfig");
            EditDBConfigNode(node, newDBNode);
            xmlDoc.Save(dbXmlConfig);
        }
        private void EditDBConfigNode(XmlNode dbConfigNode, XmlNode nodeValue)
        {
            dbConfigNode.SelectSingleNode("./DbName").InnerText = nodeValue.Attributes["name"].Value;
            dbConfigNode.SelectSingleNode("./DbHost").InnerText = nodeValue.Attributes["ip"].Value;
            dbConfigNode.SelectSingleNode("./UserID").InnerText = nodeValue.Attributes["userName"].Value;
            dbConfigNode.SelectSingleNode("./Password").InnerText = nodeValue.Attributes["pwd"].Value;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (!IsXmlFileExists())
            {
                return;
            }
            if (txtDBIP.Text.Length == 0 || txtDBName.Text.Length == 0)
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(commUseDDBXmlConfigPath);
            XmlNode node = xmlDoc.SelectSingleNode("dbList/db[@name='" + txtDBName.Text + "'][@ip='" + txtDBIP.Text + "']");
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
            }
            xmlDoc.Save(commUseDDBXmlConfigPath);
            InitCommonUsedDBList();
            ClearDBConfigTxt();
        }

        private void SetEnvirmentVarible_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            ShowMainFormDialog();
        }

        void ShowMainFormDialog()
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
    }
}
