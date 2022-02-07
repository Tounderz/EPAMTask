using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Models;
using FileCabinetApp.Services.Seach;
using FileCabinetApp.Table;

#pragma warning disable SA1600
#pragma warning disable SA1214

namespace FileCabinetApp.CommandHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private List<FileCabinetRecord> records;
        private readonly FileCabinetSearchService searchService;
        private readonly TablePrinter tablePrinter;
        private readonly List<string> columnNamesTable = new ()
        {
            ColumnNames.ColumnId,
            ColumnNames.ColumnFirstName,
            ColumnNames.ColumnLastName,
            ColumnNames.ColumnDateOfBirth,
            ColumnNames.ColumnAge,
            ColumnNames.ColumnSalary,
            ColumnNames.ColumnSymbol,
        };

        public SelectCommandHandler(IFileCabinetService service)
            : base(service)
        {
            this.searchService = new (this.service, Commands.SelectName);
            this.tablePrinter = new ();
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == Commands.SelectName)
            {
                this.Select(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Select(string parameters)
        {
            try
            {
                // вывод в консоль всех записей, по всем полям, в цикле if
                if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters) || parameters.Trim() == Separators.AllParametersPrint)
                {
                    this.records = this.service.GetRecords().ToList();
                    this.Print(this.columnNamesTable);
                    return;
                }

                string[] arrParameters = parameters.Split(Separators.Where, StringSplitOptions.RemoveEmptyEntries);

                List<string> columnNamesSorted;
                if (arrParameters[0].Trim().ToLower() == Separators.AllParametersPrint)
                {
                    columnNamesSorted = this.columnNamesTable;
                }
                else
                {
                    string[] columnNames = arrParameters[0].Trim().Split(',').Select(i => i.Trim()).ToArray();
                    columnNamesSorted = this.columnNamesTable.Where(item => columnNames.Contains(item, StringComparer.OrdinalIgnoreCase)).ToList();
                    if (columnNamesSorted.Count == 0)
                    {
                        throw new ArgumentException("Incorrect criteria for output!");
                    }
                }

                if (arrParameters.Length == 1)
                {
                    this.records = this.service.GetRecords().ToList();
                    this.Print(columnNamesSorted);
                }
                else if (arrParameters.Length == 2)
                {
                    string[] interimParameters = arrParameters[1].Trim().Split();
                    this.records = this.searchService.GetRecordsList(interimParameters);
                    this.Print(columnNamesSorted);
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        // перед выводом в консоль проверяет наличие записей
        private void Print(List<string> columnNames)
        {
            if (this.records.Count > 0)
            {
                this.tablePrinter.CreateRow(columnNames, this.records);
            }
            else
            {
                throw new ArgumentException("The list is empty!");
            }
        }
    }
}
