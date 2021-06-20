using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public class ProtocolService : IProtocolService
    {
        private readonly IHttpClientFactory _factory;

        ProtocolService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<ProtocolModel>> GetProtocolsAsync()
        {
            HttpClient client = _factory.CreateClient();

            Task<Stream> streamTask =
                client.GetStreamAsync("https://yodareneko3339.blob.core.windows.net/$web/作業手順.json");

            var protocols = await JsonSerializer.DeserializeAsync<List<ProtocolModel>>(await streamTask);

            return protocols;
        }

        public async Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol)
        {
            HttpClient client = _factory.CreateClient();

            Task<Stream> streamTask =
                client.GetStreamAsync(protocol.StepsUrl);

            var steps = await JsonSerializer.DeserializeAsync<List<ProtocolStepModel>>(await streamTask);

            return steps;
        }
    }
}