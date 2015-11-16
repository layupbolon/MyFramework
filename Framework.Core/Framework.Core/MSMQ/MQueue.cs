using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.MSMQ
{
    // <summary>
    /// 消息队列对象，由MQueueFactory创建指定路径的队列对象，可发送或批量接收消息。
    /// </summary>
    /// <typeparam name="T">消息队列存储的消息对象类型</typeparam>
    public class MQueue<T>
    {

        public MQueue(string path)
        {
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
            }

            this.InnerQueue = new MessageQueue(path);
            this.InnerQueue.Formatter = new BinaryMessageFormatter();
            this.InnerQueue.SetPermissions(ConfigurationManager.AppSettings["MSMQUser"] ?? "Everyone", MessageQueueAccessRights.FullControl);
        }

        public MQueue(MessageQueue mq)
        {
            this.InnerQueue = mq;
            this.InnerQueue.Formatter = new BinaryMessageFormatter();
            this.InnerQueue.SetPermissions(ConfigurationManager.AppSettings["MSMQUser"] ?? "Everyone", MessageQueueAccessRights.FullControl);
        }

        /// <summary>
        /// 内部消息队列对象
        /// </summary>
        protected MessageQueue InnerQueue
        {
            get;
            private set;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void SendMessage(T message)
        {
            this.InnerQueue.Send(message);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="label">消息标签</param>
        public void SendMessage(T message, string label)
        {
            this.InnerQueue.Send(message, label);
        }

        /// <summary>
        /// 批量接收消息，获取当前队列中存在的所有消息对象并移除
        /// </summary>
        /// <returns>消息对象列表</returns>
        public List<T> GetAllMessages(bool remove=true)
        {
            var list = new List<T>();
            var enumerator = this.InnerQueue.GetMessageEnumerator2();
            Message msg=null;
            while (enumerator.MoveNext())
            {
                if (remove)
                {
                    msg = enumerator.RemoveCurrent();
                    enumerator.Reset();
                }
                else
                {
                    msg = enumerator.Current;
                }
                list.Add((T)msg.Body);
            }
            return list;

        }

    }
}
