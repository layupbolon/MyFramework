using System;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

namespace Framework.Core.Data.Configuration
{
    [Serializable]
    public class DataCommand
    {

        public DataCommand()
        {
            CommandType = CommandType.Text;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        [XmlElement("commandText")]
        public string CommandText { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        [XmlArray("parameters")]
        [XmlArrayItem("param")]
        public DataParameter[] Parameters { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DefaultValueAttribute(CommandType.Text)]
        [XmlAttribute("commandType")]
        public CommandType CommandType { get; set; }

    }
}
