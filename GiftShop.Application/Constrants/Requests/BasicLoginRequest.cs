using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GiftShop.Application.Constrants.Requests;

public class BasicLoginRequest
{
    [Required]
    [JsonPropertyName("password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("deviceID")]
    public string DeviceID { get; set; } = string.Empty;
}
