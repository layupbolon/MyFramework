using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Framework.Core.Configuration;

namespace Framework.Core.Configuration
{
    [Serializable]
    [ConfigFile("Params.config")]
    [XmlRoot("ParamConfig", IsNullable = false)]
    public class ParamConfig
    {
        [XmlElement("param")]
        public List<ParamItemInfo> ParamItems { get; set; }
    }
}
