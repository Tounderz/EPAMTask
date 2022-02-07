using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#pragma warning disable SA1600

namespace FileCabinetApp.Models.XmlModels
{
    [Serializable]
    [XmlRoot("records")]
    public class RecordSerializeble
    {
        [XmlElement("record")]
        public List<PersonXmlModel> Records { get; set; }
    }
}
