using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using FileCabinetApp.CreatePerson;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable CA1822
#pragma warning disable SA1600
#pragma warning disable SA1305

namespace FileCabinetApp.Services
{
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<int, FileCabinetRecord> idDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> ageDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> salaryDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> symbolDictionary = new ();
        private readonly IRecordValidator recordValidator;

        public FileCabinetMemoryService(IRecordValidator recordValidator)
        {
            this.recordValidator = recordValidator;
        }

        public int CreateRecord(PersonModel person)
        {
            var record = this.CreateFileCabinetRecord(person, this.list.Count + 1);
            this.list.Add(record);
            this.AddAnAllDictionariesItem(record);

            return record.Id;
        }

        public int InsertRecord(int id, PersonModel person)
        {
            var record = this.CreateFileCabinetRecord(person, id);
            this.list.Add(record);
            this.AddAnAllDictionariesItem(record);

            return record.Id;
        }

        public void UpdateRecord(int id, PersonModel person)
        {
            var record = this.CreateFileCabinetRecord(person, id);
            this.RemoveAnAllDictionariesItem(id);
            this.AddAnAllDictionariesItem(record);
            this.list[id - 1] = record;
        }

        public void DeleteRecord(int id)
        {
            FileCabinetRecord record = this.list.Find(i => i.Id == id);
            this.list.Remove(record);
            this.RemoveAnAllDictionariesItem(id);
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> records = snapshot.Records;
            IList<FileCabinetRecord> recordsFromFile = snapshot.RecordsFromFile;
            bool checkId = false;

            for (int i = 0; i < recordsFromFile.Count; i++)
            {
                try
                {
                    var person = new PersonModel()
                    {
                        FirstName = recordsFromFile[i].FirstName,
                        LastName = recordsFromFile[i].LastName,
                        DateOfBirth = recordsFromFile[i].DateOfBirth,
                        Age = recordsFromFile[i].Age,
                        Salary = recordsFromFile[i].Salary,
                        Symbol = recordsFromFile[i].Symbol,
                    };

                    this.recordValidator.ValidateParameters(person);
                    for (int j = 0; j < records.Count; j++)
                    {
                        if (records[j].Id == recordsFromFile[i].Id)
                        {
                            this.RemoveAnAllDictionariesItem(this.list[i].Id);
                            this.list[i] = recordsFromFile[j];
                            this.AddAnAllDictionariesItem(this.list[i]);
                            checkId = true;
                            break;
                        }
                    }

                    if (!checkId)
                    {
                        this.list.Add(recordsFromFile[i]);
                        this.AddAnAllDictionariesItem(recordsFromFile[i]);
                    }

                    checkId = false;
                }
                catch (Exception ex)
                {
                    if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentException || ex is OverflowException)
                    {
                        Console.WriteLine($"{recordsFromFile[i].Id}: {ex.Message}");
                    }
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        public ValueTuple<int, int> GetRecordsCount()
        {
            return ValueTuple.Create(this.list.Count, 0);
        }

        public ValueTuple<int, int> PurgeRecord()
        {
            throw new NotImplementedException();
        }

        public FileCabinetRecord FindById(int id)
        {
            return this.idDictionary[id];
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> firstNameList = this.firstNameDictionary[firstName];
            return firstNameList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> lastNameList = this.lastNameDictionary[lastName];
            return lastNameList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            List<FileCabinetRecord> dateOfBirthList = this.dateOfBirthDictionary[dateOfBirth];
            return dateOfBirthList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindByAge(string age)
        {
            List<FileCabinetRecord> ageList = this.ageDictionary[age];
            return ageList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            List<FileCabinetRecord> salaryList = this.salaryDictionary[salary];
            return salaryList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindBySymbol(string symbol)
        {
            List<FileCabinetRecord> symbolList = this.symbolDictionary[symbol];
            return symbolList.AsReadOnly();
        }

        private FileCabinetRecord CreateFileCabinetRecord(PersonModel person, int id)
        {
            this.recordValidator.ValidateParameters(person);
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

        private void AddDictionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        private void AddItemDictionaryId(FileCabinetRecord record)
        {
            if (!this.idDictionary.ContainsKey(record.Id))
            {
                this.idDictionary.Add(record.Id, record);
            }
        }

        private void AddAnAllDictionariesItem(FileCabinetRecord record)
        {
            this.AddItemDictionaryId(record);
            this.AddDictionaryItem(record.FirstName, record, this.firstNameDictionary);
            this.AddDictionaryItem(record.LastName, record, this.lastNameDictionary);
            this.AddDictionaryItem(record.DateOfBirth.ToString(ConstStrings.FormatDate), record, this.dateOfBirthDictionary);
            this.AddDictionaryItem(record.Age.ToString(), record, this.ageDictionary);
            this.AddDictionaryItem(record.Salary.ToString(), record, this.salaryDictionary);
            this.AddDictionaryItem(record.Symbol.ToString(), record, this.symbolDictionary);
        }

        private void RemoveDictionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary)
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

        private void RemoveItemDictionaryId(int id)
        {
            if (this.idDictionary.ContainsKey(id))
            {
                this.idDictionary.Remove(id);
            }
        }

        private void RemoveAnAllDictionariesItem(int id)
        {
            this.RemoveItemDictionaryId(id);
            this.RemoveDictionaryItem(id, this.firstNameDictionary);
            this.RemoveDictionaryItem(id, this.lastNameDictionary);
            this.RemoveDictionaryItem(id, this.dateOfBirthDictionary);
            this.RemoveDictionaryItem(id, this.ageDictionary);
            this.RemoveDictionaryItem(id, this.salaryDictionary);
            this.RemoveDictionaryItem(id, this.symbolDictionary);
        }
    }
}