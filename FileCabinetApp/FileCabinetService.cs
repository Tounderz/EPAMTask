using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            // TODO: добавьте реализацию метода
            if (firstName.Length < 2 || firstName.Length > 60 || firstName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(firstName));
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "Can't be null");
            }

            if (lastName.Length < 2 || lastName.Length > 60 || lastName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(lastName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Can't be null");
            }

            DateTime minDate = new DateTime(1950, 01, 01);
            if (dateOfBirth > DateTime.Now || dateOfBirth < minDate)
            {
                throw new ArgumentException("Minimum date of birth 01/01/1950, and maximum - DateTime.Now", nameof(dateOfBirth));
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Symbol = symbol,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            // TODO: добавьте реализацию метода
            return this.list.ToArray();
        }

        public int GetStat()
        {
            // TODO: добавьте реализацию метода
            return this.list.Count;
        }
    }
}
