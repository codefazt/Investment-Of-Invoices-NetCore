using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Associate
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("invited")]
        public bool Invited { get; set; }

        [JsonProperty("identification")]
        public int Identification { get; set; }

        [JsonProperty("prefix")]
        public int? Prefix { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("person")]
        public string Person { get; set; }

    }

}
