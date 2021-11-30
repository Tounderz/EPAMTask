using System;
using System.Collections.Generic;

#pragma warning disable CA1305
#pragma warning disable IDE0090
#pragma warning disable IDE0060
#pragma warning disable CA1304

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
        private static AddCabinetRecord addCabinetRecordeck = new AddCabinetRecord();

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
            string firstName = addCabinetRecordeck.FirstName;
            var lastName = addCabinetRecordeck.LastName;
            DateTime dateOfBirth = addCabinetRecordeck.DateOfBirth;
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            var salary = addCabinetRecordeck.Salary;
            char symbol = addCabinetRecordeck.Symbol;
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
                var firstName = addCabinetRecordeck.FirstName;
                var lastName = addCabinetRecordeck.LastName;
                DateTime dateOfBirth = addCabinetRecordeck.DateOfBirth;
                short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
                var salary = addCabinetRecordeck.Salary;
                char symbol = addCabinetRecordeck.Symbol;
                fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, age, salary, symbol);
                Console.WriteLine($"Record #{id} is updated.");
            }
        }

        private static void Find(string parameters)
        {
            string[] str = parameters.Split(' ');
            if (str[0].ToLower() == "firstname")
            {
                var record = fileCabinetService.FindByFirstName(str[1].Trim('"'));
                PrintFind(record);
            }
            else if (str[0].ToLower() == "lastname")
            {
                var record = fileCabinetService.FindByLastName(str[1].Trim('"'));
                PrintFind(record);
            }
            else if (str[0].ToLower() == "dateofbirth")
            {
                var record = fileCabinetService.FindByDateOfBirth(str[1].Trim('"'));
                PrintFind(record);
            }
        }

        private static void PrintFind(FileCabinetRecord[] record)
        {
            foreach (var item in record)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }
    }
}