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

        // private List<Step> _steps;

        // public StepService()
        // {
        //     _steps = GetSteps();
        // }

        public async Task<List<Step>> GetStepsWeb()
        {

            HttpClient client = new HttpClient();

            Task<Stream> streamTask = client.GetStreamAsync("https://zutsukineko3339.blob.core.windows.net/$web/steps_web.json");
            List<Step> steps = await JsonSerializer.DeserializeAsync<List<Step>>(await streamTask);

            return steps;
        }

        public Task<List<Step>> GetStepsLocal()
        {

            string content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/steps.json"));

            List<Step> steps =
                JsonSerializer.Deserialize<List<Step>>(content);

            return Task.FromResult(steps);
        }
    }
}
