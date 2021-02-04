using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class CargaFactura
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("issued_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Issued_date { get; set; }
        [JsonProperty("expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Expiration_date { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
        [JsonProperty("original_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Original_amount { get; set; }
        /*[JsonProperty("currency_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency_id { get; set; }*/
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency { get; set; }
        /*[JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country_id { get; set; }*/
        /*[JsonProperty("supplier_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier_id { get; set; }*/
        [JsonProperty("supplier", NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier { get; set; }
        /*[JsonProperty("debtor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor_id { get; set; }*/
        [JsonProperty("debtor", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor { get; set; }
        [JsonProperty("request_financing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Request_financing { get; set; }
    }
}
