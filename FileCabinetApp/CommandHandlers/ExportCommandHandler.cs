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
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Specify the search criteria");
            }
            else
            {
                string[] parameterArray = parameters.Split();
                string fileType = parameterArray[0];
                string fileName = parameterArray.Last();

                switch (fileType.ToLower())
                {
                    case "csv":
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                Console.Write($"File is exist - rewrite {fileName}? [Y/n] ");
                                string checkToRewrite = Console.ReadLine().ToLower();
                                if (checkToRewrite == "y")
                                {
                                    this.GetSaveToCsv(fileName);
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                this.GetSaveToCsv(fileName);
                                break;
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine($"Export failed: can't open file {parameters}");
                        }

                        break;

                    case "xml":
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                Console.Write($"File is exist - rewrite {fileName}? [Y/n] ");
                                string checkToRewrite = Console.ReadLine().ToLower();
                                if (checkToRewrite == "y")
                                {
                                    this.GetSaveToXml(fileName);
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                this.GetSaveToXml(fileName);
                                break;
                            }
                        }
                        catch (DirectoryNotFoundException)
                        {
                            Console.WriteLine($"Export failed: can't open file {parameters}");
                        }

                        break;

                    default:
                        Console.WriteLine("Incorrect input! 1. export csv (or xml) filename.csv ; 2. export csv (or xml) full address to the file'\'filename.csv");
                        break;
                }
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
