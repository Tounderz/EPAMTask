using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable SA1202

namespace FileCabinetApp.CommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "stat the application", "The 'stat' command outputs the amount of data in the shee" },
            new string[] { "create", "create a record", "The 'create' command will create a new record in the sheet." },
            new string[] { "list", "output list", "Commands 'list' outputs list." },
            new string[] { "edit", "editor record by id", "The 'edit' command allows you to edit data by id." },
            new string[] { "find", "search model: find search parameter \"search criteria\"", "Search by parameters 'firstname or lastname or dateofbirth', search model: find search parameter \"search criteria\"." },
            new string[] { "export", "writing to a 'csv or xml' file", "The 'export' command writes a file in csv or xml format" },
            new string[] { "import", "importing records from a format file 'csv' or 'xml'", "The import command imports data from files in 'csv' or 'xml' format" },
            new string[] { "remove", "remove record by id.", "The 'remove' command record by id." },
            new string[] {"insert", "", "the 'insert' command is to add a record using the transmitted data.Inserts a record using the transmitted data." },
        };

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[ConstParameters.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ConstParameters.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[ConstParameters.CommandHelpIndex], helpMessage[ConstParameters.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.Help)
            {
                PrintHelp(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
