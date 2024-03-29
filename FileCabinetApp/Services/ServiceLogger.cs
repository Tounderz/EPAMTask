﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Services
{
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;
        private readonly string pathName;

        public ServiceLogger(IFileCabinetService fileCabinetService, string pathName)
        {
            this.fileCabinetService = fileCabinetService;
            this.pathName = pathName;
        }

        public int CreateRecord(PersonModel person)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Create() with " +
                $"FirstName = '{person.FirstName}', " +
                $"LastName = '{person.LastName}', " +
                $"DateOfBirth = '{person.DateOfBirth}', " +
                $"Age = '{person.Age}', " +
                $"Salary = '{person.Salary}', " +
                $"Symbol = '{person.Symbol}'.");

            int result = this.fileCabinetService.CreateRecord(person);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Create() returned '{result}'.");

            return result;
        }

        public int InsertRecord(int id, PersonModel person)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Insert() with " +
                $"Id = '{id}', " +
                $"FirstName = '{person.FirstName}', " +
                $"LastName = '{person.LastName}', " +
                $"DateOfBirth = '{person.DateOfBirth}', " +
                $"Age = '{person.Age}', " +
                $"Salary = '{person.Salary}', " +
                $"Symbol = '{person.Symbol}'.");

            int result = this.fileCabinetService.InsertRecord(id, person);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Insert() returned '{result}'.");

            return result;
        }

        public void UpdateRecord(int id, PersonModel person)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Edit() with " +
                $"id = '{id}', " +
                $"FirstName = '{person.FirstName}', " +
                $"LastName = '{person.LastName}', " +
                $"DateOfBirth = '{person.DateOfBirth}', " +
                $"Age = '{person.Age}', " +
                $"Salary = '{person.Salary}', " +
                $"Symbol = '{person.Symbol}'.");

            this.fileCabinetService.UpdateRecord(id, person);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Edit() edited entry by '{id}'.");
        }

        public void DeleteRecord(int id)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Edit() with " +
                $"id = '{id}'.");
            this.fileCabinetService.DeleteRecord(id);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Edit() deleting an entry by '{id}'.");
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling MakeSnapshot().");
            FileCabinetServiceSnapshot fileCabinetServiceSnapshot = this.fileCabinetService.MakeSnapshot();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - MakeSnapshot() transferring records to class 'FileCabinetServiceSnapshot'.");
            return fileCabinetServiceSnapshot;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Import().");
            this.fileCabinetService.Restore(snapshot);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Import() import records to existing records of a given file.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling List().");
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.GetRecords();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - List() output of all records.");
            return fileCabinetRecords;
        }

        public ValueTuple<int, int> GetRecordsCount()
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Start().");
            ValueTuple<int, int> result = this.fileCabinetService.GetRecordsCount();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Start() returned total count record(s) = '{result.Item1}' record(s) and delete count record(s) = '{result.Item2}'.");
            return result;
        }

        public ValueTuple<int, int> PurgeRecord()
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Purge().");
            ValueTuple<int, int> result = this.fileCabinetService.PurgeRecord();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Purge() {result.Item1} of {result.Item2} records were purged.");
            return result;
        }

        public FileCabinetRecord FindById(int id)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() Id = '{id}'.");
            FileCabinetRecord fileCabinetRecord = this.fileCabinetService.FindById(id);
            if (fileCabinetRecord != null)
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by Id = '{id}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by Id = '{id}'.");
            }

            return fileCabinetRecord;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() FirstName = '{firstName}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByFirstName(firstName);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by FirstName = '{firstName}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by FirstName = '{firstName}'.");
            }

            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() LastName = '{lastName}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByLastName(lastName);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by LastName = '{lastName}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by LastName = '{lastName}'.");
            }

            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() DateOfBirt = '{dateOfBirth}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by DateOfBirt = '{dateOfBirth}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by DateOfBirt = '{dateOfBirth}'.");
            }

            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByAge(string age)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() Age = '{age}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(age);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by Age = '{age}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by Age = '{age}'.");
            }

            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() Salary = '{salary}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(salary);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by Salary = '{salary}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by Salary = '{salary}'.");
            }

            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindBySymbol(string symbol)
        {
            using TextWriter textWrite = File.AppendText(this.pathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() Symbol = '{symbol}'.");
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(symbol);
            if (fileCabinetRecords.Any())
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by Symbol = '{symbol}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by Symbol = '{symbol}'.");
            }

            return fileCabinetRecords;
        }
    }
}