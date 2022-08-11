using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Piecemaker.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piecemaker.Blazor
{
    public class Program
    {
        private static readonly Random Random = new Random();
        internal static List<Table> Tables = new List<Table> { new Table((int)0xB0A12D) };
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static Table GetTable(int id)
        {
            var table = Tables.SingleOrDefault(table => table.Id == id);
            if (table == null)
            {
                table = new Table(id);
                Tables.Add(table);
            }
            return table;
        }
    }
}
