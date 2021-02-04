using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ParamGuests
    {
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public string Person { get; set; }
        [JsonProperty("invitation", NullValueHandling = NullValueHandling.Ignore)]
        public int Invitation { get; set; }
    }
}
