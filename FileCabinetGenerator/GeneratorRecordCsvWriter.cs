using System;
using System.IO;
using FileCabinetApp.Models;

#pragma warning disable SA1600

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
