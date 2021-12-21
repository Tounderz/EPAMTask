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
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName.Length < 2 || firstName.Length > 15 || !firstName.All(char.IsLetter))
            {
                return new Tuple<bool, string>(false, $"The {nameof(firstName)} size is from 2 to 15 characters and there should be no spaces");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName.Length < 2 || lastName.Length > 30)
            {
                return new Tuple<bool, string>(false, $"The {nameof(lastName)} size is from 2 to 30 characters and there should be no spaces");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > new DateTime(2015, 12, 31) || dateOfBirth < new DateTime(1950, 01, 01))
            {
                return new Tuple<bool, string>(false, $"{nameof(dateOfBirth)} - Minimum date is 01/01/1950, and the maximum is 12.31.2015.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSalary(decimal salary)
        {
            if (salary < 500 || salary > 1000000)
            {
                return new Tuple<bool, string>(false, $"The salary should not be less than 500 or more than a million");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSymbol(char symbol)
        {
            if (symbol.ToString().Length != 1 || char.IsLetter(symbol))
            {
                return new Tuple<bool, string>(false, $"The {nameof(symbol)} field must contain one character and must not be a letter.");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
