using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class CustomValidator : IRecordValidator
    {
        private readonly DateTime minDate = new (1950, 01, 01);
        private readonly DateTime maxDate = new (2015, 12, 31);
        private readonly int minString = 2;
        private readonly int maxFirstName = 15;
        private readonly int maxLastName = 30;
        private readonly decimal minSalary = 500;
        private readonly decimal maxSalary = 1000000;
        private readonly int syzeSymbol = 1;

        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName.Length < this.minString || firstName.Length > this.maxFirstName || !firstName.All(char.IsLetter))
            {
                return new Tuple<bool, string>(false, $"The {nameof(firstName)} size is from 2 to 15 characters and there should be no spaces");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName.Length < this.minString || lastName.Length > this.maxLastName)
            {
                return new Tuple<bool, string>(false, $"The {nameof(lastName)} size is from 2 to 30 characters and there should be no spaces");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > this.maxDate || dateOfBirth < this.minDate)
            {
                return new Tuple<bool, string>(false, $"{nameof(dateOfBirth)} - Minimum date is 01/01/1950, and the maximum is 12.31.2015.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSalary(decimal salary)
        {
            if (salary < this.minSalary || salary > this.maxSalary)
            {
                return new Tuple<bool, string>(false, $"The salary should not be less than 500 or more than a million");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSymbol(char symbol)
        {
            if (symbol.ToString().Length != this.syzeSymbol || char.IsLetter(symbol))
            {
                return new Tuple<bool, string>(false, $"The {nameof(symbol)} field must contain one character and must not be a letter.");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
