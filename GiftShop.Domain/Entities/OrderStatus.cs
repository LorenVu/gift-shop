using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Categories")]
public class OrderStatus
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Type")]
    [StringLength(100)]
    public string? Type { get; set; }

    [Required]
    [Column("Display")]
    public bool Display { get; set; }

    [Column("Code")]
    [StringLength(100)]
    public string? Code { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}
