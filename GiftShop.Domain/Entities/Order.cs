using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Domain.Entities;

[Table("Categories")]
public class Order : BaseEntity
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("CustomerName")]
    [StringLength(100)]
    public string? CustomerName { get; set; }

    [Column("Address")]
    [StringLength(100)]
    public string? Address { get; set; }

    [Column("PhoneNumber")]
    [MaxLength(12)]
    public int PhoneNumber { get; set; }

    [Required]
    [Column("TotalAmount")]
    public double TotalAmount { get; set; }

    [Column("Note")]
    public string? Note { get; set; }

    [Column("CancelReason")]
    public string? CancelReason { get; set; }

    [Required]
    [Column("PaymentId")]
    public int PaymentID { get; set; }

    [Column("StatusId")]
    public int StatusID { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<OrderStatus> OrderStatus { get; set; } = new List<OrderStatus>();
}
