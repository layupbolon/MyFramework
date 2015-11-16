using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Logging
{
    public interface IAPILogger
    {
        /// <summary>
        /// 写入API调用日志
        /// </summary>
        /// <param name="apiRoute">API路由</param>
        /// <param name="sourceUrl">来源URL</param>
        /// <param name="clientIp">客户端IP</param>
        /// <param name="clientAgent">客户端代理信息</param>
        /// <param name="postData">接口输入参数</param>
        /// <param name="returnData">接口返回值</param>
        void OperatorLog(string apiRoute, string sourceUrl, string clientIp, string clientAgent, string postData,
            string returnData);

        /// <summary>
        /// 写入API异常日志
        /// </summary>
        /// <param name="apiRoute">API路由</param>
        /// <param name="postData">接口输入参数</param>
        /// <param name="message">异常消息</param>
        /// <param name="stackTrace">异常堆栈信息</param>
        void ExceptionLog(string apiRoute, string postData, string message, string stackTrace);
    }
}
