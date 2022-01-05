using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Age { get; set; }

        public decimal Salary { get; set; }

        public char Symbol { get; set; }
    }
}
