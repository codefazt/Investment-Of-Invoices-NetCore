using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class AssociateConsulta
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("invitedAt")]
        public string InvitedAt { get; set; }

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

        [JsonProperty("phone_number")]
        public string Phone_number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("person")]
        public Prospecto Person { get; set; }
    }
}
