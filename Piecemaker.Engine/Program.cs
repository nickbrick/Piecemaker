using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piecemaker.Engine
{
    public class Program
    {
        private const int DefaultTableId = 0xB0A12D;
        private readonly Random Random = new Random();
        public List<Table> Tables = new List<Table> { new Table(DefaultTableId) };
        private System.Timers.Timer TableStatusPolling = new System.Timers.Timer(new TimeSpan(0, minutes:5, seconds: 0).TotalMilliseconds);
        private readonly TimeSpan MaxTableInactivity = new TimeSpan(0, minutes: 10, 0);
        public int AllocatedTables => Tables.Count;
        public int OpenTables => Tables.Count(table => table.Status == Status.Open);
        public int ClosedTables => Tables.Count(table => table.Status == Status.Closed);
        public int ReadyTables => Tables.Count(table => table.Status == Status.Ready);
        public int PlayingTables => Tables.Count(table => table.Status == Status.Playing);
        public int PausedTables => Tables.Count(table => table.Status == Status.Paused);
        public int FinishedTables => Tables.Count(table => table.Status == Status.Finished);
        public int EmptyTables => Tables.Count(table => table.Status == Status.Empty);
        public Program()
        {
            TableStatusPolling.Elapsed += TableStatusPolling_Elapsed;
            TableStatusPolling.Start();
        }
        private void TableStatusPolling_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Tables.RemoveAll(table => DateTime.UtcNow - table.LastActivityAt > MaxTableInactivity);
        }
        public Table GetTable(int id)
        {
            var table = Tables.SingleOrDefault(table => table.Id == id);
            if (table == null)
            {
                table = new Table(id);
                Tables.Add(table);
            }
            return table;
        }
        public Table GetRandomOpenTableOrDefault()
        {
            var tables = Tables.Where(table => table.Status == Status.Open);
            var count = tables.Count();
            if (count == 0) return GetTable(DefaultTableId);
            return tables.ElementAt(Random.Next(count));
        }
    }
}
