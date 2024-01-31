using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Images")]
public class Images
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("OriginLinkImage")]
    [StringLength(100)]
    public string? OriginLinkImage { get; set; }

    [Column("LocalLinkImage")]
    [StringLength(100)]
    public string? LocalLinkImage { get; set; }

    [Required]
    [Column("ProductId")]
    public Guid ProductID { get; set; }

    public Product Product { get; set; } = new Product();
}

