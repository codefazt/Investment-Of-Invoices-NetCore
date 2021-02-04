using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Country
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }

        [JsonProperty("settings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Settings> Settings { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; } = null;
        [JsonProperty("iso_3166_2", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso_3166_2 { get; set; }
        [JsonProperty("iso_3166_3", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso_3166_3 { get; set; }
        [JsonProperty("calling_code", NullValueHandling = NullValueHandling.Ignore)]
        public string CallingCode { get; set; }
        [JsonProperty("identifications", NullValueHandling = NullValueHandling.Ignore)]
        public List<Identification> Identifications { get; set; }
        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
        [JsonProperty("related", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Related { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
        [JsonProperty("subdivisions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Subdivision> Subdivisions { get; set; }
        [JsonProperty("currencies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Currency> Currencies { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("allieds", NullValueHandling = NullValueHandling.Ignore)]
        public List<Allieds> Allieds { get; set; }
        [JsonProperty("invoice_types", NullValueHandling = NullValueHandling.Ignore)]
        public List<Invoice_types> Invoice_types { get; set; }
        [JsonProperty("charges", NullValueHandling = NullValueHandling.Ignore)]
        public List<ChargeTypes> Charges { get; set; }
        /* [JsonProperty("charge_types", NullValueHandling = NullValueHandling.Ignore)]
         public List<Charge_types> Charge_types { get; set; }*/

        [JsonProperty("occupations", NullValueHandling = NullValueHandling.Ignore)]
        public List<Occupation> Occupations { get; set; }
        [JsonProperty("purposes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Purpos> Purposes { get; set; }

        [JsonProperty("banks", NullValueHandling = NullValueHandling.Ignore)]
        public List<Bank> Banks { get; set; } = new List<Bank>();


        [JsonProperty("entities", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> Entities { get; set; } = new List<Entity>();

        [JsonProperty("digits", NullValueHandling = NullValueHandling.Ignore)]
        public int? Digits { get; set; }

        [JsonProperty("programs", NullValueHandling = NullValueHandling.Ignore)]
        public List<Program> Program { get; set; }

    }
}
