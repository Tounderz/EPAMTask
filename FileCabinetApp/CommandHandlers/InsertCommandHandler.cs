using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable S3220

namespace FileCabinetApp.CommandHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private readonly string nameValidator;

        public InsertCommandHandler(IFileCabinetService service, string nameValidator)
            : base(service)
        {
            this.nameValidator = nameValidator;
        }

        private void Insert(string parameters)
        {
            try
            {
                string[] arrParameters = parameters.Split('(', ')');
                string[] parametersList = arrParameters[1].ToLower().Split(',').Select(i => i.Trim(' ')).ToArray();
                string[] valueList = arrParameters[3].Split(',').Select(i => i.Trim('\'', ' ')).ToArray();
                int index = Array.IndexOf(parametersList, "id");
                int id = int.Parse(valueList[index]);
                string firstName = string.Empty;
                string lastName = string.Empty;
                string dateOfBirth = string.Empty;
                string salary = string.Empty;
                string symbol = string.Empty;
                var record = this.service.FindById(id);
                if (valueList.Length != 6 || parametersList.Length != 6 || record != null)
                {
                    if (valueList.Length != 6 || parametersList.Length != 6)
                    {
                        throw new ArgumentException("Incorrect input. It is required to pass 6 fields.");
                    }
                    else
                    {
                        throw new ArgumentException("This id is busy, try again by specifying a non-existing id");
                    }
                }

                for (int i = 0; i < parametersList.Length; i++)
                {
                    switch (parametersList[i])
                    {
                        case ConstParameters.FirstName:
                            firstName = valueList[i];
                            break;
                        case ConstParameters.LastName:
                            lastName = valueList[i];
                            break;
                        case ConstParameters.DateOfBirth:
                            dateOfBirth = valueList[i];
                            break;
                        case ConstParameters.Salary:
                            salary = valueList[i];
                            break;
                        case ConstParameters.Symbol:
                            symbol = valueList[i];
                            break;
                        default:
                            Console.WriteLine("This criterion by which you add an entry is missing!");
                            break;
                    }

                    var person = CreatingPerson.NewPersonInsertAndUpdate(this.nameValidator, firstName, lastName, dateOfBirth, salary, symbol);
                    Memoization.ClearCache();
                    this.service.InsertRecord(id, person);
                    Console.WriteLine($"Record # {id} is created");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.InsertName)
            {
                this.Insert(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
