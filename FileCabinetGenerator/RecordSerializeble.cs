using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FileCabinetApp;

#pragma warning disable SA1600
#pragma warning disable SA1402
#pragma warning disable SA1649

namespace FileCabinetGenerator
{
    public class Name
    {
        [XmlAttribute("first")]
        public string First { get; set; }

        [XmlAttribute("last")]
        public string Last { get; set; }
    }

    public class Person
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("name")]
        public Name Name { get; set; }

        [XmlElement("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [XmlElement("age")]
        public string Age { get; set; }

        [XmlElement("salary")]
        public string Salary { get; set; }

        [XmlElement("symbol")]
        public string Symbol { get; set; }
    }

    [Serializable]
    [XmlRoot("records")]
    public class RecordSerializeble
    {
        [XmlElement("record")]
        public List<Person> Records { get; set; }
    }
}
