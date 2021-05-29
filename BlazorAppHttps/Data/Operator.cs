using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppHttps.Data
{
    public class Operator
    {
        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }

        [Required]

        public DateTime ProtocolStart { get; init; }

        [Required]

        public DateTime ProtocolEnd { get; set; }

        [Required]

        public string NumberOfSamples { get; set; }

        [Required]

        public string StartPlateID { get; set; }

        [Required]

        public string EndPlateID { get; set; }

    }
}
