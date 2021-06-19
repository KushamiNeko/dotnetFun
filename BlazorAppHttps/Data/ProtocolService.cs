using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public class ProtocolService : IProtocolService
    {
        private IHttpClientFactory _factory;

        ProtocolService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public List<ProtocolModel> Protocols { get; private set; }
        public ProtocolModel Protocol { get; private set; }
        public List<ProtocolStepModel> ProtocolSteps { get; private set; }

        public Task<List<ProtocolModel>> GetProtocolsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol)
        {
            throw new System.NotImplementedException();
        }
    }
}