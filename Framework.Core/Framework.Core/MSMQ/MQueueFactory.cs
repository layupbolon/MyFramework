using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.MSMQ
{
    /// <summary>
    /// 消息队列工厂，通过指定路径创建或获取相应队列对象
    /// </summary>
    public class MQueueFactory
    {

        /// <summary>
        /// 默认队列路径，在未指定路径的情况下，将创建并返回该路径的消息队列对象
        /// </summary>
        private const string DefaultPath = @".\private$\NHH";

        /// <summary>
        /// 创建默认路径的消息队列对象
        /// </summary>
        /// <typeparam name="T">消息队列存储的消息对象类型</typeparam>
        /// <returns></returns>
        public static MQueue<T> Create<T>()
        {
            return Create<T>(DefaultPath);
        }

        /// <summary>
        /// 创建指定路径的消息队列路径
        /// </summary>
        /// <typeparam name="T">消息队列存储的消息对象类型</typeparam>
        /// <param name="path">指定的消息队列名称，名称对应的队列路径需要在配置文件的AppSettings中添加配置</param>
        /// <returns></returns>
        public static MQueue<T> Create<T>(string path)
        {
            return new MQueue<T>(path);
        }
    }
}
