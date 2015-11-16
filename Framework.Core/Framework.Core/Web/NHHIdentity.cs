using Framework.Core.Security.Cryptography;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Framework.Core.Web
{
    /// <summary>
    /// 登录用户身份标识信息
    /// </summary>
    public class NHHIdentity : IIdentity
    {

        /// <summary>
        /// 登录用户身份标识信息
        /// </summary>
        /// <param name="authType">验证方式</param>
        /// <param name="data">附加数据</param>
        public NHHIdentity(string authType, SortedList<string, string> data)
        {
            this.m_AuthenticationType = authType;
            this.m_UserData = data ?? new SortedList<string, string>();
        }

        /// <summary>
        /// 登录用户身份标识信息
        /// </summary>
        /// <param name="authType">验证方式</param>
        /// <param name="data">附加数据</param>
        public NHHIdentity(string authType,string data)
        {
            this.m_AuthenticationType = authType;
            this.m_UserData = ParseString(data);
        }

        #region UserData
        public SortedList<string, string> m_UserData;
        /// <summary>
        /// 获取用户附带身份信息
        /// </summary>
        public SortedList<string, string> UserData
        {
            get
            {
                return this.m_UserData;
            }
        } 
        #endregion

        #region Name
        /// <summary>
        /// 获取当前用户的名称
        /// </summary>
        public string Name
        {
            get { return m_UserData.ContainsKey("UserName") ? m_UserData["UserName"] : (m_UserData.ContainsKey("UserID") ? m_UserData["UserID"] : string.Empty); }
        } 
        #endregion

        #region AuthenticationType
        private string m_AuthenticationType="Froms" ;
        /// <summary>
        /// 获取身份验证方式
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return this.m_AuthenticationType;
            }
        } 
        #endregion

        #region IsAuthenticated
        /// <summary>
        /// 是否验证用户
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.Name);
            }
        } 
        #endregion

        #region UserID
        private int m_UserID;
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int UserID
        {
            get
            {
                if (m_UserID == 0)
                {
                    m_UserID = Converter.ToInt(m_UserData.ContainsKey("UserID") ? m_UserData["UserID"] : string.Empty);
                }
                return m_UserID;
            }
        }
        #endregion

        #region UserName
        private string m_UserName;
        /// <summary>
        /// 当前用户名
        /// </summary>
        public string UserName
        {
            get
            {
                if (m_UserName == null)
                {
                    m_UserName = m_UserData.ContainsKey("UserName") ? m_UserData["UserName"] : string.Empty;
                }
                return m_UserName;
            }
        }
        #endregion

        #region RoleIDs
        private List<int> m_RoleIDs;
        /// <summary>
        /// 当前用户所具有的角色ID列表
        /// </summary>
        public List<int> RoleIDs
        {
            get
            {
                if (m_RoleIDs == null)
                {
                    m_RoleIDs = Converter.ToIntList(m_UserData.ContainsKey("RoleID") ? m_UserData["RoleID"] : string.Empty);
                }
                return m_RoleIDs;
            }
        }
        #endregion

        #region RoleNames
        private List<string> m_RoleNames;
        /// <summary>
        /// 当前用户所具有的角色名称列表
        /// </summary>
        public List<string> RoleNames
        {
            get
            {
                if (m_RoleNames == null)
                {
                    m_RoleNames = Converter.ToStringList(m_UserData.ContainsKey("RoleName") ? m_UserData["RoleName"] : string.Empty);
                }
                return m_RoleNames;
            }
        }
        #endregion

        #region BuildString & ParseString

        public static string BuildString(SortedList<string, string> data)
        {
            var str = string.Empty;
            foreach (var key in data.Keys)
            {
                str += string.Format("{0}:{1}|", key, data[key]);
            }
            str = str.TrimEnd('|');
            return str;
        }

        public static SortedList<string, string> ParseString(string data)
        {
            var ud = new SortedList<string, string>();

            if (!string.IsNullOrEmpty(data))
            {
                var items = data.Split('|', ':');
                for (int i = 0; i < items.Length; i += 2)
                {
                    ud.Add(items[i], i == items.Length ? null : items[i + 1]);
                }
            }
            return ud;
        }
        #endregion


    }
}
