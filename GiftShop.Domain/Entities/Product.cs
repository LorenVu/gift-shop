using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Products")]
public class Product : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(150)]
    public string? Name { get; set; }

    [Column("Code")]
    [StringLength(100)]
    public string? Code { get; set; }

    [MaxLength(2000)]
    [Column("Description")]
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
    public bool Stock { get; set; }

    [Required]
    [Column("CategoryId")]
    public Guid CategoryID { get; set; }

    [Required]
    [Column("BrandId")]
    public Guid BrandID { get; set; }

    //public Category Categories { get; set; } = new Category();

    //public Brand Brands { get; set; } = new Brand();

    public ICollection<Property> Properties { get; set; } = new List<Property>();

    public ICollection<Item> Items { get; set; } = new List<Item>();
}
