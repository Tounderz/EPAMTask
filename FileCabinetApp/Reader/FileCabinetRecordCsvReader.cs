using System;
using System.Collections.Generic;
using System.IO;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Reader
{
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            IList<FileCabinetRecord> records = new List<FileCabinetRecord>();
            string line;
            string[] arrLine;
            this.reader.BaseStream.Position = 0;

            // для пропуска шапки таблицы
            if (!this.reader.EndOfStream)
            {
                this.reader.ReadLine();
            }

            while (!this.reader.EndOfStream)
            {
                line = this.reader.ReadLine();
                arrLine = line.Split(",");
                var record = new FileCabinetRecord
                {
                    Id = int.Parse(arrLine[0]),
                    FirstName = arrLine[1],
                    LastName = arrLine[2],
                    DateOfBirth = DateTime.Parse(arrLine[3]),
                    Age = short.Parse(arrLine[4]),
                    Salary = decimal.Parse(arrLine[5]),
                    Symbol = Convert.ToChar(arrLine[6]),
                };

                records.Add(record);
            }

            return records;
        }
    }
}
