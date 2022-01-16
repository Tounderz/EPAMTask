using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1108

namespace FileCabinetApp
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

            if (!this.reader.EndOfStream) // для пропуска строки имён столбцов
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
