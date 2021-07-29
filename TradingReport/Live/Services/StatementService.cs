using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fun.Trading;

namespace TradingReport.Live.Services
{

    static class StatementService
    {

        private static readonly string _root = Path.Join(
            Environment.GetEnvironmentVariable("HOME"),
            "Documents",
            "TRADING_NOTES",
            "records",
            "KushamiNeko"
       );

        public static async Task<List<MonthlyStatement>> ReadFromDirectoryAsync(Objective objective)
        {
            var dirpath = objective switch
            {
                Objective.Trading => Path.Join(_root, "trading"),
                Objective.Investing => Path.Join(_root, "investing"),
                _ => throw new ArgumentException("unknown objective")
            };

            var files = Directory.GetFiles(dirpath);

            var statements = new List<MonthlyStatement>();

            foreach (var file in files)
            {
                statements.Add(await ReadFromFileAsync(objective, Path.GetFileName(file)));
            }

            return statements;
        }

        public static async Task<MonthlyStatement> ReadFromFileAsync(Objective objective, string file)
        {
            var filepath = objective switch
            {
                Objective.Trading => Path.Join(_root, "trading", file),
                Objective.Investing => Path.Join(_root, "investing", file),
                _ => throw new ArgumentException("unknown objective")
            };

            return await MonthlyStatement.ParseFileAsync(filepath);
        }
    }
}