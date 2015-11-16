using Framework.Core.Logging;
using Framework.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Core.Web
{
    /// <summary>
    /// NHH站点控制器基类
    /// </summary>
    public class NHHController : Controller
    {
        #region Context
        /// <summary>
        /// 当前Web上下文信息
        /// </summary>
        public NHHWebContext Context
        {
            get
            {
                return NHHWebContext.Current;
            }
        } 
        #endregion

        #region GetService
        /// <summary>
        /// 获取指定服务对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        protected virtual T GetService<T>()
        {
            return NHHServiceFactory.Instance.CreateService<T>();
        }
        /// <summary>
        /// 获取指定服务对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual T GetService<T>(params object[] args)
        {
            return NHHServiceFactory.Instance.CreateService<T>(args);
        } 
        #endregion

        #region Json
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new NJsonResult() { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
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
    }
}
