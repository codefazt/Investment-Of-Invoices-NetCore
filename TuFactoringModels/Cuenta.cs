using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TuFactoringModels
{
    public class Cuenta
    {
        [JsonProperty("bank_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank_id { get; set; }
        [JsonProperty("name_on_account", NullValueHandling = NullValueHandling.Ignore)]
        public string Name_on_account { get; set; }
        [JsonProperty("account_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Account_number { get; set; }
        [JsonProperty("account_type", NullValueHandling = NullValueHandling.Ignore)]
        public string Account_type { get; set; }
        [JsonProperty("currency_id", NullValueHandling = NullValueHandling.Ignore)]
        public int Currency_id { get; set; }
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public int Default { get; set; }

    }
}
