using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp
{
    public static class SeachMethods
    {
        private static IFileCabinetService service;
        private static List<FileCabinetRecord> records = new ();
        private static List<FileCabinetRecord> interimRecords = new ();

        public static IEnumerable<(string key, string value)> GetListParameters(string parameters)
        {
            var result = new List<(string key, string value)>();
            string[] arrKeyValue = parameters.Split(',');
            foreach (var item in arrKeyValue)
            {
                string[] interim = item.Split('=');
                result.Add((interim[0].Trim().ToLower(), interim[1].Trim('\'', ' ')));
            }

            return result;
        }

        public static Tuple<int, string[]> SeachCountAnd(string[] interimParameters)
        {
            int countAnd = 0;
            for (int i = 0; i < interimParameters.Length; i++)
            {
                if (ConstParameters.And.Contains(interimParameters[i]))
                {
                    interimParameters[i] = ConstParameters.Comma;
                    countAnd++;
                }
                else if (ConstParameters.Or.Contains(interimParameters[i]))
                {
                    interimParameters[i] = ConstParameters.Comma;
                }
            }

            return new Tuple<int, string[]>(countAnd, interimParameters);
        }

        private static void Find(string key, string value) // поиск всех одинаковых данных одного из полей, при помощи словаря
        {
            try
            {
                interimRecords.Clear();
                switch (key)
                {
                    case ConstParameters.Id:
                        int id = int.Parse(value);
                        records.Add(service.FindById(id));
                        break;

                    case ConstParameters.FirstName:
                        interimRecords = service.FindByFirstName(value.ToUpper()).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.LastName:
                        interimRecords = service.FindByLastName(value.ToUpper()).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.DateOfBirth:
                        DateTime dateOfBirth = DateTime.Parse(value);
                        interimRecords = service.FindByDateOfBirth(dateOfBirth.ToString(ConstParameters.FormatDate).ToUpper()).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Age:
                        short age = short.Parse(value);
                        interimRecords = service.FindByAge(age.ToString()).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Salary:
                        decimal salary = decimal.Parse(value);
                        interimRecords = service.FindBySalary(salary.ToString()).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Symbol:
                        char symbol = char.Parse(value);
                        interimRecords = service.FindBySymbol(symbol.ToString().ToUpper()).ToList();
                        AddRecordInList(interimRecords);
                        break;
                    default:
                        throw new ArgumentException("Incorrect input!");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }
        }

        private static void SortingByMultipleCriteria(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case ConstParameters.Id:
                        int id = int.Parse(value);
                        records = records.Where(i => i.Id == id).ToList();
                        break;

                    case ConstParameters.FirstName:
                        records = records.Where(i => i.FirstName.ToLower() == value.ToLower()).ToList();
                        break;

                    case ConstParameters.LastName:
                        records = records.Where(i => i.LastName.ToLower() == value.ToLower()).ToList();
                        break;

                    case ConstParameters.DateOfBirth:
                        DateTime dateOfBirth = DateTime.Parse(value);
                        records = records.Where(i => i.DateOfBirth == dateOfBirth).ToList();
                        break;

                    case ConstParameters.Age:
                        short age = short.Parse(value);
                        records = records.Where(i => i.Age == age).ToList();
                        break;

                    case ConstParameters.Salary:
                        decimal salary = decimal.Parse(value);
                        records = records.Where(i => i.Salary == salary).ToList();
                        break;

                    case ConstParameters.Symbol:
                        char symbol = char.Parse(value);
                        records = records.Where(i => i.Symbol == symbol).ToList();
                        break;
                    default:
                        throw new ArgumentException("Incorrect input!");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }
        }

        public static List<FileCabinetRecord> GetRecordsList(int count, IEnumerable<(string key, string value)> seachParameters, IFileCabinetService fileCabinetService)
        {
            records.Clear();
            service = fileCabinetService;
            foreach (var (key, value) in seachParameters)
            {
                Find(key, value);
            }

            if (count > 0)
            {
                foreach (var (key, value) in seachParameters)
                {
                    SortingByMultipleCriteria(key, value);
                }
            }

            return records;
        }

        private static void AddRecordInList(List<FileCabinetRecord> fileCabinetRecords)
        {
            foreach (var item in fileCabinetRecords)
            {
                if (!records.Exists(i => i.Id == item.Id))
                {
                    records.Add(item);
                }
            }
        }
    }
}
