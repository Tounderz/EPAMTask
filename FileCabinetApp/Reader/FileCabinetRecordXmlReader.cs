using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Models;
using FileCabinetApp.Models.XmlModels;

#pragma warning disable SA1600

namespace FileCabinetApp.Reader
{
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlReader xmlReader;
        private readonly XmlSerializer serializer = new (typeof(RecordSerializeble));
        private readonly IList<FileCabinetRecord> records = new List<FileCabinetRecord>();

        public FileCabinetRecordXmlReader(XmlReader xmlReader)
        {
            this.xmlReader = xmlReader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            RecordSerializeble recordsSerializeble = (RecordSerializeble)this.serializer.Deserialize(this.xmlReader);
            this.RecordDeserialize(recordsSerializeble.Records);
            return this.records;
        }

        private void RecordDeserialize(List<PersonXmlModel> people)
        {
            foreach (var person in people)
            {
                FileCabinetRecord record = new ()
                {
                    Id = int.Parse(person.Id),
                    FirstName = person.Name.First,
                    LastName = person.Name.Last,
                    DateOfBirth = Convert.ToDateTime(person.DateOfBirth),
                    Age = short.Parse(person.Age),
                    Salary = decimal.Parse(person.Salary),
                    Symbol = char.Parse(person.Symbol),
                };

                this.records.Add(record);
            }
        }
    }
}
