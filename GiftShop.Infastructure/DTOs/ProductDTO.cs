using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.DTOs;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("prize")]
    public double Prize { get; set; }

    [JsonPropertyName("discount")]
    public double Discount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("defaultImage")]
    public string? DefaultImage { get; set; }

    [JsonPropertyName("originLinkDetail")]
    public string? OriginLinkDetail { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("stock")]
    public bool Stock { get; set; }

    [JsonPropertyName("categoryId")]
    public Guid CategoryID { get; set; }

    [JsonPropertyName("brandId")]
    public Guid BrandID { get; set; }

    [JsonPropertyName("properties")]
    public List<PropertyDTO> Properties { get; set; } = new List<PropertyDTO>();
}
