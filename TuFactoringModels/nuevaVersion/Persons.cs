using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Persons
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("country")]
        public int Country { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [Display(Name = "Compañia")]
        [JsonProperty("company")]
        public string Company { get; set; }

        [Display(Name = "Nombres")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Display(Name = "Numero de Roteo")]
        [JsonProperty("routing_number")]
        public string Routing_number { get; set; }
        [JsonProperty("related")]
        public bool? Related { get; set; }

        [JsonProperty("accepted_agreement")]
        public bool Accepted_agreement { get; set; }
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonProperty("email")]
        public Email Email { get; set; } = new Email();

        [JsonProperty("phone")]
        public Phones Phone { get; set; } = new Phones();
        [JsonProperty("document")]
        public Document Document { get; set; } = new Document();
        [JsonProperty("address")]
        public Address Address { get; set; } = new Address();
        [JsonProperty("contacts")]
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();
        [JsonProperty("customers")]
        public List<Associate> Customers { get; set; } = new List<Associate>();
        [JsonProperty("suppliers")]
        public List<Associate> Suppliers { get; set; } = new List<Associate>();

    }

    public class PesonProfile
    {

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
        [JsonProperty("routing_number")]
        public string Routing_number { get; set; }
        [JsonProperty("related")]
        public bool? Related { get; set; }

        [JsonProperty("accepted_agreement")]
        public bool Accepted_agreement { get; set; }
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        [JsonProperty("email")]
        public Email Email { get; set; } = new Email();
        [JsonProperty("phone")]
        public Phones Phone { get; set; } = new Phones();
        [JsonProperty("document")]
        public Document Document { get; set; } = new Document();
        [JsonProperty("address")]
        public Address Address { get; set; } = new Address();
        [JsonProperty("contacts")]
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; } = new List<Account>();
        [JsonProperty("customers")]
        public List<AssociateConsulta> Customers { get; set; } = new List<AssociateConsulta>();
        [JsonProperty("suppliers")]
        public List<AssociateConsulta> Suppliers { get; set; } = new List<AssociateConsulta>();

    }


    public class RequestRegister
    {
        
        [JsonProperty("tokenReCaptcha", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenReCaptcha { get; set; } //El token humilde
        
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public Persons Person { get; set; } //El person humilde
        
    }
}
