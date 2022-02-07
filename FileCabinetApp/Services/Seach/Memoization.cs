using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Services.Seach
{
    public class Memoization
    {
        private static Dictionary<string, List<FileCabinetRecord>> cacheDictionary = new ();
        private readonly List<FileCabinetRecord> records = new ();
        private List<FileCabinetRecord> interimRecords = new ();

        public static void AddRecordInCacheDictionary(List<FileCabinetRecord> records, string key)
        {
            if (!cacheDictionary.ContainsKey(key))
            {
                cacheDictionary.Add(key, new List<FileCabinetRecord>());
            }

            foreach (var item in records)
            {
                cacheDictionary[key].Add(item);
            }
        }

        public static void ClearCache()
        {
            cacheDictionary.Clear();
        }

        public List<FileCabinetRecord> SeachInCache(string key)
        {
            try
            {
                this.interimRecords.Clear();
                this.interimRecords = cacheDictionary[key];
                this.AddRecordInList(this.interimRecords);
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }

            return this.records;
        }

        private void AddRecordInList(List<FileCabinetRecord> fileCabinetRecords)
        {
            foreach (var item in fileCabinetRecords)
            {
                if (!this.records.Exists(i => i.Id == item.Id))
                {
                    this.records.Add(item);
                }
            }
        }
    }
}
