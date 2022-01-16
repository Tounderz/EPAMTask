using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

#pragma warning disable SA1600
#pragma warning disable S125

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;
        private readonly List<PersonSerializeble> peopleSerializeble = new ();
        private readonly RecordSerializeble recordsSerializeble = new ();
        private readonly XmlSerializer serializer = new (typeof(RecordSerializeble));

        public FileCabinetRecordXmlWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        public void Write(List<FileCabinetRecord> records)
        {
            this.PersonSerialize(records);
            this.recordsSerializeble.Records = this.peopleSerializeble;
            this.serializer.Serialize(this.xmlWriter, this.recordsSerializeble);
        }

        private void PersonSerialize(List<FileCabinetRecord> records) // для листа сериализации
        {
            foreach (var record in records)
            {
                PersonSerializeble person = new ()
                {
                    Id = record.Id.ToString(),
                    Name = new Name()
                    {
                        First = record.FirstName.ToString(),
                        Last = record.LastName.ToString(),
                    },
                    DateOfBirth = record.DateOfBirth.ToShortDateString(),
                    Age = record.Age.ToString(),
                    Salary = record.Salary.ToString(),
                    Symbol = record.Symbol.ToString(),
                };

                this.peopleSerializeble.Add(person);
            }
        }

        // без сериализации
        // public void Write(FileCabinetRecord record)
        // {
        //    this.xmlWriter.WriteStartElement("record");
        //    this.xmlWriter.WriteAttributeString("id", record.Id.ToString());
        //    this.xmlWriter.WriteStartElement("name");
        //    this.xmlWriter.WriteAttributeString("first", record.FirstName);
        //    this.xmlWriter.WriteAttributeString("last", record.LastName);
        //    this.xmlWriter.WriteEndElement();
        //    this.xmlWriter.WriteElementString("dateOfBirth", record.DateOfBirth.ToShortDateString());
        //    this.xmlWriter.WriteElementString("age", record.Age.ToString());
        //    this.xmlWriter.WriteElementString("salary", record.Salary.ToString());
        //    this.xmlWriter.WriteElementString("symbol", record.Symbol.ToString());
        //    this.xmlWriter.WriteEndElement();
        // }
    }
}
