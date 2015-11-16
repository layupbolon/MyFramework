using Framework.Core.BizLogic;
using Framework.Core.Data;
using Framework.Core.Exceptions;
using Framework.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class NHHService<TContext>
        where TContext : DbContext, new()
    {

        #region Context
        private TContext m_Context;
        /// <summary>
        /// 业务逻辑对象对应的数据库上下文对象
        /// </summary>
        protected TContext Context
        {
            get
            {
                if (m_Context == null)
                {
                    m_Context = new TContext();
                    m_Context.Database.Log = sql => this.Logger.Debug(m_Context, "SQL Tracing", sql);
                }
                return m_Context;
            }
        } 
        #endregion

        #region CreateBizObject
        /// <summary>
        /// 构建业务逻辑对象，服务对象生命周期内需要通过该方法统一构建业务逻辑对象，确保数据访问一致性。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public BizObject<TContext, TEntity> CreateBizObject<TEntity>()
            where TEntity : class
        {
            return new BizObject<TContext, TEntity>(this.Context);
        }
        /// <summary>
        /// 构建业务逻辑对象，服务对象生命周期内需要通过该方法统一构建业务逻辑对象，确保数据访问一致性。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public T CreateBizObject<T, TEntity>()
            where TEntity : class
            where T : BizObject<TContext, TEntity>, new()
        {
            var biz = new T();
            biz.Context = this.Context;
            return biz;
        }
        #endregion

        #region CreateCacheBizObject
        /// <summary>
        /// 构建基于缓存的业务逻辑对象，服务对象生命周期内需要通过该方法统一构建业务逻辑对象，确保数据访问一致性。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public BizCacheObject<TContext, TEntity> CreateCacheBizObject<TEntity>() where TEntity : class
        {
            return new BizCacheObject<TContext, TEntity>(this.Context);
        }
        /// <summary>
        /// 构建基于缓存的业务逻辑对象，服务对象生命周期内需要通过该方法统一构建业务逻辑对象，确保数据访问一致性。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public T CreateCacheBizObject<T, TEntity>()
            where TEntity : class
            where T : BizCacheObject<TContext, TEntity>, new()
        {
            var biz = new T();
            biz.Context = this.Context;
            return biz;
        }  
        #endregion

        #region SaveChanges
        /// <summary>
        /// 提交当前服务对象的所有数据更改
        /// </summary>
        public void SaveChanges()
        {
            
            this.Context.SaveChanges();
        } 
        #endregion

        #region ExecSQL

        /// <summary>
        /// 通过SQL语句的方式直接在数据库执行操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T ExecSQLToSingle<T>(string sql)
        {
            T rlt = default(T);
            try
            {
                rlt = this.Context.Database.SqlQuery<T>(sql).SingleOrDefault();
            }
            catch (Exception ex)
            {
                this.HandleException(string.Format("直接执行SQL命令失败：{0}", sql), ex);
            }
            return rlt;
        }

        /// <summary>
        /// 通过SQL语句的方式直接在数据库执行操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> ExecSQL<T>(String sql, params SqlParameter[] parameters)
        {
            List<T> rlt = null;
            try
            {
                rlt = this.Context.Database.SqlQuery<T>(sql, parameters).ToList();
                
            }
            catch (Exception ex)
            {
                this.HandleException(string.Format("直接执行SQL命令失败：{0}", sql), ex);
            }
            return rlt;
        }
        #endregion

        #region ExecCmd
        /// <summary>
        /// 通过指定名称的预配置SQL语句的方式直接在数据库执行操作
        /// </summary>
        /// <param name="cmd">预配置SQL语句名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> ExecCmd<T>(string cmd, params SqlParameter[] parameters)
        {
            List<T> rlt = null;
            string msg=null;
            try
            {
                var c = DataCommandManager.GetDataCommand(cmd);
                if (c == null)
                {
                    msg = string.Format("直接执行SQL命令失败：配置中不存在预设SQL命令[{0}]", cmd);
                }

                //命令类型判断、参数校验等。。。
                foreach (var p in c.Parameters)
                {
                    if (!parameters.Any(x => p.Name.Equals(x.ParameterName, StringComparison.OrdinalIgnoreCase) && (p.DbType == x.DbType)))
                    {
                        msg = string.Format("直接执行SQL命令失败：配置中预设SQL命令[{0}]参数[{1}]与输入项不匹配", cmd, p.Name);
                        break;
                    }
                }
                
                rlt = this.Context.Database.SqlQuery<T>(c.CommandText, parameters).ToList();
            }
            catch (Exception ex)
            {
                this.HandleException(msg??string.Format("直接执行SQL命令失败：{0}", cmd), ex);
            }
            return rlt;
        }

        #endregion

        #region HandleException
        /// <summary>
        /// 服务未知异常处理，默认将原有异常封装成NHHException重新抛出，同时记录到应用程序日志中。
        /// </summary>
        /// <param name="message">异常消息说明</param>
        /// <param name="ex">未知异常</param>
        /// <param name="rethrow">是否重新抛出</param>
        public void HandleException(string message, Exception ex=null, bool rethrow = true)
        {
            var newex = new NHHException(message, ex);
            this.LogException(ex);
            if (rethrow)
            {
                throw ex;
            }
        } 
        #endregion

        #region Logger
        private IAPPLogger m_Logger;
        /// <summary>
        /// 获取当前服务日志记录器
        /// </summary>
        protected IAPPLogger Logger
        {
            get
            {
                if (m_Logger == null)
                {
                    m_Logger = LoggerManager.GetAPPLogger(this.GetType().Name);
                }
                return m_Logger;
            }
        }
        #endregion

        #region LogException
        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        public Exception LogException(Exception ex)
        {
            this.Logger.Error(this, ex);
            return ex;
        }
        #endregion

        #region LogMessage
        /// <summary>
        /// 记录消息日志
        /// </summary>
        /// <param name="detail">消息对象</param>
        public void LogMessage(string message)
        {
            this.Logger.Info(this, message);
        }
        /// <summary>
        /// 记录消息日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="detail">明细</param>
        public void LogMessage(string message,string detail)
        {
            this.Logger.Info(this, message, detail);
        } 
        #endregion
       
    }
}
