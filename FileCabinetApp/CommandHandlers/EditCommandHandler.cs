using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1450
#pragma warning disable SA1202
#pragma warning disable SA1214

namespace FileCabinetApp.CommandHandlers
{
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords;
        private readonly string nameValidator;

        public EditCommandHandler(IFileCabinetService service, string nameValidator)
            : base(service)
        {
            this.nameValidator = nameValidator;
        }

        private void Edit(string parameters) // изменение данных по id
        {
            try
            {
                int id = int.Parse(parameters);
                this.fileCabinetRecords = this.service.GetRecords();
                if (!this.fileCabinetRecords.All(i => i.Id == id))
                {
                    Console.WriteLine($"#{id} record in not found. ");
                }
                else
                {
                    var person = CreatingPerson.NewPerson(this.nameValidator);
                    this.service.EditRecord(id, person);
                    Console.WriteLine($"Record #{id} is updated.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("The parameter must be numeric.");
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.EditName)
            {
                this.Edit(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
