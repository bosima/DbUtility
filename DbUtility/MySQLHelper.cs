using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace FireflySoft.DbUtility
{
    /// <summary>
    /// MySQL数据访问基础类
    /// </summary>
    public class MySQLHelper
    {
        public static string connectionString = ConfigurationManager.AppSettings["MySQLConn"];

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="str"></param>
        public static void SetConnectionString(string str)
        {
            connectionString = str;
        }

        #region 通用快捷方法
        /// <summary>
        /// 执行一条SQL语句，确定记录是否存在
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns></returns>
        public static bool Exists(string sql)
        {
            object obj = GetSingle(sql);

            int cmdresult;
            if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }

            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行一条SQL语句，确定记录是否存在
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="paras">SQL参数数组</param>
        /// <returns></returns>
        public static bool Exists(string sql, params MySqlParameter[] paras)
        {
            object obj = GetSingle(sql, paras);

            int cmdresult;
            if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <returns></returns>
        public static int GetCount(string tableName, string sqlCondition)
        {
            string sql = "select count(1) from `" + tableName + "`";

            if (!string.IsNullOrWhiteSpace(sqlCondition))
            {
                sql += " where " + sqlCondition;
            }

            var ds = Query(sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="paras">SQL参数数组</param>
        /// <returns></returns>
        public static int GetCount(string tableName, string sqlCondition, MySqlParameter[] paras)
        {
            string sql = "select count(1) from `" + tableName + "`";

            if (!string.IsNullOrWhiteSpace(sqlCondition))
            {
                sql += " where " + sqlCondition;
            }

            DataSet ds = Query(sql, paras);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region  执行简单SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数（可自定义超时时间）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">执行超时时间</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlByTime(string sql, int timeout)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    cmd.CommandTimeout = timeout;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">多条SQL语句</param>		
        public static void ExecuteSqlTrans(ArrayList sqlList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            for (int n = 0; n < sqlList.Count; n++)
                            {
                                string sql = sqlList[n].ToString();

                                if (sql.Trim().Length > 1)
                                {
                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            trans.Commit();
                        }
                        catch (MySqlException ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条SQL查询语句，返回查询结果。
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    connection.Open();

                    object obj = cmd.ExecuteScalar();

                    if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader（切记要手工关闭MySqlDataReader）
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string sql)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand(sql, connection);

            connection.Open();
            MySqlDataReader myReader = cmd.ExecuteReader();
            return myReader;
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter command = new MySqlDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();

                    connection.Open();
                    command.Fill(ds, "ds");

                    return ds;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet（可自定义超时时间）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static DataSet Query(string sql, int timeout)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter command = new MySqlDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();

                    connection.Open();
                    command.SelectCommand.CommandTimeout = timeout;
                    command.Fill(ds, "ds");

                    return ds;
                }
            }
        }
        #endregion

        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">SQL参数数组</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string sql, params MySqlParameter[] paras)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, connection, null, sql, paras);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
            }
        }

        /// <summary>
        /// 执行添加SQL语句，返回记录的ID（自动产生的自增主键）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">SQL参数</param>
        /// <returns>记录的ID</returns>
        public static int ExecuteAdd(string sql, params MySqlParameter[] parms)
        {
            sql = sql + ";select @@identity as newautoid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, connection, null, sql, parms);

                    int recordID;
                    try
                    {
                        recordID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    }
                    catch
                    {
                        recordID = -1;
                    }

                    cmd.Parameters.Clear();

                    return recordID;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">SQL语句的哈希表（key为sql语句，value是该语句的MySqlParameter[]）</param>
        public static void ExecuteSqlTrans(Hashtable sqlList)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        try
                        {
                            foreach (DictionaryEntry entry in sqlList)
                            {
                                var sql = entry.Key.ToString();
                                var paras = (MySqlParameter[])entry.Value;

                                PrepareCommand(cmd, conn, trans, sql, paras);

                                int val = cmd.ExecuteNonQuery();

                                cmd.Parameters.Clear();

                                trans.Commit();
                            }
                        }
                        catch (MySqlException ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parms">SQL参数</param>
        /// <returns>查询结果</returns>
        public static object GetSingle(string sql, params MySqlParameter[] parms)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, conn, null, sql, parms);

                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();

                    if ((object.Equals(obj, null)) || (object.Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回MySqlDataReader (切记要手工关闭MySqlDataReader)
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="parms">SQL参数</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader ExecuteReader(string sql, params MySqlParameter[] parms)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                PrepareCommand(cmd, connection, null, sql, parms);

                MySqlDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();

                return myReader;
            }
            catch (MySqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string sql, params MySqlParameter[] paras)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, connection, null, sql, paras);
                    DataSet ds = new DataSet();

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();

                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// 准备SQL查询命令
        /// </summary>
        /// <param name="cmd">SQL命令对象</param>
        /// <param name="conn">SQL连接对象</param>
        /// <param name="trans">SQL事务对象</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">SQL参数数组</param>
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] paras)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = CommandType.Text;
            if (paras != null)
            {
                foreach (MySqlParameter parameter in paras)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

        #region 存储过程操作
        /// <summary>
        /// 执行存储过程  (切记要手工关闭MySqlDataReader)
        /// </summary>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数</param>
        /// <returns>MySqlDataReader</returns>
        public static MySqlDataReader RunProcedure(string spcName, IDataParameter[] paras)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = PrepareTransCommand(connection, spcName, paras);

            connection.Open();
            command.CommandType = CommandType.StoredProcedure;
            MySqlDataReader returnReader = command.ExecuteReader();
            return returnReader;
        }

        /// <summary>
        /// 执行存储过程，返回DataSet
        /// </summary>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数数组</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string spcName, IDataParameter[] paras, string tableName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter sqlDA = new MySqlDataAdapter())
                {
                    DataSet dataSet = new DataSet();

                    connection.Open();
                    sqlDA.SelectCommand = PrepareTransCommand(connection, spcName, paras);
                    sqlDA.Fill(dataSet, tableName);
                    return dataSet;
                }
            }
        }

        /// <summary>
        /// 执行存储过程，返回DataSet（可自定义超时时间）
        /// </summary>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数数组</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <param name="timeout">执行超时时间</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string spcName, IDataParameter[] paras, string tableName, int timeout)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter sqlDA = new MySqlDataAdapter())
                {
                    DataSet dataSet = new DataSet();

                    connection.Open();
                    sqlDA.SelectCommand = PrepareTransCommand(connection, spcName, paras);
                    sqlDA.SelectCommand.CommandTimeout = timeout;
                    sqlDA.Fill(dataSet, tableName);
                    return dataSet;
                }
            }
        }

        /// <summary>
        /// 执行存储过程，返回int类型的结果		
        /// </summary>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns>int类型的结果</returns>
        public static int RunProcedure(string spcName, IDataParameter[] paras, out int rowsAffected)
        {
            int result;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = PrepareTransCommandWithIntReturnVale(connection, spcName, paras))
                {
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;

                    return result;
                }
            }
        }

        /// <summary>
        /// 创建 MySqlCommand 对象（用来返回一个整数值）
        /// </summary>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数</param>
        /// <returns>MySqlCommand 对象实例</returns>
        private static MySqlCommand PrepareTransCommandWithIntReturnVale(MySqlConnection connection, string spcName, IDataParameter[] paras)
        {
            MySqlCommand command = PrepareTransCommand(connection, spcName, paras);

            command.Parameters.Add(new MySqlParameter("ReturnValue", MySqlDbType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));

            return command;
        }

        /// <summary>
        /// 构建 MySqlCommand 对象
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="spcName">存储过程名</param>
        /// <param name="paras">存储过程参数数组</param>
        /// <returns>MySqlCommand对象</returns>
        private static MySqlCommand PrepareTransCommand(MySqlConnection connection, string spcName, IDataParameter[] paras)
        {
            MySqlCommand command = new MySqlCommand(spcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParameter parameter in paras)
            {
                if (parameter != null)
                {
                    // 检查空值的参数，将其设置为DBNull.Value
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion
    }
}
