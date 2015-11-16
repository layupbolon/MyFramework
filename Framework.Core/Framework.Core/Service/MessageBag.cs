using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    /// <summary>
    /// 服务消息封装对象
    /// </summary>
    /// <typeparam name="T">实体信息</typeparam>
    public class MessageBag<T>
        where T:new()
    {
        /// <summary>
        /// 服务消息封装对象
        /// </summary>
        public MessageBag()
        {
        }

        /// <summary>
        /// 服务消息封装对象
        /// </summary>
        /// <param name="data">实体信息</param>
        public MessageBag(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 信息状态代码，默认为0-成功
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 消息状态描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 消息体数据内容
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return this.Code == 0;
        }
    }
}
