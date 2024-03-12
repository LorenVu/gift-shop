using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.DTOs;

public class UserDTO
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("userName")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("nickName")]
    public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

    [JsonPropertyName("birthDay")]
    public DateTime BirthDay { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("renameCount")]
    public int RenameCount { get; set; }

    [JsonPropertyName("deviceID")]
    public string DeviceID { get; set; } = string.Empty;

    [JsonPropertyName("blocked")]
    public bool Blocked { get; set; }
}
