using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{

    public class filterInvoice
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("factor", NullValueHandling = NullValueHandling.Ignore)]
        public string Factor_id { get; set; }

        [JsonProperty("supplier", NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier_id { get; set; }

        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string Confirmant_id { get; set; }

        [JsonProperty("debtor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor_id { get; set; }

        [JsonProperty("debtorId", NullValueHandling = NullValueHandling.Ignore)]
        public string DebtorId { get; set; }

        //[Remote(controller: "invoice", action: "invoiceNumber", ErrorMessage = "errorInvoiceNumber", HttpMethod = "post")]
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency_id { get; set; }
        [JsonProperty("financied", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Financied { get; set; }
        [JsonProperty("isOffered", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOffered { get; set; }
        [JsonProperty("bidsStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string BidsStatus { get; set; }
        [Remote(controller: "invoice", action: "expirationFrom", HttpMethod = "post", ErrorMessage = "errorFromGreatherTo")]
        [DataType(DataType.Date, ErrorMessage = "errorDate")]
        [JsonProperty("expiration_from", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpirationFrom { get; set; }

        [JsonProperty("expiration_to", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpirationTo { get; set; }

        [JsonProperty("issued_from", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? IssuedFrom { get; set; }

        [JsonProperty("issued_to", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? IssuedTo { get; set; }

        [JsonProperty("amount_from", NullValueHandling = NullValueHandling.Ignore)]
        public float? AmountFrom { get; set; }
        [JsonProperty("amount_to", NullValueHandling = NullValueHandling.Ignore)]
        public float? AmountTo { get; set; } 

        [JsonProperty("invoiceStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceStatus { get; set; }
        [JsonProperty("invoiceStatusNot", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceStatusNot { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }

        //Clientes Banco
        [JsonProperty("debtor", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("amountRiskFrom", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountRiskFrom { get; set; }
        [JsonProperty("amountRiskTo", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountRiskTo { get; set; }

        [JsonProperty("amountRiskAvailableFrom", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountRiskAvailableFrom { get; set; }
        [JsonProperty("amountRiskAvailableTo", NullValueHandling = NullValueHandling.Ignore)]
        public double? AmountRiskAvailableTo { get; set; }

        //Usuarios Backoffice

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public int? Category { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }

        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public int? City { get; set; }
        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        public int? Region { get; set; }
        [JsonProperty("people", NullValueHandling = NullValueHandling.Ignore)]
        public string People { get; set; }
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("change_from", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ChangeFrom { get; set; }

        [JsonProperty("change_to", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ChangeTo { get; set; }
        [JsonProperty("changeStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string ChangeStatus { get; set; }

        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public string Program { get; set; }
        [JsonProperty("programsAllInvoiceStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string ProgramsAllInvoiceStatus { get; set; }
        [JsonProperty("invoiceStatusProgramNot", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceStatusProgramNot { get; set; }
    }

}
