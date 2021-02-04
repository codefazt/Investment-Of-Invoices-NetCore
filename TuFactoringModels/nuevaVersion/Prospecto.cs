using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Prospecto
    {
        
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        [JsonProperty("is_fintech")]
        public bool isFintech { get; set; }
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
        public List<AccountRespond> Accounts { get; set; } = new List<AccountRespond>();
        [JsonProperty("accountants")]
        public List<Accountants> Accountants { get; set; } = new List<Accountants>();
        [JsonProperty("agreements")]
        public List<Agreements> Agreements { get; set; } = new List<Agreements>();
        [JsonProperty("identities")]
        public List<Identities> Identities { get; set; } = new List<Identities>();
        [JsonProperty("customers")]
        public List<AssociateConsulta> Customers { get; set; } = new List<AssociateConsulta>();
        [JsonProperty("suppliers")]
        public List<AssociateConsulta> Suppliers { get; set; } = new List<AssociateConsulta>();
        [JsonProperty("executives")]
        public List<Executives> Executives { get; set; } = new List<Executives>();
        [JsonProperty("quotas")]
        public List<Quotas> Quotas { get; set; } = new List<Quotas>();
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; } = new List<Entity>();
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

    }
}
