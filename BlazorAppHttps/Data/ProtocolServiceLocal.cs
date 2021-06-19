using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public class ProtocolServiceLocal : IProtocolService
    {
        public List<ProtocolModel> Protocols { get; private set; }

        // public ProtocolModel Protocol { get; private set; }

        // public List<ProtocolStepModel> ProtocolSteps { get; private set; }

        public Task<List<ProtocolModel>> GetProtocolsAsync()
        {
            if (Protocols == null)
            {
                var content =
                    File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/protocols.json"));

                Protocols = JsonSerializer.Deserialize<List<ProtocolModel>>(content);
            }

            return Task.FromResult(Protocols);
        }

        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol)
        {
            // Protocol = protocol;

            var content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot", protocol.StepsUrl));
            // ProtocolSteps = JsonSerializer.Deserialize<List<ProtocolStepModel>>(content);
            
            var steps = JsonSerializer.Deserialize<List<ProtocolStepModel>>(content);

            return Task.FromResult(steps);
        }
    }
}