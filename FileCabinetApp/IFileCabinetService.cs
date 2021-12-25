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
        public int CreateRecord(FileCabinetRecord record);

        public void EditRecord(int id, FileCabinetRecord record);

        public FileCabinetServiceSnapshot MakeSnapshot();

        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        public int GetRecordsCount();

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);
    }
}
