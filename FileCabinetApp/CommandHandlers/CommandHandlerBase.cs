using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public ICommandHandler SetNext(ICommandHandler handler) // hardle - обработка ввода
        {
            this.nextHandler = handler;
            return handler;
        }

        public virtual AppCommandRequest Handle(AppCommandRequest request) // метод возращает запрос, request - запрос на ввод.
        {
            if (this.nextHandler != null)
            {
                return this.nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}
