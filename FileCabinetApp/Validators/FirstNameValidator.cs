using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public Tuple<bool, string> ValidateParameters(Person person)
        {
            if (person.FirstName.Length < this.minLength || person.FirstName.Length > this.maxLength)
            {
                return new Tuple<bool, string>(false, $"The {nameof(person.FirstName)} size is from {this.minLength} to {this.maxLength} characters.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
