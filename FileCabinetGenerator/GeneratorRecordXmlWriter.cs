using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Models;
using FileCabinetApp.Models.XmlModels;

#pragma warning disable SA1600

namespace FileCabinetGenerator
{
    public class GeneratorRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;
        private readonly XmlSerializer serializer = new (typeof(RecordSerializeble));
        private readonly RecordSerializeble recordsSerializeble = new ();
        private readonly List<PersonXmlModel> people = new ();

        public GeneratorRecordXmlWriter(XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;
        }

        public void Write(List<FileCabinetRecord> records)
        {
            this.PersonSerialize(records);
            this.recordsSerializeble.Records = this.people;
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

                this.people.Add(person);
            }
        }
    }
}
