using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1450

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecordCsvWriter fileCabinetRecordCsvWriter;

        public List<FileCabinetRecord> FileCabinetRecords { get; set; }

        public void SaveToCsv(StreamWriter streamWriter)
        {
            this.fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(streamWriter);
            streamWriter.WriteLine("Id,First Name,Last Name,Date of birth,Age,Salary,Symbol");
            foreach (var item in this.FileCabinetRecords)
            {
                this.fileCabinetRecordCsvWriter.Write(item);
            }
        }
    }
}
