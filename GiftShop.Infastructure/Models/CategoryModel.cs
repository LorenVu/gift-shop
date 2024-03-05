using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.Models;

public class CategoryModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool? IsDeleted { get; set; }

    [JsonPropertyName("status")]
    public bool? Status { get; set; }
}
