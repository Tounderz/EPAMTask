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
            return new ValidatorBuilder()
                .ValidateFirstName(ConstParameters.MinStringLength, ConstParameters.MaxStringDefaultLength)
                .ValidateLastName(ConstParameters.MinStringLength, ConstParameters.MaxStringDefaultLength)
                .ValidateDateOfBirth(ConstParameters.MinDateDafault, ConstParameters.MaxDateDafault)
                .ValidateSalary(ConstParameters.MinSalaryDefault, ConstParameters.MaxSalaryDefault)
                .ValidateSymbol(ConstParameters.SymbolLength)
                .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(ConstParameters.MinStringLength, ConstParameters.MaxStringCustomLength)
                .ValidateLastName(ConstParameters.MinStringLength, ConstParameters.MaxStringCustomLength)
                .ValidateDateOfBirth(ConstParameters.MinDateCustom, ConstParameters.MaxDateCustom)
                .ValidateSalary(ConstParameters.MinSalaryCustom, ConstParameters.MaxSalaryCustom)
                .ValidateSymbol(ConstParameters.SymbolLength)
                .Create();
        }
    }
}
