using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Profiles
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        [JsonProperty("emails")]
        public List<Email> Emails { get; set; } = new List<Email>();
        [JsonProperty("phones")]
        public List<Phones> Phones { get; set; } = new List<Phones>();
        [JsonProperty("documents")]
        public List<Document> Documents { get; set; } = new List<Document>();
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; } = new List<Address>();
        [JsonProperty("contacts")]
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();       
        [JsonProperty("customers")]
        public List<Associate> Customers { get; set; } = new List<Associate>();
        [JsonProperty("suppliers")]
        public List<Associate> Suppliers { get; set; } = new List<Associate>();
         
    }
}
