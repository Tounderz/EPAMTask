using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable SA1600
#pragma warning disable S1643
#pragma warning disable S125

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

        public void AddRow(params string[] row)
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
                ConstParameters.PrintException(ex);
            }
        }

        public void Print()
        {
            this.lengths.ForEach(l => Console.Write($"{ConstParameters.Corner}{ConstParameters.Line}{new string(ConstParameters.Line, l)}{ConstParameters.Line}"));
            Console.WriteLine(ConstParameters.Corner);

            string line = string.Empty;
            for (int i = 0; i < this.tableHeader.Length; i++)
            {
                line += $"{ConstParameters.SideLine} {this.tableHeader[i].PadLeft(this.lengths[i])} ";
            }

            Console.WriteLine($"{line}{ConstParameters.SideLine}");

            this.lengths.ForEach(l => Console.Write($"{ConstParameters.Corner}{ConstParameters.Line}{new string(ConstParameters.Line, l)}{ConstParameters.Line}"));
            Console.WriteLine(ConstParameters.Corner);

            foreach (var row in this.rows)
            {
                line = string.Empty;
                for (int i = 0; i < row.Length; i++)
                {
                    if (int.TryParse(row[i], out int n) || DateTime.TryParse(row[i], out DateTime date))
                    {
                        line += $"{ConstParameters.SideLine} {row[i].PadLeft(this.lengths[i])} ";  // numbers are padded to the left
                    }
                    else
                    {
                        line += $"{ConstParameters.SideLine} {row[i].PadRight(this.lengths[i])} "; // string are padded to the right
                    }
                }

                // строка разделения после каждой записи
                Console.WriteLine($"{line}{ConstParameters.SideLine}");
                this.lengths.ForEach(l => Console.Write($"{ConstParameters.Corner}{ConstParameters.Line}{new string(ConstParameters.Line, l)}{ConstParameters.Line}"));
                Console.WriteLine(ConstParameters.Corner);
            }

            // строка разделения только после всех записей
            // this.lengths.ForEach(l => Console.Write($"{ConstParameters.Corner}{ConstParameters.Line}{new string(ConstParameters.Line, l)}{ConstParameters.Line}"));
            // Console.WriteLine(ConstParameters.Corner);
        }
    }
}
