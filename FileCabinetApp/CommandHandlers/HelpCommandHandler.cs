using System;
using FileCabinetApp.ConstParameters;

#pragma warning disable SA1600

namespace FileCabinetApp.CommandHandlers
{
    public class HelpCommandHandler : CommandHandlerBase
    {
        private static string[][] helpMessages = new string[][]
        {
            new string[] { Commands.HelpName, "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { Commands.HelpFullName, "prints the full help screen", "The 'full help' command prints the help screen." },
            new string[] { Commands.ExitName, "exits the application", "The 'exit' command exits the application." },
            new string[] { Commands.StatName, "stat the application", "The 'stat' command outputs the amount of data in the shee" },
            new string[] { Commands.CreateName, "create a record", "The 'create' command will create a new record in the sheet." },
            new string[]
            {
                Commands.SelectName, "The 'select' command searches by certain criteria.",
                "The 'select' command searches by certain criteria. " +
                "Command example (minimum of two criteria, maximum of seven): select parameterName where parameterName='parameterValue' and(or) parameterName='parameterValue' (specify search criteria via 'and(or)'). " +
                "Command example (to output all records): select all",
            },
            new string[] { Commands.ExportName, "writing to a 'csv or xml' file", "The 'export' command writes a file in csv or xml format" },
            new string[] { Commands.ImportName, "importing records from a format file 'csv' or 'xml'", "The import command imports data from files in 'csv' or 'xml' format" },
            new string[] { Commands.PurgeName, "Removes 'voids' in the data file", "The command defragments the data file - removes 'voids' in the data file formed by records with the IsDeleted bit set." },
            new string[]
            {
                Commands.InsertName, "insert(add) a record",
                "The 'insert' command is to add a record using the transmitted data. Inserts a record using the transmitted data. " +
                "Command example (the order in the first parentheses must match the order given after values): " +
                "insert (dateofbirth,lastname,firstname,id,salary,symbol) " +
                "values ('value dateofbirth','value lastname','value firstname','value id', 'value salary', 'value symbol') ",
            },
            new string[] { Commands.DeleteName, "delete record by parameter", "The 'delete' command record by parameter. Command example: delete where parameterName='parameterValue'." },
            new string[]
            {
                Commands.UpdateName, "update record by parameter",
                "The 'update' command allows you to update data by parameter(s). " +
                "Command example: update set parameterName='parameterValue' (parameterNames: firstname, lastname, dateofbirth, salary, symbol) " +
                "where (search criteria for records to change, you can specify several through the 'and' operator) parameterName='parameterValue' (parameterNames: id, firstname, lastname, dateofbirth, age, salary, symbol)",
            },
        };

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.HelpName)
            {
                PrintHelp(request.Parameters, HelpIndex.DescriptionHelpIndex);
                return null;
            }
            else if (request.Command.ToLower() == Commands.HelpFullName)
            {
                PrintHelp(request.Parameters, HelpIndex.ExplanationHelpIndex);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void PrintHelp(string parameters, int indexHelpMessage)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, (Predicate<string[]>)(i => string.Equals(i[HelpIndex.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase)));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][HelpIndex.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[HelpIndex.CommandHelpIndex], helpMessage[indexHelpMessage]);
                }
            }

            Console.WriteLine();
        }
    }
}