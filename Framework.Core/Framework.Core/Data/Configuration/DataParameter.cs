using System;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

namespace Framework.Core.Data.Configuration
{
    [Serializable]
    public class DataParameter
    {
        public DataParameter()
        {
            Direction = ParameterDirection.Input;
            DbType = DbType.String;
            Size = -1;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 参数数据类型
        /// </summary>
        [XmlAttribute("dbType")]
        [DefaultValueAttribute(DbType.String)]
        public DbType DbType { get; set; }

        [XmlAttribute("direction")]
        [DefaultValueAttribute(ParameterDirection.Input)]
        public ParameterDirection Direction { get; set; }

        [XmlAttribute("size")]
        public int Size{get;set;}

        /// <summary>
        /// 获取或设置用来表示 Value 属性的最大位数。 
        /// </summary>
        [XmlAttribute("precision")]
        public byte Precision { get; set; }

        /// <summary>
        /// 获取或设置 Value 解析为的小数位数。 
        /// </summary>
        [XmlAttribute("scale")]
        public byte Scale { get; set; }
    }
}
