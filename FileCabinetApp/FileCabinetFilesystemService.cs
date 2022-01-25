﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Validators;

#pragma warning disable SA1600
#pragma warning disable CA1822
#pragma warning disable SA1214
#pragma warning disable SA1202
#pragma warning disable SA1305

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private readonly IRecordValidator recordValidator;
        private List<FileCabinetRecord> list = new ();
        private readonly List<FileCabinetRecord> isDeleteRecords = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private int recordId = 0;
        private short status = 0;
        private readonly byte[] statusInBytes = new byte[sizeof(short)];
        private readonly byte[] recordIdInBytes = new byte[sizeof(int)];

        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator recordValidator)
        {
            this.fileStream = fileStream;
            this.recordValidator = recordValidator;

            if (this.fileStream.Length != 0)
            {
                byte[] recordBytes = new byte[this.fileStream.Length];
                this.fileStream.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.ConvertBytesToListRecord(recordBytes);
                for (int i = 0; i < this.list.Count; i++)
                {
                    this.AddInAllDictionaryItem(this.list[i]);
                }
            }
        }

        public int CreateRecord(Person person)
        {
            this.recordValidator.ValidateParameters(person);
            using FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (file.Length <= 0)
            {
                this.recordId++;
            }
            else
            {
                byte[] recordBytes = new byte[ConstParameters.RecordLength];
                int offset = (int)file.Length - ConstParameters.RecordLength;
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(recordBytes, 0, recordBytes.Length);
                this.recordId = BitConverter.ToInt32(recordBytes, this.statusInBytes.Length) + 1;
            }

            var record = this.GetFileCabinetRecord(person, this.recordId);
            this.AddInAllDictionaryItem(record);
            this.ConvertRecordToBytes(record);

            return this.recordId;
        }

        public int InsertRecord(int id, Person person)
        {
            this.recordValidator.ValidateParameters(person);
            this.recordId = id;
            var record = this.GetFileCabinetRecord(person, this.recordId);
            this.AddInAllDictionaryItem(record);
            this.ConvertRecordToBytes(record);

            return this.recordId;
        }

        public void EditRecord(int id, Person person)
        {
            this.recordValidator.ValidateParameters(person);
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToListRecord(recordBytes);
            FileCabinetRecord record = this.list.Find(i => i.Id == id);
            this.RemoveInAllDictionaryItem(record);
            record = this.GetFileCabinetRecord(person, record.Id);
            this.AddInAllDictionaryItem(record);
            this.ConvertRecordToBytes(record);
        }

        public void RemoveRecord(int id)
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[ConstParameters.RecordLength];
            this.status = ConstParameters.IsDelete;
            int idRecord = 0;
            int offset = 0;
            do
            {
                file.Seek(offset, SeekOrigin.Begin);
                recordBytes = new byte[ConstParameters.RecordLength];
                file.Read(recordBytes, 0, recordBytes.Length);
                offset += ConstParameters.RecordLength;
                idRecord = BitConverter.ToInt32(recordBytes, this.statusInBytes.Length);
            }
            while (idRecord != id);

            var record = this.list.Find(i => i.Id == idRecord);
            this.RemoveInAllDictionaryItem(record);

            byte[] arrBytes = BitConverter.GetBytes(this.status);
            Array.Copy(arrBytes, 0, recordBytes, 0, 2);

            offset -= ConstParameters.RecordLength;
            using BinaryWriter binaryWriter = new (this.fileStream, Encoding.ASCII, true);
            binaryWriter.Seek(offset, SeekOrigin.Begin);
            binaryWriter.Write(recordBytes);
        }

        private FileCabinetRecord GetFileCabinetRecord(Person person, int id) // создание объекта FileCabinetRecord
        {
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Age = person.Age,
                Salary = person.Salary,
                Symbol = person.Symbol,
            };

            return record;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            using FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToListRecord(recordBytes);
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> record = snapshot.Records;
            IList<FileCabinetRecord> recordsFromFile = snapshot.RecordsFromFile;
            bool checkId = false;

            for (int i = 0; i < recordsFromFile.Count; i++)
            {
                if (record.Count == 0)
                {
                    this.list.Add(recordsFromFile[i]);
                }
                else
                {
                    for (int j = 0; j < record.Count; j++)
                    {
                        if (record[j].Id == recordsFromFile[i].Id)
                        {
                            this.list[j] = recordsFromFile[i];
                            checkId = true;
                        }
                        else if (!checkId)
                        {
                            recordsFromFile[i].Id = this.list.Count + 1;
                            this.list.Add(recordsFromFile[i]);
                        }
                    }
                }

                checkId = false;
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.ConvertBytesToListRecord(recordBytes);
            }

            return this.list.AsReadOnly();
        }

        public Tuple<int, int> GetRecordsCount()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.ConvertBytesToListRecord(recordBytes);
            }

            return Tuple.Create(this.list.Count, this.isDeleteRecords.Count);
        }

        public Tuple<int, int> PurgeRecord()
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToListRecord(recordBytes);

            this.fileStream.SetLength(0);

            using BinaryWriter binaryWriter = new (this.fileStream, Encoding.ASCII, true);
            for (int i = 0; i < this.list.Count; i++)
            {
                this.status = 0;
                byte[] firstName = this.ConvertNameToBytes(this.list[i].FirstName);
                byte[] lastName = this.ConvertNameToBytes(this.list[i].LastName);
                var record = this.list[i];
                this.GetBinaryWriter(binaryWriter, this.status, record.Id, firstName, lastName, record);
                binaryWriter.Flush();
            }

            int count = this.isDeleteRecords.Count;
            this.isDeleteRecords.Clear();

            return Tuple.Create(count, this.list.Count + count);
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> lastNameList = this.firstNameDictionary[firstName];
            return lastNameList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> lastNameList = this.lastNameDictionary[lastName];
            return lastNameList.AsReadOnly();
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            List<FileCabinetRecord> dateOfBirthList = this.dateOfBirthDictionary[dateOfBirth];
            return dateOfBirthList.AsReadOnly();
        }

        private void AddDictionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        private void AddInAllDictionaryItem(FileCabinetRecord record)
        {
            this.AddDictionaryItem(record.FirstName, record, this.firstNameDictionary);
            this.AddDictionaryItem(record.LastName, record, this.lastNameDictionary);
            this.AddDictionaryItem(record.DateOfBirth.ToString(ConstParameters.FormatDate), record, this.dateOfBirthDictionary);
        }

        private void RemoveDictionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            foreach (var item in dictionary)
            {
                foreach (var el in item.Value)
                {
                    if (el.Id == id)
                    {
                        item.Value.Remove(el);
                        break;
                    }
                }
            }
        }

        private void RemoveInAllDictionaryItem(FileCabinetRecord record)
        {
            this.RemoveDictionaryItem(record.Id, this.firstNameDictionary);
            this.RemoveDictionaryItem(record.Id, this.lastNameDictionary);
            this.RemoveDictionaryItem(record.Id, this.dateOfBirthDictionary);
        }

        private void ConvertRecordToBytes(FileCabinetRecord record) // convert record to bytes
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            byte[] recordBytes = new byte[ConstParameters.RecordLength];

            using MemoryStream memoryStream = new (recordBytes);
            using BinaryWriter binaryWriter = new (memoryStream);
            byte[] firstName = this.ConvertNameToBytes(record.FirstName);
            byte[] lastName = this.ConvertNameToBytes(record.LastName);

            using BinaryWriter binary = new (this.fileStream, Encoding.ASCII, true);
            if (this.list.Exists(i => i.Id == record.Id))
            {
                using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                int offset = (ConstParameters.RecordLength * record.Id) - ConstParameters.RecordLength;
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(this.statusInBytes, 0, this.statusInBytes.Length);
                this.status = BitConverter.ToInt16(this.statusInBytes, 0);
                file.Read(this.recordIdInBytes, 0, this.recordIdInBytes.Length);
                this.recordId = BitConverter.ToInt32(this.recordIdInBytes, 0);
                binary.Seek(offset, SeekOrigin.Begin);
            }
            else
            {
                this.status = ConstParameters.NotDelete;
                binary.Seek(0, SeekOrigin.End);
            }

            this.GetBinaryWriter(binary, this.status, this.recordId, firstName, lastName, record);
            binary.Flush();
        }

        private void GetBinaryWriter(BinaryWriter binaryWriter, short status, int id, byte[] firstName, byte[] lastName, FileCabinetRecord record)
        {
            binaryWriter.Write(status);
            binaryWriter.Write(id);
            binaryWriter.Write(firstName);
            binaryWriter.Write(lastName);
            binaryWriter.Write(record.DateOfBirth.Year);
            binaryWriter.Write(record.DateOfBirth.Month);
            binaryWriter.Write(record.DateOfBirth.Day);
            binaryWriter.Write(record.Age);
            binaryWriter.Write(record.Salary);
            binaryWriter.Write(record.Symbol);
        }

        private byte[] ConvertNameToBytes(string value) // convert FirstName and LastName to bytes
        {
            byte[] nameToBytes = Encoding.ASCII.GetBytes(value);
            byte[] name = new byte[ConstParameters.MaxLengthString];
            int nameLengh = nameToBytes.Length;
            if (nameLengh > ConstParameters.MaxLengthString)
            {
                nameLengh = ConstParameters.MaxLengthString;
            }

            Array.Copy(nameToBytes, 0, name, 0, nameLengh);

            return name;
        }

        private List<FileCabinetRecord> ConvertBytesToListRecord(byte[] recordBytes) // convert bytes to record
        {
            if (recordBytes is null)
            {
                throw new ArgumentNullException(nameof(recordBytes));
            }

            List<FileCabinetRecord> records = new ();

            using MemoryStream stream = new (recordBytes);
            using BinaryReader reader = new (stream);
            reader.BaseStream.Position = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var record = new FileCabinetRecord();
                this.status = reader.ReadInt16();
                record.Id = reader.ReadInt32();
                byte[] firstNameBytes = reader.ReadBytes(ConstParameters.MaxLengthString);
                string bytesToFirstName = Encoding.ASCII.GetString(firstNameBytes, 0, firstNameBytes.Length);
                record.FirstName = bytesToFirstName.Replace("\0", string.Empty);
                byte[] lastNameBytes = reader.ReadBytes(ConstParameters.MaxLengthString);
                string bytesToLastName = Encoding.ASCII.GetString(lastNameBytes, 0, lastNameBytes.Length);
                record.LastName = bytesToLastName.Replace("\0", string.Empty);
                int dateOfYear = reader.ReadInt32();
                int dateOfMonth = reader.ReadInt32();
                int dateOfDay = reader.ReadInt32();
                record.DateOfBirth = new DateTime(dateOfYear, dateOfMonth, dateOfDay);
                record.Age = reader.ReadInt16();
                record.Salary = reader.ReadDecimal();
                record.Symbol = reader.ReadChar();
                if (this.status == 1)
                {
                    FileCabinetRecord isDeleteRecord = this.isDeleteRecords.Find(i => i.Id == record.Id);
                    if (!this.isDeleteRecords.Contains(isDeleteRecord))
                    {
                        this.isDeleteRecords.Add(record);
                    }
                }
                else
                {
                    records.Add(record);
                }
            }

            return records;
        }
    }
}
