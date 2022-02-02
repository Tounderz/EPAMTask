using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600

namespace FileCabinetApp.Table
{
    public static class FormationOfStrings
    {
        public static void GetRow(List<string> columnNames, List<FileCabinetRecord> records)
        {
            var table = new TablePrinter(columnNames.ToArray());
            List<string> row = new ();
            for (int i = 0; i < records.Count; i++)
            {
                foreach (var item in columnNames)
                {
                    switch (item)
                    {
                        case ConstParameters.ColumnId:
                            row.Add(records[i].Id.ToString());
                            break;
                        case ConstParameters.ColumnFirstName:
                            row.Add(records[i].FirstName);
                            break;
                        case ConstParameters.ColumnLastName:
                            row.Add(records[i].LastName);
                            break;
                        case ConstParameters.ColumnDateOfBirth:
                            row.Add(records[i].DateOfBirth.ToString(ConstParameters.FormatDate));
                            break;
                        case ConstParameters.ColumnAge:
                            row.Add(records[i].Age.ToString());
                            break;
                        case ConstParameters.ColumnSalary:
                            row.Add(records[i].Salary.ToString());
                            break;
                        case ConstParameters.ColumnSymbol:
                            row.Add(records[i].Symbol.ToString());
                            break;
                        default:
                            break;
                    }
                }

                table.AddRow(row.ToArray());
                row.Clear();
            }

            table.Print();
        }
    }
}
