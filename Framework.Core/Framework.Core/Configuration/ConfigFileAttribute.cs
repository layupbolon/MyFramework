using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Configuration
{
    /// <summary>
    /// 指定配置文件的属性信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConfigFileAttribute : Attribute
    {
        private string fileName = string.Empty;

        /// <summary>
        /// 用正在属性化的配置文件位置特性初始化 <see cref="ConfigFileAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="fileName"></param>
        public ConfigFileAttribute(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// 获取相对路径文件名。
        /// </summary>
        /// <value>相对路径文件名。</value>
        public virtual string FileName
        {
            get { return fileName; }
            protected set { this.fileName = value; }
        }
    }
}
