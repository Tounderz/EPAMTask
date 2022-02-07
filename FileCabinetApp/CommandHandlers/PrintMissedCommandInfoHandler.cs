using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.ConstParameters;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class PrintMissedCommandInfoHandler : CommandHandlerBase
    {
        private static List<string> commandNames = new ()
        {
            Commands.HelpName,
            Commands.HelpFullName,
            Commands.StatName,
            Commands.ExitName,
            Commands.CreateName,
            Commands.InsertName,
            Commands.UpdateName,
            Commands.SelectName,
            Commands.ExportName,
            Commands.ImportName,
            Commands.DeleteName,
            Commands.PurgeName,
        };

        private static IEnumerable<string> similarCommands;

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            PrintMissedCommandInfo(request);
            return null;
        }

        private static void PrintMissedCommandInfo(AppCommandRequest request)
        {
            Console.WriteLine($"There is no '{request.Command}' command. See 'help'.");
            similarCommands = GetSimilarCommands(request);
            Print();
            Console.WriteLine();
        }

        private static void Print()
        {
            if (similarCommands.Count() == 1)
            {
                Console.WriteLine("The most similar command is:");
                Console.WriteLine("\t{0}", similarCommands.ToArray());
            }
            else if (similarCommands.Count() > 1)
            {
                Console.WriteLine("The most similar commands are:");
                foreach (var command in similarCommands)
                {
                    Console.WriteLine("\t{0}", command);
                }
            }
        }

        private static IEnumerable<string> GetSimilarCommands(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string requestCommandSymbols = request.Command.ToUpper();
            var commandsIntersactions = commandNames.Select(i => (i, i.ToUpper())).Select(j => (j.i, j.Item2.Intersect(requestCommandSymbols).Count()));
            int max = commandsIntersactions.Max(i => i.Item2);
            return max > 2 ? commandsIntersactions.Where(i => i.Item2.Equals(max)).Select(j => j.i) : Enumerable.Empty<string>();
        }
    }
}