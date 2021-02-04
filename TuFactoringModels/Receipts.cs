using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Receipts
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string ID { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("receipt_date", NullValueHandling = NullValueHandling.Ignore)]
        public string Receipt_date { get; set; }
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Entity { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }

        [JsonProperty("payer", NullValueHandling = NullValueHandling.Ignore)]
        public People Person { get; set; }

        [JsonProperty("paying_account", NullValueHandling = NullValueHandling.Include)]
        public Account Paying_account { get; set; }

        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public People Receiver { get; set; }

        [JsonProperty("receiving_account", NullValueHandling = NullValueHandling.Ignore)]
        public TuFactoringModels.nuevaVersion.AccountRespond ReceivingAccount { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }

        [JsonProperty("paid", NullValueHandling = NullValueHandling.Ignore)]
        public double? Paid { get; set; }
        [JsonProperty("processing", NullValueHandling = NullValueHandling.Ignore)]
        public double? Processing { get; set; }
        [JsonProperty("unpaid", NullValueHandling = NullValueHandling.Ignore)]
        public double? Unpaid { get; set; }
        [JsonProperty("commission", NullValueHandling = NullValueHandling.Ignore)]
        public double? Commission { get; set; }

        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public Program Program { get; set; }

        [JsonProperty("publications", NullValueHandling = NullValueHandling.Ignore)]
        public List<Publications> Publications { get; set; }

        [JsonProperty("payments", NullValueHandling = NullValueHandling.Ignore)]
        public List<Payments> Payments { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }
    }

    public partial class ReceiptsResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Receipts> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
    }

    public partial class Payments
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("receipt", NullValueHandling = NullValueHandling.Ignore)]
        public Receipts Receipt { get; set; }

        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Entity { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public double? Amount { get; set; }

        [JsonProperty("payment_date", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentDate { get; set; }

        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        [JsonProperty("account_number", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        
    }
}
