using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1202
#pragma warning disable SA1600
#pragma warning disable S1172
#pragma warning disable IDE0060

namespace FileCabinetApp.CommandHandlers
{
    public class ExitCommandHandler : CommandHandlerBase
    {
        private readonly Action<bool> isRunning;
        private readonly FileStream fileStream;

        public ExitCommandHandler(Action<bool> isRunning, FileStream fileStream)
        {
            this.isRunning = isRunning;
            this.fileStream = fileStream;
        }

        private void Exit(string parameters)
        {
            if (this.fileStream != null)
            {
                this.fileStream.Dispose();
            }

            Console.WriteLine("Exiting an application...");
            this.isRunning(false);
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.ExitName)
            {
                this.Exit(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
