using System;

#pragma warning disable SA1600

namespace FileCabinetApp.ConstParameters
{
    public static class CommandLineParameters
    {
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
    }
}
