using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class SegmentPerson
    {
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("confirmant")]
        public string Confirmant { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
    }
}
