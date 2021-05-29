using System;
using System.Collections.Generic;


namespace BlazorAppHttps.Data
{
    public class Step
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public List<string> Descriptions { get; set; }

        public string Audio {get; set;}

        public string Video {get; set;}

    }
}
