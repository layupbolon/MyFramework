using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Logging
{
    /// <summary>
    /// 日志记录器接口
    /// 日志分Error、Warning、Info、Debug、Trace五个级别，分别用不同方法记录。
    /// </summary>
    public interface IAPPLogger
    {
        /// <summary>
        /// 日志分类
        /// </summary>
        string Category { get; set; }

        void Trace(object sender, string message);

        void Trace(object sender, Exception exception);

        void Trace(object sender, string message, string detail);

        void Debug(object sender, string message);

        void Debug(object sender, Exception exception);

        void Debug(object sender, string message, string detail);

        void Info(object sender, string message);

        void Info(object sender, Exception exception);

        void Info(object sender, string message, string detail);

        /// <summary>
        /// 记录警报级别日志
        /// </summary>
        void Warning(object sender, string message);
        /// <summary>
        /// 记录警报级别日志
        /// </summary>
        void Warning(object sender, Exception exception);
        /// <summary>
        /// 记录警报级别日志
        /// </summary>
        void Warning(object sender, string message, string detail);

        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        void Error(object sender, string message);
        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        void Error(object sender, Exception exception);
        /// <summary>
        /// 记录错误级别日志
        /// </summary>
        void Error(object sender, string message, string detail);
    }
}
