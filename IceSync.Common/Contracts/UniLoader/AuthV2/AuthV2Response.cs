using Newtonsoft.Json;

namespace IceSync.Common.Contracts.UniLoader.AuthV2;

public record AuthV2Response
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}