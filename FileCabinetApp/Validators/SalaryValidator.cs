using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal minSalary;
        private readonly decimal maxSalary;

        public SalaryValidator(decimal minSalary, decimal maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
        }

        public Tuple<bool, string> ValidateParameters(Person person)
        {
            if (person.Salary < this.minSalary || person.Salary > this.maxSalary)
            {
                return new Tuple<bool, string>(false, $"The salary should not be less than {this.minSalary} or more than a {this.maxSalary}.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
