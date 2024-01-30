using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Domain.Entities;

public class Rating
{
    [Key]
    [Required]
    [Column("Id")]
    public int ID { get; set; }

    [Required]
    [Column("Comment")]
    [StringLength(255)]
    public string? Comment { get; set; }

    [Required]
    [Column("StartPoint")]
    public double StartPoint { get; set; }

    [Required]
    [Column("UserId")]
    public long UserID { get; set; }
    public ApplicationUser User { get; set; } = new ApplicationUser();

    [Required]
    [Column("ProductId")]
    public int ProductID { get; set; }
    public Product Product { get; set; } = new Product();
}
