﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable IDE0090

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            // добавьте реализацию метода
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "Can't be null");
            }

            if (firstName.Length < 2 || firstName.Length > 60 || firstName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Can't be null");
            }

            if (lastName.Length < 2 || lastName.Length > 60 || lastName.Contains(" "))
            {
                throw new ArgumentException("The size is from 2 to 60 characters and there should be no spaces", nameof(lastName));
            }

            if (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 01, 01))
            {
                throw new ArgumentException("Minimum date of birth 01/01/1950, and maximum - DateTime.Now", nameof(dateOfBirth));
            }

            if (symbol.ToString().Length != 1)
            {
                throw new ArgumentException("The size of the string character is equal to one element", nameof(symbol));
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Age = age,
                Salary = salary,
                Symbol = symbol,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            // добавьте реализацию метода
            return this.list.ToArray();
        }

        public int GetStat()
        {
            // добавьте реализацию метода
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short age, decimal salary, char symbol)
        {
            if (id > list.Count || id < 1)
            {
                throw new ArgumentException(null, nameof(id));
            }
            else
            {
                GetRecords();
            }
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> first = new List<FileCabinetRecord>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].FirstName.ToLower() == firstName.ToLower())
                {
                    first.Add(list[i]);
                }
            }

            return first.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            List<FileCabinetRecord> first = new List<FileCabinetRecord>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LastName.ToLower() == lastname.ToLower())
                {
                    first.Add(list[i]);
                }
            }

            return first.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(string date)
        {
            List<FileCabinetRecord> first = new List<FileCabinetRecord>();
            DateTime dateOfBirth = Convert.ToDateTime(date);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].DateOfBirth.ToString("yyyy-MMM-dd") == dateOfBirth.ToString("yyyy-MMM-dd"))
                {
                    first.Add(list[i]);
                }
            }

            return first.ToArray();
        }
    }
}
