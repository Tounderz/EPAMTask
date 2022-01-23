using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class FilesystemIterator : IRecordIterator
    {
        private readonly FileCabinetFilesystemService filesystemService;
        private int index;
        private readonly int[] positions;

        public FilesystemIterator(FileCabinetFilesystemService filesystemService, IEnumerable<int> positions)
        {
            this.filesystemService = filesystemService;
            this.positions = positions.ToArray();
        }

        public FileCabinetRecord GetNext()
        {
            return this.filesystemService.ReadByPosition(this.positions[this.index++]);
        }

        public bool HasMore()
        {
            return this.index < this.positions.Length;
        }

        public int GetCount()
        {
            return this.positions.Length;
        }
    }
}
