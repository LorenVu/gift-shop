using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Categories")]
public class Category : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("Code")]
    [StringLength(100)]
    public string? Code { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

}
