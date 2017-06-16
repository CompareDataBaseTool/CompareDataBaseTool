using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
namespace CompareDataBaseTool
{
    public class DBHelper
    {
        private string dbProviderName = null;
        private string dbConnectionString = null;
        private DbConnection connection;
        DbCommand dbCommand = null;
        public DBHelper(string connectionString, string dbProvider)
        {
            dbProviderName = dbProvider;
            dbConnectionString = connectionString;
            connection = CreateDBConnection();
            dbCommand = connection.CreateCommand();
        }
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        private DbConnection CreateDBConnection()
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(dbProviderName);
            DbConnection dbConn = dbFactory.CreateConnection();
            dbConn.ConnectionString = dbConnectionString;
            return dbConn;
        }
        /// <summary>
        /// 更加sql语句生成DBCommand对象
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        private DbCommand GetSqlStringCommond(string sqlQuery)
        {
            if (dbCommand == null)
            {
                dbCommand = connection.CreateCommand();
            }
            dbCommand.CommandText = sqlQuery;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }
        #region 执行
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sqlList">sql语句的集合</param>
        /// <param name="errorMsg">错误消息</param>
        /// <returns></returns>
        public bool ExecuteTrans(List<string> sqlList,ref string errorMsg)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (DbTransaction dbTrans = connection.BeginTransaction())
            {
                DbCommand dbCmd = connection.CreateCommand();
                dbCmd.Transaction = dbTrans;
                try
                {
                    foreach (string sql in sqlList)
                    {
                        if ((sql + "").Length == 0)
                            continue;
                        sb.Remove(0, sb.Length);
                        sb.Append(sql);
                        dbCmd.CommandText = sql;
                        dbCmd.ExecuteNonQuery();
                        LogHelper.WriteLog("执行sql："+sql);
                    }
                    dbTrans.Commit();
                    flag = true;
                }
                catch(Exception ex)
                {
                    errorMsg = ex.Message;
                    LogHelper.WriteLog("异常SQL语句为：" + sb.ToString()+"||"+"异常信息为：" + ex.ToString());
                    dbTrans.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                    if (sqlList != null)
                        sqlList.Clear();
                    sb.Remove(0, sb.Length);
                }
            }
            return flag;
        }
        /// <summary>
        /// 根据sql返回DataSet
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sqlQuery)
        {
            DbCommand cmd = GetSqlStringCommond(sqlQuery);
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 根据sql返回DataTable
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlQuery)
        {
            DbCommand cmd = GetSqlStringCommond(sqlQuery);
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            try
            {
                dbDataAdapter.Fill(dataTable);
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("异常SQL语句为：" +sqlQuery + "||" + "异常信息为：" + ex.ToString());
            }
            return dataTable;
        }
        /// <summary>
        /// 根据sql返回DataTable
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlQuery,string tableName)
        {
            DbCommand cmd = GetSqlStringCommond(sqlQuery);
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(dbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dataTable.TableName = tableName;
            try
            {
                dbDataAdapter.Fill(dataTable);
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("异常SQL语句为：" + sqlQuery + "||" + "异常信息为：" + ex.ToString());
            }
            return dataTable;
        }
        /// <summary>
        /// 根据sql返回DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            DbDataReader reader = null;
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch(Exception ex)
            {
                reader = null;
                LogHelper.WriteLog("异常SQL语句为：" + sql+ "||" + "异常信息为：" + ex.ToString());
            }
            finally
            {
                cmd.Connection.Close();
            }
            return reader;
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            int ret = 0;
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                ret = cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("异常SQL语句为：" + sql + "||" + "异常信息为：" + ex.ToString());
                ret = 0;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        /// <summary>
        /// 执行sql语句返回一行一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            object ret = null;
            DbCommand cmd = GetSqlStringCommond(sql);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }
                ret = cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("异常SQL语句为：" + sql + "||" + "异常信息为：" + ex.ToString());
                ret = null;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return ret;
        }
        #endregion
    }
}
