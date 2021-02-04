using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{
    public class Accountants
    {
        [JsonProperty("peopleID", NullValueHandling = NullValueHandling.Ignore)]
        public string PeopleID { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public float? Sum { get; set; }
    }
}
