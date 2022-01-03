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

        void EditRecord(int id, Person person);

        FileCabinetServiceSnapshot MakeSnapshot();

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        int GetRecordsCount();

        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        void AddDitionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary); // добавление данных в словарь

        void RemoveDitionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary); // удаление данных из словаря по id
   }
}
