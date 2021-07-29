using System.Text.Json.Serialization;

namespace TradingReport.Simulation.Dtos
{
    class TransactionDto
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
}