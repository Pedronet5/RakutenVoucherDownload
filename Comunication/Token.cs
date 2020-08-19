using Newtonsoft.Json;

namespace RakutenVoucherDownload.Comunication
{
    public class Token
    {

        [JsonProperty("token_type")]
        public string TokenTyoe { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

    }
}
