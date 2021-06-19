using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppHttps.Data
{
    public class DnaTrayInputModel
    {
        public string Uid { get; } = Guid.NewGuid().ToString();

        public string TrayId { get; set; }

        public string Location { get; set; }
    }
}