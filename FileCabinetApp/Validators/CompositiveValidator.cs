using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1643

namespace FileCabinetApp.Validators
{
    public class CompositiveValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> recordValidators;

        public CompositiveValidator(IEnumerable<IRecordValidator> recordValidators)
        {
            this.recordValidators = recordValidators.ToList();
        }

        public Tuple<bool, string> ValidateParameters(Person person)
        {
            string validatorErrorString = string.Empty;
            bool checkValidator = true;
            foreach (var item in this.recordValidators)
            {
                if (!item.ValidateParameters(person).Item1)
                {
                    validatorErrorString += $"Validation failed: {item.ValidateParameters(person).Item2}\n";
                    checkValidator = false;
                }
            }

            return new Tuple<bool, string>(checkValidator, validatorErrorString);
        }
    }
}
