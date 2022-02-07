using System;
using System.IO;
using FileCabinetApp.ConstParameters;

#pragma warning disable SA1600

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

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.ExitName)
            {
                this.Exit(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Exit(string parameters)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameters))
                {
                    throw new ArgumentException("Incorrect command input!");
                }

                if (this.fileStream != null)
                {
                    this.fileStream.Dispose();
                }

                Console.WriteLine("Exiting an application...");
                this.isRunning(false);
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }
    }
}
