using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Core.Web
{
    /// <summary>
    /// 用户权限验证
    /// </summary>
    public class NHHAuthorizeAttribute : AuthorizeAttribute
    {
        #region RoleIDs
        private string _roleIDs;
        private List<int> _roleIDsSplit;
        /// <summary>
        /// 指定权限角色ID
        /// </summary>
        public string RoleIDs
        {
            get { return _roleIDs ?? String.Empty; }
            set
            {
                _roleIDs = value;
                _roleIDsSplit = Converter.ToIntList(value);
            }
        } 
        #endregion

        #region UserIDs
        private string _userIDs;
        private List<int> _userIDsSplit;
        /// <summary>
        /// 指定权限用户ID
        /// </summary>
        public string UserIDs
        {
            get { return _userIDs ?? String.Empty; }
            set
            {
                _userIDs = value;
                _userIDsSplit = Converter.ToIntList(value);
            }
        } 
        #endregion

        #region FunctionPoint
        /// <summary>
        /// 权限功能点Key
        /// </summary>
        public string FunctionPoint { get; set; } 
        #endregion

        #region AuthorizeCore
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {

            var user = httpContext.User;

            //未登录
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }

            //非指定名称用户
            if (!string.IsNullOrEmpty(this.Users))
            {
                var users = Converter.ToStringList(this.Users);
                if (users.Count > 0 && !users.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
                {
                    httpContext.Response.StatusCode = 401;
                    return false;
                }
            }

            //非指定名称角色
            if (!string.IsNullOrEmpty(this.Roles))
            {
                var roles = Converter.ToStringList(this.Roles);

                if (roles.Count > 0 && !roles.Any(user.IsInRole))
                {
                    httpContext.Response.StatusCode = 401;
                    return false;
                }

            }

            //非NHH用户
            var nhhUser = user as NHHPrincipal;
            if (nhhUser == null
                || nhhUser.Identity == null)
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }

            //非指定ID用户
            if (_userIDsSplit != null && _userIDsSplit.Count > 0 && !_userIDsSplit.Contains(nhhUser.Identity.UserID))
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }

            //非指定ID角色
            if (_roleIDsSplit!=null && _roleIDsSplit.Count > 0 && !_roleIDsSplit.Any(nhhUser.IsInRole))
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }

            return true;
        } 
        #endregion

        #region OnAuthorization
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //拼凑权限功能点Key
            this.FunctionPoint = filterContext.GetFullActionName();

            base.OnAuthorization(filterContext);
        }
        #endregion

    }
}
