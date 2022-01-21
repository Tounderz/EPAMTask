using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600
#pragma warning disable S1450

namespace FileCabinetApp.CommandHandlers
{
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        private ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords;

        public RemoveCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Remove(string parameters)
        {
            try
            {
                bool checkId = int.TryParse(parameters, out int id);
                if (!checkId)
                {
                    Console.WriteLine("Incorrect format, enter a numeric value after remove");
                }

                this.fileCabinetRecords = this.service.GetRecords();
                for (int i = 0; i < this.fileCabinetRecords.Count; i++)
                {
                    if (id == this.fileCabinetRecords[i].Id)
                    {
                        this.service.RemoveRecord(id);
                        Console.WriteLine($"Record #{id} is removed");
                        checkId = true;
                        break;
                    }
                }

                if (!checkId)
                {
                    throw new ArgumentException($"Record #{id} doesn't exists.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.RemoveName)
            {
                this.Remove(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
