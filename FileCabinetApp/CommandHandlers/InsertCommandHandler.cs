﻿using System;
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
                var records = this.service.GetRecords();
                if (valueList.Length != 6 || parametersList.Length != 6 || records.Any(i => i.Id == id))
                {
                    if (valueList.Length != 6 || parametersList.Length != 6)
                    {
                        Console.WriteLine("Incorrect input. It is required to pass 6 fields.");
                    }
                    else
                    {
                        Console.WriteLine("This id is busy, try again by specifying a non-existing id");
                    }
                }
                else
                {
                    for (int i = 0; i < parametersList.Length; i++)
                    {
                        switch (parametersList[i])
                        {
                            case "firstname":
                                firstName = valueList[i];
                                break;
                            case "lastname":
                                lastName = valueList[i];
                                break;
                            case "dateofbirth":
                                dateOfBirth = valueList[i];
                                break;
                            case "salary":
                                salary = valueList[i];
                                break;
                            case "symbol":
                                symbol = valueList[i];
                                break;
                            default:
                                break;
                        }
                    }

                    var person = CreatingPerson.NewPersonInsert(this.nameValidator, firstName, lastName, dateOfBirth, salary, symbol);
                    this.service.InsertRecord(id, person);
                    Console.WriteLine($"Record # {id} is created");
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is IndexOutOfRangeException || ex is ArgumentNullException)
                {
                    Console.WriteLine(ex.Message);
                }
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