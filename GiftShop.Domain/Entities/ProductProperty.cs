using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("ProductProperties")]
public class ProductProperty
{

    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [Column("Value")]
    public string? Value { get; set; }

    [Required]
    [Column("ProductId")]
    public int ProductID { get; set; }

    public Product Product { get; set; } = new Product();
}
