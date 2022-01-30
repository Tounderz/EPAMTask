using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp.CommandHandlers
{
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Action<IEnumerable<FileCabinetRecord>> action;

        public FindCommandHandler(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> action)
            : base(service) => this.action = action;

        private void Find(string parameters) // поиск всех одинаковых данных одного из полей, при помощи словаря
        {
            try
            {
                string[] arrParameters = parameters.Split(' ');
                string searchСriteria = arrParameters[0].ToLower();
                string nameParameter = arrParameters[1].Trim('"').ToUpper();
                IEnumerable<FileCabinetRecord> fileCabinetRecords;
                switch (searchСriteria)
                {
                    case ConstParameters.FirstName:
                        {
                            if (this.service.GetRecords().FirstOrDefault(i => i.FirstName.ToLower() == nameParameter.ToLower()) == null)
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

                    case ConstParameters.LastName:
                        {
                            if (this.service.GetRecords().FirstOrDefault(i => i.LastName.ToLower() == nameParameter.ToLower()) == null)
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

                    case ConstParameters.DateOfBirth:
                        {
                            if (this.service.GetRecords().FirstOrDefault(i => i.DateOfBirth.ToString("yyyy-MMM-dd").ToLower() == nameParameter.ToLower()) == null)
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

                    case ConstParameters.Age:
                        if (this.service.GetRecords().FirstOrDefault(i => i.Age == int.Parse(nameParameter)) == null)
                        {
                            Console.WriteLine("Specify the search criteria");
                            break;
                        }
                        else
                        {
                            fileCabinetRecords = this.service.FindByAge(nameParameter);
                            this.action(fileCabinetRecords);
                            break;
                        }

                    case ConstParameters.Salary:
                        if (this.service.GetRecords().FirstOrDefault(i => i.Salary == decimal.Parse(nameParameter)) == null)
                        {
                            Console.WriteLine("Specify the search criteria");
                            break;
                        }
                        else
                        {
                            fileCabinetRecords = this.service.FindBySalary(nameParameter);
                            this.action(fileCabinetRecords);
                            break;
                        }

                    case ConstParameters.Symbol:
                        if (this.service.GetRecords().FirstOrDefault(i => i.Symbol == char.Parse(nameParameter)) == null)
                        {
                            Console.WriteLine("Specify the search criteria");
                            break;
                        }
                        else
                        {
                            fileCabinetRecords = this.service.FindBySymbol(nameParameter);
                            this.action(fileCabinetRecords);
                            break;
                        }

                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentException || ex is ArgumentOutOfRangeException)
                {
                    Console.WriteLine(ex.Message);
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