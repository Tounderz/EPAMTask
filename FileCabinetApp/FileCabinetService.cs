using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0090
#pragma warning disable CA1304
#pragma warning disable CA1062
#pragma warning disable CA1305
#pragma warning disable IDE0060

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        public readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            // добавьте реализацию метода
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "Can't be null");
            }

            if (firstName.Length < 2 || firstName.Length > 60 || firstName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Can't be null");
            }

            if (lastName.Length < 2 || lastName.Length > 60 || lastName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(lastName));
            }

            if (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                throw new ArgumentException("Minimum date of birth 01/01/1950, and maximum - DateTime.Now", nameof(dateOfBirth));
            }

            if (symbol.ToString().Length != 1)
            {
                throw new ArgumentException("The size of the string character is equal to one element", nameof(symbol));
            }

            var record = AddRecord(this.list.Count + 1, firstName, lastName, dateOfBirth, age, salary, symbol);

            this.list.Add(record);
            AddDitionary(firstName, record, this.firstNameDictionary);
            AddDitionary(lastName, record, this.lastNameDictionary);
            AddDitionary(dateOfBirth.ToString("yyyy-MMM-dd"), record, this.dateOfBirthDictionary);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            // добавьте реализацию метода
            return this.list.ToArray();
        }

        public int GetStat()
        {
            // добавьте реализацию метода
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            if (id > this.list.Count || id < 1)
            {
                throw new ArgumentException(null, nameof(id));
            }

            var record = AddRecord(id, firstName, lastName, dateOfBirth, age, salary, symbol);
            RemoveDitionary(id, this.firstNameDictionary);
            AddDitionary(firstName, record, this.firstNameDictionary);
            RemoveDitionary(id, this.lastNameDictionary);
            AddDitionary(lastName, record, this.lastNameDictionary);
            RemoveDitionary(id, this.dateOfBirthDictionary);
            AddDitionary(dateOfBirth.ToString("yyyy-MMM-dd"), record, this.dateOfBirthDictionary);
            this.list[id - 1] = record;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> first = this.firstNameDictionary[firstName.ToUpper()];
            return first.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            List<FileCabinetRecord> first = this.lastNameDictionary[lastname.ToUpper()];
            return first.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string date)
        {
            DateTime dateOfBirth = Convert.ToDateTime(date);
            List<FileCabinetRecord> first = this.dateOfBirthDictionary[dateOfBirth.ToString("yyyy-MMM-dd")];
            return first.ToArray();
        }

        private static FileCabinetRecord AddRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Symbol = symbol,
            };

            return record;
        }

        private static void AddDitionary(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        private static void RemoveDitionary(int id, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            foreach (var item in dictionary)
            {
                foreach (var el in item.Value)
                {
                    if (el.Id == id)
                    {
                        item.Value.Remove(el);
                        break;
                    }
                }
            }
        }
    }
}
