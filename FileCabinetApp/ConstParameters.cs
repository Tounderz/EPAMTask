﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public const string DefaultValidatorName = "defaul";
        public const string CustomValidatorName = "custom";
        public const string MemoryServiceName = "memory";
        public const string FileServiceName = "file";

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

        public const int MinStringLength = 2;
        public const int MaxStringDefaultLength = 60;
        public const int MaxStringCustomLength = 30;
        public const int MinSalaryCustom = 500;
        public const int MaxSalaryCustom = 1000000;
        public const int MinSalaryDefault = 0;
        public const int MaxSalaryDefault = int.MaxValue;
        public const int SymbolLength = 1;

        public const string FormatDate = "yyyy-MMM-dd";
        public const int IsDelete = 1;
        public const int NotDelete = 0;
        public const int MaxLengthString = 120;
        public const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;

        public static readonly DateTime MinDateDafault = new (1950, 01, 01);
        public static readonly DateTime MaxDateDafault = DateTime.Now;
        public static readonly DateTime MinDateCustom = new (1980, 01, 01);
        public static readonly DateTime MaxDateCustom = new (2015, 12, 31);

        public const string ColumnNames = "Id,First Name,Last Name,Date of birth,Age,Salary,Symbol";
    }
}
