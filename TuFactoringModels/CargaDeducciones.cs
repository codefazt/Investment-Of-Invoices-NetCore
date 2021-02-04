using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class CargaDeducciones
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("invoice_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice_id { get; set; }
        [JsonProperty("charge_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Charge_type_id { get; set; }
        [JsonProperty("currency_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency_id { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
    }
}

