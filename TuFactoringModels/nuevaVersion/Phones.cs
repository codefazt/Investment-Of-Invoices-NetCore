using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Phones
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

        [Display(Name = "Numero de Teléfono")]
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("country")]
        public int Country { get; set; }
    }
}
