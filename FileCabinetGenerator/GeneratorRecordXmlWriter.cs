using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using FileCabinetApp;

#pragma warning disable SA1600

namespace FileCabinetGenerator
{
    public class GeneratorRecordXmlWriter
    {
        private readonly XmlWriter xmlWriter;
        private readonly XmlSerializer serializer = new (typeof(RecordSerializeble));
        private readonly RecordSerializeble recordsSerializeble = new ();
        private readonly List<Person> people = new ();

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

        private void PersonSerialize(List<FileCabinetRecord> records) // для листа сериализации
        {
            foreach (var record in records)
            {
                Person person = new ()
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

                this.people.Add(person);
            }
        }
    }
}
