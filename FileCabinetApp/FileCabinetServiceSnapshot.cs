using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#pragma warning disable SA1600
#pragma warning disable S1450
#pragma warning disable SA1201
#pragma warning disable S125

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;
        private FileCabinetRecordCsvWriter fileCabinetRecordCsvWriter;
        private FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter;
        private FileCabinetRecordCsvReader fileCabinetRecordCsvReader;
        private FileCabinetRecordXmlReader fileCabinetRecordXmlReader;

        public ReadOnlyCollection<FileCabinetRecord> Records { get; }

        public IList<FileCabinetRecord> RecordsFromFile { get; private set; }

        public FileCabinetServiceSnapshot(FileCabinetRecord[] fileCabinetRecords)
        {
            this.records = fileCabinetRecords;
            this.Records = new ReadOnlyCollection<FileCabinetRecord>(this.records);
        }

        public void SaveToCsv(StreamWriter streamWriter)
        {
            this.fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            streamWriter.WriteLine(ConstParameters.ColumnNames);
            foreach (var item in this.records)
            {
                this.fileCabinetRecordCsvWriter.Write(item);
            }

            streamWriter.Flush();
        }

        public void SaveToXml(StreamWriter streamWriter)
        {
            XmlWriter xmlWriter = XmlWriter.Create(streamWriter);
            this.fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(xmlWriter);
            this.fileCabinetRecordXmlWriter.Write(this.records.ToList());

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
            this.fileCabinetRecordCsvReader = new FileCabinetRecordCsvReader(streamReader);
            this.RecordsFromFile = new ReadOnlyCollection<FileCabinetRecord>(this.fileCabinetRecordCsvReader.ReadAll());
        }

        public void LoadFromXml(StreamReader streamReader)
        {
            XmlReader xmlReader = XmlReader.Create(streamReader);
            this.fileCabinetRecordXmlReader = new FileCabinetRecordXmlReader(xmlReader);
            this.RecordsFromFile = new ReadOnlyCollection<FileCabinetRecord>(this.fileCabinetRecordXmlReader.ReadAll());
        }
    }
}
