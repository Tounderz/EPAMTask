using System;
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
#pragma warning disable SA1515
#pragma warning disable S125

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private List<FileCabinetRecord> list = new ();
        private readonly List<FileCabinetRecord> isDeleteRecords = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private int recordId = 0;
        private short status = 0;
        private readonly byte[] statusInBytes = new byte[sizeof(short)];
        private readonly byte[] recordIdInBytes = new byte[sizeof(int)];

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
                byte[] recordBytes = new byte[ConstParameters.RecordLength];
                int offset = (int)file.Length - ConstParameters.RecordLength;
                file.Seek(offset, SeekOrigin.Begin);
                file.Read(recordBytes, 0, recordBytes.Length);
                this.recordId = BitConverter.ToInt32(recordBytes, this.statusInBytes.Length) + 1;
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
            FileCabinetRecord record = this.list.Find(i => i.Id == id);
            record = this.GetFileCabinetRecord(person, record.Id);
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
            this.list = this.ConvertBytesToRecord(recordBytes);
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
                this.list = this.ConvertBytesToRecord(recordBytes);
            }

            return this.list.AsReadOnly();
        }

        public Tuple<int, int> GetRecordsCount()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.ConvertBytesToRecord(recordBytes);
            }

            return Tuple.Create(this.list.Count, this.isDeleteRecords.Count);
        }

        public Tuple<int, int> PurgeRecord()
        {
            using var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] recordBytes = new byte[file.Length];
            file.Read(recordBytes, 0, recordBytes.Length);
            this.list = this.ConvertBytesToRecord(recordBytes);

            this.fileStream.SetLength(0);

            using BinaryWriter binaryWriter = new (this.fileStream, Encoding.ASCII, true);
            for (int i = 0; i < this.list.Count; i++)
            {
                this.status = 0;
                byte[] firstName = this.ConvertNameToBytes(this.list[i].FirstName);
                byte[] lastName = this.ConvertNameToBytes(this.list[i].LastName);
                var record = this.list[i];
                this.GetBinaryWriter(binaryWriter, this.status, record.Id, firstName, lastName, record);
                // binaryWriter.Write(this.status);
                // binaryWriter.Write(record.Id);
                // binaryWriter.Write(firstName);
                // binaryWriter.Write(lastName);
                // binaryWriter.Write(record.DateOfBirth.Year);
                // binaryWriter.Write(record.DateOfBirth.Month);
                // binaryWriter.Write(record.DateOfBirth.Day);
                // binaryWriter.Write(record.Age);
                // binaryWriter.Write(record.Salary);
                // binaryWriter.Write(record.Symbol);
                binaryWriter.Flush();
            }

            int count = this.isDeleteRecords.Count;
            this.isDeleteRecords.Clear();

            return Tuple.Create(count, this.list.Count + count);
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
                this.AddDitionaryItem(item.DateOfBirth.ToString(ConstParameters.FormatDate), item, this.dateOfBirthDictionary);
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
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            byte[] arrBytes = new byte[ConstParameters.RecordLength];

            using MemoryStream memoryStream = new (arrBytes);
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
            // binary.Write(this.status);
            // binary.Write(this.recordId);
            // binary.Write(firstName);
            // binary.Write(lastName);
            // binary.Write(record.DateOfBirth.Year);
            // binary.Write(record.DateOfBirth.Month);
            // binary.Write(record.DateOfBirth.Day);
            // binary.Write(record.Age);
            // binary.Write(record.Salary);
            // binary.Write(record.Symbol);
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
