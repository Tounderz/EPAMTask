using System;
using System.Collections.Generic;

#pragma warning disable CA1305
#pragma warning disable IDE0090
#pragma warning disable IDE0060

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dmitry Grudinsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static FileCabinetService fileCabinetService = new FileCabinetService();
        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("start", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "start", "start the application", "The 'start' command outputs the amount of data in the shee" },
            new string[] { "create", "create a record", "The 'create' command will create a new record in the sheet." },
            new string[] { "list", "output list", "Commands 'list' outputs list." },
            new string[] { "edit", "editor", "The 'edit' command allows you to edit data by id." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            var firstName = FirstName();
            var lastName = LastName();
            DateTime dateOfBirth = DateOfBirth();
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            var salary = Salary();
            char symbol = Symbol();

            fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, age, salary, symbol);
        }

        private static void List(string parameters)
        {
            var list = fileCabinetService.GetRecords();
            foreach (var item in list)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static void Edit(string parameters)
        {
            int id = int.Parse(parameters);
            if (id < 1 || id > fileCabinetService.GetRecords().Length)
            {
                Console.WriteLine($"#{id} record is not found.");
            }
            else
            {
                var list = fileCabinetService.GetRecords();
                for (int i = 0; i < list.Length; i++)
                {
                    if (i + 1 == id)
                    {
                        list[i].FirstName = FirstName();
                        list[i].LastName = LastName();
                        list[i].DateOfBirth = DateOfBirth();
                        list[i].Age = Convert.ToInt16(DateTime.Now.Year - list[i].DateOfBirth.Year);
                        list[i].Salary = Salary();
                        list[i].Symbol = Symbol();
                    }

                    Console.WriteLine($"Record #{id} is updated.");
                }
            }
        }

        private static void Find(string parameters)
        {
            string[] str = parameters.Split(' ');
            if (str[0].ToLower() == "firstname")
            {
                var list = fileCabinetService.FindByFirstName(str[1].Trim('"'));
                foreach (var item in list)
                {
                    Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
                }
            }
        }

        private static string FirstName()
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

        private static string LastName()
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

        private static DateTime DateOfBirth()
        {
            Console.Write("Date of birth: ");
            DateTime dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("/", "."));
            while (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                Console.WriteLine("Incorrect data in the 'Date of birth' fields, the minimum date is 01/01/1950, and the maximum is now.");
                Console.Write("Date of birth: ");
                dateOfBirth = Convert.ToDateTime(Console.ReadLine().Replace("/", "."));
            }

            return dateOfBirth;
        }

        private static decimal Salary()
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

        private static char Symbol()
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