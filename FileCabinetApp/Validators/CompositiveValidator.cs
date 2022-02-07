using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public class CompositiveValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> recordValidators;

        public CompositiveValidator(IEnumerable<IRecordValidator> recordValidators)
        {
            this.recordValidators = recordValidators.ToList();
        }

        public void ValidateParameters(PersonModel person)
        {
            foreach (var item in this.recordValidators)
            {
                item.ValidateParameters(person);
            }
        }
    }
}
