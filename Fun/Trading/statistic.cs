using System;
using System.Collections.Generic;
using System.Linq;

namespace Fun.Trading
{

    public class TradingStatistic
    {

        public DateTimeOffset PeriodStart { get; init; }

        public DateTimeOffset PeriodEnd { get; init; }

        public List<MonthlyStatement> Statements { get; init; }

        public List<TradeRecord> Records { get; init; }

        private IEnumerable<TradeRecord> _all
        {
            get
            {
                if (Statements is { Count: > 0 })
                {
                    return Statements.SelectMany(statement => statement.Records).Where(record => record.Open >= PeriodStart && record.Close <= PeriodEnd);
                }
                else if (Records is { Count: > 0 })
                {
                    return Records.Where(record => record.Open >= PeriodStart && record.Close <= PeriodEnd);
                }
                else
                {
                    throw new ArgumentException("empty statements and records");
                }

            }
        }
        private IEnumerable<TradeRecord> _winners => _all.Where(record => record.PL > 0);
        private IEnumerable<TradeRecord> _losers => _all.Where(record => record.PL < 0);

        private IEnumerable<TradeRecord> _long => _all.Where(record => record.Direction == TradeDirection.Long);
        private IEnumerable<TradeRecord> _short => _all.Where(record => record.Direction == TradeDirection.Short);

        private IEnumerable<TradeRecord> _longWinners => _all.Where(record => record.Direction == TradeDirection.Long && record.PL > 0);
        private IEnumerable<TradeRecord> _longLosers => _all.Where(record => record.Direction == TradeDirection.Long && record.PL < 0);

        private IEnumerable<TradeRecord> _shortWinners => _all.Where(record => record.Direction == TradeDirection.Short && record.PL > 0);
        private IEnumerable<TradeRecord> _shortLosers => _all.Where(record => record.Direction == TradeDirection.Short && record.PL < 0);

        public int NumberOfTrade => _all.Count();
        public int NumberOfLongTrade => _long.Count();
        public int NumberOfShortTrade => _short.Count();

        public double BattingAvg => ((double)_winners.Count() / (double)_all.Count());
        public double BattingAvgL => ((double)_longWinners.Count() / (double)_long.Count());
        public double BattingAvgS => ((double)_shortWinners.Count() / (double)_short.Count());

        public double WinPlAvg
        {
            get
            {
                var winnersPL = _winners.Select(record => record.PL);
                return winnersPL.Count() > 0 ? winnersPL.Average() : Double.NaN;
            }
        }
        public double WinPlAvgL
        {
            get
            {
                var winnersPL = _longWinners.Select(record => record.PL);
                return winnersPL.Count() > 0 ? winnersPL.Average() : Double.NaN;
            }
        }
        public double WinPlAvgS
        {
            get
            {
                var winnersPL = _shortWinners.Select(record => record.PL);
                return winnersPL.Count() > 0 ? winnersPL.Average() : Double.NaN;
            }
        }

        public double WinPlMax => _winners.Select(record => record.PL).Max();
        public double WinPlMaxL => _longWinners.Select(record => record.PL).Max();
        public double WinPlMaxS => _shortWinners.Select(record => record.PL).Max();

        public double WinPlMin => _winners.Select(record => record.PL).Min();
        public double WinPlMinL => _longWinners.Select(record => record.PL).Min();
        public double WinPlMinS => _shortWinners.Select(record => record.PL).Min();

        public double LossPlAvg
        {
            get
            {
                var losersPL = _losers.Select(record => record.PL);
                return losersPL.Count() > 0 ? losersPL.Average() : Double.NaN;
            }
        }
        public double LossPlAvgL
        {
            get
            {
                var losersPL = _longLosers.Select(record => record.PL);
                return losersPL.Count() > 0 ? losersPL.Average() : Double.NaN;
            }
        }
        public double LossPlAvgS
        {
            get
            {
                var losersPL = _shortLosers.Select(record => record.PL);
                return losersPL.Count() > 0 ? losersPL.Average() : Double.NaN;
            }
        }

        public double LossPlMax => _losers.Select(record => record.PL).Max();
        public double LossPlMaxL => _longLosers.Select(record => record.PL).Max();
        public double LossPlMaxS => _shortLosers.Select(record => record.PL).Max();

        public double LossPlMin => _losers.Select(record => record.PL).Min();
        public double LossPlMinL => _longLosers.Select(record => record.PL).Min();
        public double LossPlMinS => _shortLosers.Select(record => record.PL).Min();

        public double WinLossRatio => WinPlAvg / Math.Abs(LossPlAvg);
        public double WinLossRatioL => WinPlAvgL / Math.Abs(LossPlAvgL);
        public double WinLossRatioS => WinPlAvgS / Math.Abs(LossPlAvgS);

