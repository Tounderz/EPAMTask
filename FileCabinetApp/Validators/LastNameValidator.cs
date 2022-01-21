using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public void ValidateParameters(Person person)
        {
            if (person.LastName.Length < this.minLength || person.LastName.Length > this.maxLength)
            {
                throw new ArgumentException($"The {nameof(person.LastName)} size is from {this.minLength} to {this.maxLength} characters.");
            }
        }
    }
}
