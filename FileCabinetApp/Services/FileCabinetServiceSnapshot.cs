using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Models;
using FileCabinetApp.Reader;
using FileCabinetApp.Writer;

#pragma warning disable SA1201
#pragma warning disable S125
#pragma warning disable SA1600

namespace FileCabinetApp.Services
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        public ReadOnlyCollection<FileCabinetRecord> Records { get; }

        public IList<FileCabinetRecord> RecordsFromFile { get; private set; }

        public FileCabinetServiceSnapshot(FileCabinetRecord[] fileCabinetRecords)
        {
            this.records = fileCabinetRecords;
            this.Records = new ReadOnlyCollection<FileCabinetRecord>(this.records);
        }

        public void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter fileCabinetRecordCsvWriter = new (streamWriter);
            streamWriter.WriteLine(ColumnNames.StringColumnNamesCsv);
            foreach (var item in this.records)
            {
                fileCabinetRecordCsvWriter.Write(item);
            }

            streamWriter.Flush();
        }

        public void SaveToXml(StreamWriter streamWriter)
        {
            XmlWriter xmlWriter = XmlWriter.Create(streamWriter);
            FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new (xmlWriter);
            fileCabinetRecordXmlWriter.Write(this.records.ToList());

            // запись без сериализации
            // xmlWriter.WriteStartDocument();
            // xmlWriter.WriteStartElement("records");
            // foreach (var item in this.records)
            // {
            //    this.fileCabinetRecordXmlWriter.Write(item);
            // }

            // xmlWriter.WriteEndDocument();
            // xmlWriter.Flush();
        }

        public void LoadFromCsv(StreamReader streamReader)
        {
            FileCabinetRecordCsvReader fileCabinetRecordCsvReader = new (streamReader);
            this.RecordsFromFile = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetRecordCsvReader.ReadAll());
        }

        public void LoadFromXml(StreamReader streamReader)
        {
            XmlReader xmlReader = XmlReader.Create(streamReader);
            FileCabinetRecordXmlReader fileCabinetRecordXmlReader = new (xmlReader);
            this.RecordsFromFile = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetRecordXmlReader.ReadAll());
        }
    }
}
