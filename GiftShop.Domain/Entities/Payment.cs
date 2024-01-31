using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Payments")]
public class Payment
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

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
    public Guid StatusID { get; set; }

    public Status OrderStatus { get; set; } = new Status();
}
