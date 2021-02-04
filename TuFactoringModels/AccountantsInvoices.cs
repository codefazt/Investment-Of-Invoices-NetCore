using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class AccountantsInvoices
    {
        [JsonProperty("peopleID", NullValueHandling = NullValueHandling.Ignore)]
        public string PeopleID { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double Sum { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }

    public class ListAccountantsInvoices
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<AccountantsInvoices> List = new List<AccountantsInvoices>();
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }
}
