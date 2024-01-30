using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Categories")]
public class Payment
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Amount")]
    public double Amount { get; set; }

    [Required]
    [Column("PaymentMethod")]
    public string? PaymentMethod { get; set; }

    [Required]
    [Column("TransactionId")]
    public string? TransactionID { get; set; }

    [Required]
    [Column("PaymentCode")]
    public string? PaymentCode { get; set; }

    [Required]
    [Column("TransactionDate")]
    public DateTime TransactionDate { get; set; }

    [Required]
    [Column("Fee")]
    public double Fee { get; set; }

    [Required]
    [Column("StatusId")]
    public int StatusID { get; set; }

    public OrderStatus OrderStatus { get; set; } = new OrderStatus();
}
