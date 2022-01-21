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
        private readonly int minSalary;
        private readonly int maxSalary;

        public SalaryValidator(int minSalary, int maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
        }

        public void ValidateParameters(Person person)
        {
            if (person.Salary < this.minSalary || person.Salary > this.maxSalary)
            {
                throw new ArgumentException($"The salary should not be less than {this.minSalary} or more than a {this.maxSalary}.");
            }
        }
    }
}
