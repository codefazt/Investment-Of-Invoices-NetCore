using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Address
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("line1")]
        [Display(Name = "Dirección Principal")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        [Display(Name = "Dirección Opcional")]
        public string Line2 { get; set; }

        [JsonProperty("region")]
        [Display(Name = "Region")]
        public int Region { get; set; }

        [JsonProperty("city")]
        [Display(Name = "Ciudad")]
        public int City { get; set; }
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }
    }
}
