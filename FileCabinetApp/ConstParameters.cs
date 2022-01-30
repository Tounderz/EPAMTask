using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable S1075
#pragma warning disable SA1600

namespace FileCabinetApp
{
    public static class ConstParameters
    {
        public const string DeveloperName = "Dmitry Grudinsky";
        public const string HintMessage = "Enter your command, or enter 'help' to get help.";

        public const int CommandHelpIndex = 0;
        public const int DescriptionHelpIndex = 1;
        public const int ExplanationHelpIndex = 2;

        public const string DefaultValidatorName = "default";
        public const string CustomValidatorName = "custom";
        public const string MemoryServiceName = "memory";
        public const string FileServiceName = "file";
        public const string StopWatchLineParameter = "-use-stopwatch";
        public const string LoggerLineParameter = "-use-logger";
        public const string LongValidatorLineParameter = "--validation-rules";
        public const string ShortValidatorLineParameter = "-v";
        public const string LongTypeLineParameter = "--storage";
        public const string ShortTypeLineParameter = "-s";

        public const string HelpName = "help";
        public const string HelpFullName = "helpfull";
        public const string StatName = "stat";
        public const string ListName = "list";
        public const string ExitName = "exit";
        public const string CreateName = "create";
        public const string InsertName = "insert";
        public const string UpdateName = "update";
        public const string FindName = "find";
        public const string ExportName = "export";
        public const string ImportName = "import";
        public const string DeleteName = "delete";
        public const string PurgeName = "purge";
        public const string SelectName = "select";

        public const string FormatDate = "yyyy-MMM-dd";
        public const int IsDelete = 1;
        public const int NotDelete = 0;
        public const int MaxLengthString = 120;
        public const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;

        public const string CsvType = "csv";
        public const string XmlType = "xml";

        public const string ColumnNames = "Id,First Name,Last Name,Date of birth,Age,Salary,Symbol";

        public const string ValidationRulesPathName = @"C:\Users\basta\source\repos\EPAMTask\FileCabinetApp\Validators\";

        public const string LoggerPathName = "serviceLogger.txt";
        public const string DBPathName = "cabinet-records.db";
        public const string ValidationRulesFileName = "validation-rules.json";

        public const string And = "and";
        public const string Or = "or";
        public const string Where = "where";
        public const string Set = "set";
        public const string AllParametersPrint = "all";

        public const string Id = "id";
        public const string FirstName = "firstname";
        public const string LastName = "lastname";
        public const string DateOfBirth = "dateofbirth";
        public const string Age = "age";
        public const string Salary = "salary";
        public const string Symbol = "symbol";
        public const string ColumnId = "Id";
        public const string ColumnFirstName = "FirstName";
        public const string ColumnLastName = "LastName";
        public const string ColumnDateOfBirth = "DateOfBirth";
        public const string ColumnAge = "Age";
        public const string ColumnSalary = "Salary";
        public const string ColumnSymbol = "Symbol";
        public const string Comma = ",";

        public const string SideLine = "|";
        public const string Corner = "+";
        public const char Line = '-';

        public static void PrintException(Exception ex)
        {
            if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is IndexOutOfRangeException || ex is DirectoryNotFoundException)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}