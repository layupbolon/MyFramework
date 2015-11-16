using System.Data.Entity.Validation;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace Framework.Core.Logging
{
    #region NHHLogger

    public class NHHAPILogger:IAPILogger
    {
        #region Context
        private APILogingContext m_Context;
        /// <summary>
        /// 日志上下文对象
        /// </summary>
        protected APILogingContext Context
        {
            get
            {
                if (m_Context == null)
                {
                    //日志数据库链接
                    var dbname = ParamManager.GetStringValue("logging:database");
                    m_Context = string.IsNullOrEmpty(dbname) ? new APILogingContext() : new APILogingContext(dbname);
                }
                return m_Context;
            }
        }
        #endregion

        /// <summary>
        /// 写入API调用日志
        /// </summary>
        /// <param name="apiRoute">API路由</param>
        /// <param name="sourceUrl">来源URL</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientAgent">客户端代理信息</param>
        /// <param name="postData">接口输入参数</param>
        /// <param name="returnData">接口返回值</param>
        public void OperatorLog(string apiRoute,string sourceUrl,string clientIp,string clientAgent,string postData,string returnData)
        {
                var log = new APIOperationLog();
                log.APIRoute = apiRoute;
                log.SourceURL = sourceUrl;
                log.ClientIP = clientIp;
                log.ClientAgent = clientAgent;
                log.PostData = postData;
                log.ReturnData = returnData;
                log.InDate = DateTime.Now;

                this.Context.APIOperationLogs.Add(log);
                //this.Context.Configuration.ValidateOnSaveEnabled = false;  
                this.Context.SaveChanges();

        }

        /// <summary>
        /// 写入API异常日志
        /// </summary>
        /// <param name="apiRoute">API路由</param>
        /// <param name="postData">接口输入参数</param>
        /// <param name="message">异常消息</param>
        /// <param name="stackTrace">异常堆栈信息</param>
        public void ExceptionLog(string apiRoute, string postData, string message, string stackTrace)
        {
            var log = new APIExceptionLog();
            log.APIRoute = apiRoute;
            log.PostData = postData;
            log.Message = message;
            log.StackTrace = stackTrace;
            log.InDate = DateTime.Now;

            this.Context.APIExceptionLogs.Add(log);
            this.Context.SaveChanges();
        }

    } 
    #endregion

    #region LoggingContext
    /// <summary>
    /// 系统日志记录数据库上下文
    /// </summary>
    public class APILogingContext : DbContext
    {
        public APILogingContext()
            : base()
        {

        }
        public APILogingContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public DbSet<APIOperationLog> APIOperationLogs { get; set; }

        public DbSet<APIExceptionLog> APIExceptionLogs { get; set; }
    }
    #endregion

    #region AppEventLog
    /// <summary>
    /// API调用日志实体对象
    /// </summary>
    public class APIOperationLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        [StringLength(100)]
        public string APIRoute { get; set; }
        [StringLength(100)]
        public string SourceURL { get; set; }
        [StringLength(20)]
        public string ClientIP { get; set; }
        [StringLength(200)]
        public string ClientAgent { get; set; }
        
        public string PostData { get; set; }
        
        public string ReturnData { get; set; }
       
        public DateTime InDate { get; set; }
    }
    /// <summary>
    /// API异常日志实体对象
    /// </summary>
    public class APIExceptionLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        [StringLength(100)]
        public string APIRoute { get; set; }
        [StringLength(2000)]
        public string PostData { get; set; }
        [StringLength(500)]
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public DateTime InDate { get; set; }
    }

    
    #endregion
    
}
