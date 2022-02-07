using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Services;
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
        private static string nameValidator = CommandLineParameters.DefaultValidatorName;
        private static readonly List<string> ParametersListValue = new ()
        {
            CommandLineParameters.LongValidatorLineParameter,
            CommandLineParameters.ShortValidatorLineParameter,
            CommandLineParameters.LongTypeLineParameter,
            CommandLineParameters.ShortTypeLineParameter,
            CommandLineParameters.StopWatchLineParameter,
            CommandLineParameters.LoggerLineParameter,
        };

        public static void Main(string[] args)
        {
            nameValidator = CommandLineParameters.DefaultValidatorName;
            Console.WriteLine($"File Cabinet Application, developed by {ConstStrings.DeveloperName}");
            if (args.Length != 0)
            {
                CommandLineOptions(args);
            }
            else
            {
                Console.WriteLine("Using default validation rules.");
                Console.WriteLine("Using memory service.");
            }

            Console.WriteLine(ConstStrings.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(ConstStrings.HintMessage);
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
            var createCommandHandler = new CreateCommandHandler(fileCabinetService, nameValidator);
            var insertCommandHandler = new InsertCommandHandler(fileCabinetService, nameValidator);
            var updateCommandHandler = new UpdateCommandHandler(fileCabinetService, nameValidator);
            var selectCommandHandler = new SelectCommandHandler(fileCabinetService);
            var exportCommandHandler = new ExportCommandHandler(fileCabinetService);
            var importCommandHandler = new ImportCommandHandler(fileCabinetService);
            var deleteCommandHandler = new DeleteCommandHandler(fileCabinetService);
            var purgeCommandHandler = new PurgeCommandHandler(fileCabinetService);
            helpCommandHandler.SetNext(statCommandHandler);
            statCommandHandler.SetNext(exitCommandHandler);
            exitCommandHandler.SetNext(createCommandHandler);
            createCommandHandler.SetNext(insertCommandHandler);
            insertCommandHandler.SetNext(updateCommandHandler);
            updateCommandHandler.SetNext(selectCommandHandler);
            selectCommandHandler.SetNext(exportCommandHandler);
            exportCommandHandler.SetNext(importCommandHandler);
            importCommandHandler.SetNext(deleteCommandHandler);
            deleteCommandHandler.SetNext(purgeCommandHandler);
            purgeCommandHandler.SetNext(printMissedCommandInfoHandler);
            return helpCommandHandler;
        }

        private static void CommandLineOptions(string[] args)
        {
            string parametorLine = string.Join(' ', args).ToLower();
            string[] rulesValidetion = parametorLine.Trim(' ').Split(' ', '=');
            List<string> listCommandLineParameter = rulesValidetion.Where(i => ParametersListValue.Any(y => y.Equals(i))).ToList();
            bool checkStopWatch = listCommandLineParameter.Contains(CommandLineParameters.StopWatchLineParameter);
            bool ckeckLogger = listCommandLineParameter.Contains(CommandLineParameters.LoggerLineParameter);
            for (int i = 0; i < listCommandLineParameter.Count; i++)
            {
                switch (listCommandLineParameter[i].ToLower())
                {
                    case CommandLineParameters.LongValidatorLineParameter:
                    case CommandLineParameters.ShortValidatorLineParameter:
                        if (string.Equals(rulesValidetion[(i * 2) + 1], CommandLineParameters.CustomValidatorName))
                        {
                            recordValidator = new ValidatorBuilder().CreateCustom();
                            fileCabinetService = new FileCabinetMemoryService(recordValidator);
                            nameValidator = CommandLineParameters.CustomValidatorName;
                            Console.WriteLine("Using custom validation rules.");
                        }
                        else if (string.Equals(rulesValidetion[(i * 2) + 1], CommandLineParameters.DefaultValidatorName))
                        {
                            Console.WriteLine("Using default validation rules.");
                        }

                        break;

                    case CommandLineParameters.LongTypeLineParameter:
                    case CommandLineParameters.ShortTypeLineParameter:
                        if (string.Equals(rulesValidetion[(2 * i) + 1], CommandLineParameters.FileServiceName))
                        {
                            fileStream = new FileStream(PathName.DBPathName, FileMode.OpenOrCreate);
                            fileCabinetService = new FileCabinetFilesystemService(fileStream, recordValidator);
                            Console.WriteLine("Using file service.");
                        }
                        else if (string.Equals(rulesValidetion[(2 * i) + 1], CommandLineParameters.MemoryServiceName))
                        {
                            Console.WriteLine("Using memory service.");
                        }

                        break;

                    default:
                        Console.WriteLine("Using memory service.");
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
                    fileCabinetService = new ServiceLogger(fileCabinetService, PathName.LoggerPathName);
                    Console.WriteLine("Using ServiceLogger");
                }
            }
        }
    }
}