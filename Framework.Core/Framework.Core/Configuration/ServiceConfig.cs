using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Framework.Core.Configuration;

namespace Framework.Core.Configuration
{
    [Serializable]
    [ConfigFile("Services.config")]
    [XmlRoot("ServiceConfig", IsNullable = false)]
    public class ServiceConfig
    {
        [XmlElement("service")]
        public List<ServiceItemInfo> Services { get; set; }
    }
}
