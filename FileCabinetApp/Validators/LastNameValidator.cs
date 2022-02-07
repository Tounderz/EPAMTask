using System;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

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

        public void ValidateParameters(PersonModel person)
        {
            if (person.LastName.Length < this.minLength || person.LastName.Length > this.maxLength)
            {
                throw new ArgumentException($"The {nameof(person.LastName)} size is from {this.minLength} to {this.maxLength} characters.");
            }
        }
    }
}
