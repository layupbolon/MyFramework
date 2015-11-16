using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Framework.Core.Configuration
{
    public class ServiceItemInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        private Type m_ServiceType;
        [XmlIgnore]
        public Type ServiceType
        {
            get
            {
                if (m_ServiceType == null)
                {
                    m_ServiceType = Type.GetType(ServiceName);
                }
                return m_ServiceType;
            }
        }


        [XmlAttribute("service")]
        public string ServiceName { get; set; }

        private Type m_ClassType;
        [XmlIgnore]
        public Type ClassType
        {
            get
            {
                if (m_ClassType == null)
                {
                    m_ClassType = Type.GetType(ClassName);
                }
                return m_ClassType;
            }
        }

       
        [XmlAttribute("class")]
        public string ClassName { get; set; }
    }
}
