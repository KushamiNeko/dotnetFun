using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public interface IProtocolService
    {
        public List<ProtocolModel> Protocols { get; }

        // public Protocol Protocol { get; }

        // public List<ProtocolStepModel> ProtocolSteps { get; }

        public Task<List<ProtocolModel>> GetProtocolsAsync();
        
        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol);
    }
}