using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime of;

        public DateOfBirthValidator(DateTime from, DateTime of)
        {
            this.from = from;
            this.of = of;
        }

        public void ValidateParameters(Person person)
        {
            if (person.DateOfBirth > this.of || person.DateOfBirth < this.from)
            {
                throw new ArgumentException($"{nameof(person.DateOfBirth)} - Minimum date of birth {this.from:dd/MM/yyyy}, and maximum - {this.of:dd/MM/yyyy}.");
            }
        }
    }
}
