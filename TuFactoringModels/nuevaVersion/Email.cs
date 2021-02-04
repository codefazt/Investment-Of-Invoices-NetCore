using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Email
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

        [Display(Name = "Correo")]
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
