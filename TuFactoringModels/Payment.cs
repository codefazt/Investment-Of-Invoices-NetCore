using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Payment
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency { get; set; }
        [JsonProperty("receipt", NullValueHandling = NullValueHandling.Ignore)]
        public string Receipt { get; set; }
        [JsonProperty("payment_date", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentDate { get; set; }
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public string Entity { get; set; }
        [JsonProperty("account_number", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
    }
}
