using System.IO;
using System.Net;
using System.Text;

namespace Framework.Core.Utility
{
    /// <summary>
    ///http协议模拟请求类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 请求并发限制数目
        /// </summary>
        private static int DefaultConnectionLimit = 1;
        private const string Accept = "application/json";
        private const string UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022 ; .NET CLR 4.0.30319)";
        private const string ContentType = "application/json;charset=utf-8";

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="postString">传输的文本</param>
        /// <param name="IsPost">是否为POST</param>
        /// <param name="cookieContainer">cookie相关</param>
        /// <param name="referer">Referer HTTP标头的值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string GetResult(string url, string postString, bool IsPost, CookieContainer cookieContainer, string referer, Encoding encoding)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = DefaultConnectionLimit;//设置并发连接数限制上额
            DefaultConnectionLimit++;
            if (string.IsNullOrEmpty(postString)) IsPost = false;

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = IsPost ? "POST" : "GET";
                if (cookieContainer != null) httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.ContentType = ContentType;
                httpWebRequest.Accept = Accept;
                httpWebRequest.UserAgent = UserAgent;
                if (!string.IsNullOrEmpty(referer)) httpWebRequest.Referer = referer;
                if (IsPost)
                {
                    byte[] byteRequest = encoding.GetBytes(postString);
                    httpWebRequest.ContentLength = byteRequest.Length;
                    Stream stream = httpWebRequest.GetRequestStream();
                    stream.Write(byteRequest, 0, byteRequest.Length);
                    stream.Close();
                }

                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                var result = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebRequest.Abort();

                foreach (Cookie cookie in httpWebResponse.Cookies) //获取cookie
                {
                    cookieContainer.Add(cookie);
                }

                httpWebResponse.Close();
                return result;
            }
            catch
            {
                DefaultConnectionLimit--;
                return string.Empty;
            }

        }

        #region 重载方法

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为POST，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="postString">传输的文本</param>
        /// <param name="cookieContainer">cookie相关</param>
        /// <param name="referer">Referer HTTP标头的值</param>
        /// <returns></returns>
        public static string GetResult(string url, string postString, CookieContainer cookieContainer, string referer)
        {
            return GetResult(url, postString, true, cookieContainer, referer, Encoding.UTF8);
        }

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为POST，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="postString">传输的文本</param>
        /// <param name="cookieContainer">cookie相关</param>
        /// <returns></returns>
        public static string GetResult(string url, string postString, CookieContainer cookieContainer)
        {
            return GetResult(url, postString, true, cookieContainer, url, Encoding.UTF8);
        }

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为POST，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="postString">传输的文本</param>
        /// <returns></returns>
        public static string GetResult(string url, string postString)
        {
            return GetResult(url, postString, true, null, "", Encoding.UTF8);
        }

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为GET，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="cookieContainer">cookie相关</param>
        /// <param name="referer">Referer HTTP标头的值</param>
        /// <returns></returns>
        public static string GetResult(string url, CookieContainer cookieContainer, string referer)
        {
            return GetResult(url, "", false, cookieContainer, referer, Encoding.UTF8);
        }

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为GET，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <param name="cookieContainer">cookie相关</param>
        /// <returns></returns>
        public static string GetResult(string url, CookieContainer cookieContainer)
        {
            return GetResult(url, "", false, cookieContainer, url, Encoding.UTF8);
        }

        /// <summary>
        /// 发送资源请求。返回请求到的响应文本
        /// 默认为GET，UTF8字符编码
        /// </summary>
        /// <param name="url">请求url地址</param>
        /// <returns></returns>
        public static string GetResult(string url)
        {
            return GetResult(url, "", false, null, url, Encoding.UTF8);
        }

        #endregion

        /// <summary>
        /// 获取非文本的响应正文
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <returns></returns>
        public static Stream GetStream(string url, CookieContainer cookieContainer)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {

                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                return responseStream;
            }
            catch
            {
                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                }
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return null;
            }
        }
    }
}
