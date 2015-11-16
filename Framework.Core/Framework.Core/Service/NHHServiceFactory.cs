using Framework.Core.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    /// <summary>
    /// 服务工厂类
    /// </summary>
    public class NHHServiceFactory
    {

        protected Hashtable ServiceBindings { get; private set; }

        /// <summary>
        /// 服务工厂
        /// </summary>
        public NHHServiceFactory()
        {
            
            var config = ConfigManager.GetConfig<ServiceConfig>();
            ServiceBindings = Hashtable.Synchronized(new Hashtable(config.Services.Count));
            foreach (var service in config.Services)
            {
                ServiceBindings.Add(service.ServiceType, service.ClassType);
            }
            
        }

        #region Instance
        private static NHHServiceFactory m_Instance = new NHHServiceFactory();
        /// <summary>
        /// 服务工厂实例
        /// </summary>
        public static NHHServiceFactory Instance
        {
            get
            {
                return m_Instance;
            }
        } 
        #endregion

        #region RegisterBinding
        /// <summary>
        /// 注册服务类型绑定
        /// </summary>
        /// <param name="service">类型</param>
        /// <param name="clzss">实现</param>
        public void RegisterBinding(Type service, Type clzss)
        {
            ServiceBindings[service] = clzss;
        }

        /// <summary>
        /// 注册服务类型绑定
        /// </summary>
        /// <typeparam name="S">类型</typeparam>
        /// <typeparam name="Z">实现</typeparam>
        public void RegisterBinding<S, Z>()
        {
            ServiceBindings[typeof(S)] = typeof(Z);
        } 
        #endregion

        #region CreateService
        /// <summary>
        /// 创建指定服务对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns></returns>
        public T CreateService<T>()
        {
            var type = typeof(T);
            if (!ServiceBindings.ContainsKey(type))
            {
                return default(T);
            }

            type = ServiceBindings[type] as Type;

            if (type == null)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(type);
        }
        /// <summary>
        /// 创建指定服务对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="args">构造参数</param>
        /// <returns></returns>
        public T CreateService<T>(params object[] args)
        {
            var type = typeof(T);
            if (!ServiceBindings.ContainsKey(type))
            {
                return default(T);
            }

            type = ServiceBindings[type] as Type;

            if (type == null)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(type, args);
        } 
        #endregion
    }
}
