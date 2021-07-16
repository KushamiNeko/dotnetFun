using System;
using System.Collections.Generic;
using System.Linq;

namespace Fun.Trading
{

    // func plRange(records []*TradeRecord) (min float64, max float64) {
    // 	max = float64(math.MinInt32)
    // 	min = float64(math.MaxInt32)

    // 	for _, r := range records {
    // 		if r.PL() > max {
    // 			max = r.PL()
    // 		}

    // 		if r.PL() < min {
    // 			min = r.PL()
    // 		}
    // 	}

    // 	return min, max
    // }

    // func holdingRangeHour(records []*TradeRecord) (min float64, max float64) {
    // 	max = float64(math.MinInt32)
    // 	min = float64(math.MaxInt32)

    // 	for _, r := range records {
    // 		diff := holdingLengthHour(r)

    // 		if diff > max {
    // 			max = diff
    // 		}

    // 		if diff < min {
    // 			min = diff
    // 		}
    // 	}

    // 	return min, max
    // }

    // func plAvg(records []*TradeRecord) float64 {
    // 	total := 0.0

    // 	for _, r := range records {
    // 		total += r.PL()
    // 	}

    // 	return total / float64(len(records))
    // }

    // func holdingAvgHour(records []*TradeRecord) float64 {
    // 	total := 0.0

    // 	for _, r := range records {
    // 		total += holdingLengthHour(r)
    // 	}

    // 	return total / float64(len(records))
    // }

    // func holdingLengthHour(r *TradeRecord) float64 {
    // 	to := r.TradeOpen()
    // 	tc := r.TradeClose()

    // 	diffM := tc.Sub(to).Minutes()

    // 	return diffM / 60.0
    // }

    public class TradingStatistic
    {

        public DateTimeOffset PeriodStart { get; init; }

        public DateTimeOffset PeriodEnd { get; init; }

        public List<MonthlyStatement> Statements { get; init; }

        private IEnumerable<TradeRecord> _all => Statements.SelectMany(statement => statement.Records)
                                                    .Where(record => record.Open >= PeriodStart && record.Close <= PeriodEnd);
        private IEnumerable<TradeRecord> _winners => _all.Where(record => record.PL > 0);
        private IEnumerable<TradeRecord> _losers => _all.Where(record => record.PL < 0);

        private IEnumerable<TradeRecord> _long => _all.Where(record => record.Direction == TradeDirection.Long);
        private IEnumerable<TradeRecord> _short => _all.Where(record => record.Direction == TradeDirection.Short);

        private IEnumerable<TradeRecord> _longWinners => _all.Where(record => record.Direction == TradeDirection.Long && record.PL > 0);
        private IEnumerable<TradeRecord> _longLosers => _all.Where(record => record.Direction == TradeDirection.Long && record.PL < 0);

        private IEnumerable<TradeRecord> _shortWinners => _all.Where(record => record.Direction == TradeDirection.Short && record.PL > 0);
        private IEnumerable<TradeRecord> _shortLosers => _all.Where(record => record.Direction == TradeDirection.Short && record.PL < 0);

        public double BattingAvg => ((double)_winners.Count() / (double)_all.Count());
        public double BattingAvgL => ((double)_longWinners.Count() / (double)_long.Count());
        public double BattingAvgS => ((double)_shortWinners.Count() / (double)_short.Count());

        public double WinPlAvg => _winners.Select(record => record.PL).Average();
        public double WinPlAvgL => _longWinners.Select(record => record.PL).Average();
        public double WinPlAvgS => _shortWinners.Select(record => record.PL).Average();

        public double WinPlMax => _winners.Select(record => record.PL).Max();
        public double WinPlMaxL => _longWinners.Select(record => record.PL).Max();
        public double WinPlMaxS => _shortWinners.Select(record => record.PL).Max();

        public double WinPlMin => _winners.Select(record => record.PL).Min();
        public double WinPlMinL => _longWinners.Select(record => record.PL).Min();
        public double WinPlMinS => _shortWinners.Select(record => record.PL).Min();

