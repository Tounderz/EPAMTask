﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

#pragma warning disable CA1304
#pragma warning disable CA1305
#pragma warning disable SA1600
#pragma warning disable S1450
#pragma warning disable SA1108
#pragma warning disable SA1203
#pragma warning disable S1075

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dmitry Grudinsky";
        private const string DefaultValidator = "defaul";
        private const string CustomValidator = "custom";
        private const string MemoryService = "memory";
        private const string FileService = "file";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator recordValidator;
        private static FileCabinetRecord cabinetRecord = new ();
        private static FileCabinetServiceSnapshot fileCabinetServiceSnapshot = new ();
        private static StreamWriter streamWriter;
        private const string PathCsv = @"C:\Users\basta\source\repos\EPAMTask\FileCabinetApp\bin\Debug\records.csv";
        private const string PathXml = @"C:\Users\basta\source\repos\EPAMTask\FileCabinetApp\bin\Debug\records.xml";

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("start", Start),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
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
            new string[] { "export", "writing to a 'csv or xml' file", "The 'export' command writes a file in csv or xml format" },
        };

        public static void Main(string[] args)
        {
            Console.Write("Enter the validation option (default or custom): ");
            string validator = Console.ReadLine().ToLower();

            Console.Write("Enter the storage service (memory or file): ");
            string storage = Console.ReadLine().ToLower();

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            PrintValidator(CheckValidatorOrStorage(validator, DefaultValidator, CustomValidator));
            PrintStorage(CheckValidatorOrStorage(storage, MemoryService, FileService));

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

        private static void Start(string parameters) // вывод количества объектов в списке
        {
            var recordsCount = fileCabinetService.GetRecordsCount();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters) // добовление нового объекта
        {
            int id = fileCabinetService.GetRecordsCount() + 1;
            cabinetRecord = AddParametor(id);
            fileCabinetService.CreateRecord(cabinetRecord);
        }

        private static void List(string parameters) // вывод всех объектов в листе
        {
            var list = fileCabinetService.GetRecords();
            foreach (var item in list)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static void Edit(string parameters) // изменение данных по id
        {
            while (CkeckEdit(parameters))
            {
                int id = int.Parse(parameters);
                if (id < 1 || id > fileCabinetService.GetRecordsCount())
                {
                    Console.WriteLine($"#{id} record is not found.");
                    break;
                }
                else
                {
                    cabinetRecord = AddParametor(id);
                    fileCabinetService.EditRecord(id, cabinetRecord);
                    Console.WriteLine($"Record #{id} is updated.");
                    break;
                }
            }
        }

        private static void Find(string parameters) // поиск всех одинаковых данных одного из полей, при помощи словаря
        {
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Specify the search criteria");
            }
            else
            {
                string[] str = parameters.Split(' ');
                switch (str[0].ToLower())
                {
                    case "firstname":
                        {
                            parameters = str[1].Trim('"').ToUpper();
                            if (string.IsNullOrEmpty(parameters) || fileCabinetService.GetRecords().FirstOrDefault(i => i.FirstName.ToLower() == parameters.ToLower()) == null)
                            {
                                Console.WriteLine("Specify the search criteria");
                                break;
                            }
                            else
                            {
                                var record = fileCabinetService.FindByFirstName(parameters);
                                PrintFind(record);
                                break;
                            }
                        }

                    case "lastname":
                        {
                            parameters = str[1].Trim('"').ToUpper();
                            if (string.IsNullOrEmpty(parameters) || fileCabinetService.GetRecords().FirstOrDefault(i => i.LastName.ToLower() == parameters.ToLower()) == null)
                            {
                                Console.WriteLine("Specify the search criteria");
                                break;
                            }
                            else
                            {
                                var record = fileCabinetService.FindByLastName(parameters);
                                PrintFind(record);
                                break;
                            }
                        }

                    case "dateofbirth":
                        {
                            parameters = str[1].Trim('"').ToUpper();
                            if (string.IsNullOrEmpty(parameters) || (fileCabinetService.GetRecords().FirstOrDefault(i => i.DateOfBirth.ToString("yyyy-MMM-dd").ToLower() == parameters.ToLower()) == null))
                            {
                                Console.WriteLine("You didn't enter the search parameter or you entered it incorrectly. It takes a year (xxxx), a month (the first three letters), a day with two digits if the date is less than 10, then add 0 (xx)");
                                break;
                            }
                            else
                            {
                                var record = fileCabinetService.FindByDateOfBirth(parameters);
                                PrintFind(record);
                                break;
                            }
                        }

                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
        }

        private static void Export(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Specify the search criteria");
            }
            else
            {
                string[] parameterArray = parameters.Split();
                parameters = parameterArray[1];
                switch (parameterArray[0].ToLower())
                {
                    case "csv":
                        if (parameters == "records.csv" || parameters == PathCsv)
                        {
                            if (File.Exists(PathCsv))
                            {
                                Console.Write($"File is exist - rewrite {PathCsv}? [Y/n] ");
                                string checkToRewrite = Console.ReadLine().ToLower();
                                if (checkToRewrite == "y")
                                {
                                    GetSaveToCsv();
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                GetSaveToCsv();
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Export failed: can't open file {parameters}");
                            break;
                        }

                    case "xml":
                        if (parameters == "records.xml" || parameters == PathXml)
                        {
                            if (File.Exists(PathXml))
                            {
                                Console.Write($"File is exist - rewrite {PathXml}? [Y/n] ");
                                string checkToRewrite = Console.ReadLine().ToLower();
                                if (checkToRewrite == "y")
                                {
                                    GetSaveToXml();
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                GetSaveToXml();
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Export failed: can't open file {parameters}");
                            break;
                        }

                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
        }

        private static void PrintFind(ReadOnlyCollection<FileCabinetRecord> record) // метод для вывода в консоль данных по запросу из метода Find
        {
            foreach (var item in record)
            {
                Console.WriteLine($"#{item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth:yyyy-MMM-dd}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static bool CkeckEdit(string parameters) // проверка параметра на отстутствие не числовых данных, для метода Edit
        {
            bool ckeck = true;
            if (parameters.Length == 0)
            {
                Console.WriteLine("Enter the 'edit' 'number' to change separated by a space.");
                return false;
            }

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

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> StringConverter(string str)
        {
            if (!str.All(char.IsLetter))
            {
                return new Tuple<bool, string, string>(false, "There should be only letters and not be empty", str);
            }

            return new Tuple<bool, string, string>(true, null, str);
        }

        private static Tuple<bool, string, DateTime> DateConverter(string str)
        {
            try
            {
                DateTime value = Convert.ToDateTime(str.Replace("/", "."));
                return new Tuple<bool, string, DateTime>(true, null, value);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, DateTime>(false, "Incorrect date format", default);
            }
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string str)
        {
            try
            {
                decimal value = decimal.Parse(str.Replace(".", ","));
                return new Tuple<bool, string, decimal>(true, null, value);
            }
            catch (Exception)
            {
                return new Tuple<bool, string, decimal>(false, "Only numbers and a comma or dot, to separate the fractional part", 0);
            }
        }

        private static Tuple<bool, string, char> CharConverter(string str)
        {
            if (str.Length != 1)
            {
                return new Tuple<bool, string, char>(false, "It cannot be empty and more than one character.", ' ');
            }

            return new Tuple<bool, string, char>(true, null, char.Parse(str));
        }

        private static FileCabinetRecord AddParametor(int id)
        {
            Console.Write($"First name: ");
            string firstName = ReadInput(StringConverter, recordValidator.ValidateFirstName);
            Console.Write($"Last name: ");
            string lastName = ReadInput(StringConverter, recordValidator.ValidateLastName);
            Console.Write("Date of birth: ");
            DateTime dateOfBirth = ReadInput(DateConverter, recordValidator.ValidateDateOfBirth);
            short age = Convert.ToInt16(DateTime.Now.Year - dateOfBirth.Year);
            Console.Write("Salary: ");
            decimal salary = ReadInput(DecimalConverter, recordValidator.ValidateSalary);
            Console.Write("Any character: ");
            char symbol = ReadInput(CharConverter, recordValidator.ValidateSymbol);

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

        private static void GetSaveToCsv()
        {
            streamWriter = new StreamWriter(PathCsv);
            fileCabinetServiceSnapshot = fileCabinetService.MakeSnapshot();
            fileCabinetServiceSnapshot.SaveToCsv(streamWriter);
            streamWriter.Close();
            Console.WriteLine("All records are exported to file records.csv.");
        }

        private static void GetSaveToXml()
        {
            streamWriter = new StreamWriter(PathXml);
            fileCabinetServiceSnapshot = fileCabinetService.MakeSnapshot();
            fileCabinetServiceSnapshot.SaveToXml(streamWriter);
            streamWriter.Close();
            Console.WriteLine("All records are exported to file records.xml.");
        }

        private static int CheckValidatorOrStorage(string validator, string parametorOne, string parametorTwo)
        {
            int checkRules = 0;
            foreach (var item in validator) // какая валидация выбрана пользователем
            {
                if (validator.ToLower().Contains(parametorOne))
                {
                    checkRules = 1;
                    break;
                }
                else if (validator.ToLower().Contains(parametorTwo))
                {
                    checkRules = -1;
                    break;
                }
            }

            return checkRules;
        }

        private static void PrintValidator(int checkRules)
        {
            switch (checkRules) // согласно выбранной вадилидацией пользователем, создание объекта нужного класса
            {
                case 0:
                case 1:
                    {
                        Console.WriteLine("Using default validation rules.");
                        recordValidator = new DefaultValidator(); // для default
                        break;
                    }

                case -1:
                    {
                        Console.WriteLine("Using custom validation rules.");
                        recordValidator = new CustomValidator(); // для cusmot
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Incorrect input!");
                        break;
                    }
            }
        }

        private static void PrintStorage(int checkRules)
        {
            switch (checkRules) // согласно выбранной вадилидацией пользователем, создание объекта нужного класса
            {
                case 0:
                case 1:
                    {
                        Console.WriteLine("Are you using FileCabinetMemoryService");
                        fileCabinetService = new FileCabinetMemoryService(); // для memory
                        break;
                    }

                case -1:
                    {
                        Console.WriteLine("Are you using FileCabinetFileService");
                        fileCabinetService = new FileCabinetFilesystemService(); // для file
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Incorrect input!");
                        break;
                    }
            }
        }
    }
}