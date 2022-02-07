using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;
using FileCabinetApp.Services.Seach;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private List<FileCabinetRecord> records;

        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.DeleteName)
            {
                this.Delete(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Delete(string parameters)
        {
            try
            {
                FileCabinetSearchService searchService = new (this.service, Commands.DeleteName);
                string[] arrParameters = parameters.Trim().Split(Separators.Where, StringSplitOptions.RemoveEmptyEntries);
                string[] interimParameters = arrParameters[0].Trim().Split();

                this.records = searchService.GetRecordsList(interimParameters);
                StringBuilder stringBuilder = new ();
                for (int i = 0; i < this.records.Count; i++)
                {
                    this.service.DeleteRecord(this.records[i].Id);
                    if (i + 1 == this.records.Count)
                    {
                        stringBuilder.Append($"#{this.records[i].Id}");
                    }
                    else
                    {
                        stringBuilder.Append($"#{this.records[i].Id}, ");
                    }
                }

                if (this.records.Count == 1)
                {
                    Console.WriteLine($"Record {stringBuilder} is deleted.");
                }
                else
                {
                    Console.WriteLine($"Records {stringBuilder} are deleted.");
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }
    }
}
