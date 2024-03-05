using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.Models;

public class BrandModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("isDelete")]
    public bool IsDelete { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; }
}
