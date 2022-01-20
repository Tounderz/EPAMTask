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
        private static readonly DateTime MinDate = new (1950, 01, 01);
        private static readonly DateTime MaxDateDafault = DateTime.Now;
        private static readonly DateTime MaxDateCustom = new (2015, 12, 31);

        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(ConstParameters.MinString, ConstParameters.MaxStringDefault)
                .ValidateLastName(ConstParameters.MinString, ConstParameters.MaxStringDefault)
                .ValidateDateOfBirth(MinDate, MaxDateDafault)
                .ValidateSalary(ConstParameters.MinSalaryDefault, ConstParameters.MaxSalaryDefault)
                .ValidateSymbol(ConstParameters.SyzeSymbol)
                .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(ConstParameters.MinString, ConstParameters.MaxFirstNameCustom)
                .ValidateLastName(ConstParameters.MinString, ConstParameters.MaxLastNameCustom)
                .ValidateDateOfBirth(MinDate, MaxDateCustom)
                .ValidateSalary(ConstParameters.MinSalaryCustom, ConstParameters.MaxSalaryCustom)
                .ValidateSymbol(ConstParameters.SyzeSymbol)
                .Create();
        }
    }
}
