using System;
using FileCabinetApp.CommandHandlers;

#pragma warning disable SA1600

namespace FileCabinetApp.Interfaces
{
    public interface ICommandHandler
    {
        ICommandHandler SetNext(ICommandHandler handler);

        AppCommandRequest Handle(AppCommandRequest request);
    }
}
