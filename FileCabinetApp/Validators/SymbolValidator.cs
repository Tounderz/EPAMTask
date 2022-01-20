using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Tuple<bool, string> ValidateParameters(Person person)
        {
            if (person.Symbol.ToString().Length != this.syzeSymbol || char.IsDigit(person.Symbol) || char.IsLetter(person.Symbol))
            {
                return new Tuple<bool, string>(false, $"The {nameof(person.Symbol)} field should not contain letters or numbers.");
            }

            return new Tuple<bool, string>(true, string.Empty);
        }
    }
}
