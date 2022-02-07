using System;

#pragma warning disable SA1600

namespace FileCabinetApp.ConstParameters
{
    public static class ColumnNames
    {
        public const string ColumnId = "Id";
        public const string ColumnFirstName = "FirstName";
        public const string ColumnLastName = "LastName";
        public const string ColumnDateOfBirth = "DateOfBirth";
        public const string ColumnAge = "Age";
        public const string ColumnSalary = "Salary";
        public const string ColumnSymbol = "Symbol";

        public const string StringColumnNamesCsv = "Id,First Name,Last Name,Date of birth,Age,Salary,Symbol";
    }
}
