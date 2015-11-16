using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;


namespace Framework.Core.Web
{
    /// <summary>
    /// NHH站点访问线程上下文信息
    /// </summary>
    public class NHHWebContext
    {

        /// <summary>
        /// 站点访问线程上下文信息
        /// </summary>
        public NHHWebContext(HttpContext context)
        {
            this.Context = new HttpContextWrapper(context);
        }

        #region Context
        /// <summary>
        /// 当前HTTP上下文信息
        /// </summary>
        public HttpContextWrapper Context { get; set; } 
        #endregion

        #region Current
        [ThreadStatic]
        protected static NHHWebContext currentContext = null;

        /// <summary>
        /// 当前Web上下文信息
        /// </summary>
        public static NHHWebContext Current
        {
            get
            {
                if (currentContext == null)
                {
                    currentContext = new NHHWebContext(HttpContext.Current);
                }
                return currentContext;
            }
            internal set
            {
                currentContext = value;
            }
        }
        #endregion

        #region User
        private NHHPrincipal m_User;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public NHHPrincipal User
        {
            get
            {
                if (m_User == null)
                {
                    m_User =(this.Context != null ? this.Context.User : Thread.CurrentPrincipal) as NHHPrincipal;
                }
                return m_User;
            }
            set
            {
                if (this.Context != null)
                    this.Context.User = value;
                Thread.CurrentPrincipal = value;
                this.m_User = value;
            }
        }
        #endregion

        #region UserID
        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public int UserID
        {
            get
            {
                if (this.User == null
                    || this.User.Identity == null)
                    return 0;

                return this.User.Identity.UserID;
            }
        } 
        #endregion

        #region UserName
        /// <summary>
        /// 当前登录用户名称
        /// </summary>
        public string UserName
        {
            get
            {
                if (this.User == null
                   || this.User.Identity == null)
                    return string.Empty;

                return this.User.Identity.UserName;
            }
        } 
        #endregion

        #region RoleIDs
        /// <summary>
        /// 当前登录用户具有的角色ID
        /// </summary>
        public List<int> RoleIDs
        {
            get
            {
                if (this.User == null
                   || this.User.Identity == null)
                    return new List<int>();

                return this.User.Identity.RoleIDs;
            }
        }
        #endregion

        #region RoleNames
        /// <summary>
        /// 当前登录用户具有的角色名称
        /// </summary>
        public List<string> RoleNames
        {
            get
            {
                if (this.User == null
                   || this.User.Identity == null)
                    return new List<string>();

                return this.User.Identity.RoleNames;
            }
        }
        #endregion

        #region GetUserData

        public string GetUserData(string key)
        {
            if (this.User == null
                   || this.User.Identity == null)
                return null;

            return this.User.Identity.UserData.ContainsKey(key) ? this.User.Identity.UserData[key] : string.Empty;
        }
        #endregion

        #region SignIn & SignOut
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userData"></param>
        public void SignIn(SortedList<string, string> userData)
        {
            string token = null;
            SignIn(userData, out token);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="token"></param>
        public void SignIn(SortedList<string, string> userData, out string token)
        {
            token = null;
            var data = NHHIdentity.BuildString(userData);
            var type = ParamManager.GetStringValue("auth:type").ToUpper();
            switch (type)
            {
                case "NHH":
                    {
                        HttpContext.Current.Response.Headers[NHHAuthentication.NHHAuthHeaderName] = token = NHHAuthentication.Encrypt(data);
                        this.User = new NHHPrincipal(new NHHIdentity( "NHH",userData));
                        break;
                    }
                case "FROMS":
                    {
                        if (FormsAuthentication.IsEnabled)
                        {
                            var ticket = new FormsAuthenticationTicket(1, userData["UserName"] ?? userData["UserID"], DateTime.Now, DateTime.Now.AddMinutes(120), false, data);
                            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
                            this.User = new NHHPrincipal(new NHHIdentity("Froms",userData));
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

        /// <summary>
        /// 用户登出
        /// </summary>
        public void SignOut()
        {

            this.User = null;

            if (FormsAuthentication.IsEnabled)
            {
                FormsAuthentication.SignOut();
            }
        } 
        #endregion

    }
}
