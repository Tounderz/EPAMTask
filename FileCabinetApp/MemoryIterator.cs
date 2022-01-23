using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class MemoryIterator : IRecordIterator
    {
        private readonly FileCabinetRecord[] fileCabinetRecords;
        private int index;

        public MemoryIterator(IEnumerable<FileCabinetRecord> fileCabinetRecords)
        {
            this.fileCabinetRecords = fileCabinetRecords.ToArray();
        }

        public FileCabinetRecord GetNext()
        {
            return this.fileCabinetRecords[this.index++];
        }

        public bool HasMore()
        {
            return this.index < this.fileCabinetRecords.Length;
        }

        public int GetCount()
        {
            return this.fileCabinetRecords.Length;
        }
    }
}
