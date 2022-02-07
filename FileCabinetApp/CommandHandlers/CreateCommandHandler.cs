using System;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.CreatePerson;
using FileCabinetApp.Interfaces;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private readonly CreatingPerson creatingPerson;

        public CreateCommandHandler(IFileCabinetService service, string nameValidator)
            : base(service)
        {
            this.creatingPerson = new CreatingPerson(nameValidator);
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.CreateName)
            {
                this.Create(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Create(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var person = this.creatingPerson.AddPerson();
            Console.WriteLine($"Record # {this.service.CreateRecord(person)} is created.");
        }
    }
}
