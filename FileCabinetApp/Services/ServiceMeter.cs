using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Services
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;
        private readonly Stopwatch stopwatch = new ();

        public ServiceMeter(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        public int CreateRecord(PersonModel person)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            int result = this.fileCabinetService.CreateRecord(person);
            this.stopwatch.Stop();
            Console.WriteLine($"Create method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public int InsertRecord(int id, PersonModel person)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            int result = this.fileCabinetService.InsertRecord(id, person);
            this.stopwatch.Stop();
            Console.WriteLine($"Insert method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public void UpdateRecord(int id, PersonModel person)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            this.fileCabinetService.UpdateRecord(id, person);
            this.stopwatch.Stop();
            Console.WriteLine($"Edit method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        public void DeleteRecord(int id)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            this.fileCabinetService.DeleteRecord(id);
            this.stopwatch.Stop();
            Console.WriteLine($"Remove method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.fileCabinetService.MakeSnapshot();
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            this.fileCabinetService.Restore(snapshot);
            this.stopwatch.Stop();
            Console.WriteLine($"Restore method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.GetRecords();
            this.stopwatch.Stop();
            Console.WriteLine($"List method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public ValueTuple<int, int> GetRecordsCount()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            ValueTuple<int, int> result = this.fileCabinetService.GetRecordsCount();
            this.stopwatch.Stop();
            Console.WriteLine($"Stat method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public ValueTuple<int, int> PurgeRecord()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            ValueTuple<int, int> result = this.fileCabinetService.PurgeRecord();
            this.stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public FileCabinetRecord FindById(int id)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            FileCabinetRecord fileCabinetRecord = this.fileCabinetService.FindById(id);
            this.stopwatch.Stop();
            Console.WriteLine($"FindById method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecord;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByFirstName(firstName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByFirstName method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByLastName(lastName);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByLastName method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByDateOfBirth method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindByAge(string age)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(age);
            this.stopwatch.Stop();
            Console.WriteLine($"FindByAge method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(salary);
            this.stopwatch.Stop();
            Console.WriteLine($"FindBySalary method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }

        public IEnumerable<FileCabinetRecord> FindBySymbol(string symbol)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            IEnumerable<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(symbol);
            this.stopwatch.Stop();
            Console.WriteLine($"FindBySymbol method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return fileCabinetRecords;
        }
    }
}