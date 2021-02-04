using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ClienteOfConfirmant
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("emails")]
        public List<Email> Emails { get; set; } = new List<Email>();
        
        [JsonProperty("confirmings")]
        public List<Confirming>  Confirmings { get; set; } = new List<Confirming>();

        [JsonProperty("documents")]
        public List<Document> Documents { get; set; } = new List<Document>();


        [JsonProperty("addresses")]
        public List<Addresse> Addresses { get; set; } = new List<Addresse>();

        [JsonProperty("phone")]
        public List<PhonePrincipal> Phone { get; set; } = new List<PhonePrincipal>();

        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();

        [JsonProperty("contacts")]
        public List<Contactos> Contacts { get; set; } = new List<Contactos>();
    }

    public class Email
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }

    public class Confirming
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("amountRisk", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountRisk { get; set; }

    }

    public class Document
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("display_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Display_number { get; set; }
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public int? Prefix { get; set; }
        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public int? Identification { get; set; }
    }

    public class Account
    {
        
        [JsonProperty("nameOnAccount", NullValueHandling = NullValueHandling.Ignore)]
        public string NameOnAccount { get; set; }
        
        [JsonProperty("accountNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string AaccountNumber { get; set; }
        
        [JsonProperty("accountType", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountType { get; set; }
        
        [JsonProperty("currencyId", NullValueHandling = NullValueHandling.Ignore)]
        public int CurrencyId { get; set; }

        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public int Default { get; set; }
        [JsonProperty("validationStatus", NullValueHandling = NullValueHandling.Ignore)]
        public int? ValidationStatus { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }
    }

    public class Addresse
    {
        [JsonProperty("street")]
        public string Street { get; set; }
        
        [JsonProperty("building")]
        public string Building { get; set; }
        
        [JsonProperty("cityId")]
        public int CityId { get; set; }
        
        [JsonProperty("zip")]
        public string Zip { get; set; }
    }

    public class Contactos
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("last_name")]
        public string Last_name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("identificationId")]
        public int IdentificationId { get; set; }

        [JsonProperty("prefixId")]
        public int PrefixId { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
