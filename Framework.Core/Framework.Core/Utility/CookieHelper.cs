using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Core.Utility
{
    /// <summary>
    /// Cookie管理辅助类
    /// </summary>
    public class CookieManager
    {
        #region [ Fields ]
        private static readonly DateTime ExpirationDate = new DateTime(1990, 1, 1);
        #endregion

        #region [ Properties ]

        /// <summary>
        /// 当前请求的Cookie信息
        /// </summary>
        private static HttpCookieCollection RequestCookies
        {
            get
            {
                return HttpContext.Current.Request.Cookies;
            }
        }

        /// <summary>
        /// 当前反馈的Cookie信息
        /// </summary>
        private static HttpCookieCollection ResponseCookies
        {
            get
            {
                return HttpContext.Current.Response.Cookies;
            }
        }

        #endregion

        #region [ Methods ]

        #region [ Get ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public string GetCookie(string cookieName)
        {
            HttpCookie cookie = GetHttpCookie(cookieName);
            return GetCookie(cookie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key)
        {
            HttpCookie cookie = GetHttpCookie(cookieName);
            return GetCookie(cookie, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="fromResponse"></param>
        /// <returns></returns>
        public NameValueCollection GetCookies(string cookieName)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();

            HttpCookie cookie = GetHttpCookie(cookieName);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return nameValueCollection;
            }

            string[] keyPair = cookie.Value.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string keypairItem in keyPair)
            {
                string[] detailCookieItem = keypairItem.Split(new char[] { '=' });
                if (detailCookieItem != null && detailCookieItem.Length == 2)
                {
                    nameValueCollection.Add(Decode(detailCookieItem[0]), Decode(detailCookieItem[1]));
                }
            }
            return nameValueCollection;
        }

        /// <summary>
        /// 获取Cookie内容
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private static string GetCookie(HttpCookie cookie)
        {
            if (cookie == null)
            {
                return null;
            }

            return Decode(cookie.Value);
        }

        /// <summary>
        /// 通过指定Key获取Cookie内容
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string GetCookie(HttpCookie cookie, string key)
        {
            if (cookie == null)
            {
                return null;
            }

            key = Encode(key);
            return Decode(cookie[key]);
        }

        /// <summary>
        /// 通过指定名称获取Cookie内容
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetHttpCookie(string cookieName)
        {
            bool fromResponse = HasCookieName(ResponseCookies, cookieName);
            return fromResponse ? ResponseCookies[cookieName] : RequestCookies[cookieName];
        }

        /// <summary>
        /// 判断指定名称的Cookie是否存在
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        private static bool HasCookieName(HttpCookieCollection cookies, string cookieName)
        {
            bool hasCookie = false;
            if (cookies != null && cookieName != null)
            {
                foreach (string name in cookies)
                {
                    if (name.Equals(cookieName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        hasCookie = true;
                        break;
                    }
                }
            }

            return hasCookie;
        }

        #endregion

        #region [ Save ]

        /// <summary>
        /// 设置一维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        public static void SetCookie(string cookieName, string value)
        {
            SetCookie(cookieName, value, TimeSpan.FromTicks(0L));
        }

        /// <summary>
        /// 设置一维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expireDate"></param>
        public static void SetCookie(string cookieName, string value, TimeSpan expireDate)
        {
            SetCookie(cookieName, value, "", "/", false, false, expireDate);
        }

        /// <summary>
        /// 设置一维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <param name="httpOnly"></param>
        /// <param name="secure"></param>
        /// <param name="expireDate"></param>
        public static void SetCookie(
            string cookieName, string value, string domain, string path, bool httpOnly, bool secure, TimeSpan expireDate)
        {
            HttpCookie cookie = GetHttpCookie(cookieName);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }

            value = Encode(value);
            cookie.Value = value;
            cookie.Domain = domain;
            cookie.Path = path;
            cookie.HttpOnly = httpOnly;
            cookie.Secure = secure;
            if (expireDate.Ticks != 0L)
            {
                cookie.Expires = DateTime.Now.Add(expireDate);
            }

            ResponseCookies.Set(cookie);
        }

        /// <summary>
        /// 设置二维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, TimeSpan.FromTicks(0L));
        }

        /// <summary>
        /// 设置二维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireDate"></param>
        public static void SetCookie(string cookieName, string key, string value, TimeSpan expireDate)
        {
            SetCookie(cookieName, key, value, "", "/", false, false, expireDate);
        }

        /// <summary>
        /// 设置二维Cookie。
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="path"></param>
        /// <param name="httpOnly"></param>
        /// <param name="secure"></param>
        /// <param name="expireDate"></param>
        public static void SetCookie(
            string cookieName, string key, string value, string domain, string path, bool httpOnly, bool secure, TimeSpan expireDate)
        {
            HttpCookie cookie = GetHttpCookie(cookieName);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }

            key = Encode(key);
            value = Encode(value);

            cookie[key] = value;
            cookie.Domain = domain;
            cookie.Path = path;
            cookie.HttpOnly = httpOnly;
            cookie.Secure = secure;
            if (expireDate.Ticks != 0L)
            {
                cookie.Expires = DateTime.Now.Add(expireDate);
            }

            ResponseCookies.Set(cookie);
        }

        #endregion

        #region Clear Cookie

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="domainName"></param>
        public static void ClearCookie(string cookieName, string domainName)
        {
            HttpCookie cookie = ResponseCookies[cookieName];
            if (cookie == null)
            {
                return;
            }

            cookie.Value = "";
            cookie.Expires = ExpirationDate;
            if (!string.IsNullOrEmpty(domainName))
            {
                cookie.Domain = domainName;
            }
        }

        /// <summary>
        /// 从多值Cookie移除个key
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="domainName"></param>
        public static void ClearCookie(string cookieName, string key, string domainName)
        {
            key = Encode(key);

            HttpCookie cookie = ResponseCookies[cookieName];
            if (cookie == null)
            {
                return;
            }

            cookie.Values.Remove(key);
            if (!string.IsNullOrEmpty(domainName))
            {
                cookie.Domain = domainName;
            }
        }

        #endregion


        #region [ Helper ]

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return HttpUtility.UrlEncode(input).Replace("_", "%5F").Replace("&", "%26");
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decode(string input)
        {
            return HttpUtility.UrlDecode(input);
        }

        #endregion

        #endregion
    }
}
