﻿using System;
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
        private short status = 0;
        private List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private int recordId = 0;
        private const int MaxLengthString = 120;
        private const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord(Person person)
        {
            using (FileStream file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (file.Length <= 0)
                {
                    this.recordId++;
                }
                else
                {
                    byte[] recordBytes = new byte[RecordLength];
                    int offset = (int)file.Length - RecordLength;
                    file.Seek(offset, SeekOrigin.Begin);
                    file.Read(recordBytes, 0, recordBytes.Length);
                    byte[] status = new byte[sizeof(short)];
                    this.recordId = BitConverter.ToInt32(recordBytes, status.Length) + 1;
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

                this.RecordToBytes(record);
                return this.recordId;
            }
        }

        public void EditRecord(int id, Person person)
        {
            throw new NotImplementedException();
        }

        private void RecordToBytes(FileCabinetRecord record) // convert record to bytes
        {
            byte[] arrBytes = new byte[RecordLength];

            using (MemoryStream memoryStream = new (arrBytes))
            using (BinaryWriter binaryWriter = new (memoryStream))
            {
                byte[] bytesToFirstName = Encoding.ASCII.GetBytes(record.FirstName);
                int firstNameLengh = bytesToFirstName.Length;
                if (firstNameLengh > MaxLengthString)
                {
                    firstNameLengh = MaxLengthString;
                }

                byte[] firstName = new byte[MaxLengthString * 2];
                Array.Copy(bytesToFirstName, 0, firstName, 0, firstNameLengh);

                byte[] bytesToLastName = Encoding.ASCII.GetBytes(record.LastName);
                int lastNameLengh = bytesToFirstName.Length;
                if (lastNameLengh > MaxLengthString)
                {
                    lastNameLengh = MaxLengthString;
                }

                byte[] lastName = new byte[MaxLengthString * 2];
                Array.Copy(bytesToLastName, 0, lastName, 0, lastNameLengh);

                using (BinaryWriter binary = new (this.fileStream, Encoding.ASCII, true))
                {
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
            }
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetRecordsCount()
        {
            throw new NotImplementedException();
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
    }
}
