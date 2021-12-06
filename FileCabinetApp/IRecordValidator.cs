using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public string FirstName();

        public string LastName();

        public DateTime DateOfBirth();

        public decimal Salary();

        public char Symbol();

        public FileCabinetRecord AddRecord(int id);
    }
}
