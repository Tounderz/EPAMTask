using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

#pragma warning disable SA1600
#pragma warning disable SA1203
#pragma warning disable CA1822
#pragma warning disable SA1214
#pragma warning disable SA1202

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private const int MaxLengthString = 120;
        private const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;
        private int recordId = 0;
        private short status = 0;
        private readonly byte[] statusInBytes = new byte[sizeof(short)];
        private readonly byte[] recordIdInBytes = new byte[sizeof(int)];
        private const string FormatDate = "yyyy-MMM-dd";

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord(Person person)
        {
            using FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (file.Length <= 0)
            {
                this.recordId++;
            }
            else
            {
                byte[] bytesRecord = new byte[RecordLength];
                int offset = (int)file.Length - RecordLength;
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(bytesRecord, 0, bytesRecord.Length);
                this.recordId = BitConverter.ToInt32(bytesRecord, this.statusInBytes.Length) + 1;
            }

            var record = this.GetFileCabinetRecord(person, this.recordId);
            this.ConvertRecordToBytes(record);
            return this.recordId;
        }

        public void EditRecord(int id, Person person)
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToRecord(recordBytes);
            var record = this.GetFileCabinetRecord(person, id);
            this.ConvertRecordToBytes(record);
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
            byte[] recordBuffer = new byte[file.Length];
            file.Read(recordBuffer, 0, recordBuffer.Length);
            this.list = this.ConvertBytesToRecord(recordBuffer);
            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            ReadOnlyCollection<FileCabinetRecord> record = snapshot.Records;
            IList<FileCabinetRecord> recordFromFile = snapshot.RecordsFromFile;
            bool checkId = false;

            for (int i = 0; i < recordFromFile.Count; i++)
            {
                if (record.Count == 0)
                {
                    this.list.Add(recordFromFile[i]);
                }
                else
                {
                    for (int j = 0; j < record.Count; j++)
                    {
                        if (record[j].Id == recordFromFile[i].Id)
                        {
                            this.list[j] = recordFromFile[i];
                            checkId = true;
                        }
                        else if (!checkId)
                        {
                            recordFromFile[i].Id = this.list.Count + 1;
                            this.list.Add(recordFromFile[i]);
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
                this.list = this.ConvertBytesToRecord(recordBytes);
            }

            return this.list.AsReadOnly();
        }

        public int GetRecordsCount()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.ConvertBytesToRecord(recordBytes);
            }

            return this.list.Count;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.firstNameDictionary.Clear();
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToRecord(recordBytes);
            foreach (var item in this.list)
            {
                this.AddDitionaryItem(item.FirstName, item, this.firstNameDictionary);
            }

            List<FileCabinetRecord> firstNameList = this.firstNameDictionary[firstName];
            return firstNameList.AsReadOnly();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.lastNameDictionary.Clear();
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToRecord(recordBytes);
            foreach (var item in this.list)
            {
                this.AddDitionaryItem(item.LastName, item, this.lastNameDictionary);
            }

            List<FileCabinetRecord> firstNameList = this.lastNameDictionary[lastName];
            return firstNameList.AsReadOnly();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.dateOfBirthDictionary.Clear();
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToRecord(recordBytes);
            foreach (var item in this.list)
            {
                this.AddDitionaryItem(item.DateOfBirth.ToString(FormatDate), item, this.dateOfBirthDictionary);
            }

            List<FileCabinetRecord> firstNameList = this.dateOfBirthDictionary[dateOfBirth];
            return firstNameList.AsReadOnly();
        }

        public void AddDitionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            var keyStr = key.ToUpper(CultureInfo.InvariantCulture);
            if (!dictionary.ContainsKey(keyStr))
            {
                dictionary.Add(keyStr, new List<FileCabinetRecord>());
            }

            dictionary[keyStr].Add(record);
        }

        public void RemoveDitionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary)
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

        private void ConvertRecordToBytes(FileCabinetRecord record) // convert record to bytes
        {
            byte[] arrBytes = new byte[RecordLength];

            using MemoryStream memoryStream = new (arrBytes);
            using BinaryWriter binaryWriter = new (memoryStream);
            byte[] firstName = this.ConvertNameToBytes(record.FirstName);
            byte[] lastName = this.ConvertNameToBytes(record.LastName);

            using BinaryWriter binary = new (this.fileStream, Encoding.ASCII, true);
            if (this.list.Exists(i => i.Id == record.Id))
            {
                using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                int offset = (RecordLength * record.Id) - RecordLength;
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(this.statusInBytes, 0, this.statusInBytes.Length);
                this.status = BitConverter.ToInt16(this.statusInBytes, 0);
                file.Read(this.recordIdInBytes, 0, this.recordIdInBytes.Length);
                this.recordId = BitConverter.ToInt32(this.recordIdInBytes, 0);
                binary.Seek(offset, SeekOrigin.Begin);
            }
            else
            {
                this.status = 0;
                binary.Seek(0, SeekOrigin.End);
            }

            binary.Write(this.status);
            binary.Write(this.recordId);
            binary.Write(firstName);
            binary.Write(lastName);
            binary.Write(record.DateOfBirth.Year);
            binary.Write(record.DateOfBirth.Month);
            binary.Write(record.DateOfBirth.Day);
            binary.Write(record.Age);
            binary.Write(record.Salary);
            binary.Write(record.Symbol);
            binary.Flush();
        }

        private byte[] ConvertNameToBytes(string name) // convert FirstName and LastName to bytes
        {
            byte[] nameToBytes = Encoding.ASCII.GetBytes(name);
            byte[] names = new byte[MaxLengthString];
            int nameLengh = nameToBytes.Length;
            if (nameLengh > MaxLengthString)
            {
                nameLengh = MaxLengthString;
            }

            Array.Copy(nameToBytes, 0, names, 0, nameLengh);

            return names;
        }

        private List<FileCabinetRecord> ConvertBytesToRecord(byte[] recordBytes) // convert bytes to record
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
                byte[] firstNameBytes = reader.ReadBytes(MaxLengthString);
                string bytesToFirstName = Encoding.ASCII.GetString(firstNameBytes, 0, firstNameBytes.Length);
                record.FirstName = bytesToFirstName.Replace("\0", string.Empty);
                byte[] lastNameBytes = reader.ReadBytes(MaxLengthString);
                string bytesToLastName = Encoding.ASCII.GetString(lastNameBytes, 0, lastNameBytes.Length);
                record.LastName = bytesToLastName.Replace("\0", string.Empty);
                int dateOfYear = reader.ReadInt32();
                int dateOfMonth = reader.ReadInt32();
                int dateOfDay = reader.ReadInt32();
                record.DateOfBirth = new DateTime(dateOfYear, dateOfMonth, dateOfDay);
                record.Age = reader.ReadInt16();
                record.Salary = reader.ReadDecimal();
                record.Symbol = reader.ReadChar();

                records.Add(record);
            }

            return records;
        }
    }
}
