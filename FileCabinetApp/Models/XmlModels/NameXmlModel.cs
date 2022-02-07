using System.Xml.Serialization;

#pragma warning disable SA1600

namespace FileCabinetApp.Models.XmlModels
{
    public class NameXmlModel
    {
        [XmlAttribute("first")]
        public string First { get; set; }

        [XmlAttribute("last")]
        public string Last { get; set; }
    }
}
