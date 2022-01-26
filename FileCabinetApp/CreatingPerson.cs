using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators;

#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp
{
    public static class CreatingPerson
    {
        private static int minFirstNameLength;
        private static int maxFirstNameLength;
        private static int minLastNameLength;
        private static int maxLastNameLength;
        private static int minSalary;
        private static int maxSalary;
        private static DateTime minDate;
        private static DateTime maxDate;
        private static int symbolLength;
        private static string parameter;

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> StringConverter(string str)
        {
            return new Tuple<bool, string, string>(true, str, str);
        }

        private static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            if (firstName.Length < minFirstNameLength || firstName.Length > maxFirstNameLength)
            {
                return new Tuple<bool, string>(false, $"The 'First Name' field size is from {minFirstNameLength} to {maxFirstNameLength} characters.");
            }
            else if (!firstName.All(char.IsLetter))
            {
                return new Tuple<bool, string>(false, $"The 'First Name' field should consist only of letters.");
            }
            else
            {
                return new Tuple<bool, string>(true, firstName);
            }
        }

        private static Tuple<bool, string> LastNameValidator(string lastName)
        {
            if (lastName.Length < minLastNameLength || lastName.Length > maxLastNameLength)
            {
                return new Tuple<bool, string>(false, $"The 'Last Name' field size is from {minLastNameLength} to {maxLastNameLength} characters.");
            }
            else if (!lastName.All(char.IsLetter))
            {
                return new Tuple<bool, string>(false, $"The 'Last Name' field should consist only of letters.");
            }
            else
            {
                return new Tuple<bool, string>(true, lastName);
            }
        }

        private static Tuple<bool, string, DateTime> DateConverter(string dateOfBirth)
        {
            try
            {
                DateTime value = Convert.ToDateTime(dateOfBirth.Replace("/", "."));
                return new Tuple<bool, string, DateTime>(true, string.Empty, value);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, DateTime>(false, "Incorrect date format", default);
            }
        }

        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            if (dateOfBirth < minDate || dateOfBirth > maxDate)
            {
                return new Tuple<bool, string>(false, $"The 'Date Of Birth' field - Minimum date of birth {minDate:dd/MM/yyyy}, and maximum - {maxDate:dd/MM/yyyy}.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string salary)
        {
            try
            {
                decimal value = decimal.Parse(salary.Replace(".", ","));
                return new Tuple<bool, string, decimal>(true, string.Empty, value);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, decimal>(false, "Only numbers and a comma or dot, to separate the fractional part", 0);
            }
        }

        private static Tuple<bool, string> SalaryValidator(decimal salary)
        {
            if (salary < minSalary || salary > maxSalary)
            {
                return new Tuple<bool, string>(false, $"The 'Salary' field should not be less than {minSalary} or more than a {maxSalary}.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private static Tuple<bool, string, char> CharConverter(string symbol)
        {
            if (symbol.Length != symbolLength || string.IsNullOrWhiteSpace(symbol))
            {
                return new Tuple<bool, string, char>(false, "It cannot be empty and more than one character.", ' ');
            }

            return new Tuple<bool, string, char>(true, string.Empty, char.Parse(symbol));
        }

        private static Tuple<bool, string> SymbolValidator(char symbol)
        {
            if (symbol.ToString().Length != symbolLength)
            {
                return new Tuple<bool, string>(false, $"The 'Symbol' field size of the string character is equal to one element.");
            }
            else if (char.IsDigit(symbol) || char.IsLetter(symbol))
            {
                return new Tuple<bool, string>(false, $"The 'Symbol' field should not contain letters or numbers.");
            }
            else
            {
                return new Tuple<bool, string>(true, null);
            }
        }

        private static T ReadInputInsert<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = parameter;
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    return default;
                }
                else
                {
                    value = conversionResult.Item3;
                }

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    return default;
                }
                else
                {
                    return value;
                }
            }
            while (true);
        }

        public static Person NewPersonInsert(string nameValidator, string firstName, string lastName, string dateOfBirth, string salary, string symbol)
        {
            ValidatorParameters(nameValidator);
            parameter = firstName;
            firstName = ReadInputInsert(StringConverter, FirstNameValidator);
            parameter = lastName;
            lastName = ReadInputInsert(StringConverter, LastNameValidator);
            parameter = dateOfBirth;
            DateTime date = ReadInputInsert(DateConverter, DateOfBirthValidator);
            short age = Convert.ToInt16(DateTime.Now.Year - date.Year);
            parameter = salary;
            decimal sal = ReadInputInsert(DecimalConverter, SalaryValidator);
            parameter = symbol;
            char symb = ReadInputInsert(CharConverter, SymbolValidator);

            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = date,
                Age = age,
                Salary = sal,
                Symbol = symb,
            };

            return person;
        }

        public static Person NewPerson(string nameValidator)
        {
            ValidatorParameters(nameValidator);
            Console.Write($"First name: ");
            string firstName = ReadInput(StringConverter, FirstNameValidator);
            Console.Write($"Last name: ");
            string lastName = ReadInput(StringConverter, LastNameValidator);
            Console.Write("Date of birth: ");
            DateTime dateOfBirth = ReadInput(DateConverter, DateOfBirthValidator);
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            Console.Write("Salary: ");
            decimal salary = ReadInput(DecimalConverter, SalaryValidator);
            Console.Write("Any character: ");
            char symbol = ReadInput(CharConverter, SymbolValidator);

            var person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Symbol = symbol,
            };

            return person;
        }

        private static void ValidatorParameters(string nameValidator)
        {
            ConfigurationReaderJson configurationReaderJson = new (nameValidator);
            (minFirstNameLength, maxFirstNameLength) = configurationReaderJson.FirstNameCreterios();
            (minLastNameLength, maxLastNameLength) = configurationReaderJson.LastNameCreterios();
            (minSalary, maxSalary) = configurationReaderJson.SalaryCreterios();
            (minDate, maxDate) = configurationReaderJson.DateOfBirthCriterions();
            symbolLength = configurationReaderJson.SymbolCreterios();
        }
    }
}