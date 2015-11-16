using Framework.Core.Caching;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Framework.Core.Web
{
    /// <summary>
    /// NHH站点处理模块，用于初始化设置站点请求信息
    /// </summary>
    public class NHHWebModule : IHttpModule
    {

        #region LoadUserPermissions
       
        /// <summary>
        /// 加载用户相关配置信息
        /// </summary>
        protected virtual void LoadUserConfig(int user)
        {
            //TODO：载入用户相关配置信息
        }
        
        /// <summary>
        /// 获取指定ID用户功能权限列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual List<string> GetUserPermissions(int user)
        {
            //TODO：获取指定ID用户功能权限列表
            return new List<string>();
        }
        #endregion

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        protected void context_BeginRequest(object sender, EventArgs e)
        {
            //此处放置初始化请求代码
            var app = (HttpApplication)sender;
            NHHWebContext.Current = new NHHWebContext(app.Context);
        }

        protected void context_AuthenticateRequest(object sender, EventArgs e)
        {
            var type = ParamManager.GetStringValue("auth:type").ToUpper();
            switch (type)
            {
                case "NHH":
                    {
                        var head = HttpContext.Current.Request.Headers[NHHAuthentication.NHHAuthHeaderName];
                        if (head != null)
                        {
                            var ticket = NHHAuthentication.Decrypt(head);
                            var identity = new NHHIdentity("NHH",ticket);
                            this.LoadUserConfig(identity.UserID);
                            var principal = new NHHPrincipal(identity, GetUserPermissions(identity.UserID));
                            NHHWebContext.Current.User = principal;
                        }
                        break;
                    }
                case "FROMS":
                    {
                        if (FormsAuthentication.IsEnabled)
                        {
                            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                            if (cookie != null)
                            {
                                //登录用户主体信息
                                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                                var identity = new NHHIdentity("Forms",ticket.UserData);
                                this.LoadUserConfig(identity.UserID);
                                var principal = new NHHPrincipal(identity, GetUserPermissions(identity.UserID));
                                NHHWebContext.Current.User = principal;
                            }
                        }
                        break;
                    }
                case "NONE":
                default:
                    {
                        break;
                    }
            }
        }

        public void Dispose()
        {
            //此处放置清除代码。
        } 
        #endregion


    }
}
