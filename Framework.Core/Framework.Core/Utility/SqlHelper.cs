using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    /// <summary>
    /// Sql助手
    /// </summary>
    public class SqlHelper
    {
        #region 私有构造函数和方法

        private SqlHelper() { }

        /// <summary> 
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">SqlParameters数组</param> 
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters) 
        { 
            if (command == null) throw new ArgumentNullException("command"); 
            if (commandParameters != null) 
            { 
                foreach (SqlParameter p in commandParameters) 
                { 
                    if (p != null) 
                    { 
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && 
                            (p.Value == null)) 
                        { 
                            p.Value = DBNull.Value; 
                        } 
                        command.Parameters.Add(p); 
                    } 
                } 
            } 
        }
        
        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数 
        /// </summary> 
        /// <param name="command">要处理的SqlCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param> 
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, string commandText, SqlParameter[] commandParameters) 
        { 
            if (command == null) throw new ArgumentNullException("command"); 
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it 
            if (connection.State != ConnectionState.Open) 
            { 
                connection.Open(); 
            }

            // 给命令分配一个数据库连接. 
            command.Connection = connection;

            // 设置命令文本(存储过程名或SQL语句) 
            command.CommandText = commandText;

            // 分配事务 
            if (transaction != null) 
            { 
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction"); 
                command.Transaction = transaction; 
            }

            // 设置命令类型. 
            command.CommandType = CommandType.Text;

            // 分配命令参数 
            if (commandParameters != null) 
            { 
                AttachParameters(command, commandParameters); 
            } 
            return; 
        }

        #endregion 私有构造函数和方法结束

        #region 数据库连接 
        /// <summary> 
        /// 一个有效的数据库连接字符串 
        /// </summary> 
        /// <returns></returns> 
        public static string GetConnSting() 
        { 
            return ConfigurationManager.ConnectionStrings["NhhConnStr"].ConnectionString; 
        } 

        /// <summary> 
        /// 一个有效的数据库连接对象 
        /// </summary> 
        /// <returns></returns> 
        public static SqlConnection GetConnection() 
        { 
            SqlConnection Connection = new SqlConnection(SqlHelper.GetConnSting()); 
            return Connection; 
        } 
        #endregion

        #region ExecuteNonQuery命令

        /// <summary> 
        /// 执行指定连接字符串,类型的SqlCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery("PublishOrders"); 
        /// </remarks> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string commandText) 
        { 
            return ExecuteNonQuery(commandText, (SqlParameter[])null); 
        }

        /// <summary> 
        /// 执行指定连接字符串,类型的SqlCommand.如果没有提供参数,不返回结果. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery("PublishOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="commandText">存储过程名称或SQL语句</param> 
        /// <param name="commandParameters">SqlParameter参数数组</param> 
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = GetConnSting();
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 创建SqlCommand命令,并进行预处理 
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandText, commandParameters);

                // Finally, execute the command 
                int retval = cmd.ExecuteNonQuery();

                // 清除参数,以便再次使用. 
                cmd.Parameters.Clear();
                connection.Close();
                return retval;
            }
        }        

        /// <summary> 
        /// 执行带事务的SqlCommand. 
        /// </summary> 
        /// <remarks> 
        /// 示例.:  
        ///  int result = ExecuteNonQuery(trans, "PublishOrders"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回影响的行数/returns> 
        public static int ExecuteNonQuery(SqlTransaction transaction, string commandText) 
        { 
            return ExecuteNonQuery(transaction, commandText, (SqlParameter[])null); 
        }

        /// <summary> 
        /// 执行带事务的SqlCommand(指定参数). 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int result = ExecuteNonQuery(trans, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的数据库连接对象</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回影响的行数</returns> 
        public static int ExecuteNonQuery(SqlTransaction transaction, string commandText, params SqlParameter[] commandParameters) 
        { 
            if (transaction == null) throw new ArgumentNullException("transaction"); 
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 预处理 
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandText, commandParameters);

            // 执行 
            int retval = cmd.ExecuteNonQuery();
            // 清除参数集,以便再次使用. 
            cmd.Parameters.Clear(); 
            return retval; 
        }

        #endregion ExecuteNonQuery方法结束

        #region ExecuteDataset方法
        /// <summary>
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataTable. 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = GetConnSting();
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (var connection = new SqlConnection(connectionString))
            {
                // 预处理 
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandText, commandParameters);

                // 创建SqlDataAdapter和DataSet. 
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable table = new DataTable();
                    // 填充DataSet. 
                    da.Fill(table);
                    cmd.Parameters.Clear();
                    connection.Close();
                    return table;
                }
            }
        }

        /// <summary> 
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  DataSet ds = ExecuteDataset("GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回一个包含结果集的DataSet</returns> 
        public static DataSet ExecuteDataset(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = GetConnSting();
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (var connection = new SqlConnection(connectionString))
            {
                // 预处理 
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, (SqlTransaction)null, commandText, commandParameters);

                // 创建SqlDataAdapter和DataSet. 
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    // 填充DataSet. 
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                    connection.Close();
                    return ds;
                }
            }
        }

        #endregion ExecuteDataset数据集命令结束

        #region ExecuteScalar 返回结果集中的第一行第一列
        /// <summary> 
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar("GetOrderCount"); 
        /// </remarks> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(string commandText)
        {
            // 执行参数为空的方法 
            return ExecuteScalar(commandText, (SqlParameter[])null);
        }

        /// <summary> 
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar("GetOrderCount", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(string commandText, params SqlParameter[] commandParameters)
        {
            string connectionString = GetConnSting();
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

            using (var connection = new SqlConnection(connectionString))
            {
                // 创建SqlCommand命令,并进行预处理 
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, commandText, commandParameters);

                // 执行SqlCommand命令,并返回结果. 
                object retval = cmd.ExecuteScalar();
                // 清除参数,以便再次使用. 
                cmd.Parameters.Clear();
                connection.Close();
                return retval;
            }
        }
                
        /// <summary> 
        /// 执行指定数据库事务的命令,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount"); 
        /// </remarks> 
        /// <param name="transaction">一个有效的连接事务</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(SqlTransaction transaction, string commandText) 
        { 
            // 执行参数为空的方法 
            return ExecuteScalar(transaction, commandText, (SqlParameter[])null); 
        }

        /// <summary> 
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="transaction">一个有效的连接事务</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param> 
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(SqlTransaction transaction, string commandText, params SqlParameter[] commandParameters) 
        { 
            if (transaction == null) throw new ArgumentNullException("transaction"); 
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 创建SqlCommand命令,并进行预处理 
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandText, commandParameters);

            // 执行SqlCommand命令,并返回结果. 
            object retval = cmd.ExecuteScalar();
            // 清除参数,以便再次使用. 
            cmd.Parameters.Clear(); 
            return retval; 
        }

        #endregion ExecuteScalar
    }
}
