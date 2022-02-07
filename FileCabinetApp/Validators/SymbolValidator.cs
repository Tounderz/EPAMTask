using System;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class SymbolValidator : IRecordValidator
    {
        private readonly int syzeSymbol;

        public SymbolValidator(int syzeSymbol)
        {
            this.syzeSymbol = syzeSymbol;
        }

        public void ValidateParameters(PersonModel person)
        {
            if (person.Symbol.ToString().Length != this.syzeSymbol || char.IsDigit(person.Symbol) || char.IsLetter(person.Symbol))
            {
                throw new ArgumentException($"The {nameof(person.Symbol)} field should not contain letters or numbers.");
            }
        }
    }
}
