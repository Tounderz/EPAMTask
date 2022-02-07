using System;

#pragma warning disable CA1822
#pragma warning disable SA1600

namespace FileCabinetApp.CreatePerson
{
    public class Converters
    {
        public ValueTuple<bool, string, string> StringConverter(string str)
        {
            return new ValueTuple<bool, string, string>(true, str, str);
        }

        public ValueTuple<bool, string, DateTime> DateConverter(string dateOfBirth)
        {
            try
            {
                DateTime value = Convert.ToDateTime(dateOfBirth.Replace("/", "."));
                return new ValueTuple<bool, string, DateTime>(true, string.Empty, value);
            }
            catch (Exception)
            {
                return new ValueTuple<bool, string, DateTime>(false, "Incorrect date format", default);
            }
        }

        public ValueTuple<bool, string, decimal> DecimalConverter(string salary)
        {
            try
            {
                decimal value = decimal.Parse(salary.Replace(".", ","));
                return new ValueTuple<bool, string, decimal>(true, string.Empty, value);
            }
            catch (Exception)
            {
                return new ValueTuple<bool, string, decimal>(false, "Only numbers and a comma or dot, to separate the fractional part", 0);
            }
        }

        public ValueTuple<bool, string, char> CharConverter(string symbol)
        {
            if (symbol.Length != 1 || string.IsNullOrWhiteSpace(symbol))
            {
                return new ValueTuple<bool, string, char>(false, "It cannot be empty and more than one character.", ' ');
            }

            return new ValueTuple<bool, string, char>(true, string.Empty, char.Parse(symbol));
        }
    }
}
