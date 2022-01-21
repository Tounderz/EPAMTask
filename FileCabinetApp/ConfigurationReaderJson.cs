using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

#pragma warning disable SA1600

namespace FileCabinetApp
{
    public class ConfigurationReaderJson
    {
        private readonly IConfiguration configuration;
        private readonly string nameValidator;

        public ConfigurationReaderJson(string nameValidator)
        {
            this.nameValidator = nameValidator;
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(ConstParameters.ValidationRulesPathName)
                .AddJsonFile(ConstParameters.ValidationRulesFileName)
                .Build();
        }

        public Tuple<int, int> FirstNameCreterios()
        {
            int firstNameMinLength = this.configuration.GetSection(this.nameValidator).GetSection("firstName").GetValue<int>("min");
            int firstNameMaxLength = this.configuration.GetSection(this.nameValidator).GetSection("firstName").GetValue<int>("max");

            return new Tuple<int, int>(firstNameMinLength, firstNameMaxLength);
        }

        public Tuple<int, int> LastNameCreterios()
        {
            int lastNameMinLength = this.configuration.GetSection(this.nameValidator).GetSection("lastName").GetValue<int>("min");
            int lastNameMaxLength = this.configuration.GetSection(this.nameValidator).GetSection("lastName").GetValue<int>("max");

            return new Tuple<int, int>(lastNameMinLength, lastNameMaxLength);
        }

        public Tuple<DateTime, DateTime> DateOfBirthCriterions()
        {
            var dateOfBirthMin = this.configuration.GetSection(this.nameValidator).GetSection("dateOfBirth").GetValue<DateTime>("from");
            var dateOfBirthMax = this.configuration.GetSection(this.nameValidator).GetSection("dateOfBirth").GetValue<DateTime>("to");

            return Tuple.Create(dateOfBirthMin, dateOfBirthMax);
        }

        public Tuple<int, int> SalaryCreterios()
        {
            int salatyMin = this.configuration.GetSection(this.nameValidator).GetSection("salary").GetValue<int>("min");
            int salaryMax = this.configuration.GetSection(this.nameValidator).GetSection("salary").GetValue<int>("max");

            return new Tuple<int, int>(salatyMin, salaryMax);
        }

        public int SymbolCreterios()
        {
            return this.configuration.GetSection(this.nameValidator).GetSection("symbol").GetValue<int>("length");
        }
    }
}
