using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class User : IIdentity
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public virtual Guid Id { get; set; }
        [JsonProperty("user_name", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string UserName { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Email { get; set; }
        [JsonProperty("email_confirmed", NullValueHandling = NullValueHandling.Ignore)]
        public virtual bool EmailConfirmed { get; set; }
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public virtual String PasswordHash { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NormalizedUserName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AuthenticationType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAuthenticated { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [PersonalData]
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }
        [PersonalData]
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [PersonalData]
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int CountryId { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [PersonalData]
        [JsonProperty("owner_id", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerId { get; set; }
        [PersonalData]
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string Confirmant { get; set; }
        [PersonalData]
        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<Role> Roles { get; set; }
        [JsonProperty("roles_id", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Roles_id { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
        [JsonProperty("foto", NullValueHandling = NullValueHandling.Ignore)]
        public string Foto { get; set; }
        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public int? Identification { get; set; }
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public int? Prefix { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        [JsonProperty("contratoProveedor", NullValueHandling = NullValueHandling.Ignore)]
        public string ContratoProveedor { get; set; }
        [JsonProperty("terminoCondiciones", NullValueHandling = NullValueHandling.Ignore)]
        public string TerminoCondiciones { get; set; }
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public Prospecto Person { get; set; }

        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreateAt { get; set; }
    }

    public partial class UsersResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<User> List { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
