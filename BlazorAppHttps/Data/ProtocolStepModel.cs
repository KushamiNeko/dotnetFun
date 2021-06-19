using System;
using System.Collections.Generic;


namespace BlazorAppHttps.Data
{
    public class ProtocolStepModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public List<string> Descriptions { get; set; }

        public string AudioUrl { get; set; }

        public string VideoUrl { get; set; }
    }
}