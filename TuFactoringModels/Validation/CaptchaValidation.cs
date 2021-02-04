using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TuFactoringModels.Validation
{
    public class CaptchaValidation
    {
        public string Site { get; }
        public string Secret { get; }

        public CaptchaValidation()
        {
            Site = "6LexhPMUAAAAAMc2tL-GP1Sc7-UL-TvnfmZlNtIk";
            Secret = "6LexhPMUAAAAAChlTnpz02Lz4XBhGrdyOS1TvJm6";
        }

        public class ResultCaptcha
        {
            [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
            public bool Success { get; set; }
            [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
            public float Score { get; set; }
            [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore)]
            public string Action { get; set; }
            [JsonProperty("challenge_ts", NullValueHandling = NullValueHandling.Ignore)]
            public DateTime ChallengeTs { get; set; }
            [JsonProperty("hostname", NullValueHandling = NullValueHandling.Ignore)]
            public string Hostname { get; set; }
            [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
            public string Errors { get; set; }

        } 

        public async Task<ResultCaptcha> Validate(string token)
        {
            ResultCaptcha result = new ResultCaptcha();

            try
            {

                var client = new HttpClient();

                var ruta = $"https://www.google.com/recaptcha/api/siteverify?secret={Secret}&response={token}";

                var respuesta = await client.GetStringAsync(requestUri: ruta);

                result = JsonConvert.DeserializeObject<ResultCaptcha>(respuesta);

            }
            catch (Exception e)
            {
                result.Errors = e.Message;
                return result;
            }

            return result;
        }

    }
}
