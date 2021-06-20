using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public interface IProtocolService
    {
        public Task<List<ProtocolModel>> GetProtocolsAsync();
        
        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(ProtocolModel protocol);
    }
}