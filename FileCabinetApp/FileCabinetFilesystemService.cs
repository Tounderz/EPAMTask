using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly FileStream fileStream;
        private int recordId = 0;
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord(Person person)
        {
            using (var file = File.Open(this.fileStream.Name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (file.Length <= 0)
                {
                    this.recordId++;
                }
                else
                {
                    byte[] recordByte = new byte[file.Length];
                    file.Read(recordByte, 0, recordByte.Length);
                    //this.list = 
                }
            }

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

        //private List<FileCabinetRecord> RecordToBytes(byte[] arrBytes)
        //{
        //    if (arrBytes == null)
        //    {
        //        throw new ArgumentNullException(nameof(arrBytes));
        //    }

        //    List<FileCabinetRecord> records = new ();
        //    using (var memoryStram = new MemoryStream(arrBytes))
        //    {
        //        using (var binReader = new BinaryReader(memoryStram))
        //        {
        //            while (binReader.BaseStream.Position < binReader.BaseStream.Length)
        //            {
        //                FileCabinetRecord record = new ();
        //                record.Id = binReader.ReadInt32();

        //                record.Age = binReader.ReadInt16();
        //                record.Salary = binReader.ReadDecimal();
        //                record.Symbol = binReader.ReadChar();
        //            }
        //        }
        //    }
        //}
    }
}
