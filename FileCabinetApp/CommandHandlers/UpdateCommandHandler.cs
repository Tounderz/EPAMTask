using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable SA1214
#pragma warning disable SA1204

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
                StringBuilder stringBuilder = new ();
                string[] arrParameters = parameters.Trim().Split("where", StringSplitOptions.RemoveEmptyEntries);
                var newParameters = GetListParameters(arrParameters[0].Replace("set", string.Empty, StringComparison.OrdinalIgnoreCase).Trim());
                string[] interimParameters = arrParameters[1].Trim().Split();
                IEnumerable<(string key, string value)> seachParameters;
                for (int i = 0; i < interimParameters.Length; i++)
                {
                    if (ConstParameters.UpdateAnd.Contains(interimParameters[i]))
                    {
                        interimParameters[i] = ",";
                    }
                }

                seachParameters = GetListParameters(string.Join(string.Empty, interimParameters));
                this.fileCabinetRecords = this.service.GetRecords().ToList();
                if (!seachParameters.Any())
                {
                    throw new ArgumentException("Invalid criteria for the 'where' field!");
                }
                else
                {
                    foreach (var (key, value) in seachParameters)
                    {
                        this.GetRecordsListSeach(key, value);
                    }
                }

                if (this.fileCabinetRecords.Count <= 0)
                {
                    throw new ArgumentException("There is no record with these search parameters!");
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
                if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is IndexOutOfRangeException || ex is ArgumentException)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static IEnumerable<(string key, string value)> GetListParameters(string parameters)
        {
            var result = new List<(string key, string value)>();
            string[] arrKeyValue = parameters.Split(',');
            foreach (var item in arrKeyValue)
            {
                string[] interim = item.Split('=');
                result.Add((interim[0].Trim(), interim[1].Trim('\'', ' ')));
            }

            return result;
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
                    case "firstname":
                        firstName = value;
                        break;
                    case "lastname":
                        lastName = value;
                        break;
                    case "dateofbirth":
                        dateOfBirth = value;
                        break;
                    case "salary":
                        salary = value;
                        break;
                    case "symbol":
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

        private void GetRecordsListSeach(string key, string value)
        {
            switch (key)
            {
                case "id":
                    this.fileCabinetRecords.RemoveAll(i => i.Id != int.Parse(value));
                    break;
                case "firstname":
                    this.fileCabinetRecords.RemoveAll(i => i.FirstName.ToLower() != value);
                    break;
                case "lastname":
                    this.fileCabinetRecords.RemoveAll(i => i.LastName.ToLower() != value);
                    break;
                case "dateofbirth":
                    this.fileCabinetRecords.RemoveAll(i => i.DateOfBirth != DateTime.Parse(value));
                    break;
                case "age":
                    this.fileCabinetRecords.RemoveAll(i => i.Age != short.Parse(value));
                    break;
                case "salary":
                    this.fileCabinetRecords.RemoveAll(i => i.Salary != decimal.Parse(value));
                    break;
                case "symbol":
                    this.fileCabinetRecords.RemoveAll(i => i.Symbol != char.Parse(value));
                    break;
                default:
                    Console.WriteLine("This criterion is missing!");
                    break;
            }
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
