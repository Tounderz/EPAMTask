using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1822
#pragma warning disable CA1305

namespace FileCabinetApp
{
    public class AddCabinetRecord
    {
        public string FirstName
        {
            get
            {
                Console.Write($"First name: ");
                var name = Console.ReadLine();
                while (name.Length < 2 || name.Length > 60 || name.Contains(" ") || string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Incorrect data in the 'First name' field, size from 2 to 60 character.");
                    Console.Write($"First Name: ");
                    name = Console.ReadLine();
                }

                return name;
            }
        }

        public string LastName
        {
            get
            {
                Console.Write($"Last name: ");
                var name = Console.ReadLine();
                while (name.Length < 2 || name.Length > 60 || name.Contains(" ") || string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Incorrect data in the 'Last name' field, size from 2 to 60 character.");
                    Console.Write($"First Name: ");
                    name = Console.ReadLine();
                }

                return name;
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                Console.Write("Date of birth: ");
                DateTime dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("-", "."));
                while (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
                {
                    Console.WriteLine("Incorrect data in the 'Date of birth' fields, the minimum date is 01/01/1950, and the maximum is now.");
                    Console.Write("Date of birth: ");
                    dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("/", "."));
                }

                return dateOfBirth;
            }
        }

        public decimal Salary
        {
            get
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

                return decimal.Parse(str.Replace(".", ","));
            }
        }

        public char Symbol
        {
            get
            {
                Console.Write("Any character: ");
                string str = Console.ReadLine();
                while (str.Length != 1)
                {
                    Console.WriteLine("The 'Any character' field must contain one character.");
                    Console.Write("Any character: ");
                    str = Console.ReadLine();
                }

                return char.Parse(str);
            }
        }
    }
}
