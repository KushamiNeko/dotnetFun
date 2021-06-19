using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppHttps.Data
{
    public interface IStepService
    {
        public Task<List<ProtocolStepModel>> GetProtocolStepsAsync(string stepsUrl);
    }
}