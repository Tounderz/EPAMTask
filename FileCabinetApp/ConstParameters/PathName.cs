using System;

#pragma warning disable S1075
#pragma warning disable SA1600

namespace FileCabinetApp.ConstParameters
{
    public static class PathName
    {
        public const string LoggerPathName = "serviceLogger.txt";
        public const string DBPathName = "cabinet-records.db";
        public const string ValidationRulesFileName = "validation-rules.json";
        public const string ValidationRulesPathName = @"C:\Users\basta\source\repos\EPAMTask\FileCabinetApp\Validators\";
    }
}
