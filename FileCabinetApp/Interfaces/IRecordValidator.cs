using System;
using FileCabinetApp.Models;

#pragma warning disable SA1600

namespace FileCabinetApp.Interfaces
{
    public interface IRecordValidator
    {
        void ValidateParameters(PersonModel person);
    }
}
