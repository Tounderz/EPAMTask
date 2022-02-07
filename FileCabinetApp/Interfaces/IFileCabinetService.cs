using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Models;
using FileCabinetApp.Services;

#pragma warning disable SA1600

namespace FileCabinetApp.Interfaces
{
   public interface IFileCabinetService
   {
        int CreateRecord(PersonModel person);

        int InsertRecord(int id, PersonModel person);

        void UpdateRecord(int id, PersonModel person);

        void DeleteRecord(int id);

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        ValueTuple<int, int> GetRecordsCount();

        ValueTuple<int, int> PurgeRecord();

        FileCabinetRecord FindById(int id);

        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        IEnumerable<FileCabinetRecord> FindByAge(string age);

        IEnumerable<FileCabinetRecord> FindBySalary(string salary);

        IEnumerable<FileCabinetRecord> FindBySymbol(string symbol);
    }
}