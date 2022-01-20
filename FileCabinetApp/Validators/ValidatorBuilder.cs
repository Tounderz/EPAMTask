using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> recordValidators = new ();

        public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
        {
            this.recordValidators.Add(new FirstNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
        {
            this.recordValidators.Add(new LastNameValidator(minLength, maxLength));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime datetimeFrom, DateTime datetimeTo)
        {
            this.recordValidators.Add(new DateOfBirthValidator(datetimeFrom, datetimeTo));
            return this;
        }

        public ValidatorBuilder ValidateSalary(decimal minSalary, decimal maxSalary)
        {
            this.recordValidators.Add(new SalaryValidator(minSalary, maxSalary));
            return this;
        }

        public ValidatorBuilder ValidateSymbol(int symbol)
        {
            this.recordValidators.Add(new SymbolValidator(symbol));
            return this;
        }

        public CompositiveValidator Create()
        {
            return new CompositiveValidator(this.recordValidators);
        }
    }
}
