using System;
using System.Collections.Generic;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetGenerator
{
    public class GeneratorService
    {
        private readonly List<FileCabinetRecord> records = new ();
        private readonly List<char> symbolList = new () { '!', '@', '#', '$', '%', '^', '&', '*', '`', '~', '(', ')', '-', '_', '=', '+', '№', ';', ':', '?', '/', '|', ',', '.' };
        private readonly DateTime minDate = new (1950, 01, 01);
        private readonly DateTime maxDate = DateTime.Now;
        private DateTime dateOfBirth;
        private int index = 0;

        public void GeneratorCreateRecord(int startId, int count)
        {
            int lastId = startId + count;
            while (startId < lastId)
            {
                this.RandomGenerator();
                var record = new FileCabinetRecord
                {
                    Id = startId,
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    DateOfBirth = this.dateOfBirth,
                    Age = Convert.ToInt16(this.maxDate.Year - this.dateOfBirth.Year),
                    Salary = new Random().Next(0, int.MaxValue),
                    Symbol = this.symbolList[this.index],
                };

                startId++;
                this.records.Add(record);
            }
        }

        public GeneratorSnapshot MakeSnapshot()
        {
            return new GeneratorSnapshot(this.records.ToArray());
        }

        private void RandomGenerator()
        {
            Random random = new ();
            this.index = random.Next(this.symbolList.Count - 1);
            int year = random.Next(this.minDate.Year, this.maxDate.Year);
            int month = random.Next(1, 12);
            int day = random.Next(1, DateTime.DaysInMonth(year, month));
            this.dateOfBirth = new DateTime(year, month, day);
        }
    }
}
