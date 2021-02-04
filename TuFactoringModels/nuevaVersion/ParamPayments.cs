using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ParamPayments
    {
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("receipt")]
        public Receipts Receipt { get; set; }
        [JsonProperty("entity")]
        public string Entity { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("receiving_account")]
        public string Receiving_account { get; set; }
        [JsonProperty("payment_date")]
        public string Payment_date { get; set; }
        [JsonProperty("currency")]
        public int? Currency { get; set; }
        [JsonProperty("paying_account")]
        public string Paying_account { get; set; }
    }
}
