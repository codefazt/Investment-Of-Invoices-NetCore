using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Bids
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }
        [JsonProperty("publication_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Publication_id { get; set; }
        [JsonProperty("publication", NullValueHandling = NullValueHandling.Ignore)]
        public Publications Publication { get; set; }
        [JsonProperty("factor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Factor_id { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Discount { get; set; }
        [JsonProperty("factor", NullValueHandling = NullValueHandling.Ignore)]
        public People Factor { get; set; }
        [JsonProperty("auction_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Auction_id { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string Created_at { get; set; }
        
    }
}
