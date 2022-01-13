using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp;

#pragma warning disable SA1600
#pragma warning disable CA1062

namespace FileCabinetGenerator
{
    public class GeneratorRecordCsvWriter
    {
        private readonly TextWriter textWriter;

        public GeneratorRecordCsvWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Write(FileCabinetRecord record)
        {
            var line = $"{record.Id},{record.FirstName},{record.LastName},{record.DateOfBirth:dd/MM/yyyy},{record.Age},{record.Salary},{record.Symbol}";
            this.textWriter.WriteLine(line);
            this.textWriter.Flush();
        }
    }
}
