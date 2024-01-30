using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Promotions")]
public class Promotion : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Column("Code")]
    [StringLength(100)]
    public string? Code { get; set; }

    [Column("DiscountPercent")]
    public double DiscountPercent { get; set; }
}
