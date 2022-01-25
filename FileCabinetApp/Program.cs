using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Validators;

#pragma warning disable CA1304
#pragma warning disable SA1600
#pragma warning disable SA1214
#pragma warning disable S3220

namespace FileCabinetApp
{
    public static class Program
    {
        private static bool isRunning = true;
        private static IRecordValidator recordValidator = new ValidatorBuilder().CreateDefault();
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(recordValidator);
        private static FileStream fileStream;
        private static string nameValidator = ConstParameters.DefaultValidatorName;
        private static readonly List<string> ParametersListValue = new ()
        {
            ConstParameters.LongValidatorLineParameter,
            ConstParameters.ShortValidatorLineParameter,
            ConstParameters.LongTypeLineParameter,
            ConstParameters.ShortTypeLineParameter,
            ConstParameters.StopWatchLineParameter,
            ConstParameters.LoggerLineParameter,
        };

        public static void Main(string[] args)
        {
            nameValidator = ConstParameters.DefaultValidatorName;
            Console.WriteLine($"File Cabinet Application, developed by {ConstParameters.DeveloperName}");
            if (args.Length != 0)
            {
                CommandLineOptions(args);
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
                Console.WriteLine("Using memory service rules.");
            }

            Console.WriteLine(ConstParameters.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(ConstParameters.HintMessage);
                    continue;
                }

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                CommandHandlerBase commandHandlerBase = CreateCommandHandlers(fileCabinetService);
                commandHandlerBase.Handle(
                    new AppCommandRequest
                    {
                        Command = command,
                        Parameters = parameters,
                    });
            }
            while (isRunning);
        }

        private static void GetIsRunning(bool getIsRunning) => isRunning = getIsRunning;

        private static CommandHandlerBase CreateCommandHandlers(IFileCabinetService fileCabinetService)
        {
            var printMissedCommandInfoHandler = new PrintMissedCommandInfoHandler();
            var helpCommandHandler = new HelpCommandHandler();
            var statCommandHandler = new StatCommandHandler(fileCabinetService);
            var exitCommandHandler = new ExitCommandHandler(GetIsRunning, fileStream);
            var listCommandHandler = new ListCommandHandler(fileCabinetService, DefaultRecordPrint);
            var createCommandHandler = new CreateCommandHandler(fileCabinetService, nameValidator);
            var insertCommandHandler = new InsertCommandHandler(fileCabinetService, nameValidator);
            var editCommandHandler = new EditCommandHandler(fileCabinetService, nameValidator);
            var findCommandHandler = new FindCommandHandler(fileCabinetService, DefaultRecordPrint);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var removeCommandHandler = new RemoveCommandHandler(fileCabinetService);
            var deleteCommandHandler = new DeleteCommandHandler(fileCabinetService);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            helpCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(listCommandHandler);
            listCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(insertCommandHandler);
            insertCommandHandler.SetNext(editCommandHandler);
            editCommandHandler.SetNext(findCommandHandler);
            findCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(removeCommandHandler);
            removeCommandHandler.SetNext(deleteCommandHandler);
            deleteCommandHandler.SetNext(purgeCommandHandler);
            purgeCommandHandler.SetNext(printMissedCommandInfoHandler);
            return helpCommandHandler;
        }

        private static void DefaultRecordPrint(IEnumerable<FileCabinetRecord> fileCabinetRecords) // метод для вывода в консоль данных
        {
            foreach (var item in fileCabinetRecords)
            {
                Console.WriteLine($"# {item.Id}, {item.FirstName}, {item.LastName}, {item.DateOfBirth.ToString(ConstParameters.FormatDate)}, {item.Age}, {item.Salary}, {item.Symbol}");
            }
        }

        private static void CommandLineOptions(string[] args)
        {
            string parametorLine = string.Join(' ', args).ToLower();
            string[] rulesValidetion = parametorLine.Trim(' ').Split(' ', '=');
            List<string> listCommandLineParameter = rulesValidetion.Where(i => ParametersListValue.Any(y => y.Equals(i))).ToList();
            bool checkStopWatch = listCommandLineParameter.Contains(ConstParameters.StopWatchLineParameter);
            bool ckeckLogger = listCommandLineParameter.Contains(ConstParameters.LoggerLineParameter);
            for (int i = 0; i < listCommandLineParameter.Count; i++)
            {
                switch (listCommandLineParameter[i].ToLower())
                {
                    case ConstParameters.LongValidatorLineParameter:
                    case ConstParameters.ShortValidatorLineParameter:
                        if (string.Equals(rulesValidetion[(i * 2) + 1], ConstParameters.CustomValidatorName))
                        {
                            recordValidator = new ValidatorBuilder().CreateCustom();
                            fileCabinetService = new FileCabinetMemoryService(recordValidator);
                            nameValidator = ConstParameters.CustomValidatorName;
                            Console.WriteLine("Using custom validation rules.");
                        }
                        else if (string.Equals(rulesValidetion[(i * 2) + 1], ConstParameters.DefaultValidatorName))
                        {
                            Console.WriteLine("Using default validation rules.");
                        }

                        break;

                    case ConstParameters.LongTypeLineParameter:
                    case ConstParameters.ShortTypeLineParameter:
                        if (string.Equals(rulesValidetion[(2 * i) + 1], ConstParameters.FileServiceName))
                        {
                            fileStream = new FileStream(ConstParameters.DBPathName, FileMode.OpenOrCreate);
                            fileCabinetService = new FileCabinetFilesystemService(fileStream, recordValidator);
                            Console.WriteLine("Using file service rules.");
                        }
                        else if (string.Equals(rulesValidetion[(2 * i) + 1], ConstParameters.MemoryServiceName))
                        {
                            Console.WriteLine("Using memory service rules.");
                        }

                        break;

                    default:
                        Console.WriteLine("Using memory service rules.");
                        Console.WriteLine("Using default validation rules.");
                        break;
                }

                if (checkStopWatch)
                {
                    fileCabinetService = new ServiceMeter(fileCabinetService);
                    Console.WriteLine("Using ServiceMeter");
                }

                if (ckeckLogger)
                {
                    fileCabinetService = new ServiceLogger(fileCabinetService);
                    Console.WriteLine("Using ServiceLogger");
                }
            }
        }
    }
}