using System.Xml.Serialization;

#pragma warning disable SA1600

namespace FileCabinetApp.Models.XmlModels
{
    public class PersonXmlModel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("name")]
        public NameXmlModel Name { get; set; }

        [XmlElement("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [XmlElement("age")]
        public string Age { get; set; }

        [XmlElement("salary")]
        public string Salary { get; set; }

        [XmlElement("symbol")]
        public string Symbol { get; set; }
    }
}
