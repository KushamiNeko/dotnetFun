using System;
using System.Collections.Generic;
using System.Globalization;
using TradingReport.Simulation.Dtos;

namespace TradingReport.Simulation.Models
{

    class TransactionComparer : IComparer<Transaction>
    {
        public int Compare(Transaction x, Transaction y)
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

    class Transaction
    {
        public string Index { get; init; }
        public double TimeStamp { get; init; }
        public DateTimeOffset DateTime { get; init; }
        public string Symbol { get; init; }
        public string Operation { get; init; }
        public double Leverage { get; init; }
        public double Price { get; init; }

        public static Transaction FromDto(TransactionDto dto)
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

            return new Transaction
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
}