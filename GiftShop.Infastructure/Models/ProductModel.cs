
using System.Text.Json.Serialization;

namespace GiftShop.Infastructure.Models;

public class ProductModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("orderByPrize")]
    public bool OrderByPrize { get; set; }

    [JsonPropertyName("orderByDiscount")]
    public bool OrderByDiscount { get; set; }

    [JsonPropertyName("stock")]
    public bool Stock { get; set; }

    /// <summary>
    /// Format: dd/MM/yyyy
    /// </summary>
    [JsonPropertyName("fromDate")]
    public string? FromDate { get; set; }

    /// <summary>
    /// Format: dd/MM/yyyy
    /// </summary>
    [JsonPropertyName("toDate")]
    public string? ToDate { get; set; }
}
