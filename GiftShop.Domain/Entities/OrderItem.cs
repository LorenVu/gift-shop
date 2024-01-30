using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("Price")]
    public double Price { get; set; }

    [Required]
    [Column("OrderId")]
    public int OrderID { get; set; }

    [Required]
    [Column("ProductId")]
    public int ProductID { get; set; }

    [Required]
    [Column("PromotionId")]
    public int PromotionID { get; set; }

    public Order Order { get; set; } = new Order();
    public Product Product { get; set; } = new Product();
    public Promotion Promotion { get; set; } = new Promotion();
}
