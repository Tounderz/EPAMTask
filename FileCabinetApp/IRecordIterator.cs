using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public interface IRecordIterator
    {
        FileCabinetRecord GetNext();

        bool HasMore();

        int GetCount();
    }
}
