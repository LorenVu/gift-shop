using System.ComponentModel.DataAnnotations;

namespace GiftShop.Domain.Entities;

public class UserLog
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [StringLength(20)]
    public string Action { get; set; } = string.Empty;

    [StringLength(512)]
    public string Value { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }

    public ApplicationUser User { get; set; } = new ApplicationUser();
}
