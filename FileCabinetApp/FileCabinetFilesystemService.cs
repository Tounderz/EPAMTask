using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

#pragma warning disable SA1600
#pragma warning disable SA1203
#pragma warning disable S1450
#pragma warning disable CA1822

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
                byte[] statusInBytes = new byte[sizeof(short)];
                this.recordId = BitConverter.ToInt32(bytesRecord, statusInBytes.Length) + 1;
            }

            var record = new FileCabinetRecord
            {
                Id = this.recordId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Age = person.Age,
                Salary = person.Salary,
                Symbol = person.Symbol,
            };

            this.GetRecordToBytes(record);
            return this.recordId;
        }

        public void EditRecord(int id, Person person)
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.GetBytesToRecord(recordBytes);
            }

            return this.list.AsReadOnly();
        }

        public int GetRecordsCount()
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] recordBytes = new byte[file.Length];
                file.Read(recordBytes, 0, recordBytes.Length);
                this.list = this.GetBytesToRecord(recordBytes);
            }

            return this.list.Count;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public void AddDitionaryItem(string key, FileCabinetRecord record, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            throw new NotImplementedException();
        }

        public void RemoveDitionaryItem(int id, Dictionary<string, List<FileCabinetRecord>> dictionary)
        {
            throw new NotImplementedException();
        }

        private void GetRecordToBytes(FileCabinetRecord record) // convert record to bytes
        {
            byte[] arrBytes = new byte[RecordLength];

            using MemoryStream memoryStream = new (arrBytes);
            using BinaryWriter binaryWriter = new (memoryStream);
            byte[] firstName = this.GetNameBytes(record.FirstName);
            byte[] lastName = this.GetNameBytes(record.LastName);

            using BinaryWriter binary = new (this.fileStream, Encoding.ASCII, true);
            binary.Seek(0, SeekOrigin.End);
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

        private byte[] GetNameBytes(string name) // convert FirstName and LastName to bytes
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

        private List<FileCabinetRecord> GetBytesToRecord(byte[] recordBytes) // convert bytes to record
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
