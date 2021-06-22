using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public class ProtocolServiceLocal : IProtocolService
    {
        public Task<List<ProtocolModel>> GetProtocolsAsync()
        {
            var content =
                File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/protocols.json"));

            var protocols = JsonSerializer.Deserialize<List<ProtocolModel>>(content);

            return Task.FromResult(protocols);
        }

        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol)
        {
            var content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot", protocol.StepsUrl));

            var steps = JsonSerializer.Deserialize<List<ProtocolStepModel>>(content);

            return Task.FromResult(steps);
        }
    }
}