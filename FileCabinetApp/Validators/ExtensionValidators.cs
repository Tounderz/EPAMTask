using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public static class ExtensionValidators
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return CreateValidator(ConstParameters.DefaultValidatorName);
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return CreateValidator(ConstParameters.CustomValidatorName);
        }

        private static IRecordValidator CreateValidator(string nameValidator)
        {
            ConfigurationReaderJson configurationReaderJson = new (nameValidator);
            (int firstNameMinLength, int firstNameMaxLength) = configurationReaderJson.FirstNameCreterios();
            (int lastNameMinLength, int lastNameMaxLength) = configurationReaderJson.LastNameCreterios();
            (DateTime from, DateTime of) = configurationReaderJson.DateOfBirthCriterions();
            (int salaryMin, int salaryMax) = configurationReaderJson.SalaryCreterios();
            int symbolLength = configurationReaderJson.SymbolCreterios();

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