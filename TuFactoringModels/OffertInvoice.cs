using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class OffertInvoice
    {
        [JsonProperty("publication_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Publication_id { get; set; }
        [JsonProperty("bid_amount", NullValueHandling = NullValueHandling.Ignore)]
        public float? Bid_amount { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? country { get; set; }
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string confirmant { get; set; }
    }
}
