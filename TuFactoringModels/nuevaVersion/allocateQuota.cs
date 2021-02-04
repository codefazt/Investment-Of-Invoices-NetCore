using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class AllocateQuota
    {
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("confirmant")]
        public string Confirmant { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("person")]
        public string Person { get; set; }
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
        [JsonProperty("available")]
        public double Available { get; set; }
        [JsonProperty("currency")]
        public int Currency { get; set; }
    }
}
