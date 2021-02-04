using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ParamProspecto
    {

        [JsonProperty("country")]
        public int Country { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        //[JsonProperty("discriminator")]
       // public string Discriminator { get; set; }
        //[JsonProperty("category")]
        //public int? Category { get; set; }
        [JsonProperty("document")]
        public DocumentIdentification Document { get; set; }
        [JsonProperty("filter")]
        public filterInvoice Filter { get; set; }
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class DocumentIdentification
    {

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int Country { get; set; }

        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public int Identification { get; set; }

        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public int? Prefix { get; set; }

        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

    }

    public class FilterBackOffice
    {

        [JsonProperty("state")]
        public int? State { get; set; }
        [JsonProperty("cities")]
        public int? Cities { get; set; }
        [JsonProperty("category")]
        public int? Category { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("participant")]
        public string Participant { get; set; }
        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
        [JsonProperty("people")]
        public string People { get; set; }
    }
}
