using Newtonsoft.Json;
using System.Collections.Generic;

namespace TuFactoringModels
{
    public class Auctions
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country_id { get; set; }            
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("dated")]
        public string Date { get; set; }
        [JsonProperty("opened")]
        public string Opening { get; set; }
        [JsonProperty("closed")]
        public string Closing { get; set; }
        [JsonProperty("payed")]
        public string Payments { get; set; }
        [JsonProperty("conciliation")]
        public string Conciliation { get; set; }
        [JsonProperty("finalized")]
        public string Ending { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        [JsonProperty("Bid", NullValueHandling = NullValueHandling.Ignore)]
        public List<Bids> Bid { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updateAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateAt { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }

    public partial class AuctionsResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Auctions> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
