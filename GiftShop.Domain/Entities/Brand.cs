using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Domain.Entities;

[Table("Categories")]
public class Brand : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("Name")]
    [StringLength(100)]
    public string? Code { get; set; }
}
