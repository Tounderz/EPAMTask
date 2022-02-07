using System;
using System.IO;

#pragma warning disable SA1600

namespace FileCabinetApp.ConstParameters
{
    public static class PrintException
    {
        public const string IncorrectInput = "This criterion is missing!";

        public static void Print(Exception ex)
        {
            if (ex is FormatException || ex is ArgumentNullException || ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is IndexOutOfRangeException || ex is DirectoryNotFoundException)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
