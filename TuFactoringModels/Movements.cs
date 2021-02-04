using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Movements
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("movement_dated", NullValueHandling = NullValueHandling.Ignore)]
        public string MovementDated { get; set; }

        [JsonProperty("account_number", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }

        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public string Amount { get; set; }

        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Entity { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("payment_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Payment_id { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
        [JsonProperty("error_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
        [JsonProperty("error_msg", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMsg { get; set; }

    }
}
