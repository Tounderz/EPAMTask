using System;

#pragma warning disable SA1600

namespace FileCabinetApp.ConstParameters
{
    public static class ConstParametersFileService
    {
        public const int IsDelete = 1;
        public const int NotDelete = 0;
        public const int MaxLengthString = 120;
        public const int RecordLength = (MaxLengthString * 2) + (sizeof(int) * 4) + sizeof(char) + sizeof(decimal) + (sizeof(short) * 2) - 1;
    }
}
