using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class AppCommandRequest
    {
        public string Command { get; set; }

        public string Parameters { get; set; }
    }
}
