using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Fun.Trading;
using TradingReport.Live.Services;
using TradingReport.Simulation.Dtos;
using TradingReport.Simulation.Models;
using TradingReport.Simulation.Services;

namespace TradingReport
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var statistic = new TradingStatistic
            {
                PeriodStart = new DateTimeOffset(new DateTime(2016, 09, 01)),
                PeriodEnd = new DateTimeOffset(new DateTime(2021, 12, 31)),
                // PeriodStart = default,
                // PeriodEnd = default,
                Records = await TransactionService.ReadFromBookAsync(user: "default", title: "ZN@01"),
            };

            // var statistic = new TradingStatistic
            // {
            //     PeriodStart = default,
            //     PeriodEnd = default,
            //     Statements = await StatementService.ReadFromDirectoryAsync(Objective.Trading)
            // };

            Console.WriteLine(statistic);
        }
    }
}
