using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable S1450
#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp.CommandHandlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private FileCabinetServiceSnapshot fileCabinetServiceSnapshot;

        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
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
                    case ConstParameters.CsvType:
                        this.GetSaveToCsv(fileName);
                        break;
                    case ConstParameters.XmlType:
                        this.GetSaveToXml(fileName);
                        break;
                    default:
                        throw new ArgumentException("Incorrect input! 1. export csv(or xml) filename.csv(or xml) ; 2. export csv(or xml) full address to the file'\'filename.csv(or xml)!");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.ExportName)
            {
                this.Export(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void GetSaveToCsv(string fileNameCsv)
        {
            using StreamWriter streamWriter = new (fileNameCsv);
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
            this.fileCabinetServiceSnapshot.SaveToCsv(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameCsv}.");
        }

        private void GetSaveToXml(string fileNameXml)
        {
            using StreamWriter streamWriter = new (fileNameXml);
            this.fileCabinetServiceSnapshot = this.service.MakeSnapshot();
            this.fileCabinetServiceSnapshot.SaveToXml(streamWriter);
            streamWriter.Close();
            Console.WriteLine($"All records are exported to file {fileNameXml}.");
        }
    }
}
