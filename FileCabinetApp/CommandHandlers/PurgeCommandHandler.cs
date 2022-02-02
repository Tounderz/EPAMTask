using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600
#pragma warning disable S1172

namespace FileCabinetApp.CommandHandlers
{
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        public PurgeCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Purge(string parameters)
        {
            Tuple<int, int> tuple = this.service.PurgeRecord();
            Console.WriteLine($"Data file processing is completed: {tuple.Item1} of {tuple.Item2} records were purged.");
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.PurgeName)
            {
                this.Purge(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}