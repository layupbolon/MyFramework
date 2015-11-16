using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Framework.Core.Web
{
    /// <summary>
    /// 登录用户主体信息
    /// </summary>
    public class NHHPrincipal : IPrincipal 
    {
        private readonly NHHIdentity m_Identity;
        /// <summary>
        /// 登录用户功能权限
        /// </summary>
        private readonly List<string> m_Permissions;
    

        /// <summary>
        /// 登录用户主体信息
        /// </summary>
        /// <param name="identity">身份标识信息</param>
        public NHHPrincipal(NHHIdentity identity)
        {
            this.m_Identity = identity;

        }

        /// <summary>
        /// 登录用户主体信息
        /// </summary>
        /// <param name="identity">身份标识信息</param>
        /// <param name="permissions">功能权限</param>
        public NHHPrincipal(NHHIdentity identity, List<string> permissions)
        {
            this.m_Identity = identity;
            this.m_Permissions = permissions ?? new List<string>();
        }

        #region Identity

        /// <summary>
        /// 登录用户身份标识信息
        /// </summary>
        public NHHIdentity Identity
        {
            get
            {
                return this.m_Identity;
            }
        }

        IIdentity IPrincipal.Identity
        {
            get
            {
                return this.m_Identity;
            }
        }
        #endregion

        #region IsInRole
        /// <summary>
        /// 确定当前登录用户主体是否属于指定ID的角色。
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public bool IsInRole(int id)
        {

            if (this.Identity == null)
                return false;

            return this.Identity.RoleIDs.Contains(id);
        }

        /// <summary>
        /// 确定当前登录用户主体是否属于指定Name的角色。
        /// </summary>
        /// <param name="role">角色Name</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            if (this.Identity == null)
                return false;

            return this.Identity.RoleNames.Contains(role);
        }
        #endregion

        #region IsInPermission
        /// <summary>
        /// 确定当前登录用户主体是否具有指定功能权限。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsInPermission(string action)
        {
            return m_Permissions.Contains(action, StringComparer.OrdinalIgnoreCase);
        } 
        #endregion




    }
}
