using System;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class AppCommandRequest
    {
        public string Command { get; set; }

        public string Parameters { get; set; }
    }
}
