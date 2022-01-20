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

        public Tuple<bool, string> ValidateParameters(Person person)
        {
            if (person.DateOfBirth > this.of || person.DateOfBirth < this.from)
            {
                return new Tuple<bool, string>(false, $"{nameof(person.DateOfBirth)} - Minimum date of birth {this.from:dd/MM/yyyy}, and maximum - {this.of:dd/MM/yyyy}.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
