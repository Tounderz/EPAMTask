using System;
using System.Collections.ObjectModel;

#pragma warning disable CA1304
#pragma warning disable CA1305
#pragma warning disable SA1600
#pragma warning disable S1450

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dmitry Grudinsky";
        private const string DefaultValidation = "Using default validation rules.";
        private const string CustomValidation = "Using custom validation rules.";
        private const string Default = "defaul";
        private const string Custom = "custom";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static bool isRunning = true;
        private static FileCabinetService cabinetService = new FileCabinetService();
        private static IRecordValidator recordValidator;
        private static FileCabinetRecord cabinetRecord = new ();

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("start", Start),
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
            new string[] { "find", "search model: find search parameter \"search criteria\"", "Search by parameters 'firstname or lastname or dateofbirth', search model: find search parameter \"search criteria\"." },
        };

        public static void Main(string[] args)
        {
            int checkValidationRules = 0;
            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(Default))
                {
                    checkValidationRules = 1;
                    break;
                }
                else if (arg.ToLower().Contains(Custom))
                {
                    checkValidationRules = -1;
                    break;
                }
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            switch (checkValidationRules)
            {
                case 0:
                case 1:
                    {
                        Console.WriteLine(DefaultValidation);
                        recordValidator = new DefaultValidator();
                        break;
                    }

                case -1:
                    {
                        Console.WriteLine(CustomValidation);
                        recordValidator = new CustomValidator();
                        break;
                    }

                default:
                    break;
            }

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

        private static void Start(string parameters)
        {
            var recordsCount = cabinetService.GetStart();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            int id = cabinetService.GetStart() + 1;
            cabinetRecord = recordValidator.AddRecord(id);
            cabinetService.CreateRecord(cabinetRecord);
        }

        private static void List(string parameters)
        {
            var list = cabinetService.GetRecords();
            foreach (var item in list)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static void Edit(string parameters)
        {
            while (CkeckEdit(parameters))
            {
                int id = int.Parse(parameters);
                if (id < 1 || id > cabinetService.GetStart())
                {
                    Console.WriteLine($"#{id} record is not found.");
                    break;
                }
                else
                {
                    cabinetRecord = recordValidator.AddRecord(id);
                    cabinetService.EditRecord(id, cabinetRecord);
                    Console.WriteLine($"Record #{id} is updated.");
                    break;
                }
            }
        }

        private static void Find(string parameters)
        {
            string[] str = parameters.Split(' ');
            parameters = str[1].Trim('"').ToUpper();
            switch (str[0].ToLower())
            {
                case "firstname":
                    {
                        var record = cabinetService.FindByFirstName(parameters);
                        PrintFind(record);
                        break;
                    }

                case "lastname":
                    {
                        var record = cabinetService.FindByLastName(parameters);
                        PrintFind(record);
                        break;
                    }

                case "dateofbirth":
                    {
                        var record = cabinetService.FindByDateOfBirth(parameters);
                        PrintFind(record);
                        break;
                    }

                default:
                    Console.WriteLine("Incorrect input!");
                    break;
            }
        }

        private static void PrintFind(ReadOnlyCollection<FileCabinetRecord> record)
        {
            foreach (var item in record)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static bool CkeckEdit(string parameters)
        {
            bool ckeck = true;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (!char.IsDigit(parameters[i]))
                {
                    Console.WriteLine("The parameter must be numeric.");
                    ckeck = false;
                    break;
                }
                else
                {
                    ckeck = true;
                }
            }

            return ckeck;
        }
    }
}