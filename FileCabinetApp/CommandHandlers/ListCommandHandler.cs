using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1450
#pragma warning disable SA1214
#pragma warning disable SA1202
#pragma warning disable S1172
#pragma warning disable IDE0060

namespace FileCabinetApp.CommandHandlers
{
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords;
        private readonly Action<IEnumerable<FileCabinetRecord>> action;

        public ListCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> action)
            : base(service)
        {
            this.action = action;
        }

        private void List(string parameters)
        {
            this.fileCabinetRecords = this.service.GetRecords();
            this.action(this.fileCabinetRecords);
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.ListName)
            {
                this.List(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
