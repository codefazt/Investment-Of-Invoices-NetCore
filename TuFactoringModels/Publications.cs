using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.Interface;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Publications : IStatus
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("invoice_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice_id { get; set; }
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)]
        public Invoices Invoice { get; set; }
        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public Program Program { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency currency { get; set; }
        [JsonProperty("bank_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank_id { get; set; }
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Start_date { get; set; }
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Confirmant { get; set; }
        [JsonProperty("term_days", NullValueHandling = NullValueHandling.Ignore)]
        public int? Term_days { get; set; }
        [JsonProperty("reservation", NullValueHandling = NullValueHandling.Ignore)]
        public string Reservation { get; set; }
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Discount { get; set; }
        [JsonProperty("isOffered", NullValueHandling = NullValueHandling.Ignore)]
        public bool? isOffered { get; set; }
        [JsonProperty("bids", NullValueHandling = NullValueHandling.Ignore)]
        public List<Bids> Bids { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        [JsonProperty("state_mostrar", NullValueHandling = NullValueHandling.Ignore)]
        public string StateMostrar { get; set; }
        [JsonProperty("error")]
        public Exception Error { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
        [JsonProperty("earnings", NullValueHandling = NullValueHandling.Ignore)]
        public double? Earnings { get; set; }
        [JsonProperty("profitability", NullValueHandling = NullValueHandling.Ignore)]
        public double? Profitability { get; set; }
        [JsonProperty("payable", NullValueHandling = NullValueHandling.Ignore)]
        public double? Payable { get; set; }
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public double? Rate { get; set; }
        [JsonProperty("commission", NullValueHandling = NullValueHandling.Ignore)]
        public double? Commission { get; set; }
        [JsonProperty("receivable", NullValueHandling = NullValueHandling.Ignore)]
        public double? Receivable { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("receipts", NullValueHandling = NullValueHandling.Ignore)]
        public List<Receipts> Receipts { get; set; }
        [JsonProperty("changelogs", NullValueHandling = NullValueHandling.Ignore)]
        public List<ChangeLogs> ChangeLogs { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
    }
    
    public partial class PublicationsResponse
    {

        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Publications> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }

}
