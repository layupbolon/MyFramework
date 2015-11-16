using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml.Serialization;
using Framework.Core.Caching;
using Framework.Core.IO;
using Framework.Core.Utility;
using System.Collections;

namespace Framework.Core.Configuration
{
    /// <summary>
    /// 配置管理器，用于获取自定义配置信息
    /// </summary>
    public class ConfigManager
    {
        public static string GetConfigPath(Type configType)
        {
            if (configType == null)
            {
                throw new ArgumentNullException("configType", "configType can not be null!");
            }

            var path = string.Empty;
            var attr = Attribute.GetCustomAttribute(configType, typeof(ConfigFileAttribute)) as ConfigFileAttribute;
            if (attr == null)
            {
                path = configType.Name + ".config";
            }
            else
            {
                path = attr.FileName;
            }

            return PathUtils.GetFullPath(path);
        }

        protected static Hashtable configSerializers = Hashtable.Synchronized(new Hashtable());
        protected static XmlSerializer GetSerializer(Type type)
        {
            XmlSerializer serializer = null;
            if(configSerializers.ContainsKey(type))
                serializer = configSerializers[type] as XmlSerializer;
            if(serializer==null)
                serializer = new XmlSerializer(type);
            configSerializers[type]=serializer;
            return serializer;

        }
        public static T LoadCustomConfig<T>(string path)
        {
            var config = default(T);
            var serializer = GetSerializer(typeof(T));
            using (var file = File.OpenRead(path))
            {
                config = (T)serializer.Deserialize(file);
            }
            return config;
        }

        public static void SaveCustomConfig<T>(string path, T config)
        {
            var serializer = GetSerializer(typeof(T));
            using (var file = File.OpenWrite(path))
            {
                serializer.Serialize(file, config);
            }
        }



        public static T LoadDotNetConfig<T>()
        {
            System.Configuration.Configuration app;
            if (System.Web.HttpContext.Current == null)
            {
                app= ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                app = WebConfigurationManager.OpenWebConfiguration("~");
            }
            foreach (var config in app.Sections)
            {
                if (config is T)
                    return (T)config ;
            }
            return default(T);
        }

        public static T LoadDotNetConfig<T>(string section)
        {
            object config;
            if (System.Web.HttpContext.Current == null)
            {
                config = ConfigurationManager.GetSection(section);
            }
            else
            {
                config = WebConfigurationManager.GetSection(section);
            }
            if (config is T)
                return (T)config;
            return default(T);
        }

        protected static CacheManager configCache = new CacheManager() { SlidingExpiration = TimeSpan.Zero, Priority = CacheItemPriority.Default };

        /// <summary>
        /// 获取指定类型的配置信息
        /// </summary>
        /// <typeparam name="T">指定配置项类型</typeparam>
        /// <returns></returns>
        public static T GetConfig<T>()
        {
            var config = default(T);
            var configType = typeof(T);
            if (configType.IsSubclassOf(typeof(ConfigurationSection)))
            {
                config = LoadDotNetConfig<T>();
            }
            else
            {
                config = configCache.Get<T>(configType.FullName);
                if (config == null)
                {
                    var path = GetConfigPath(configType);
                    config = LoadCustomConfig<T>(path);
                    if (config != null)
                    {
                        configCache.Add(configType.FullName, config, path);
                    }
                }
            }
            return config;
        }

        public static T GetConfig<T>(string section)
        {
            var config = default(T);
            var configType = typeof(T);
            if (configType.IsSubclassOf(typeof(ConfigurationSection)))
            {
                config = LoadDotNetConfig<T>(section);
            }
            else
            {
                config = configCache.Get<T>(configType.FullName);
                if (config == null)
                {
                    var path = GetConfigPath(configType);
                    config = LoadCustomConfig<T>(path);
                    if (config != null)
                    {
                        configCache.Add(configType.FullName, config, path);
                    }
                }
            }
            return config;
        }


    }
}
