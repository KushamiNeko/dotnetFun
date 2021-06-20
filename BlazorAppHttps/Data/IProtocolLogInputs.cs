using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public interface IProtocolLogInputs
    {
        public Dictionary<string, string> KeyValue { get; }

        public List<string> ErrorMessages { get; }

        public bool IsValid { get; }

        public void Validate();
    }
}