using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace BlazorAppHttps.Data
{
    public class StepService
    {
        // public List<Step> Steps
        // {
        //     get
        //     {
        //         return _steps;
        //     }
        // }

        private List<ProtocolStepModel> _steps = null;

        // public StepService()
        // {
        //     _steps = GetSteps();
        // }

        public async Task<List<ProtocolStepModel>> GetStepsWeb()
        {
            if (_steps == null)
            {
                HttpClient client = new HttpClient();

                Task<Stream> streamTask =
                    client.GetStreamAsync("https://yodareneko3339.blob.core.windows.net/$web/steps_web.json");
                // List<Step> steps = await JsonSerializer.DeserializeAsync<List<Step>>(await streamTask);
                _steps = await JsonSerializer.DeserializeAsync<List<ProtocolStepModel>>(await streamTask);
            }

            // return steps;
            return _steps;
        }

        public Task<List<ProtocolStepModel>> GetStepsLocal()
        {
            if (_steps == null)
            {
                string content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/steps.json"));

                // List<Step> steps = JsonSerializer.Deserialize<List<Step>>(content);
                _steps = JsonSerializer.Deserialize<List<ProtocolStepModel>>(content);
            }

            // return Task.FromResult(steps);
            return Task.FromResult(_steps);
        }
    }
}