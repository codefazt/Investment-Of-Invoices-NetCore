using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Prospectos
    {
        [JsonProperty("count")]
        public int? Count { get; set; }
        [JsonProperty("list")]
        public List<Prospecto> List { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

    }
}
