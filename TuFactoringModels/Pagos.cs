using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Pagos
    {
        [JsonProperty("payments", NullValueHandling = NullValueHandling.Ignore)]
        public Payment Payments { get; set; } = new Payment();
        [JsonProperty("banks", NullValueHandling = NullValueHandling.Ignore)]
        public List<Bank> Banks { get; set; } = new List<Bank>();
        [JsonProperty("receipts", NullValueHandling = NullValueHandling.Ignore)]
        public List<Receipts> Facturas { get; set; } = new List<Receipts>();
        [JsonProperty("auction", NullValueHandling = NullValueHandling.Ignore)]
        public List<Auctions> Subasta { get; set; } = new List<Auctions>();
        [JsonProperty("alliedAccounts", NullValueHandling = NullValueHandling.Ignore)]
        public AlliedAccount AlliedAccounts { get; set; }
        [JsonProperty("accounts", NullValueHandling = NullValueHandling.Ignore)]
        public List<AccountRespond> Accounts { get; set; }
        [JsonProperty("entities", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> Entities { get; set; } = new List<Entity>();

        [JsonProperty("settings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Settings> Settings { get; set; } = new List<Settings>();
    }

}