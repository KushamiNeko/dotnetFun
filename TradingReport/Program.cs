using System;
using System.Collections.Generic;
using System.IO;
using Fun.Trading;

namespace TradingReport
{
    class Program
    {
        private static readonly string Root = Path.Join(
            Environment.GetEnvironmentVariable("HOME"),
            "Documents",
            "TRADING_NOTES",
            "records",
            "KushamiNeko",
            "trading");

        static void Main(string[] args)
        {
            var file = Path.Join(Root, "2021-03-31.txt");
            var statement = MonthlyStatement.Parse(file);

            var statistic = new TradingStatistic
            {
                PeriodStart = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(9, 0, 0)).AddMonths(-6),
                PeriodEnd = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(9, 0, 0)),
                Statements = new List<MonthlyStatement> { statement }
            };

            // Console.WriteLine(statistic.BattingAvg);
            // Console.WriteLine(statistic.BattingAvgL);
            // Console.WriteLine(statistic.BattingAvgS);

            // Console.WriteLine(statistic.WinPlAvg);
            // Console.WriteLine(statistic.WinPlAvgL);
            // Console.WriteLine(statistic.WinPlAvgS);

            // Console.WriteLine(statistic.LossPlAvg);
            // Console.WriteLine(statistic.LossPlAvgL);
            // Console.WriteLine(statistic.LossPlAvgS);

            Console.WriteLine(statistic);
        }
    }
}
