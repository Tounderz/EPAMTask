using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp.Validators;

#pragma warning disable CA1822
#pragma warning disable SA1600
#pragma warning disable CA1305
#pragma warning disable SA1202
#pragma warning disable SA1305

namespace FileCabinetApp
{
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<int, FileCabinetRecord> idDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> ageDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> salatyDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> symbolDictionary = new ();
        private readonly IRecordValidator recordValidator;

        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        public int CreateRecord(Person person)
        {
            this.recordValidator.ValidateParameters(person);
            var record = this.GetFileCabinetRecord(person, this.list.Count + 1);
            this.list.Add(record);
            this.AddInAllDictionaryNewItem(record);

            return record.Id;
        }

        public int InsertRecord(int id, Person person)
        {
            this.recordValidator.ValidateParameters(person);
            var record = this.GetFileCabinetRecord(person, id);
            this.list.Add(record);
            this.AddInAllDictionaryNewItem(record);

            return record.Id;
        }

        public void UpdateRecord(int id, Person person)
        {
            this.recordValidator.ValidateParameters(person);
            var record = this.GetFileCabinetRecord(person, id);
            this.RemoveInAllDictionaryItem(id);
            this.AddInAllDictionaryNewItem(record);
            this.list[id - 1] = record;
        }

        public void DeleteRecord(int id)
        {
            FileCabinetRecord record = this.list.Find(i => i.Id == id);
            this.list.Remove(record);
            this.RemoveInAllDictionaryItem(id);
        }

        private FileCabinetRecord GetFileCabinetRecord(Person person, int id) // создание объекта FileCabinetRecord
        {
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Age = person.Age,
                Salary = person.Salary,
                Symbol = person.Symbol,
            };

            return record;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> record = snapshot.Records;
            IList<FileCabinetRecord> recordFromFile = snapshot.RecordsFromFile;
            bool checkId = false;

            for (int i = 0; i < recordFromFile.Count; i++)
            {
                if (record.Count == 0)
                {
                    this.list.Add(recordFromFile[i]);
                }
                else
                {
                    for (int j = 0; j < record.Count; j++)
                    {
                        if (record[j].Id == recordFromFile[i].Id)
                        {
                            this.list[j] = recordFromFile[i];
                            checkId = true;
                        }
                        else if (!checkId)
                        {
                            recordFromFile[i].Id = this.list.Count + 1;
                            this.list.Add(recordFromFile[i]);
                        }
                    }
                }

                checkId = false;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        public Tuple<int, int> GetRecordsCount()
        {
            return Tuple.Create(this.list.Count, 0);
        }

        public Tuple<int, int> PurgeRecord()
        {
            throw new NotImplementedException();
        }

        public FileCabinetRecord FindById(int id)
        {
            return this.idDictionary[id];
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            return this.firstNameDictionary.TryGetValue(firstName, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            return this.lastNameDictionary.TryGetValue(lastName, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            return this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        public IEnumerable<FileCabinetRecord> FindByAge(string age)
        {
            return this.ageDictionary.TryGetValue(age, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            return this.salatyDictionary.TryGetValue(salary, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        public IEnumerable<FileCabinetRecord> FindBySymbol(string symbol)
        {
            return this.symbolDictionary.TryGetValue(symbol, out List<FileCabinetRecord> records) ?
                records : new List<FileCabinetRecord>();
        }

        private void AddDictionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary) // добавление данных в словарь
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        private void AddDictionaryId(FileCabinetRecord record)
        {
            if (!this.idDictionary.ContainsKey(record.Id))
            {
                this.idDictionary.Add(record.Id, record);
            }
        }

        private void AddInAllDictionaryNewItem(FileCabinetRecord record)
        {
            this.AddDictionaryId(record);
            this.AddDictionaryItem(record.FirstName, record, this.firstNameDictionary);
            this.AddDictionaryItem(record.LastName, record, this.lastNameDictionary);
            this.AddDictionaryItem(record.DateOfBirth.ToString(ConstParameters.FormatDate), record, this.dateOfBirthDictionary);
            this.AddDictionaryItem(record.Age.ToString(), record, this.ageDictionary);
            this.AddDictionaryItem(record.Salary.ToString(), record, this.salatyDictionary);
            this.AddDictionaryItem(record.Symbol.ToString(), record, this.symbolDictionary);
        }

        private void RemoveDictionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary) // удаление данных из словаря по id
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

        private void RemoveDictionaryId(int id)
        {
            if (this.idDictionary.ContainsKey(id))
            {
                this.idDictionary.Remove(id);
            }
        }

        private void RemoveInAllDictionaryItem(int id)
        {
            this.RemoveDictionaryId(id);
            this.RemoveDictionaryItem(id, this.firstNameDictionary);
            this.RemoveDictionaryItem(id, this.lastNameDictionary);
            this.RemoveDictionaryItem(id, this.dateOfBirthDictionary);
            this.RemoveDictionaryItem(id, this.ageDictionary);
            this.RemoveDictionaryItem(id, this.salatyDictionary);
            this.RemoveDictionaryItem(id, this.symbolDictionary);
        }
    }
}