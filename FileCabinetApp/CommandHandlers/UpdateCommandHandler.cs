using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.CreatePerson;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;
using FileCabinetApp.Services.Seach;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private readonly CreatingPerson creatingPerson;
        private List<FileCabinetRecord> fileCabinetRecords;

        public UpdateCommandHandler(IFileCabinetService service, string nameValidator)
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

            if (request.Command.ToLower() == Commands.UpdateName)
            {
                this.Update(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Update(string parameters)
        {
            try
            {
                FileCabinetSearchService searchService = new (this.service, Commands.UpdateName);
                string[] arrParameters = parameters.Trim().Split(Separators.Where, StringSplitOptions.RemoveEmptyEntries);
                if (arrParameters.Length == 1 || arrParameters[1] == " " || string.IsNullOrEmpty(arrParameters[1]))
                {
                    throw new ArgumentException("Incorrect call to update parameters!");
                }

                var newParameters = searchService.GetListParameters(arrParameters[0].Replace(Separators.Set, string.Empty, StringComparison.OrdinalIgnoreCase).Trim());
                string[] interimParameters = arrParameters[1].Trim().Split();
                this.fileCabinetRecords = searchService.GetRecordsList(interimParameters);
                StringBuilder stringBuilder = new ();
                for (int i = 0; i < this.fileCabinetRecords.Count; i++)
                {
                    this.GetRecordNewParameter(newParameters, this.fileCabinetRecords[i]);
                    if (i + 1 == this.fileCabinetRecords.Count)
                    {
                        stringBuilder.Append($"#{this.fileCabinetRecords[i].Id}");
                    }
                    else
                    {
                        stringBuilder.Append($"#{this.fileCabinetRecords[i].Id}, ");
                    }
                }

                if (this.fileCabinetRecords.Count == 1)
                {
                    Console.WriteLine($"Record {stringBuilder} is updated.");
                }
                else
                {
                    Console.WriteLine($"Records {stringBuilder} are updated.");
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void GetRecordNewParameter(IEnumerable<(string key, string value)> newParameters, FileCabinetRecord record)
        {
            string firstName = record.FirstName;
            string lastName = record.LastName;
            string dateOfBirth = record.DateOfBirth.ToString();
            string salary = record.Salary.ToString();
            string symbol = record.Symbol.ToString();
            foreach (var (key, value) in newParameters)
            {
                switch (key.ToLower())
                {
                    case CriteriaNames.FirstName:
                        firstName = value;
                        break;
                    case CriteriaNames.LastName:
                        lastName = value;
                        break;
                    case CriteriaNames.DateOfBirth:
                        dateOfBirth = value;
                        break;
                    case CriteriaNames.Salary:
                        salary = value;
                        break;
                    case CriteriaNames.Symbol:
                        symbol = value;
                        break;
                    default:
                        throw new ArgumentException(PrintException.IncorrectInput);
                }
            }

            var person = this.creatingPerson.AddPersonInsertAndUpdate(firstName, lastName, dateOfBirth, salary, symbol);
            this.service.UpdateRecord(record.Id, person);
        }
    }
}
