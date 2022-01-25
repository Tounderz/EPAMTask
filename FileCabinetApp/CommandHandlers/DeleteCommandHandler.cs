using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600
#pragma warning disable S3220

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
                string[] arrParameters = parameters.Split(' ', '=');
                string nameParameter = arrParameters[1].ToLower();
                string valueParameter = arrParameters[2].Trim('\'', ' ').ToUpper();
                switch (nameParameter)
                {
                    case "id":
                        this.RemoveRecordById(int.Parse(valueParameter));
                        break;
                    case "firstname":
                        this.list = this.service.FindByFirstName(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    case "lastname":
                        this.list = this.service.FindByLastName(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    case "dateofbirth":
                        this.list = this.service.FindByDateOfBirth(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    case "age":
                        this.list = this.service.FindByAge(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    case "salary":
                        this.list = this.service.FindBySalary(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    case "symbol":
                        this.list = this.service.FindBySymbol(valueParameter).ToList();
                        this.DeleteRecordByParameter(valueParameter);
                        break;
                    default:
                        Console.WriteLine("This criterion, by which you delete, is missing!");
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentOutOfRangeException)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void RemoveRecordById(int id)
        {
            var record = this.service.GetRecords().ToList().Find(i => i.Id == id);
            if (record != null)
            {
                this.service.RemoveRecord(id);
                Console.WriteLine($"Record {id} is deleted.");
            }
            else
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
            }
        }

        private void DeleteRecordByParameter(string value)
        {
            StringBuilder stringBuilder = new ();
            int count = 0;
            for (int i = 0; i < this.list.Count; i++)
            {
                this.service.RemoveRecord(this.list[i].Id);
                stringBuilder.Append($"#{this.list[i].Id}, ");
                count++;
            }

            if (count == 0)
            {
                Console.WriteLine($"Record #{value} doesn't exists.");
            }
            else if (count == 1)
            {
                Console.WriteLine($"Record {stringBuilder} is deleted.");
            }
            else
            {
                Console.WriteLine($"Record {stringBuilder} are deleted.");
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
