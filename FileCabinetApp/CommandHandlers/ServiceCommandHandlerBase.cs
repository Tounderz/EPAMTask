using System;
using FileCabinetApp.Interfaces;

#pragma warning disable SA1600
#pragma warning disable SA1401

namespace FileCabinetApp.CommandHandlers
{
    public class ServiceCommandHandlerBase : CommandHandlerBase
    {
        protected readonly IFileCabinetService service;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }
    }
}
