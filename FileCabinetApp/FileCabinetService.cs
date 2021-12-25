using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

#pragma warning disable CA1822
#pragma warning disable SA1600
#pragma warning disable CA1305
#pragma warning disable SA1203
#pragma warning disable SA1101

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly FileCabinetServiceSnapshot fileCabinetServiceSnapshot = new ();
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private const string FormatDate = "yyyy-MMM-dd";

        public int CreateRecord(FileCabinetRecord record)
        {
            this.list.Add(record);
            AddDitionaryItem(record.FirstName, record, this.firstNameDictionary);
            AddDitionaryItem(record.LastName, record, this.lastNameDictionary);
            AddDitionaryItem(record.DateOfBirth.ToString(FormatDate), record, this.dateOfBirthDictionary);

            return record.Id;
        }

        public void EditRecord(int id, FileCabinetRecord record)
        {
            if (id > this.list.Count || id < 1)
            {
                throw new ArgumentException(null, nameof(id));
            }

            RemoveDitionaryItem(id, this.firstNameDictionary);
            AddDitionaryItem(record.FirstName, record, this.firstNameDictionary);
            RemoveDitionaryItem(id, this.lastNameDictionary);
            AddDitionaryItem(record.LastName, record, this.lastNameDictionary);
            RemoveDitionaryItem(id, this.dateOfBirthDictionary);
            AddDitionaryItem(record.DateOfBirth.ToString(FormatDate), record, this.dateOfBirthDictionary);
            this.list[id - 1] = record;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            fileCabinetServiceSnapshot.FileCabinetRecords = list;
            return fileCabinetServiceSnapshot;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        public int GetRecordsCount()
        {
            return this.list.Count;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> firstNameList = this.firstNameDictionary[firstName];
            return firstNameList.AsReadOnly();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> lastNameList = this.lastNameDictionary[lastName];
            return lastNameList.AsReadOnly();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            List<FileCabinetRecord> dateOfBirthList = this.dateOfBirthDictionary[dateOfBirth];
            return dateOfBirthList.AsReadOnly();
        }

        private void AddDitionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary) // добавление данных в словарь
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        private void RemoveDitionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary) // удаление данных из словаря по id
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
