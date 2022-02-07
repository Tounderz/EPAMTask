using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Models;
using FileCabinetApp.Models.XmlModels;

#pragma warning disable S125
#pragma warning disable SA1600

namespace FileCabinetApp.Writer
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;
        private readonly List<PersonXmlModel> peopleSerializeble = new ();
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

        private void PersonSerialize(List<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                PersonXmlModel person = new ()
                {
                    Id = record.Id.ToString(),
                    Name = new NameXmlModel()
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
