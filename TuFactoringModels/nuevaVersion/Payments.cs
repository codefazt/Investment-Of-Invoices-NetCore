using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Payments
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("receipt")]
        public Receipts Receipt { get; set; }
        [JsonProperty("entity")]
        public Entity Entity { get; set; }
        [JsonProperty("amount")]
        public Double Amount { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("account_number")]
        public string Account_number { get; set; }
        [JsonProperty("payment_date")]
        public string Payment_date { get; set; }
        [JsonProperty("currency")]
        public Currency Currency { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }
}
