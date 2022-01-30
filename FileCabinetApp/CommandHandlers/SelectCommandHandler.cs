using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileCabinetApp.Table;

#pragma warning disable SA1600
#pragma warning disable SA1202
#pragma warning disable S1450
#pragma warning disable SA1214

namespace FileCabinetApp.CommandHandlers
{
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        private List<FileCabinetRecord> fileCabinetRecords;
        private readonly List<string> columnNamesTable = new ()
        {
            ConstParameters.ColumnId,
            ConstParameters.ColumnFirstName,
            ConstParameters.ColumnLastName,
            ConstParameters.ColumnDateOfBirth,
            ConstParameters.ColumnAge,
            ConstParameters.ColumnSalary,
            ConstParameters.ColumnSymbol,
        };

        public SelectCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        private void Select(string parameters)
        {
            try
            {
                string[] arrParameters = parameters.Split(ConstParameters.Where, StringSplitOptions.RemoveEmptyEntries);

                // вывод всех записей в цикле if
                if (arrParameters.Length == 1 && arrParameters[0] == ConstParameters.AllParametersPrint)
                {
                    this.fileCabinetRecords = this.service.GetRecords().ToList();
                    FormationOfStrings.GetRow(this.columnNamesTable, this.fileCabinetRecords);
                    return;
                }

                List<string> columnNamesSorted;
                if (arrParameters[0].Trim().ToLower() == ConstParameters.AllParametersPrint)
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

                string[] interimParameters = arrParameters[1].Trim().Split();
                if (interimParameters.Length < 3)
                {
                    throw new ArgumentException("The minimum number of search criteria is two!");
                }

                Tuple<int, string[]> seachCountAnd = SeachMethods.SeachCountAnd(interimParameters);
                interimParameters = seachCountAnd.Item2;
                int countAnd = seachCountAnd.Item1;

                IEnumerable<(string key, string value)> seachParameters = SeachMethods.GetListParameters(string.Join(string.Empty, interimParameters));
                if (seachParameters.Count() < 2)
                {
                    throw new ArgumentException("Incorrect data entry for the search!");
                }

                this.fileCabinetRecords = SeachMethods.GetRecordsList(countAnd, seachParameters, this.service);
                if (this.fileCabinetRecords.Count > 0)
                {
                    FormationOfStrings.GetRow(columnNamesSorted, this.fileCabinetRecords);
                }
                else
                {
                    throw new ArgumentException("There is no record(s)with these search parameters!");
                }
            }
            catch (Exception ex)
            {
                ConstParameters.PrintException(ex);
            }
        }

        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"The {nameof(request)} is null.");
            }

            if (request.Command.ToLower() == ConstParameters.SelectName)
            {
                this.Select(request.Parameters);
                return null;
            }
            else
            {
                return base.Handle(request);
            }
        }
    }
}
