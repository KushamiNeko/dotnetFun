using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fun.Trading
{
    public enum Objective
    {
        Trading,
        Investing
    }

    public enum TradeDirection
    {
        Long,
        Short
    }

    public class AccountBalance
    {
        // futures, equities, crypto account
        public string AccountType { get; init; }

        // startingBalance float64
        public double EndingBalance { get; init; }
    }

    public class TradeRecord
    {
        public DateTimeOffset Open { get; init; }

        public DateTimeOffset Close { get; init; }

        public TradeDirection Direction { get; init; }

        public double PL { get; init; }

        public int Quantity { get; init; }
    }

    public class MonthlyStatement
    {
        // the objective of the statement
        // eg.
        // trading or investing
        // public string Objective { get; init; }
        public Objective Objective { get; init; }

        /*
            list all accounts used for specified objective
            eg.
            objective: trading
            balance:
                    1 futures account for main trading
                    1 equities account for holding money market ETF
                    1 crypto account for earning crypto interests
        */
        public List<AccountBalance> Balances { get; init; }

        public double EndingBalance => Balances.Select(balance => balance.EndingBalance).Sum();

        // account percentage risk per trade
        public double PercentageRiskPerTrade { get; init; }

        // trading pl records
        public List<TradeRecord> Records { get; init; }

        private const string ObjectivePattern = @"^\s*objective\s*:\s*(?<Objective>\w+)\s*$";

        private const string AccountBalancePattern = @"^\s*(?<Account>\w+)\s*balance:\s*(?<Balance>[0-9,.-]+)\s*$";

        private const string PercentageRiskPattern =
            @"^\s*account percentage risk per trade\s*:\s*(?<Risk>[0-9.]+)\s*%\s*$";

        private const string _datetimePattern = @"(?<{{GROUP}}>\d{4}\s*[-]\s*\d{2}\s*[-]\s*\d{2}(?:\s*\d{2}:\d{2})*)";

        private static readonly string _tradeRecordPattern =
            string.Format(
                @"^\s*{0}(?:\s*[-~]\s*{1})*\s*:\s*(?<PL>[0-9,.-]+)\s*(?:@\s*(?<Direction>[LS])(?<Contract>\d*))$",
                _datetimePattern.Replace("{{GROUP}}", "Open"),
                _datetimePattern.Replace("{{GROUP}}", "Close")
            );

        public static async Task<MonthlyStatement> ParseFileAsync(string file)
        {
            var statement = await File.ReadAllTextAsync(file);

            var regex = new Regex(ObjectivePattern, RegexOptions.Multiline);
            var matches = regex.Matches(statement);

            if (matches is not { Count: 1 })
            {
                throw new ArgumentException("the statement contains no objective information");
            }

            // string objective;
            Objective objective;
            if (matches[0].Groups["Objective"].Success)
            {
                // objective = matches[0].Groups["Objective"].Value;
                objective = matches[0].Groups["Objective"].Value switch
                {
                    "trading" => Objective.Trading,
                    "investing" => Objective.Investing,
                    _ => throw new ArgumentException("unknown objective")
                };
            }
            else
            {
                throw new ArgumentException("invalid objective information");
            }


            regex = new Regex(AccountBalancePattern, RegexOptions.Multiline);
            matches = regex.Matches(statement);

            if (matches is not { Count: > 0 })
            {
                throw new ArgumentException("the statement contains no account balance information");
            }

            var balances = new List<AccountBalance>();
            foreach (Match match in matches)
            {
                var groups = match.Groups;

                if (groups["Account"].Success && groups["Balance"].Success)
                {
                    var account = groups["Account"].Value.Trim();

                    if (!Double.TryParse(groups["Balance"].Value.Replace(",", ""), out double balance))
                    {
                        throw new ArgumentException("invalid acount balance information");
                    }

                    balances.Add(new AccountBalance
                    {
                        AccountType = account,
                        EndingBalance = balance,
                    });
                }
                else
                {
                    throw new ArgumentException("invalid acount balance information");
                }
            }

            regex = new Regex(PercentageRiskPattern, RegexOptions.Multiline);

            double percentageRisk;

            matches = regex.Matches(statement);

            if (matches is not { Count: 1 })
            {
                throw new ArgumentException("the statement contains no percentage risk information");
            }

            if (matches[0].Groups["Risk"].Success)
            {
                var match = matches[0];
                if (!Double.TryParse(match.Groups["Risk"].Value, out percentageRisk))
                {
                    throw new ArgumentException("invalid percentage risk information");
                }
            }
            else
            {
                throw new ArgumentException("invalid percentage risk information");
            }

            regex = new Regex(_tradeRecordPattern, RegexOptions.Multiline);
            matches = regex.Matches(statement);

            if (matches is not { Count: > 0 })
            {
                throw new ArgumentException("the statement contains no trade record information");
            }

            var records = new List<TradeRecord>();
            foreach (Match match in matches)
            {
                var groups = match.Groups;

                if (!(groups["Open"].Success && groups["PL"].Success && groups["Direction"].Success))
                {
                    throw new ArgumentException("invalid trade record information");
                }

                if (!DateTimeOffset.TryParseExact(groups["Open"].Value, new String[]
                {
                    @"yyyy-MM-dd",
                    @"yyyy-MM-dd HH:mm"
                }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset open))
                {
                    throw new ArgumentException("invalid trade record information");
                }

                DateTimeOffset close;
                if (groups["Close"].Success && !String.IsNullOrWhiteSpace(groups["Close"].Value))
                {
                    if (!DateTimeOffset.TryParseExact(groups["Close"].Value, new String[]
                    {
                        @"yyyy-MM-dd",
                        @"yyyy-MM-dd HH:mm"
                    }, CultureInfo.InvariantCulture, DateTimeStyles.None, out close))
                    {
                        throw new ArgumentException("invalid trade record information");
                    }
                }
                else
                {
                    close = open;
                }

                var direction = groups["Direction"].Value switch
                {
                    "L" => TradeDirection.Long,
                    "S" => TradeDirection.Short,
                    _ => throw new ArgumentException("invalid trade record information"),
                };

                if (!Double.TryParse(groups["PL"].Value.Replace(",", ""), out double pl))
                {
                    throw new ArgumentException("invalid trade record information");
                }

                int quantity;
                if (groups["Contract"].Success && !String.IsNullOrWhiteSpace(groups["Contract"].Value))
                {
                    if (!Int32.TryParse(groups["Contract"].Value, out quantity))
                    {
                        throw new ArgumentException("invalid trade record information");
                    }
                }
                else
                {
                    quantity = 1;
                }

                records.Add(new TradeRecord
                {
                    Open = open,
                    Close = close,
                    Direction = direction,
                    PL = pl,
                    Quantity = quantity,
                });
            }

            return new MonthlyStatement
            {
                Objective = objective,
                Balances = balances,
                PercentageRiskPerTrade = percentageRisk,
                Records = records,
            };
        }
    }
}