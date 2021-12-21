using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public interface IRecordValidator
    {
        public Tuple<bool, string> ValidateFirstName(string firstName);

        public Tuple<bool, string> ValidateLastName(string lastName);

        public Tuple<bool, string> ValidateDateOfBirth(DateTime dateOfBirth);

        public Tuple<bool, string> ValidateSalary(decimal salary);

        public Tuple<bool, string> ValidateSymbol(char symbol);
    }
}
