using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Core.Web
{
    #region ControllerContextExtension
    public static class ControllerContextExtension
    {
        /// <summary>
        /// 获取当前上下文的Area名称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetAreaName(this ControllerContext context)
        {
            return (string)context.RouteData.DataTokens["area"];
        }

        /// <summary>
        /// 获取当前上下文的Controler名称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetControllerName(this ControllerContext context)
        {
            return (string)context.RouteData.Values["controller"];
        }

        /// <summary>
        /// 获取当前上下文的Action名称
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetActionName(this ControllerContext context)
        {
            return (string)context.RouteData.Values["action"];
        }

        /// <summary>
        /// 获取当前上下文的完整Action名称，格式如下：{Area}/{Controler}/{Action}。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetFullActionName(this ControllerContext context)
        {
            var areaName = context.RouteData.DataTokens["area"];
            var controllerName = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            return string.Format("{0}/{1}/{2}", areaName, controllerName, actionName);
        }
    } 
    #endregion
}
