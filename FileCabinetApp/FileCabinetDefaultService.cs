using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable CA1305
#pragma warning disable S3928
#pragma warning disable SA1202
#pragma warning disable CA1822

namespace FileCabinetApp
{
    public class FileCabinetDefaultService
    {
        private const string Space = " ";

        private string FirstName()
        {
            Console.Write($"First name: ");
            string firstName = Console.ReadLine();
            while (firstName.Length < 2 || firstName.Length > 60 || firstName.Contains(Space) || string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("Incorrect data in the 'First name' field, size from 2 to 60 character.");
                Console.Write($"First Name: ");
                firstName = Console.ReadLine();
            }

            return firstName;
        }

        private string LastName()
        {
            Console.Write($"Last name: ");
            string lastName = Console.ReadLine();
            while (lastName.Length < 2 || lastName.Length > 60 || lastName.Contains(Space) || string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Incorrect data in the 'Last name' field, size from 2 to 60 character.");
                Console.Write($"Last Name: ");
                lastName = Console.ReadLine();
            }

            return lastName;
        }

        private DateTime DateOfBirth()
        {
            Console.Write("Date of birth: ");
            var dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("-", "."));
            while (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                Console.WriteLine("Incorrect data in the 'Date of birth' fields, the minimum date is 01/01/1950, and the maximum is now.");
                Console.Write("Date of birth: ");
                dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("/", "."));
            }

            return dateOfBirth;
        }

        private decimal Salary()
        {
            Console.Write("Salary: ");
            string str = Console.ReadLine();
            for (int i = 0; i < str.Length; i++)
            {
                while (char.IsLetter(str[i]))
                {
                    Console.WriteLine("The 'salary' line consists only of digits and a dot or comma for the fractional part.");
                    Console.Write("Salary: ");
                    str = Console.ReadLine();
                }
            }

            var salary = decimal.Parse(str.Replace(".", ","));
            return salary;
        }

        private char Symbol()
        {
            Console.Write("Any character: ");
            string str = Console.ReadLine();
            while (str.Length != 1)
            {
                Console.WriteLine("The 'Any character' field must contain one character.");
                Console.Write("Any character: ");
                str = Console.ReadLine();
            }

            char symbol = char.Parse(str);
            return symbol;
        }

        public FileCabinetRecord Valideters(int id)
        {
            var firstName = this.FirstName();
            var lastName = this.LastName();
            var dateOfBirth = this.DateOfBirth();
            var age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            var salary = this.Salary();
            var symbol = this.Symbol();

            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"{nameof(firstName)} can't be null");
            }

            if (firstName.Length < 2 || firstName.Length > 60 || firstName.Contains(Space))
            {
                throw new ArgumentException($"The {nameof(firstName)} size is from 2 to 60 characters and there should be no spaces");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"{nameof(lastName)} can't be null");
            }

            if (lastName.Length < 2 || lastName.Length > 60 || lastName.Contains(Space))
            {
                throw new ArgumentException($"The {nameof(lastName)} size is from 2 to 60 characters and there should be no spaces");
            }

            if (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} - Minimum date of birth 01/01/1950, and maximum - DateTime.Now");
            }

            if (symbol.ToString().Length != 1)
            {
                throw new ArgumentException($"The {nameof(symbol)} size of the string character is equal to one element");
            }

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Symbol = symbol,
            };

            return record;
        }
    }
}
