using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fun.Trading;

namespace TradingReport
{

    class RecordDto
    {
        [JsonPropertyName("index")]
        public string Index { get; init; }

        [JsonPropertyName("time_stamp")]
        public string TimeStamp { get; init; }

        [JsonPropertyName("datetime")]
        public string DateTime { get; init; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; init; }

        [JsonPropertyName("operation")]
        public string Operation { get; init; }

        [JsonPropertyName("leverage")]
        public string Leverage { get; init; }

        [JsonPropertyName("price")]
        public string Price { get; init; }
    }

    class Record
    {
        public string Index { get; init; }
        public double TimeStamp { get; init; }
        public DateTimeOffset DateTime { get; init; }
        public string Symbol { get; init; }
        public string Operation { get; init; }
        public double Leverage { get; init; }
        public double Price { get; init; }

        public static Record FromDto(RecordDto dto)
        {
            if (!Double.TryParse(dto.TimeStamp, out double timeStamp))
            {
                throw new ArgumentException("invalid timestamp");
            }

            if (!DateTimeOffset.TryParseExact(dto.DateTime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset datetime))
            {
                throw new ArgumentException("invalid datetime");
            }

            if (!Double.TryParse($"{dto.Operation}{dto.Leverage}", out double leverage))
            {
                throw new ArgumentException("invalid leverage");
            }

            if (!Double.TryParse(dto.Price, out double price))
            {
                throw new ArgumentException("invalid price");
            }

            return new Record
            {
                Index = dto.Index,
                TimeStamp = timeStamp,
                DateTime = datetime,
                Symbol = dto.Symbol,
                Operation = dto.Operation,
                Leverage = leverage,
                Price = price
            };
        }
    }

    class RecordComparer : IComparer<Record>
    {
        public int Compare(Record x, Record y)
        {
            if (x.DateTime == y.DateTime)
            {
                if (x.TimeStamp > y.TimeStamp)
                {
                    return 1;
                }
                else if (x.TimeStamp < y.TimeStamp)
                {
                    return -1;
                }
                else
                {
                    // throw new ArgumentException("invalid timestamp");
                    return 0;
                }
            }
            else
            {
                if (x.DateTime > y.DateTime)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
    }

    class Program
    {
        private static readonly string Root = Path.Join(
            Environment.GetEnvironmentVariable("HOME"),
            "Documents",
            "TRADING_NOTES",
            "records",
            "KushamiNeko",
            "trading");

        private static readonly string RecordFile = Path.Join(
             Environment.GetEnvironmentVariable("HOME"),
             "Documents",
             "database",
             "market_wizards",
             "records_KXFvrP9w9dezw2Jg5h88Inu1FXdWK62B.json"
        );

        static async Task Main(string[] args)
        {

            using var openStream = File.OpenRead(RecordFile);
            var dtos = await JsonSerializer.DeserializeAsync<List<RecordDto>>(openStream);
            var records = dtos.Select(dto => Record.FromDto(dto)).ToList();

            records.Sort(new RecordComparer());

            Record start = null;

            var tradeRecords = new List<TradeRecord>();

            var opened = 0.0;

            var transactions = new List<Record>();

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

            var statistic = new TradingStatistic
            {
                PeriodStart = new DateTimeOffset(new DateTime(2017, 12, 1)),
                PeriodEnd = new DateTimeOffset(new DateTime(2021, 1, 1)),
                Records = tradeRecords
            };


            // var file = Path.Join(Root, "2021-03-31.txt");
            // var statement = MonthlyStatement.Parse(file);

            // var statistic = new TradingStatistic
            // {
            //     PeriodStart = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(9, 0, 0)).AddMonths(-6),
            //     PeriodEnd = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(9, 0, 0)),
            //     Statements = new List<MonthlyStatement> { statement }
            // };

            Console.WriteLine(statistic);
        }
    }
}
