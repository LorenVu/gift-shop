using Newtonsoft.Json;
using System.Security.Claims;
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

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    [JsonPropertyName("nickName")]
    public string? NickName { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("errorCode")]
    public long ErrorCode { get; set; }

    [JsonPropertyName("errorCodeEx")]
    public long ErrorCodeEx { get; set; }

    [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("firstLogin")]
    public bool FirstLogin { get; set; }
}
