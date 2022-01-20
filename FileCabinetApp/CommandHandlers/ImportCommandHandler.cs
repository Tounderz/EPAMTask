using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600
#pragma warning disable S1450

namespace FileCabinetApp.CommandHandlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private FileCabinetServiceSnapshot fileCabinetServiceSnapshot;

        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Import(string parameters)
        {
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Specify the search criteria");
            }
            else
            {
                string[] parameterArray = parameters.Split();
                string fileType = parameterArray[0];
                string pathName = parameterArray.Last();
                string fileName = Path.GetFileName(pathName);

                switch (fileType)
                {
                    case "csv":
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                this.GetLoadFromCsv(fileName, pathName);
                            }
                            else
                            {
                                Console.WriteLine($"Import error: file {pathName} is not exist.");
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine("Incorrect input! 1. import csv filename.csv ; 2. import csv full address to the file'\'filename.csv");
                        }

                        break;

                    case "xml":
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                this.GetLoadFromXml(fileName, pathName);
                            }
                            else
                            {
                                Console.WriteLine($"Import error: file {pathName} is not exist.");
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine("Incorrect input! 1. import xml filename.csv ; 2. import xml full address to the file'\'filename.csv");
                        }

                        break;

                    default:
                        Console.WriteLine("Incorrect input! 1. import csv (or xml) filename.csv ; 2. import csv (or xml) full address to the file'\'filename.csv");
                        break;
                }
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.ImportName)
            {
                this.Import(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void GetLoadFromCsv(string fileNameCsv, string pathName)
        {
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
            using FileStream fileStream = File.Open(fileNameCsv, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new (fileStream.Name, Encoding.ASCII);
            this.fileCabinetServiceSnapshot.LoadFromCsv(streamReader);
            Console.WriteLine($"{this.fileCabinetServiceSnapshot.RecordsFromFile.Count} records were imported from {pathName}");
            this.service.Restore(this.fileCabinetServiceSnapshot);
        }

        private void GetLoadFromXml(string fileNameXml, string pathName)
        {
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
            using FileStream fileStream = File.Open(fileNameXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new (fileStream.Name, Encoding.ASCII);
            this.fileCabinetServiceSnapshot.LoadFromXml(streamReader);
            Console.WriteLine($"{this.fileCabinetServiceSnapshot.RecordsFromFile.Count} records were imported from {pathName}");
            this.service.Restore(this.fileCabinetServiceSnapshot);
        }
    }
}
