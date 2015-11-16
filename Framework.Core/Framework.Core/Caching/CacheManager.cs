using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Framework.Core.Caching
{
    /// <summary>
    /// 缓存管理对象
    /// </summary>
    public class CacheManager
    {
        public CacheManager()
            : this(MemoryCache.Default)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">缓存配置项名称</param>
        public CacheManager(string name)
            : this(new MemoryCache(name))
        {

        }

        public CacheManager(ObjectCache cache)
        {
            this.Cache = cache;
            this.SlidingExpiration = new TimeSpan(0, 5, 0);
            this.Priority = CacheItemPriority.Default;
        }

        #region Default
        protected static CacheManager m_Default = new CacheManager();
        /// <summary>
        /// 默认缓存实例
        /// </summary>
        /// <returns></returns>
        public static CacheManager Default { get { return m_Default; } }
        #endregion

        #region SlidingExpiration
        /// <summary>
        /// 失效间隔,默认为5分钟
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; }
        #endregion

        #region Priority
        /// <summary>
        /// 优先级
        /// </summary>
        public CacheItemPriority Priority { get; set; }
        #endregion

        #region Cache
        /// <summary>
        /// 缓存对象
        /// </summary>
        protected ObjectCache Cache { get; set; }
        #endregion

        #region BuildDefaultPolicy
        protected CacheItemPolicy BuildDefaultPolicy()
        {
            return new CacheItemPolicy() { SlidingExpiration = this.SlidingExpiration, Priority = this.Priority };
        }
        #endregion

        #region Add
        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            var policy = this.BuildDefaultPolicy();
            return this.Add(key, value, policy);
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependency">缓存依赖</param>
        /// <returns></returns>
        public bool Add(string key, object value, ChangeMonitor dependency)
        {
            var policy = this.BuildDefaultPolicy();
            policy.ChangeMonitors.Add(dependency);
            return Cache.Add(key, value, policy);
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependency">缓存依赖文件</param>
        /// <returns></returns>
        public bool Add(string key, object value, string dependencyFile)
        {
            var policy = this.BuildDefaultPolicy();
            var files = new List<string>();
            files.Add(dependencyFile);
            var dependency = new HostFileChangeMonitor(files);
            policy.ChangeMonitors.Add(dependency);
            return Cache.Add(key, value, policy);
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(string key, object value, CacheItemPolicy policy)
        {
            return Cache.Add(key, value, policy);
        }

        #endregion

        #region AddOrGetExisting
        public object AddOrGetExisting(string key, object value)
        {
            var policy = this.BuildDefaultPolicy();
            return this.AddOrGetExisting(key, value, policy);
        }


        public object AddOrGetExisting(string key, object value, ChangeMonitor dependency)
        {
            var policy = this.BuildDefaultPolicy();
            policy.ChangeMonitors.Add(dependency);
            return this.Cache.AddOrGetExisting(key, value, policy);
        }

        public object AddOrGetExisting(string key, object value, string dependencyFile)
        {
            var policy = this.BuildDefaultPolicy();
            var files = new List<string>();
            files.Add(dependencyFile);
            var dependency = new HostFileChangeMonitor(files);
            return this.Cache.AddOrGetExisting(key, value, policy);
        }


        public object AddOrGetExisting(string key, object value, CacheItemPolicy policy)
        {
            return this.Cache.AddOrGetExisting(key, value, policy);
        }

        public T AddOrGetExisting<T>(string key, T value)
        {
            var policy = this.BuildDefaultPolicy();
            var result = this.Cache.AddOrGetExisting(key, value, policy);
            return result is T ? (T)result : default(T);
        }

        public T AddOrGetExisting<T>(string key, T value, ChangeMonitor dependency)
        {
            var policy = this.BuildDefaultPolicy();
            policy.ChangeMonitors.Add(dependency);
            var result = this.Cache.AddOrGetExisting(key, value, policy);
            return result is T ? (T)result : default(T);
        }

        public T AddOrGetExisting<T>(string key, T value, string dependencyFile)
        {
            var policy = this.BuildDefaultPolicy();
            var files = new List<string>();
            files.Add(dependencyFile);
            var dependency = new HostFileChangeMonitor(files);
            var result = this.Cache.AddOrGetExisting(key, value, policy);
            return result is T ? (T)result : default(T);
        }

        public T AddOrGetExisting<T>(string key, T value, CacheItemPolicy policy)
        {
            var result = this.Cache.AddOrGetExisting(key, value, policy);
            return result is T ? (T)result : default(T);
        }
        #endregion

        #region Get
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var result = Cache.Get(key);
            return result is T ? (T)result : default(T);
        }
        #endregion

        #region Set
        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            var policy = this.BuildDefaultPolicy();
            Cache.Set(key, value, policy);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value, CacheItemPolicy policy)
        {
            Cache.Set(key, value, policy);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="Value"></param>
        public void Set<T>(string key, T value)
        {
            var policy = this.BuildDefaultPolicy();
            Cache.Set(key, value, policy);
        }

        /// <summary>
        /// 设置缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value, CacheItemPolicy policy)
        {
            Cache.Set(key, value, policy);
        }
        #endregion

        #region Remove
        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Remove(string key)
        {
            return Cache.Remove(key);
        }

        public T Remove<T>(string key)
        {
            var result = Cache.Remove(key);
            return result is T ? (T)result : default(T);
        }
        #endregion

        #region Clear
        public void Clear(string key)
        {
            var keys = new List<string>();
            foreach (var item in this.Cache)
            {
                if (item.Key.StartsWith(key))
                {
                    keys.Add(item.Key);
                }
            }

            foreach (var k in keys)
            {
                this.Remove(k);
            }
        }
        #endregion

        #region Contains
        /// <summary>
        /// 判断指定的缓存项是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this.Cache.Contains(key);
        } 
        #endregion
    }
}
