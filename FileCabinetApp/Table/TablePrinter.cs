using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.ConstParameters;
using FileCabinetApp.Models;

#pragma warning disable SA1600
#pragma warning disable S1643
#pragma warning disable S125
#pragma warning disable CA1822

namespace FileCabinetApp.Table
{
   public class TablePrinter
    {
        private readonly string[] tableHeader;
        private readonly List<int> lengths;
        private readonly List<string[]> rows = new ();

        public TablePrinter(params string[] tableHeader)
        {
            this.tableHeader = tableHeader;
            this.lengths = tableHeader.Select(t => t.Length).ToList();
        }

        public void CreateRow(List<string> columnNames, List<FileCabinetRecord> records)
        {
            var table = new TablePrinter(columnNames.ToArray());
            List<string> row = new ();
            for (int i = 0; i < records.Count; i++)
            {
                foreach (var item in columnNames)
                {
                    switch (item)
                    {
                        case ColumnNames.ColumnId:
                            row.Add(records[i].Id.ToString());
                            break;
                        case ColumnNames.ColumnFirstName:
                            row.Add(records[i].FirstName);
                            break;
                        case ColumnNames.ColumnLastName:
                            row.Add(records[i].LastName);
                            break;
                        case ColumnNames.ColumnDateOfBirth:
                            row.Add(records[i].DateOfBirth.ToString(ConstStrings.FormatDate));
                            break;
                        case ColumnNames.ColumnAge:
                            row.Add(records[i].Age.ToString());
                            break;
                        case ColumnNames.ColumnSalary:
                            row.Add(records[i].Salary.ToString());
                            break;
                        case ColumnNames.ColumnSymbol:
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

        private void AddRow(params string[] row)
        {
            try
            {
                if (row.Length != this.tableHeader.Length)
                {
                    throw new ArgumentException("The added row is not equal to the number of positions in the table header!");
                }

                this.rows.Add(row.Select(o => o.ToString()).ToArray());
                for (int i = 0; i < this.tableHeader.Length; i++)
                {
                    if (this.rows.Last()[i].Length > this.lengths[i])
                    {
                        this.lengths[i] = this.rows.Last()[i].Length;
                    }
                }
            }
            catch (Exception ex)
            {
                PrintException.Print(ex);
            }
        }

        private void Print()
        {
            this.PrintDividingLine();
            string line = string.Empty;
            for (int i = 0; i < this.tableHeader.Length; i++)
            {
                line += $"{TableLines.SideLine} {this.tableHeader[i].PadLeft(this.lengths[i])} ";
            }

            Console.WriteLine($"{line}{TableLines.SideLine}");
            this.PrintDividingLine();
            foreach (var row in this.rows)
            {
                line = string.Empty;
                for (int i = 0; i < row.Length; i++)
                {
                    if (int.TryParse(row[i], out int n) || DateTime.TryParse(row[i], out DateTime date))
                    {
                        line += $"{TableLines.SideLine} {row[i].PadLeft(this.lengths[i])} ";
                    }
                    else
                    {
                        line += $"{TableLines.SideLine} {row[i].PadRight(this.lengths[i])} ";
                    }
                }

                Console.WriteLine($"{line}{TableLines.SideLine}");

                // строка разделения после каждой записи
                // this.lengths.ForEach((Action<int>)(l => Console.Write($"{TableLines.Corner}{TableLines.Line}{new string(TableLines.Line, l)}{TableLines.Line}")));
                // Console.WriteLine(ConstParameters.Corner);
            }

            this.PrintDividingLine();
        }

        private void PrintDividingLine()
        {
            this.lengths.ForEach(l => Console.Write($"{TableLines.Corner}{TableLines.Line}{new string(TableLines.Line, l)}{TableLines.Line}"));
            Console.WriteLine(TableLines.Corner);
        }
    }
}
