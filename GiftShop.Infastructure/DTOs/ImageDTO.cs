using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.DTOs;

public class ImageDTO
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("originLinkImage")]
    public string? OriginLinkImage { get; set; }

    [JsonPropertyName("localLinkImage")]
    public string? LocalLinkImage { get; set; }

    [JsonPropertyName("productID")]
    public Guid ProductID { get; set; }
}
