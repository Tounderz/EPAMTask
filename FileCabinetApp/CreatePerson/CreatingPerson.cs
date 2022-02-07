using System;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Models;

#pragma warning disable SA1600
#pragma warning disable SA1214

namespace FileCabinetApp.CreatePerson
{
    public class CreatingPerson
    {
        private string parameter;
        private readonly InputValidation inputValidation;
        private readonly Converters converters;

        public CreatingPerson(string nameValidator)
        {
            this.inputValidation = new InputValidation(nameValidator);
            this.converters = new Converters();
        }

        public PersonModel AddPersonInsertAndUpdate(string firstName, string lastName, string dateOfBirth, string salary, string symbol)
        {
            this.parameter = firstName;
            firstName = this.ReadInputInsertAndUpdate(this.converters.StringConverter, this.inputValidation.FirstNameValidator);
            this.parameter = lastName;
            lastName = this.ReadInputInsertAndUpdate(this.converters.StringConverter, this.inputValidation.LastNameValidator);
            this.parameter = dateOfBirth;
            DateTime date = this.ReadInputInsertAndUpdate(this.converters.DateConverter, this.inputValidation.DateOfBirthValidator);
            short age = Convert.ToInt16(DateTime.Now.Year - date.Year);
            this.parameter = salary;
            decimal sal = this.ReadInputInsertAndUpdate(this.converters.DecimalConverter, this.inputValidation.SalaryValidator);
            this.parameter = symbol;
            char symb = this.ReadInputInsertAndUpdate(this.converters.CharConverter, this.inputValidation.SymbolValidator);

            var person = new PersonModel
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

        public PersonModel AddPerson()
        {
            Console.Write($"{ColumnNames.ColumnFirstName}: ");
            string firstName = ReadInput(this.converters.StringConverter, this.inputValidation.FirstNameValidator);
            Console.Write($"{ColumnNames.ColumnLastName}: ");
            string lastName = ReadInput(this.converters.StringConverter, this.inputValidation.LastNameValidator);
            Console.Write($"{ColumnNames.ColumnDateOfBirth}: ");
            DateTime dateOfBirth = ReadInput(this.converters.DateConverter, this.inputValidation.DateOfBirthValidator);
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            Console.Write($"{ColumnNames.ColumnSalary}: ");
            decimal salary = ReadInput(this.converters.DecimalConverter, this.inputValidation.SalaryValidator);
            Console.Write($"{ColumnNames.ColumnSymbol}: ");
            char symbol = ReadInput(this.converters.CharConverter, this.inputValidation.SymbolValidator);

            var person = new PersonModel
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

        private static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
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

        private T ReadInputInsertAndUpdate<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = this.parameter;
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
    }
}