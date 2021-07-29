using System.Text.Json.Serialization;

namespace TradingReport.Simulation.Dtos
{
    class BookDto
    {
        [JsonPropertyName("index")]
        public string Index { get; init; }
        
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("last_modified")]
        public string LastModified { get; init; }
    }
}