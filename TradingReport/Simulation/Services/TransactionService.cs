using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Fun.Trading;
using TradingReport.Simulation.Dtos;
using TradingReport.Simulation.Models;

namespace TradingReport.Simulation.Services
{
    static class TransactionService
    {

        private static readonly string _root = Path.Join(
            Environment.GetEnvironmentVariable("HOME"),
            "Documents",
            "database",
            "market_wizards"
       );

        private static readonly string _userFile = Path.Join(
            _root,
           "admin_user.json"
       );

        public static async Task<List<TradeRecord>> ReadFromBookAsync(string user, string title)
        {
            using var userStream = File.OpenRead(_userFile);
            var userDtos = await JsonSerializer.DeserializeAsync<List<UserDto>>(userStream);

            var bookIndex = userDtos.Where(dto => dto.Name.Equals(user)).FirstOrDefault()?.Uid;
            if (String.IsNullOrWhiteSpace(bookIndex))
            {
                throw new ArgumentException("invalid user");
            }

            using var bookStream = File.OpenRead(Path.Join(_root, $"books_{bookIndex}.json"));
            var bookDtos = await JsonSerializer.DeserializeAsync<List<BookDto>>(bookStream);
            var recordIndex = bookDtos.Where(dto => dto.Title.Equals(title)).FirstOrDefault()?.Index;
            if (String.IsNullOrWhiteSpace(recordIndex))
            {
                throw new ArgumentException("invalid title");
            }

            return await ReadFromFileAsync(Path.Join(_root, $"records_{recordIndex}.json"));
        }

        public static async Task<List<TradeRecord>> ReadFromFileAsync(string file)
        {

            using var openStream = File.OpenRead(file);
            var dtos = await JsonSerializer.DeserializeAsync<List<TransactionDto>>(openStream);
            var records = dtos.Select(dto => Transaction.FromDto(dto)).ToList();

            records.Sort(new TransactionComparer());

            var tradeRecords = new List<TradeRecord>();

            Transaction start = null;
            var opened = 0.0;

            var transactions = new List<Transaction>();

            foreach (var record in records)
            {
                opened += record.Leverage;
                transactions.Add(record);

                if (start == null)
                {
                    start = record;
                    continue;
                }

                if (opened == 0)
                {
                    var open = transactions.Where(record => record.Operation.Equals(start.Operation));
                    var close = transactions.Where(record => !record.Operation.Equals(start.Operation));

                    var leverage = Math.Abs(open.Select(record => record.Leverage).Sum());

                    var openPrice = open.Select(record => record.Price * Math.Abs(record.Leverage)).Sum() / leverage;
                    var closePrice = close.Select(record => record.Price * Math.Abs(record.Leverage)).Sum() / leverage;

                    var trade = new TradeRecord
                    {
                        Open = start.DateTime,
                        Close = record.DateTime,
                        Direction = start.Leverage switch
                        {
                            < 0 => TradeDirection.Short,
                            > 0 => TradeDirection.Long,
                            _ => throw new ArgumentException("invalid trade direction")
                        },
                        PL = start.Leverage switch
                        {
                            > 0 => ((closePrice - openPrice) * leverage) * 1000.0,
                            < 0 => (-1 * (closePrice - openPrice) * leverage) * 1000.0,
                            _ => throw new ArgumentException("invalid trade direction")
                        }
                    };

                    tradeRecords.Add(trade);

                    transactions.Clear();
                    start = null;
                }
            }

            return tradeRecords;
        }
    }
}