using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Financiable
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Expiration_date { get; set; }
        [JsonProperty("term_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? TermDays { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
        [JsonProperty("original_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Original_amount { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank { get; set; }
        [JsonProperty("bank_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank_id { get; set; }
        [JsonProperty("supplier", NullValueHandling = NullValueHandling.Ignore)]
        public People supplier { get; set; }
        [JsonProperty("request_financing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Request_financing { get; set; }

        [JsonProperty("publications", NullValueHandling = NullValueHandling.Ignore)]
        public List<Publications> Publications { get; set; }
        [JsonProperty("publication", NullValueHandling = NullValueHandling.Ignore)]
        public Publications Publication { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }

    }

    public class FinanciableResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Financiable> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
