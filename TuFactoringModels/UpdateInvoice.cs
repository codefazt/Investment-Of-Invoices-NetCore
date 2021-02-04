using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class UpdateInvoice
    {
        [JsonProperty("invoice_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice_id { get; set; }
        [JsonProperty("debtor", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor_id { get; set; }
        [JsonProperty("publication_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Publication_id { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? country { get; set; }
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string confirmant { get; set; }
        [JsonProperty("bid_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Bid_id { get; set; }
    }
}