        public double WinLossHoldRatio => WinHoldAvg / Math.Abs(LossHoldAvg);
        public double WinLossHoldRatioL => WinHoldAvgL / Math.Abs(LossHoldAvgL);
        public double WinLossHoldRatioS => WinHoldAvgS / Math.Abs(LossHoldAvgS);

        // the unit of holding properties is minutes
        public double WinHoldAvg
        {
            get
            {
                var winnersHours = _winners.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return winnersHours.Count() > 0 ? winnersHours.Average() : Double.NaN;
            }
        }
        public double WinHoldAvgL
        {
            get
            {
                var winnersHours = _longWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return winnersHours.Count() > 0 ? winnersHours.Average() : Double.NaN;
            }
        }
        public double WinHoldAvgS
        {
            get
            {
                var winnersHours = _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return winnersHours.Count() > 0 ? winnersHours.Average() : Double.NaN;
            }
        }

        public double WinHoldMax => _winners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();
        public double WinHoldMaxL => _longWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();
        public double WinHoldMaxS => _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();

        public double WinHoldMin => _winners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();
        public double WinHoldMinL => _longWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();
        public double WinHoldMinS => _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();

        public double LossHoldAvg
        {
            get
            {
                var losersHours = _losers.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return losersHours.Count() > 0 ? losersHours.Average() : Double.NaN;
            }
        }
        public double LossHoldAvgL
        {
            get
            {
                var losersHours = _longLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return losersHours.Count() > 0 ? losersHours.Average() : Double.NaN;
            }
        }
        public double LossHoldAvgS
        {
            get
            {
                var losersHours = _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes);
                return losersHours.Count() > 0 ? losersHours.Average() : Double.NaN;
            }
        }

        public double LossHoldMax => _losers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();
        public double LossHoldMaxL => _longLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();
        public double LossHoldMaxS => _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Max();

        public double LossHoldMin => _losers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();
        public double LossHoldMinL => _longLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();
        public double LossHoldMinS => _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalMinutes).Min();

        public double ExpectedValue => (WinPlAvg * BattingAvg) + (LossPlAvg * (1.0 - BattingAvg));
        public double ExpectedValueL => (WinPlAvgL * BattingAvgL) + (LossPlAvgL * (1.0 - BattingAvgL));
        public double ExpectedValueS => (WinPlAvgS * BattingAvgS) + (LossPlAvgS * (1.0 - BattingAvgS));


        private int _fieldWidth = 18;
        private string _separator = " | ";


        public override string ToString()
        {
            var title = GetStringRow(new List<string>{
                "",
                "N",
                "Batting Avg.(%)",
                "Win PL Avg.($)",
                "Loss PL Avg.($)",
                "Win Loss Ratio($)",
                "Win Loss Ratio(T)",
                // "Win Hold Avg.(H)",
                // "Loss Hold Avg.(H)",
                "Expected Value($)",
            });

            var allTrade = GetStringRow(new List<string>{
                "All",
                NumberOfTrade.ToString(),
                (BattingAvg * 100.0).ToString("F"),
                WinPlAvg.ToString("C2"),
                LossPlAvg.ToString("C2"),
                WinLossRatio.ToString("F"),
                WinLossHoldRatio.ToString("F"),
                // WinHoldAvg.ToString("F"),
                // LossHoldAvg.ToString("F"),
                ExpectedValue.ToString("C2"),
            });

            var longTrade = GetStringRow(new List<string>{
                "Long",
                NumberOfLongTrade.ToString(),
                (BattingAvgL * 100.0).ToString("F"),
                WinPlAvgL.ToString("C2"),
                LossPlAvgL.ToString("C2"),
                WinLossRatioL.ToString("F"),
                WinLossHoldRatioL.ToString("F"),
                // WinHoldAvgL.ToString("F"),
                // LossHoldAvgL.ToString("F"),
                ExpectedValueL.ToString("C2"),

            });

            var shortTrade = GetStringRow(new List<string>{
                "Short",
                NumberOfShortTrade.ToString(),
                (BattingAvgS * 100.0).ToString("F"),
                WinPlAvgS.ToString("C2"),
                LossPlAvgS.ToString("C2"),
                WinLossRatioS.ToString("F"),
                WinLossHoldRatioS.ToString("F"),
                // WinHoldAvgS.ToString("F"),
                // LossHoldAvgS.ToString("F"),
                ExpectedValueS.ToString("C2"),

            });

            return $"{title}\n{allTrade}\n{longTrade}\n{shortTrade}";
        }

        private string GetStringRow(List<string> values)
        {
            var row = String.Join(_separator, values.Select(value => value.PadLeft(_fieldWidth, ' ')));
            var bottom = new String('-', (_fieldWidth * values.Count) + ((values.Count - 1) * _separator.Length));

            return $"{row}\n{bottom}";
        }

    }

}