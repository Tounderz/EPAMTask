using System;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.PurgeName)
            {
                this.Purge(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Purge(string parameters)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameters))
                {
                    throw new ArgumentException("Incorrect command input!");
                }

                ValueTuple<int, int> tuple = this.service.PurgeRecord();
                Console.WriteLine($"Data file processing is completed: {tuple.Item1} of {tuple.Item2} records were purged.");
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }
    }
}