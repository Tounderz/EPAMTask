using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Validators;

#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp.CommandHandlers
{
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordValidator recordValidator;

        public CreateCommandHandler(IFileCabinetService service, IRecordValidator recordValidator)
            : base(service)
        {
            this.recordValidator = recordValidator;
        }

        private void Create(string parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var person = CreatingPerson.NewPerson();
            if (!this.recordValidator.ValidateParameters(person).Item1)
            {
                Console.WriteLine(this.recordValidator.ValidateParameters(person).Item2);
            }
            else
            {
                Console.WriteLine($"Record # {this.service.CreateRecord(person)} is created.");
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.CreateName)
            {
                this.Create(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
