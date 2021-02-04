using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class DetallesClientes
    {
        [JsonProperty("company")]
        public string Company { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("document")]
        public DocumentDetalles Document { get; set; } = new DocumentDetalles();
        [JsonProperty("representante")]
        public Contact Representante { get; set; } = new Contact();
        [JsonProperty("contacto")]
        public Contact Contacto { get; set; } = new Contact();
        [JsonProperty("address")]
        public AddressDetalles Address { get; set; } = new AddressDetalles();
        [JsonProperty("accounts")]
        public Account Accounts { get; set; } = new Account();
        [JsonProperty("limiteCredito")]
        public LimiteCredito LimiteCredito { get; set; } = new LimiteCredito();
        [JsonProperty("cuentaBancaria")]
        public CuentaBancaria CuentaBancaria { get; set; } = new CuentaBancaria();
        [JsonProperty("accountsList")]
        public List<CuentaBancaria> AccountsList { get; set; } = new List<CuentaBancaria>();
        [JsonProperty("limiteCreditoList")]
        public List<LimiteCredito> LimiteCreditoList { get; set; } = new List<LimiteCredito>();
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }

    public class AddressDetalles
    {
        [JsonProperty("line1")]
        public string Line1 { get; set; }
        [JsonProperty("line2")]
        public string Line2 { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
    }

    public class DocumentDetalles
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }

    public class LimiteCredito
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("usage")]
        public double Usage { get; set; }

        [JsonProperty("available")]
        public double Available { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
        [JsonProperty("iso_4217")]
        public string Iso_4217 { get; set; }
        
    }

    public class CuentaBancaria
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("entity")]
        public string Entity { get; set; }
        [JsonProperty("accountType")]
        public string AccountType { get; set; }
        [JsonProperty("currency")]
        public int? Currency { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; }
        [JsonProperty("simbol")]
        public string Simbol { get; set; }
    }
}
