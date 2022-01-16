using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable S3220
#pragma warning disable S1450
#pragma warning disable SA1214
#pragma warning disable SA1600

namespace FileCabinetGenerator
{
    public static class Program
    {
        private static bool checkPath = true;
        private static string getPathName;
        private static string getFileFormat;
        private static int getStartId;
        private static int countRecords;
        private static bool checkParametersCommandLine = true;
        private static readonly List<string> ParametersList = new () { "-t", "--output-type", "-o", "--output", "-a", "--records-amount", "-i", "--start-id" };
        private static GeneratorService generatorService = new ();
        private static GeneratorSnapshot generatorSnapshot;

        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                CommandLineOptions(args);
            }
            else
            {
                Console.WriteLine("You have not entered the parameters!");
            }

            CreateRecord();
            Export();
            if (!checkPath)
            {
                Console.WriteLine("\t Check the file path you entered");
            }
            else
            {
                Console.WriteLine($"{countRecords} records were written to {getPathName}");
            }
        }

        private static void CommandLineOptions(string[] args) // параметры командной строки
        {
            string parametorLine = string.Join(' ', args).ToLower();
            string[] rulesValidetion = parametorLine.Trim(' ').Split(' ', '=');
            List<string> listCommandLineParameter = rulesValidetion.Where(i => ParametersList.Any(y => y.Equals(i, StringComparison.Ordinal))).ToList();

            for (int i = 0; i < listCommandLineParameter.Count; i++)
            {
                switch (listCommandLineParameter[i].ToLower())
                {
                    case "-t":
                    case "--output-type":
                        getFileFormat = rulesValidetion[(i * 2) + 1];
                        break;

                    case "-o":
                    case "--output":
                        getPathName = rulesValidetion[(i * 2) + 1];
                        break;
                    case "-a":
                    case "--records-amount":
                        checkParametersCommandLine = int.TryParse(rulesValidetion[(i * 2) + 1], out countRecords);
                        break;

                    case "-i":
                    case "--start-id":
                        checkParametersCommandLine = int.TryParse(rulesValidetion[(i * 2) + 1], out getStartId);
                        break;

                    default:
                        Console.WriteLine("You entered an unknown command");
                        break;
                }
            }

            if (!checkParametersCommandLine)
            {
                Console.WriteLine("Incorrect input!");
            }
        }

        private static void CreateRecord()
        {
            generatorService.GeneratorCreateRecord(getStartId, countRecords);
        }

        private static void Export()
        {
            string fileName = Path.GetFileName(getPathName);
            switch (getFileFormat.ToLower())
            {
                case "csv":
                    try
                    {
                        GetSaveToCsv(fileName);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine($"Export failed: can't open file {getPathName}");
                    }

                    break;

                case "xml":
                    try
                    {
                        GetSaveToXml(fileName);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine($"Export failed: can't open file {getPathName}");
                    }

                    break;

                default:
                    Console.WriteLine("Incorrect input! 1. export csv (or xml) filename.csv ; 2. export csv (or xml) full address to the file'\'filename.csv");
                    checkPath = false;
                    break;
            }
        }

        private static void GetSaveToCsv(string fileNameCsv) // запись csv
        {
            using StreamWriter streamWriter = new (fileNameCsv);
            generatorSnapshot = generatorService.MakeSnapshot();
            generatorSnapshot.SaveToCsv(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameCsv}.");
        }

        private static void GetSaveToXml(string fileNameXml) // запись xml
        {
            using StreamWriter streamWriter = new (fileNameXml);
            generatorSnapshot = generatorService.MakeSnapshot();
            generatorSnapshot.SaveToXml(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameXml}.");
        }
    }
}
