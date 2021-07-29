using System.Text.Json.Serialization;

namespace TradingReport.Simulation.Dtos
{
    class UserDto
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("uid")]
        public string Uid { get; init; }
    }
}