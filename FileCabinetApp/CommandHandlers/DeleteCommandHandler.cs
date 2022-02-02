using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private List<FileCabinetRecord> list;

        public DeleteCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Delete(string parameters)
        {
            try
            {
                string[] arrParameters = parameters.Trim().Split(ConstParameters.Where, StringSplitOptions.RemoveEmptyEntries);
                string[] interimParameters = arrParameters[0].Trim().Split();

                this.list = SeachMethods.GetRecordsList(interimParameters, this.service, ConstParameters.DeleteName);
                StringBuilder stringBuilder = new ();
                for (int i = 0; i < this.list.Count; i++)
                {
                    this.service.DeleteRecord(this.list[i].Id);
                    if (i + 1 == this.list.Count)
                    {
                        stringBuilder.Append($"#{this.list[i].Id}");
                    }
                    else
                    {
                        stringBuilder.Append($"#{this.list[i].Id}, ");
                    }
                }

                if (this.list.Count == 1)
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
                ConstParameters.PrintException(ex);
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.DeleteName)
            {
                this.Delete(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
