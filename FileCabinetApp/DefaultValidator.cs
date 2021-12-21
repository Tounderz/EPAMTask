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
        public Tuple<bool, string> ValidateFirstName(string firstName)
        {
            if (firstName.Length < 2 || firstName.Length > 60)
            {
                return new Tuple<bool, string>(false, $"The {nameof(firstName)} size is from 2 to 60 characters.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateLastName(string lastName)
        {
            if (lastName.Length < 2 || lastName.Length > 60)
            {
                return new Tuple<bool, string>(false, $"The {nameof(lastName)} size is from 2 to 60 characters.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                return new Tuple<bool, string>(false, $"{nameof(dateOfBirth)} - Minimum date of birth 01/01/1950, and maximum - DateTime.Now.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSalary(decimal salary)
        {
            if (salary < 0 || salary > decimal.MaxValue)
            {
                return new Tuple<bool, string>(false, $"The salary should not be less than 0");
            }

            return new Tuple<bool, string>(true, null);
        }

        public Tuple<bool, string> ValidateSymbol(char symbol)
        {
            if (symbol.ToString().Length != 1)
            {
                return new Tuple<bool, string>(false, $"The {nameof(symbol)} size of the string character is equal to one element");
            }

            return new Tuple<bool, string>(true, null);
        }
    }
}
