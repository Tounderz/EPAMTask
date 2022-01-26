using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
   public interface IFileCabinetService
   {
        int CreateRecord(Person person);

        int InsertRecord(int id, Person person);

        void UpdateRecord(int id, Person person);

        void DeleteRecord(int id);

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        Tuple<int, int> GetRecordsCount();

        Tuple<int, int> PurgeRecord();

        IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        IEnumerable<FileCabinetRecord> FindByAge(string age);

        IEnumerable<FileCabinetRecord> FindBySalary(string salary);

        IEnumerable<FileCabinetRecord> FindBySymbol(string symbol);
    }
}