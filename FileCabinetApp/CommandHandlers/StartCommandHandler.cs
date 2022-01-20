using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable S1172

namespace FileCabinetApp.CommandHandlers
{
    public class StartCommandHandler : ServiceCommandHandlerBase
    {
        public StartCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Start(string parameters) // вывод количества объектов в списке
        {
            var recordsCount = this.service.GetRecordsCount();
            Console.WriteLine($"{recordsCount.Item1} record(s).\n{recordsCount.Item2} delete record(s)");
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.StartName)
            {
                this.Start(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
