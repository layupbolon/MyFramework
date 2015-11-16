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

    public class NHHAPPLogger : IAPPLogger
    {
        public NHHAPPLogger()
        {
            this.Category = "General";

        }

        public NHHAPPLogger(string category)
        {
            this.Category = category;
        }

        /// <summary>
        /// 日志分类
        /// </summary>
        public string Category { get; set; }

        #region Context
        private LoggingContext m_Context;
        /// <summary>
        /// 日志上下文对象
        /// </summary>
        protected LoggingContext Context
        {
            get
            {
                if (m_Context == null)
                {
                    //日志数据库链接
                    var dbname = ParamManager.GetStringValue("logging:database");
                    m_Context = string.IsNullOrEmpty(dbname) ? new LoggingContext() : new LoggingContext(dbname);
                }
                return m_Context;
            }
        }
        #endregion

        #region Level
        protected static readonly Dictionary<string, int> LoggingLevels = new Dictionary<string, int>() { { "ERROR", 1 }, { "WARNING", 2 }, { "INFO", 3 }, { "DEBUG", 4 }, { "TRACE", 5 } };
        private int m_Level = -1;
        protected int Level
        {
            get
            {
                if (m_Level == -1)
                {
                    var lname = (ParamManager.GetStringValue("logging:level") ?? "DEBUG").ToUpper();
                    m_Level = LoggingLevels[lname];
                }
                return m_Level;
            }
        }
        #endregion

        #region Log
        protected void Log(object sender, string level, Exception exception)
        {
            var message = string.Empty;
            var detail = string.Empty;
            if (exception != null)
            {
                message = exception.Message;
                detail += string.Format("Message：{0}\r\n", exception.Message);
                detail += string.Format("Source：{0}\r\n", exception.Source);
                detail += string.Format("TargetSite：{0}\r\n", exception.TargetSite);
                detail += string.Format("StackTrace：{0}\r\n", exception.StackTrace);
            
            }

            this.Log(sender, level, message, detail);
        }

        protected void Log(object sender, string level, string message, string detail)
        {
            var log = new AppEventLog();
            log.Sender = sender.GetType().FullName;
            log.Category = this.Category;
            log.Level = level;
            log.Message = message;
            log.Detail = detail;

            if (HttpContext.Current != null)
            {
                log.HostName = HttpContext.Current.Server.MachineName;
                log.Url = HttpContext.Current.Request.Url.ToString();
                log.ClientIP = HttpContext.Current.Request.UserHostAddress;
            }

            log.EventTime = DateTime.Now;

            this.Context.AppEventLogs.Add(log);
            this.Context.SaveChanges();
        } 
        #endregion

        #region Trace
        public void Trace(object sender, string message)
        {
            if (this.Level >= 5)
            {
                this.Log(sender, "Trace", message, null);
            }
        }

        public void Trace(object sender, Exception exception)
        {
            if (this.Level >= 5)
            {
                this.Log(sender, "Trace", exception);
            }
        }

        public void Trace(object sender, string message, string detail)
        {
            if (this.Level >= 5)
            {
                this.Log(sender, "Trace", message, detail);
            }
        } 
        #endregion

        #region Debug
        public void Debug(object sender, string message)
        {
            if (this.Level >= 4)
            {
                this.Log(sender, "Debug", message, null);
            }
        }

        public void Debug(object sender, Exception exception)
        {
            if (this.Level >= 4)
            {
                this.Log(sender, "Debug", exception);
            }
        }

        public void Debug(object sender, string message, string detail)
        {
            if (this.Level >= 4)
            {
                this.Log(sender, "Debug", message, detail);
            }
        } 
        #endregion

        #region Info
        public void Info(object sender, string message)
        {
            if (this.Level >= 3)
            {
                this.Log(sender, "Info", message, null);
            }
        }

        public void Info(object sender, Exception exception)
        {
            if (this.Level >= 3)
            {
                this.Log(sender, "Info", exception);
            }
        }

        public void Info(object sender, string message, string detail)
        {
            if (this.Level >= 3)
            {
                this.Log(sender, "Info", message, detail);
            }
        } 
        #endregion

        #region Warning
        /// <summary>
        /// 记录警报级别日志
        /// </summary>
        public void Warning(object sender, string message)
        {
            if (this.Level >= 2)
            {
                this.Log(sender, "Warning", message, null);
            }
        }
        /// <summary>
        /// 记录警报级别日志
        /// </summary>
        public void Warning(object sender, Exception exception)
        {
            if (this.Level >= 2)
            {
                this.Log(sender, "Warning", exception);
            }
        }
        public void Warning(object sender, string message, string detail)
        {
            if (this.Level >= 2)
            {
                this.Log(sender, "Warning", message, detail);
            }
        } 
        #endregion

        #region Error
        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        public void Error(object sender, string message)
        {
            if (this.Level >= 1)
            {
                this.Log(sender, "Error", message, null);
            }
        }

        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        public void Error(object sender, Exception exception)
        {
            if (this.Level >= 1)
            {
                this.Log(sender, "Error", exception);
            }
        }
        public void Error(object sender, string message, string detail)
        {
            if (this.Level >= 1)
            {
                this.Log(sender, "Error", message, detail);
            }
        } 
        #endregion
    } 
    #endregion

    #region LoggingContext
    /// <summary>
    /// 系统日志记录数据库上下文
    /// </summary>
    public class LoggingContext : DbContext
    {
        public LoggingContext()
            : base()
        {

        }
        public LoggingContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public DbSet<AppEventLog> AppEventLogs { get; set; }
    }
    #endregion

    #region AppEventLog
    /// <summary>
    /// 系统日志记录实体对象
    /// </summary>
    public class AppEventLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppEventLogID { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        [StringLength(50)]
        public string Level { get; set; }
        [StringLength(200)]
        public string Sender { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }
        [StringLength(200)]
        public string HostName { get; set; }
        [StringLength(500)]
        public string Url { get; set; }
        [StringLength(200)]
        public string ClientIP { get; set; }

        public DateTime EventTime { get; set; }
    }  
    #endregion
    
}
