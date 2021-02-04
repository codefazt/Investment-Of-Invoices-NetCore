using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class AlliedAccount
    {
        [JsonProperty("titular", NullValueHandling = NullValueHandling.Ignore)]
        public string Titular { get; set; }
        [JsonProperty("documentoidentidad", NullValueHandling = NullValueHandling.Ignore)]
        public string Documentoidentidad { get; set; }
        [JsonProperty("cuentas", NullValueHandling = NullValueHandling.Ignore)]
        public List<AcountData> Cuentas { get; set; } = new List<AcountData>();
    }

    public class AcountData
    {
        [JsonProperty("idbanco", NullValueHandling = NullValueHandling.Ignore)]
        public string Idbanco { get; set; }
        [JsonProperty("nombre", NullValueHandling = NullValueHandling.Ignore)]
        public string Nombre { get; set; }
        [JsonProperty("cuenta", NullValueHandling = NullValueHandling.Ignore)]
        public string Cuenta { get; set; }
        [JsonProperty("currencysymbol", NullValueHandling = NullValueHandling.Ignore)]
        public string Currencysymbol { get; set; }
        [JsonProperty("currencyiso", NullValueHandling = NullValueHandling.Ignore)]
        public string Currencyiso { get; set; }
    }
}

