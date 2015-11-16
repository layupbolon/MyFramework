using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Data
{
    public class NHHDbLogFormatter : DatabaseLogFormatter
    {
        public NHHDbLogFormatter(DbContext context, Action<string> writeAction)
            : base(context, writeAction)
        {
        }

        public override void LogCommand<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            var cmd = command.CommandText.ToLower();

            //记录写入相关的SQL脚本信息
            if (cmd.IndexOf("insert ") >= 0 || cmd.IndexOf("update ") >= 0 || cmd.IndexOf("delete ") >= 0)
            {
                var sql = command.CommandText + ";\r\n";
                foreach (DbParameter p in command.Parameters)
                {
                    sql += string.Format(" --> @{0}={1} \r\n", p.ParameterName.TrimStart('@'), p.Value);
                }

               
                Write(sql);
            }
        }

        public override void LogResult<TResult>(
            DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            //记录异常信息
            if (interceptionContext.Exception != null)
            {
                var err = string.Format("Exception:{0} \r\n --> Error executing command: {1}", interceptionContext.Exception.ToString(), command.CommandText);
                Write(err);
            }
        }

        public override void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //不记录连接打开信息
        }

        public override void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            //不记录连接关闭信息
        }

        public override void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        {
            //不记录事务开启信息
        }

        public override void Committed(DbTransaction transaction, DbTransactionInterceptionContext interceptionContext)
        {
            //不记录事务提交信息
        }

    }
}
