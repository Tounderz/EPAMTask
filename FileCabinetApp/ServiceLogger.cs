using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class ServiceLogger : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;

        public ServiceLogger(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        public int CreateRecord(Person person)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
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

        public void EditRecord(int id, Person person)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Edit() with " +
                $"id = '{id}', " +
                $"FirstName = '{person.FirstName}', " +
                $"LastName = '{person.LastName}', " +
                $"DateOfBirth = '{person.DateOfBirth}', " +
                $"Age = '{person.Age}', " +
                $"Salary = '{person.Salary}', " +
                $"Symbol = '{person.Symbol}'.");

            this.fileCabinetService.EditRecord(id, person);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Edit() edited entry by '{id}'.");
        }

        public void RemoveRecord(int id)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Edit() with " +
                $"id = '{id}'.");
            this.fileCabinetService.RemoveRecord(id);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Edit() deleting an entry by '{id}'.");
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling MakeSnapshot().");
            FileCabinetServiceSnapshot fileCabinetServiceSnapshot = this.fileCabinetService.MakeSnapshot();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - MakeSnapshot() transferring records to class 'FileCabinetServiceSnapshot'.");
            return fileCabinetServiceSnapshot;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Import().");
            this.fileCabinetService.Restore(snapshot);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Import() import records to existing records of a given file.");
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling List().");
            ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords = this.fileCabinetService.GetRecords();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - List() output of all records.");
            return fileCabinetRecords;
        }

        public Tuple<int, int> GetRecordsCount()
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Start().");
            Tuple<int, int> result = this.fileCabinetService.GetRecordsCount();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Start() returned total count record(s) = '{result.Item1}' record(s) and delete count record(s) = '{result.Item2}'.");
            return result;
        }

        public Tuple<int, int> PurgeRecord()
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Purge().");
            Tuple<int, int> result = this.fileCabinetService.PurgeRecord();
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Purge() {result.Item1} of {result.Item2} records were purged.");
            return result;
        }

        public IRecordIterator FindByFirstName(string firstName)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() FirstName = '{firstName}'.");
            IRecordIterator fileCabinetRecords = this.fileCabinetService.FindByFirstName(firstName);
            if (fileCabinetRecords.GetCount() > 0)
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by FirstName = '{firstName}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by FirstName = '{firstName}'.");
            }

            return fileCabinetRecords;
        }

        public IRecordIterator FindByLastName(string lastName)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() LastName = '{lastName}'.");
            IRecordIterator fileCabinetRecords = this.fileCabinetService.FindByLastName(lastName);
            if (fileCabinetRecords.GetCount() > 0)
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by LastName = '{lastName}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by LastName = '{lastName}'.");
            }

            return fileCabinetRecords;
        }

        public IRecordIterator FindByDateOfBirth(string dateOfBirth)
        {
            using TextWriter textWrite = File.AppendText(ConstParameters.LoggerPathName);
            textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Calling Find() DateOfBirt = '{dateOfBirth}'.");
            IRecordIterator fileCabinetRecords = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
            if (fileCabinetRecords.GetCount() > 0)
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() record(s) found by DateOfBirt = '{dateOfBirth}'.");
            }
            else
            {
                textWrite.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm} - Find() no record(s) found by DateOfBirt = '{dateOfBirth}'.");
            }

            return fileCabinetRecords;
        }

        public void AddDictionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary) => throw new NotImplementedException();

        public void RemoveDictionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary) => throw new NotImplementedException();
    }
}
