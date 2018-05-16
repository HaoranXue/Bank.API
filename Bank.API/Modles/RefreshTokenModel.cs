using Newtonsoft.Json;

namespace Bank.API.Modles
{ 
    public class RefreshTokenModel
    {
        [JsonProperty("expires_in")]
        public int ExpiryTime { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string Refresh_Token { get; set; }
    }
}