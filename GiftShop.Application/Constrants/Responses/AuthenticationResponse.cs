using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GiftShop.Application.Constrants.Responses;

public class AuthenticationResponse
{
    [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("success")]
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonProperty("userId")]
    [JsonPropertyName("userId")]
    public long UserId { get; set; }

    [JsonProperty("errorCode")]
    [JsonPropertyName("errorCode")]
    public long ErrorCode { get; set; }

    [JsonProperty("errorCodeEx")]
    [JsonPropertyName("errorCodeEx")]
    public long ErrorCodeEx { get; set; }

    [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonProperty("firstLogin")]
    [JsonPropertyName("firstLogin")]
    public bool FirstLogin { get; set; }
}
