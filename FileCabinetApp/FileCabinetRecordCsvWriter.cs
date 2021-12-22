using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter textWriter;

        public FileCabinetRecordCsvWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Write(FileCabinetRecord record)
        {
            var line = $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth},{record.Age},{record.Salary},{record.Symbol}";
            this.textWriter.WriteLine(line);
            this.textWriter.Flush();
        }
    }
}
