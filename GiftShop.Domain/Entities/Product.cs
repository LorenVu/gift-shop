using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Products")]
public class Product : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("Code")]
    [StringLength(100)]
    public string? Code { get; set; }

    [Column("Description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Required]
    [Column("Prize")]
    public double Prize { get; set; }

    [Column("Discount")]
    public double Discount { get; set; }

    [Required]
    [Column("Currency")]
    [StringLength(100)]
    public string? Currency { get; set; }

    [Required]
    [Column("DefaultImage")]
    public string? DefaultImage { get; set; }

    [Column("OriginLinkDetail")]
    public string? OriginLinkDetail { get; set; }

    [Column("Url")]
    public string? Url { get; set; }

    [Required]
    [Column("Stock")]
    public int Stock { get; set; }

    [Required]
    [Column("CategoryId")]
    public int CategoryID { get; set; }

    [Required]
    [Column("BrandId")]
    public int BrandID { get; set; }

    public Category Categories { get; set; } = new Category();

    public Brand Brands { get; set; } = new Brand();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
