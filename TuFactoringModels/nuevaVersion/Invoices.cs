using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.Interface;

namespace TuFactoringModels
{
    #region Invoice
    public class Invoices : IStatus
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("issued_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Issued_date { get; set; }
        [JsonProperty("expiration_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Expiration_date { get; set; }
        [JsonProperty("term_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? Term_days { get; set; }
        [JsonProperty("term_finalized", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Term_finalized { get; set; }
        [JsonProperty("original_amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Original_amount { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("state_mostrar", NullValueHandling = NullValueHandling.Ignore)]
        public string StateMostrar { get; set; }
        [JsonProperty("charges", NullValueHandling = NullValueHandling.Ignore)]
        public List<Charges> Charges { get; set; }
        [JsonProperty("publication", NullValueHandling = NullValueHandling.Ignore)]
        public Publications Publication { get; set; }
        [JsonProperty("publications", NullValueHandling = NullValueHandling.Ignore)]
        public List<Publications> Publications { get; set; }
        [JsonProperty("currency_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency_id { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country_id { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("request_financing")]
        public bool? Request_financing { get; set; }
        [JsonProperty("supplier_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Supplier_id { get; set; }
        [JsonProperty("debtor_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Debtor_id { get; set; }
        [JsonProperty("supplier", NullValueHandling = NullValueHandling.Ignore)]
        public People Supplier { get; set; }
        [JsonProperty("debtor", NullValueHandling = NullValueHandling.Ignore)]
        public People Debtor { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public Exception Error { get; set; }
        [JsonProperty("clientname", NullValueHandling = NullValueHandling.Ignore)]
        public string Clientname { get; set; }
        [JsonProperty("suppliername", NullValueHandling = NullValueHandling.Ignore)]
        public string Suppliername { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("reference", NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
        [JsonProperty("comision", NullValueHandling = NullValueHandling.Ignore)]
        public string Comision { get; set; }
        [JsonProperty("profitability", NullValueHandling = NullValueHandling.Ignore)]
        public double? Profitability { get; set; }
        [JsonProperty("receivable", NullValueHandling = NullValueHandling.Ignore)]
        public double? Receivable { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("changelogs", NullValueHandling = NullValueHandling.Ignore)]
        public List<ChangeLogs> ChangeLogs { get; set; }


    }
    #endregion

    #region Partal ChangeLogs
    public partial class ChangeLogs
    {
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public string To { get; set; }
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }
        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }
        [JsonProperty("changedAt", NullValueHandling = NullValueHandling.Ignore)]
        public string ChangedAt { get; set; }
        [JsonProperty("changedBy", NullValueHandling = NullValueHandling.Ignore)]
        public string ChangedBy { get; set; }
    }

    #endregion

    #region Partial Charges
    public partial class Charges
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("invoice_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice_id { get; set; }
        [JsonProperty("charge_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Charge_type_id { get; set; }
        [JsonProperty("currency_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Currency_id { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
    }
    #endregion

    #region Partial ChargeTypes
    public partial class ChargeTypes
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("regexp", NullValueHandling = NullValueHandling.Ignore)]
        public string RegExp { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }
    }
    #endregion

    #region Partial InvoicesResponse
    public partial class InvoicesResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Invoices> List { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
    #endregion
}
