using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("UserLogs")]
public class UserLog
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(20)]
    public string Action { get; set; } = string.Empty;

    [StringLength(512)]
    public string Value { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = new ApplicationUser();
}
