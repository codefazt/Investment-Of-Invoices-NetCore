using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Identities
    {
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
