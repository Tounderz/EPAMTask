using System;
using System.IO;
using System.Linq;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Services;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private readonly FileCabinetServiceSnapshot fileCabinetServiceSnapshot;

        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.ExportName)
            {
                this.Export(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Export(string parameters)
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
                    throw new ArgumentException("Incorrect input! 1. export csv(or xml) filename.csv(or xml) ; 2. export csv(or xml) full address to the file'\'filename.csv(or xml)!");
                }

                if (File.Exists(pathName))
                {
                    Console.Write($"File is exist - rewrite {fileName}? [Y/n]");
                    string checkToRewrite = Console.ReadLine().ToLower();
                    if (checkToRewrite == "n")
                    {
                        return;
                    }
                }

                switch (fileType)
                {
                    case TypeFile.CsvType:
                        this.GetSaveToCsv(fileName);
                        break;
                    case TypeFile.XmlType:
                        this.GetSaveToXml(fileName);
                        break;
                    default:
                        throw new ArgumentException("Incorrect input! 1. export csv(or xml) filename.csv(or xml) ; 2. export csv(or xml) full address to the file'\'filename.csv(or xml)!");
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void GetSaveToCsv(string fileNameCsv)
        {
            using StreamWriter streamWriter = new (fileNameCsv);
            this.fileCabinetServiceSnapshot.SaveToCsv(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameCsv}.");
        }

        private void GetSaveToXml(string fileNameXml)
        {
            using StreamWriter streamWriter = new (fileNameXml);
            this.fileCabinetServiceSnapshot.SaveToXml(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameXml}.");
        }
    }
}
