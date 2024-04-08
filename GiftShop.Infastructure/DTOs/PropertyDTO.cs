
using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.DTOs;

public class PropertyDTO
{
    [JsonIgnore]
    [JsonPropertyName("id")]
    public Guid ID { get; set; } = Guid.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("productId")]
    public Guid ProductID { get; set; }
}
