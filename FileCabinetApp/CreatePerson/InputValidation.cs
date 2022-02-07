using System;
using System.Linq;
using FileCabinetApp.Reader;

#pragma warning disable SA1600

namespace FileCabinetApp.CreatePerson
{
    public class InputValidation
    {
        private readonly int minFirstNameLength;
        private readonly int maxFirstNameLength;
        private readonly int minLastNameLength;
        private readonly int maxLastNameLength;
        private readonly int minSalary;
        private readonly int maxSalary;
        private readonly DateTime minDate;
        private readonly DateTime maxDate;
        private readonly int symbolLength;

        public InputValidation(string nameValidator)
        {
            ConfigurationReaderJson configurationReaderJson = new (nameValidator);
            (this.minFirstNameLength, this.maxFirstNameLength) = configurationReaderJson.FirstNameCriteria();
            (this.minLastNameLength, this.maxLastNameLength) = configurationReaderJson.LastNameCriteria();
            (this.minSalary, this.maxSalary) = configurationReaderJson.SalaryCriteria();
            (this.minDate, this.maxDate) = configurationReaderJson.DateOfBirthCriteria();
            this.symbolLength = configurationReaderJson.SymbolCriteria();
        }

        public ValueTuple<bool, string> FirstNameValidator(string firstName)
        {
            if (firstName.Length < this.minFirstNameLength || firstName.Length > this.maxFirstNameLength)
            {
                return new ValueTuple<bool, string>(false, $"The 'First Name' field size is from {this.minFirstNameLength} to {this.maxFirstNameLength} characters.");
            }
            else if (!firstName.All(char.IsLetter))
            {
                return new ValueTuple<bool, string>(false, $"The 'First Name' field should consist only of letters.");
            }
            else
            {
                return new ValueTuple<bool, string>(true, firstName);
            }
        }

        public ValueTuple<bool, string> LastNameValidator(string lastName)
        {
            if (lastName.Length < this.minLastNameLength || lastName.Length > this.maxLastNameLength)
            {
                return new ValueTuple<bool, string>(false, $"The 'Last Name' field size is from {this.minLastNameLength} to {this.maxLastNameLength} characters.");
            }
            else if (!lastName.All(char.IsLetter))
            {
                return new ValueTuple<bool, string>(false, $"The 'Last Name' field should consist only of letters.");
            }
            else
            {
                return new ValueTuple<bool, string>(true, lastName);
            }
        }

        public ValueTuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < this.minDate || dateOfBirth > this.maxDate)
            {
                return new ValueTuple<bool, string>(false, $"The 'Date Of Birth' field - Minimum date of birth {this.minDate:dd/MM/yyyy}, and maximum - {this.maxDate:dd/MM/yyyy}.");
            }

            return new ValueTuple<bool, string>(true, string.Empty);
        }

        public ValueTuple<bool, string> SalaryValidator(decimal salary)
        {
            if (salary < this.minSalary || salary > this.maxSalary)
            {
                return new ValueTuple<bool, string>(false, $"The 'Salary' field should not be less than {this.minSalary} or more than a {this.maxSalary}.");
            }

            return new ValueTuple<bool, string>(true, string.Empty);
        }

        public ValueTuple<bool, string> SymbolValidator(char symbol)
        {
            if (symbol.ToString().Length != this.symbolLength)
            {
                return new ValueTuple<bool, string>(false, $"The 'Symbol' field size of the string character is equal to one element.");
            }
            else if (char.IsDigit(symbol) || char.IsLetter(symbol))
            {
                return new ValueTuple<bool, string>(false, $"The 'Symbol' field should not contain letters or numbers.");
            }
            else
            {
                return new ValueTuple<bool, string>(true, null);
            }
        }
    }
}
