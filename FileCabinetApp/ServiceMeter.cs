using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService fileCabinetService;
        private readonly Stopwatch stopwatch = new ();

        public ServiceMeter(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        public int CreateRecord(Person person)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            int result = this.fileCabinetService.CreateRecord(person);
            this.stopwatch.Stop();
            Console.WriteLine($"Create method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public void EditRecord(int id, Person person)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            this.fileCabinetService.EditRecord(id, person);
            this.stopwatch.Stop();
            Console.WriteLine($"Edit method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
        }

        public void RemoveRecord(int id)
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            this.fileCabinetService.RemoveRecord(id);
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

        public Tuple<int, int> GetRecordsCount()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            Tuple<int, int> result = this.fileCabinetService.GetRecordsCount();
            this.stopwatch.Stop();
            Console.WriteLine($"Start method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
        }

        public Tuple<int, int> PurgeRecord()
        {
            this.stopwatch.Reset();
            this.stopwatch.Start();
            Tuple<int, int> result = this.fileCabinetService.PurgeRecord();
            this.stopwatch.Stop();
            Console.WriteLine($"Purge method execution duration is {this.stopwatch.ElapsedTicks} ticks.");
            return result;
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
    }
}
