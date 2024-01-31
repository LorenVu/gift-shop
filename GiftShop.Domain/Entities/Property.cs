using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Properties")]
public class Property
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Required]
    [Column("Value")]
    public string? Value { get; set; }

    [Required]
    [Column("ProductId")]
    public Guid ProductID { get; set; }

    public Product Product { get; set; } = new Product();
}