        public double LossPlAvg => _losers.Select(record => record.PL).Average();
        public double LossPlAvgL => _longLosers.Select(record => record.PL).Average();
        public double LossPlAvgS => _shortLosers.Select(record => record.PL).Average();

        public double LossPlMax => _losers.Select(record => record.PL).Max();
        public double LossPlMaxL => _longLosers.Select(record => record.PL).Max();
        public double LossPlMaxS => _shortLosers.Select(record => record.PL).Max();

        public double LossPlMin => _losers.Select(record => record.PL).Min();
        public double LossPlMinL => _longLosers.Select(record => record.PL).Min();
        public double LossPlMinS => _shortLosers.Select(record => record.PL).Min();

        public double WinLossRatio => WinPlAvg / Math.Abs(LossPlAvg);
        public double WinLossRatioL => WinPlAvgL / Math.Abs(LossPlAvgL);
        public double WinLossRatioS => WinPlAvgS / Math.Abs(LossPlAvgS);

        // the unit of holding properties is hour
        public double WinHoldAvg => _winners.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();
        public double WinHoldAvgL => _longWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();
        public double WinHoldAvgS => _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();

        public double WinHoldMax => _winners.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();
        public double WinHoldMaxL => _longWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();
        public double WinHoldMaxS => _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();

        public double WinHoldMin => _winners.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();
        public double WinHoldMinL => _longWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();
        public double WinHoldMinS => _shortWinners.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();

        public double LossHoldAvg => _losers.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();
        public double LossHoldAvgL => _longLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();
        public double LossHoldAvgS => _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Average();

        public double LossHoldMax => _losers.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();
        public double LossHoldMaxL => _longLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();
        public double LossHoldMaxS => _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Max();

        public double LossHoldMin => _losers.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();
        public double LossHoldMinL => _longLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();
        public double LossHoldMinS => _shortLosers.Select(record => record.Close.Subtract(record.Open).TotalHours).Min();

        public double ExpectedValue => (WinPlAvg * BattingAvg) + (LossPlAvg * (1.0 - BattingAvg));
        public double ExpectedValueL => (WinPlAvgL * BattingAvgL) + (LossPlAvgL * (1.0 - BattingAvgL));
        public double ExpectedValueS => (WinPlAvgS * BattingAvgS) + (LossPlAvgS * (1.0 - BattingAvgS));


        private int _fieldWidth = 20;
        private string _separator = " | ";


        public override string ToString()
        {
            var title = GetStringRow(new List<string>{
                "Direction",
                "Batting Avg. (%)",
                "Win PL Avg. ($)",
                "Loss PL Avg. ($)",
                "Win Loss Ratio",
                "Win Hold Avg. (H)",
                "Loss Hold Avg. (H)",
                "Expected Value. ($)",
            });

            var allTrade = GetStringRow(new List<string>{
                "All",
                (BattingAvg * 100.0).ToString("F"),
                WinPlAvg.ToString("C2"),
                LossPlAvg.ToString("C2"),
                WinLossRatio.ToString("F"),
                WinHoldAvg.ToString("F"),
                LossHoldAvg.ToString("F"),
                ExpectedValue.ToString("C2"),
            });

            // var longTrade = GetStringRow(new List<string>{
            //     "Long",
            //     (BattingAvgL * 100.0).ToString("F"),
            //     WinPlAvgL.ToString("C2"),
            //     LossPlAvgL.ToString("C2"),
            //     WinLossRatioL.ToString("F"),
            //     WinHoldAvgL.ToString("F"),
            //     LossHoldAvgL.ToString("F"),
            //     ExpectedValueL.ToString("C2"),

            // });

            var shortTrade = GetStringRow(new List<string>{
                "Short",
                (BattingAvgS * 100.0).ToString("F"),
                WinPlAvgS.ToString("C2"),
                LossPlAvgS.ToString("C2"),
                WinLossRatioS.ToString("F"),
                WinHoldAvgS.ToString("F"),
                LossHoldAvgS.ToString("F"),
                ExpectedValueS.ToString("C2"),

            });

            // return $"{title}\n{allTrade}\n{longTrade}\n{shortTrade}";
            return $"{title}\n{allTrade}\n{shortTrade}";
        }

