using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class DefaultValidator : IRecordValidator
    {
        private readonly DateTime minDate = new (1950, 01, 01);
        private readonly DateTime maxDate = DateTime.Now;
        private readonly int minString = 2;
        private readonly int maxString = 60;
        private readonly decimal minSalary = 0;
        private readonly decimal maxSalary = decimal.MaxValue;
        private readonly int syzeSymbol = 1;

        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName.Length < this.minString || firstName.Length > this.maxString)
            {
                return new Tuple<bool, string>(false, $"The {nameof(firstName)} size is from 2 to 60 characters.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName.Length < this.minString || lastName.Length > this.maxString)
            {
                return new Tuple<bool, string>(false, $"The {nameof(lastName)} size is from 2 to 60 characters.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > this.maxDate || dateOfBirth < this.minDate)
            {
                return new Tuple<bool, string>(false, $"{nameof(dateOfBirth)} - Minimum date of birth 01/01/1950, and maximum - DateTime.Now.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSalary(decimal salary)
        {
            if (salary < this.minSalary || salary > this.maxSalary)
            {
                return new Tuple<bool, string>(false, $"The salary should not be less than 0");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSymbol(char symbol)
        {
            if (symbol.ToString().Length != this.syzeSymbol)
            {
                return new Tuple<bool, string>(false, $"The {nameof(symbol)} size of the string character is equal to one element");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
