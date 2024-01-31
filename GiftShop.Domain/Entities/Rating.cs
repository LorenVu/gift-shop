using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftShop.Domain.Entities;

[Table("Ratings")]
public class Rating
{
    [Key]
    [Required]
    [Column("Id")]
    public Guid ID { get; set; }

    [Required]
    [Column("Comment")]
    [StringLength(255)]
    public string? Comment { get; set; }

    [Required]
    [Column("StartPoint")]
    public double StartPoint { get; set; }

    [Required]
    [Column("UserId")]
    public Guid UserID { get; set; }

    [Required]
    [Column("ProductId")]
    public Guid ProductID { get; set; }

    public ApplicationUser User { get; set; } = new ApplicationUser();
    public Product Product { get; set; } = new Product();
}