        private string GetStringRow(List<string> values)
        {
            var row = String.Join(_separator, values.Select(value => value.PadLeft(_fieldWidth, ' ')));
            var bottom = new String('-', (_fieldWidth * values.Count) + ((values.Count - 1) * _separator.Length));

            return $"{row}\n{bottom}";
        }

    }


    // func (t *TradingStatistic) String() string {
    // 	t.calculate()

    // 	const fieldLayout = `%- 18s`

    // 	var builder strings.Builder

    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString(" | ")
    // 	builder.WriteString(fieldLayout)
    // 	builder.WriteString("\n")

    // 	row := builder.String()

    // 	separator := fmt.Sprintf(
    // 		row,
    // 		"",
    // 		"",
    // 		"",
    // 		"",
    // 		"",
    // 		"",
    // 		"",
    // 		"",
    // 	)

    // 	separator = strings.ReplaceAll(separator, " ", "-")

    // 	// regex := regexp.MustCompile(`-\|-`)
    // 	// separator = regex.ReplaceAllString(separator, " | ")

    // 	builder = strings.Builder{}

    // 	fmt.Fprintf(
    // 		&builder,
    // 		row,
    // 		"",
    // 		"Batting Avg. (%)",
    // 		"Win PL Avg. ($|¥)",
    // 		"Loss PL Avg. ($|¥)",
    // 		"Win Loss Ratio",
    // 		"Win Hold Avg. (H)",
    // 		"Loss Hold Avg. (H)",
    // 		"EV. ($|¥)",
    // 	)

    // 	builder.WriteString(separator)

    // 	fmt.Fprintf(
    // 		&builder,
    // 		row,
    // 		"ALL",
    // 		fmt.Sprintf("%.3f", t.battingAvg*100.0),
    // 		fmt.Sprintf("%.3f", t.winAvg),
    // 		fmt.Sprintf("%.3f", t.lossAvg),
    // 		fmt.Sprintf("%.3f", t.winLossRatio),
    // 		fmt.Sprintf("%.3f", t.winHoldAvg),
    // 		fmt.Sprintf("%.3f", t.lossHoldAvg),
    // 		fmt.Sprintf("%.3f", t.expectedValue),
    // 	)

    // 	builder.WriteString(separator)

    // 	fmt.Fprintf(
    // 		&builder,
    // 		row,
    // 		"LONG",
    // 		fmt.Sprintf("%.3f", t.battingAvgL*100.0),
    // 		fmt.Sprintf("%.3f", t.winAvgL),
    // 		fmt.Sprintf("%.3f", t.lossAvgL),
    // 		fmt.Sprintf("%.3f", t.winLossRatioL),
    // 		fmt.Sprintf("%.3f", t.winHoldAvgL),
    // 		fmt.Sprintf("%.3f", t.lossHoldAvgL),
    // 		fmt.Sprintf("%.3f", t.expectedValueL),
    // 	)

    // 	builder.WriteString(separator)

    // 	fmt.Fprintf(
    // 		&builder,
    // 		row,
    // 		"SHORT",
    // 		fmt.Sprintf("%.3f", t.battingAvgS*100.0),
    // 		fmt.Sprintf("%.3f", t.winAvgS),
    // 		fmt.Sprintf("%.3f", t.lossAvgS),
    // 		fmt.Sprintf("%.3f", t.winLossRatioS),
    // 		fmt.Sprintf("%.3f", t.winHoldAvgS),
    // 		fmt.Sprintf("%.3f", t.lossHoldAvgS),
    // 		fmt.Sprintf("%.3f", t.expectedValueS),
    // 	)

    // 	return builder.String()
    // }


}