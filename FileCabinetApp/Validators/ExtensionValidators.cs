using System;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Reader;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public static class ExtensionValidators
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return CreateValidator(CommandLineParameters.DefaultValidatorName);
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return CreateValidator(CommandLineParameters.CustomValidatorName);
        }

        private static IRecordValidator CreateValidator(string nameValidator)
        {
            ConfigurationReaderJson configurationReaderJson = new (nameValidator);
            (int firstNameMinLength, int firstNameMaxLength) = configurationReaderJson.FirstNameCriteria();
            (int lastNameMinLength, int lastNameMaxLength) = configurationReaderJson.LastNameCriteria();
            (DateTime from, DateTime of) = configurationReaderJson.DateOfBirthCriteria();
            (int salaryMin, int salaryMax) = configurationReaderJson.SalaryCriteria();
            int symbolLength = configurationReaderJson.SymbolCriteria();

            return new ValidatorBuilder()
                .ValidateFirstName(firstNameMinLength, firstNameMaxLength)
                .ValidateLastName(lastNameMinLength, lastNameMaxLength)
                .ValidateDateOfBirth(from, of)
                .ValidateSalary(salaryMin, salaryMax)
                .ValidateSymbol(symbolLength)
                .Create();
        }
    }
}