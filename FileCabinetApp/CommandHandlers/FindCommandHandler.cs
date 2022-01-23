using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable S1450
#pragma warning disable SA1214

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private ReadOnlyCollection<FileCabinetRecord> fileCabinetRecords;
        private readonly Action<IEnumerable<FileCabinetRecord>> action;

        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> action)
            : base(service)
        {
            this.action = action;
        }

        private void Find(string parameters) // поиск всех одинаковых данных одного из полей, при помощи словаря
        {
            if (string.IsNullOrEmpty(parameters))
            {
                Console.WriteLine("Specify the search criteria");
            }
            else
            {
                string[] arrParameters = parameters.Split(' ');
                string searchСriteria = arrParameters[0].ToLower();
                string nameParameter = arrParameters[1].Trim('"').ToUpper();
                IEnumerable<FileCabinetRecord> fileCabinetRecords;
                switch (searchСriteria)
                {
                    case "firstname":
                        {
                            if (string.IsNullOrEmpty(nameParameter) || this.service.GetRecords().FirstOrDefault(i => i.FirstName.ToLower() == nameParameter.ToLower()) == null)
                            {
                                Console.WriteLine("Specify the search criteria");
                                break;
                            }
                            else
                            {
                                fileCabinetRecords = this.service.FindByFirstName(nameParameter);
                                this.action(fileCabinetRecords);
                                break;
                            }
                        }

                    case "lastname":
                        {
                            if (string.IsNullOrEmpty(nameParameter) || this.service.GetRecords().FirstOrDefault(i => i.LastName.ToLower() == nameParameter.ToLower()) == null)
                            {
                                Console.WriteLine("Specify the search criteria");
                                break;
                            }
                            else
                            {
                                fileCabinetRecords = this.service.FindByLastName(nameParameter);
                                this.action(fileCabinetRecords);
                                break;
                            }
                        }

                    case "dateofbirth":
                        {
                            if (string.IsNullOrEmpty(nameParameter) || (this.service.GetRecords().FirstOrDefault(i => i.DateOfBirth.ToString("yyyy-MMM-dd").ToLower() == nameParameter.ToLower()) == null))
                            {
                                Console.WriteLine("You didn't enter the search parameter or you entered it incorrectly. It takes a year (xxxx), a month (the first three letters), a day with two digits if the date is less than 10, then add 0 (xx)");
                                break;
                            }
                            else
                            {
                                fileCabinetRecords = this.service.FindByDateOfBirth(nameParameter);
                                this.action(fileCabinetRecords);
                                break;
                            }
                        }

                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.FindName)
            {
                this.Find(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
