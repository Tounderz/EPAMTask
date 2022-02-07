using System;

#pragma warning disable SA1600

namespace FileCabinetApp.Models
{
    public class PersonModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Age { get; set; }

        public decimal Salary { get; set; }

        public char Symbol { get; set; }
    }
}
