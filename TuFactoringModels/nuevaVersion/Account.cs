using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Display(Name = "Banco")]
        [JsonProperty("entity")]
        public string Entity { get; set; }

        [Display(Name = "Tipo de Cuenta")]
        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [Display(Name = "Moneda")]
        [JsonProperty("currency")]
        public int? Currency { get; set; }

        [Display(Name = "Numero de Cuenta")]
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; }
        [JsonProperty("status")]
        public bool Status { get; set; }
    }

    public class AccountRespond
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("entity")]
        public Entity Entity { get; set; }
        [JsonProperty("accountType")]
        public string AccountType { get; set; }
        [JsonProperty("currency")]
        public int? Currency { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; }
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
    }

}
