using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

namespace BlazorAppHttps.Data
{
    public class StepService
    {

        public List<Step> Steps
        {
            get
            {
                return _steps;
            }
        }

        private List<Step> _steps;

        public StepService()
        {
            _steps = GetSteps();
        }

        private List<Step> GetSteps()
        {
            string content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/steps.json"));

            List<Step> steps =
                JsonSerializer.Deserialize<List<Step>>(content);

            return steps;
        }
    }
}
