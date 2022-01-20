using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
