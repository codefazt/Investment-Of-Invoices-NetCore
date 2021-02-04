using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ParamCreditLimit
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("confirmant")]
        public string Confirmant { get; set; }

        [JsonProperty("country")]
        public int Country { get; set; }

        [JsonProperty("filter")]
        public filterInvoice Filter { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
