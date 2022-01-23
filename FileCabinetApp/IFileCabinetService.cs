﻿using System;
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

        void RemoveRecord(int id);

        FileCabinetServiceSnapshot MakeSnapshot();

        void Restore(FileCabinetServiceSnapshot snapshot);

        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        Tuple<int, int> GetRecordsCount();

        Tuple<int, int> PurgeRecord();

        IRecordIterator FindByFirstName(string firstName);

        IRecordIterator FindByLastName(string lastName);

        IRecordIterator FindByDateOfBirth(string dateOfBirth);
   }
}
