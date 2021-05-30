using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppHttps.Data
{
    public class DNATrayInput
    {
        public string UID { get; } = Guid.NewGuid().ToString();

        [Required]
        public string TrayID { get; set; }

        [Required]

        public string Location { get; set; }

    }
}
