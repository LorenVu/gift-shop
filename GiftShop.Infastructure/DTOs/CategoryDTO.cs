using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.DTOs;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("isDelete")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("status")]
    public bool Status { get; set; }
}
