
using System.Xml.Serialization;
namespace Framework.Core.Configuration
{
    public class ParamItemInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Content { get; set; }
    }
}
