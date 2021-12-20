using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable CA1305

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

        public string ValidateParameters(FileCabinetRecord record)
        {
            string firstName = record.FirstName;
            string lastName = record.LastName;
            DateTime dateOfBirth = record.DateOfBirth;
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            string salary = record.Salary.ToString();
            string symbol = record.Symbol.ToString();
            string error = "Validation is successful.";

            if (firstName is null)
            {
                error = $"{nameof(firstName)} can't be null";
                return error;
            }

            if (firstName.Length < 2 || firstName.Length > 15 || !firstName.All(char.IsLetter))
            {
                error = $"The {nameof(firstName)} size is from 2 to 15 characters and there should be no spaces";
                return error;
            }

            if (lastName is null)
            {
                error = $"{nameof(lastName)} can't be null";
                return error;
            }

            if (lastName.Length < 2 || lastName.Length > 30 || !lastName.All(char.IsLetter))
            {
                error = $"The {nameof(lastName)} size is from 2 to 30 characters and there should be no spaces";
                return error;
            }

            if (dateOfBirth > new DateTime(2015, 12, 31) || dateOfBirth < new DateTime(1950, 01, 01))
            {
                error = $"{nameof(dateOfBirth)} - Minimum date is 01/01/1950, and the maximum is 12.31.2015.";
                return error;
            }

            if (symbol.Length != 1 || symbol.All(char.IsLetter))
            {
                error = $"The {nameof(symbol)} field must contain one character and must not be a letter.";
                return error;
            }

            for (int i = 0; i < salary.Length; i++)
            {
                if (char.IsLetter(salary[i]))
                {
                    error = $"The {nameof(salary)} parameter must contain only numeric value";
                    return error;
                }
            }

            return error;
        }
    }
}
