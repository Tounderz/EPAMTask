using System;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        public StatCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.StatName)
            {
                this.Stat(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Stat(string parameters) // вывод количества объектов в списке
        {
            try
            {
                if (!string.IsNullOrEmpty(parameters))
                {
                    throw new ArgumentException("Incorrect command input!");
                }

                var recordsCount = this.service.GetRecordsCount();
                Console.WriteLine($"{recordsCount.Item1} record(s).\n{recordsCount.Item2} delete record(s)");
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }
    }
}
