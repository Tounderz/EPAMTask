﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Validators
{
    public interface IRecordValidator
    {
        Tuple<bool, string> ValidateParameters(Person person);
    }
}