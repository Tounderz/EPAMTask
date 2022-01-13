using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileCabinetApp;

#pragma warning disable S1450
#pragma warning disable SA1600
#pragma warning disable SA1203

namespace FileCabinetGenerator
{
    public class GeneratorSnapshot
    {
        private readonly FileCabinetRecord[] records;
        private GeneratorRecordCsvWriter generatorRecordCsvWriter;
        private GeneratorRecordXmlWriter generatorRecordXmlWriter;
        private const string ColumnNames = "Id,First Name,Last Name,Date of birth,Age,Salary,Symbol";

        public GeneratorSnapshot(FileCabinetRecord[] fileCabinetRecords)
        {
            this.records = fileCabinetRecords;
        }

        public void SaveToCsv(StreamWriter streamWriter)
        {
            this.generatorRecordCsvWriter = new GeneratorRecordCsvWriter(streamWriter);
            streamWriter.WriteLine(ColumnNames);
            foreach (var item in this.records)
            {
                this.generatorRecordCsvWriter.Write(item);
            }

            streamWriter.Flush();
        }

        public void SaveToXml(StreamWriter streamWriter)
        {
            XmlWriterSettings settings = new ();
            settings.Indent = true;
            using XmlWriter xmlWriter = XmlWriter.Create(streamWriter, settings);
            this.generatorRecordXmlWriter = new GeneratorRecordXmlWriter(xmlWriter);
            this.generatorRecordXmlWriter.Write(this.records.ToList());
        }
    }
}
