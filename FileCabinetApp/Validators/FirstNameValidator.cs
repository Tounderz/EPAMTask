using System;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

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

        public void ValidateParameters(PersonModel person)
        {
            if (person.FirstName.Length < this.minLength || person.FirstName.Length > this.maxLength)
            {
                throw new ArgumentException($"The {nameof(person.FirstName)} size is from {this.minLength} to {this.maxLength} characters.");
            }
        }
    }
}
