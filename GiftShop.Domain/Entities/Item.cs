using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Items")]
public class Item
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("Quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("Price")]
    public double Price { get; set; }

    [Required]
    [Column("OrderId")]
    public Guid OrderID { get; set; }

    [Required]
    [Column("ProductId")]
    public Guid ProductID { get; set; }

    [Required]
    [Column("PromotionId")]
    public Guid PromotionID { get; set; }

    public Order Order { get; set; } = new Order();
    public Product Product { get; set; } = new Product();
    public Promotion Promotion { get; set; } = new Promotion();
}
