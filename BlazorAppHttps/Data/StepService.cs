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
            Console.WriteLine("step service constructor");
            _steps = GetSteps();
        }

        // public Task<List<Step>> GetStepsAsync()
        private List<Step> GetSteps()
        {
            string content = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"/wwwroot/steps.json"));

            List<Step> steps =
                JsonSerializer.Deserialize<List<Step>>(content);


            // var steps = new Step[] {
            //     new Step {
            //          Title = "IDの確認​​",
            //          Descriptions = new List<string> {
            //             @"袋と採便ブラシ上のIDが同一であることを確認してください。​",
            //             @"書類確認者から袋を渡されます。採便ブラシを袋の中に入れ、書類確認者へ渡してください。​",
            //          },
            //         //  Voice = "voice/2.mp3",
            //         //  Video = "video/2.gif",
            //         } ,
            //     new Step {
            //             Title = @"採便ブラシをブラシ立てへ立てる​（書類確認者とダブルチェック）​",
            //             Descriptions = new List<string> {
            //                  @"採便ブラシを次亜塩素酸で拭き、IDとブラシ立ての位置を書類確認者に確認してください。​",
            //                         @"サンプルの量が少ない場合は、書類確認者に報告してください。​",
            //                        @"採便ブラシに緩みがある場合には、ネジ部分にテープシールを巻き、書類確認者に報告してください。​",
            //             },
            //             // Voice = "voice/2.mp3",
            //             Video = "video/2.gif",
            //         },
            //         new Step {
            //             Title = @"hello​",
            //             Descriptions = new List<string> {
            //                 @"world​",
            //             }
            //         },

            //     };

            // return Task.FromResult(steps);
            return steps;

        }
    }
}
