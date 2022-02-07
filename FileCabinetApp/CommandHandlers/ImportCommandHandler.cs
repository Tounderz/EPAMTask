using System;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Services;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private readonly FileCabinetServiceSnapshot fileCabinetServiceSnapshot;

        public ImportCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.ImportName)
            {
                this.Import(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Import(string parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(parameters))
                {
                    throw new ArgumentException("Incorrect input!");
                }

                string[] arrParameters = parameters.Split();
                if (arrParameters.Length != 2)
                {
                    throw new ArgumentException("You didn't specify the file type or path(or file name)");
                }

                string fileType = arrParameters[0].ToLower();
                string pathName = arrParameters.Last();
                string fileName = Path.GetFileName(pathName);
                string typeFile = Path.GetExtension(fileName).TrimStart('.').ToLower();
                if (fileType != typeFile)
                {
                    throw new ArgumentException("Incorrect input! 1. import csv(or xml) filename.csv(or xml) ; 2. import csv(or xml) full address to the file'\'filename.csv(or xml)!");
                }

                if (!File.Exists(pathName))
                {
                    throw new ArgumentException($"Import error: file {pathName} is not exist.");
                }

                switch (fileType)
                {
                    case TypeFile.CsvType:
                        this.GetLoadFromCsv(fileName, pathName);
                        break;
                    case TypeFile.XmlType:
                        this.GetLoadFromXml(fileName, pathName);
                        break;
                    default:
                        throw new ArgumentException("Incorrect input! 1. import csv(or xml) filename.csv(or xml); 2. import csv(or xml) full address to the file'\'filename.csv(or xml)!");
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void GetLoadFromCsv(string fileNameCsv, string pathName)
        {
            using FileStream fileStream = File.Open(fileNameCsv, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new (fileStream.Name, Encoding.ASCII);
            this.fileCabinetServiceSnapshot.LoadFromCsv(streamReader);
            Console.WriteLine($"{this.fileCabinetServiceSnapshot.RecordsFromFile.Count} records were imported from {pathName}");
            this.service.Restore(this.fileCabinetServiceSnapshot);
        }

        private void GetLoadFromXml(string fileNameXml, string pathName)
        {
            using FileStream fileStream = File.Open(fileNameXml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new (fileStream.Name, Encoding.ASCII);
            this.fileCabinetServiceSnapshot.LoadFromXml(streamReader);
            Console.WriteLine($"{this.fileCabinetServiceSnapshot.RecordsFromFile.Count} records were imported from {pathName}");
            this.service.Restore(this.fileCabinetServiceSnapshot);
        }
    }
}
