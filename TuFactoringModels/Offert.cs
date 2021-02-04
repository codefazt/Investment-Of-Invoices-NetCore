using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Offert
    {
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int Country { get; set; }
        [JsonProperty("publication_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Publication_id { get; set; }
        [JsonProperty("factor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Factor_id { get; set; }
        [JsonProperty("bid_amount", NullValueHandling = NullValueHandling.Ignore)]
        public float Bid_amount { get; set; }
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
    }
}
