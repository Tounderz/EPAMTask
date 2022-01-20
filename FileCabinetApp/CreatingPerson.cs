﻿using System;
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
            if (string.IsNullOrEmpty(firstName))
            {
                return new Tuple<bool, string>(false, $"The 'First Name' field should not be empty.");
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
            if (string.IsNullOrEmpty(lastName))
            {
                return new Tuple<bool, string>(false, $"The 'Last Name' field should not be empty.");
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
            if (dateOfBirth == DateTime.MinValue)
            {
                return new Tuple<bool, string>(false, $"Incorrect field 'Date of birth' data.");
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
            if (salary < 0)
            {
                return new Tuple<bool, string>(false, "The 'Salary' field cannot be less than 0.");
            }
            else
            {
                return new Tuple<bool, string>(true, string.Empty);
            }
        }

        private static Tuple<bool, string, char> CharConverter(string symbol)
        {
            if (symbol.Length != 1 || string.IsNullOrWhiteSpace(symbol))
            {
                return new Tuple<bool, string, char>(false, "It cannot be empty and more than one character.", ' ');
            }

            return new Tuple<bool, string, char>(true, string.Empty, char.Parse(symbol));
        }

        private static Tuple<bool, string> SymbolValidator(char symbol)
        {
            if (symbol.ToString().Length != ConstParameters.SyzeSymbol)
            {
                return new Tuple<bool, string>(false, $"The {nameof(symbol)} size of the string character is equal to one element.");
            }

            return new Tuple<bool, string>(true, null);
        }

        public static Person NewPerson()
        {
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
    }
}