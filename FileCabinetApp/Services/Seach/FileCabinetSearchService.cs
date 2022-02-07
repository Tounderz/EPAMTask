using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable SA1600
#pragma warning disable CA1822
#pragma warning disable SA1204

namespace FileCabinetApp.Services.Seach
{
    public class FileCabinetSearchService
    {
        private readonly IFileCabinetService service;
        private readonly Memoization memoization = new ();
        private List<FileCabinetRecord> records = new ();
        private List<FileCabinetRecord> interimRecords = new ();
        private static Dictionary<string, List<string>> cache = new ();
        private readonly string nameMethod;

        public FileCabinetSearchService(IFileCabinetService service, string nameMethod)
        {
            this.service = service;
            this.nameMethod = nameMethod;
        }

        public IEnumerable<(string key, string value)> GetListParameters(string parameters)
        {
            var result = new List<(string key, string value)>();
            string[] arrKeyValue = parameters.Split(',');
            foreach (var item in arrKeyValue)
            {
                string[] interim = item.Split('=');
                string key = interim[0].Trim().ToLower();
                string value = interim[1].Trim('\'', ' ').ToLower();
                result.Add((key, value));
            }

            return result;
        }

        public List<FileCabinetRecord> GetRecordsList(string[] interimParameters)
        {
            this.records.Clear();
            ValueTuple<int, string[]> seachCountAnd = this.SeachCountAnd(interimParameters);
            interimParameters = seachCountAnd.Item2;
            int countAnd = seachCountAnd.Item1;
            IEnumerable<(string key, string value)> seachCriteria = this.GetListParameters(string.Join(string.Empty, interimParameters));
            this.CheckCountSeachParameters(seachCriteria);
            if (this.service is FileCabinetMemoryService)
            {
                foreach (var (key, value) in seachCriteria)
                {
                    if (cache.Any(i => i.Key == key && i.Value.Contains(value)))
                    {
                        this.records = this.memoization.SeachInCache($"{key}={value}");
                    }
                    else
                    {
                        this.SeachInService(key, value);
                        this.AddDictionaryCache(key, value);
                    }
                }
            }
            else
            {
                foreach (var (key, value) in seachCriteria)
                {
                    this.SeachInService(key, value);
                }
            }

            if (countAnd > 0)
            {
                foreach (var (key, value) in seachCriteria)
                {
                    this.SortingByMultipleCriteria(key, value);
                }
            }

            if (this.nameMethod == Commands.InsertName || this.nameMethod == Commands.UpdateName || this.nameMethod == Commands.DeleteName)
            {
                Memoization.ClearCache();
                cache.Clear();
            }

            this.CheckRecordsCount();
            return this.records;
        }

        private void AddDictionaryCache(string key, string value)
        {
            if (!cache.ContainsKey(key))
            {
                cache.Add(key, new List<string>());
            }

            if (!cache[key].Contains(value))
            {
                cache[key].Add(value);
            }
        }

        private ValueTuple<int, string[]> SeachCountAnd(string[] interimParameters)
        {
            int countAnd = 0;
            for (int i = 0; i < interimParameters.Length; i++)
            {
                if (Separators.And.Contains(interimParameters[i]))
                {
                    interimParameters[i] = Separators.Comma;
                    countAnd++;
                }
                else if (Separators.Or.Contains(interimParameters[i]))
                {
                    interimParameters[i] = Separators.Comma;
                }
            }

            return new ValueTuple<int, string[]>(countAnd, interimParameters);
        }

        private void SeachInService(string key, string value)
        {
            try
            {
                string keyCache = $"{key}={value}";
                this.interimRecords.Clear();
                switch (key)
                {
                    case CriteriaNames.Id:
                        int id = int.Parse(value);
                        this.interimRecords.Add(this.service.FindById(id));
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.FirstName:
                        this.interimRecords = this.service.FindByFirstName(value.ToUpper()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.LastName:
                        this.interimRecords = this.service.FindByLastName(value.ToUpper()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.DateOfBirth:
                        DateTime dateOfBirth = DateTime.Parse(value);
                        this.interimRecords = this.service.FindByDateOfBirth(dateOfBirth.ToString(ConstStrings.FormatDate).ToUpper()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.Age:
                        short age = short.Parse(value);
                        this.interimRecords = this.service.FindByAge(age.ToString()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.Salary:
                        decimal salary = decimal.Parse(value);
                        this.interimRecords = this.service.FindBySalary(salary.ToString()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;

                    case CriteriaNames.Symbol:
                        char symbol = char.Parse(value);
                        this.interimRecords = this.service.FindBySymbol(symbol.ToString()).ToList();
                        Memoization.AddRecordInCacheDictionary(this.interimRecords, keyCache);
                        this.AddRecordInList(this.interimRecords);
                        break;
                    default:
                        throw new ArgumentException(PrintException.IncorrectInput);
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void SortingByMultipleCriteria(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case CriteriaNames.Id:
                        int id = int.Parse(value);
                        this.records = this.records.Where(i => i.Id == id).ToList();
                        break;

                    case CriteriaNames.FirstName:
                        this.records = this.records.Where(i => i.FirstName.ToLower() == value).ToList();
                        break;

                    case CriteriaNames.LastName:
                        this.records = this.records.Where(i => i.LastName.ToLower() == value).ToList();
                        break;

                    case CriteriaNames.DateOfBirth:
                        DateTime dateOfBirth = DateTime.Parse(value);
                        this.records = this.records.Where(i => i.DateOfBirth == dateOfBirth).ToList();
                        break;

                    case CriteriaNames.Age:
                        short age = short.Parse(value);
                        this.records = this.records.Where(i => i.Age == age).ToList();
                        break;

                    case CriteriaNames.Salary:
                        decimal salary = decimal.Parse(value);
                        this.records = this.records.Where(i => i.Salary == salary).ToList();
                        break;

                    case CriteriaNames.Symbol:
                        char symbol = char.Parse(value);
                        this.records = this.records.Where(i => i.Symbol == symbol).ToList();
                        break;
                    default:
                        throw new ArgumentException(PrintException.IncorrectInput);
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void CheckCountSeachParameters(IEnumerable<(string key, string value)> seachParameters)
        {
            if (!seachParameters.Any() && (this.nameMethod == Commands.DeleteName || this.nameMethod == Commands.UpdateName))
            {
                throw new ArgumentException("The minimum number of search criteria is one!");
            }
            else if (seachParameters.Count() < 2 && this.nameMethod == Commands.SelectName)
            {
                throw new ArgumentException("Incorrect data entry for the search!");
            }
        }

        private void CheckRecordsCount()
        {
            if (this.records.Count < 1)
            {
                throw new ArgumentException("There is no record(s) with these search parameters!");
            }
        }

        private void AddRecordInList(List<FileCabinetRecord> fileCabinetRecords)
        {
            foreach (var item in fileCabinetRecords)
            {
                if (!this.records.Exists(i => i.Id == item.Id))
                {
                    this.records.Add(item);
                }
            }
        }
    }
}
