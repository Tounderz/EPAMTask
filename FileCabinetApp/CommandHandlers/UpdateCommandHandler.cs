using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable SA1214

namespace FileCabinetApp.CommandHandlers
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private List<FileCabinetRecord> fileCabinetRecords;
        private readonly string nameValidator;

        public UpdateCommandHandler(IFileCabinetService service, string nameValidator)
            : base(service)
        {
            this.nameValidator = nameValidator;
        }

        private void Update(string parameters)
        {
            try
            {
                string[] arrParameters = parameters.Trim().Split(ConstParameters.Where, StringSplitOptions.RemoveEmptyEntries);
                if (arrParameters.Length == 1 || arrParameters[1] == " " || string.IsNullOrEmpty(arrParameters[1]))
                {
                    throw new ArgumentException("Incorrect call to update parameters!");
                }

                var newParameters = SeachMethods.GetListParameters(arrParameters[0].Replace(ConstParameters.Set, string.Empty, StringComparison.OrdinalIgnoreCase).Trim());
                string[] interimParameters = arrParameters[1].Trim().Split();
                if (interimParameters.Length == 1 || interimParameters[0] == " " || interimParameters[0] == string.Empty)
                {
                    throw new ArgumentException("The minimum number of search criteria is one!");
                }

                Tuple<int, string[]> seachCountAnd = SeachMethods.SeachCountAnd(interimParameters);
                interimParameters = seachCountAnd.Item2;
                int countAnd = seachCountAnd.Item1;

                IEnumerable<(string key, string value)> seachParameters = SeachMethods.GetListParameters(string.Join(string.Empty, interimParameters));
                if (!seachParameters.Any())
                {
                    throw new ArgumentException($"Incorrect data entry for the search!");
                }

                this.fileCabinetRecords = SeachMethods.GetRecordsList(countAnd, seachParameters, this.service);

                StringBuilder stringBuilder = new ();
                if (this.fileCabinetRecords.Count <= 0)
                {
                    throw new ArgumentException("There is no record(s) with these search parameters!");
                }
                else if (this.fileCabinetRecords.Count == 1)
                {
                    foreach (var item in this.fileCabinetRecords)
                    {
                        this.GetRecordNewParameter(newParameters, item);
                        stringBuilder.Append($"# {item.Id}");
                    }

                    Console.WriteLine($"Update {stringBuilder} is record");
                }
                else
                {
                    foreach (var item in this.fileCabinetRecords)
                    {
                        this.GetRecordNewParameter(newParameters, item);
                        stringBuilder.Append($"# {item.Id}, ");
                    }

                    Console.WriteLine($"Update {stringBuilder} are record");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
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
                    case ConstParameters.FirstName:
                        firstName = value;
                        break;
                    case ConstParameters.LastName:
                        lastName = value;
                        break;
                    case ConstParameters.DateOfBirth:
                        dateOfBirth = value;
                        break;
                    case ConstParameters.Salary:
                        salary = value;
                        break;
                    case ConstParameters.Symbol:
                        symbol = value;
                        break;
                    default:
                        Console.WriteLine("This criterion is missing!");
                        break;
                }
            }

            var person = CreatingPerson.NewPersonInsert(this.nameValidator, firstName, lastName, dateOfBirth, salary, symbol);
            this.service.UpdateRecord(record.Id, person);
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.UpdateName)
            {
                this.Update(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
