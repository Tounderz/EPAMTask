using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;

        public FileCabinetRecordXmlWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        public void Write(FileCabinetRecord record)
        {
            this.xmlWriter.WriteStartElement("record");
            this.xmlWriter.WriteAttributeString("id", record.Id.ToString());
            this.xmlWriter.WriteStartElement("name");
            this.xmlWriter.WriteAttributeString("first", record.FirstName);
            this.xmlWriter.WriteAttributeString("last", record.LastName);
            this.xmlWriter.WriteEndElement();
            this.xmlWriter.WriteElementString("dateOfBirth", record.DateOfBirth.ToShortDateString());
            this.xmlWriter.WriteElementString("age", record.Age.ToString());
            this.xmlWriter.WriteElementString("salary", record.Salary.ToString());
            this.xmlWriter.WriteElementString("symbol", record.Symbol.ToString());
            this.xmlWriter.WriteEndElement();
        }
    }
}
