using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public static class Memoization
    {
        private static List<FileCabinetRecord> records = new ();
        private static List<FileCabinetRecord> interimRecords = new ();
        private static List<FileCabinetRecord> cacheList = new ();

        public static void AddCache(List<FileCabinetRecord> records)
        {
            foreach (var item in records)
            {
                if (!cacheList.Exists(i => i.Id == item.Id))
                {
                    cacheList.Add(item);
                }
            }
        }

        public static List<FileCabinetRecord> SeachInCache(string key, string value)
        {
            try
            {
                interimRecords.Clear();
                switch (key)
                {
                    case ConstParameters.Id:
                        int id = int.Parse(value);
                        records.Add(cacheList.FirstOrDefault(i => i.Id == id));
                        break;

                    case ConstParameters.FirstName:
                        interimRecords = cacheList.Where(i => i.FirstName.ToLower() == value).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.LastName:
                        interimRecords = cacheList.Where(i => i.LastName.ToLower() == value).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.DateOfBirth:
                        DateTime dateOfBirth = DateTime.Parse(value);
                        interimRecords = cacheList.Where(i => i.DateOfBirth == dateOfBirth).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Age:
                        short age = short.Parse(value);
                        interimRecords = cacheList.Where(i => i.Age == age).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Salary:
                        decimal salary = decimal.Parse(value);
                        interimRecords = cacheList.Where(i => i.Salary == salary).ToList();
                        AddRecordInList(interimRecords);
                        break;

                    case ConstParameters.Symbol:
                        char symbol = char.Parse(value);
                        interimRecords = cacheList.Where(i => i.Symbol == symbol).ToList();
                        AddRecordInList(interimRecords);
                        break;
                    default:
                        throw new ArgumentException(ConstParameters.IncorrectInput);
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }

            return records;
        }

        public static void ClearCache()
        {
            cacheList.Clear();
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
