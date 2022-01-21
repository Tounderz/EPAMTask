using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1075

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

        public const string Help = "help";
        public const string StartName = "start";
        public const string ListName = "list";
        public const string ExitName = "exit";
        public const string CreateName = "create";
        public const string EditName = "edit";
        public const string FindName = "find";
        public const string ExportName = "export";
        public const string ImportName = "import";
        public const string RemoveName = "remove";
        public const string PurgeName = "purge";

        public const string FormatDate = "yyyy-MMM-dd";
        public const int IsDelete = 1;
        public const int NotDelete = 0;
        public const int MaxLengthString = 120;
        public const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;

        public const string ColumnNames = "Id,First Name,Last Name,Date of birth,Age,Salary,Symbol";

        public const string ValidationRulesPathName = @"C:\Users\basta\source\repos\EPAMTask\FileCabinetApp\";

        public const string LoggerPathName = "serviceLogger.txt";
        public const string DBPathName = "cabinet-records.db";
        public const string ValidationRulesFileName = "validation-rules.json";
    }
}
