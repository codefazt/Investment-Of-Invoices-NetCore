using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Verification
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("list")]
        public List<ListVerification> List { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

    }
}
