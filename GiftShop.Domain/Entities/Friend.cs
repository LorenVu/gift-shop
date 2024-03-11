using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftShop.Domain.Entities;

[Table("Friends")]
public class Friend
{
    [Key]
    [Required]
    public Guid FriendshipID { get; set; }

    [Required]
    public string UserID { get; set; }

    [Required]
    public string FriendID { get; set; }

    public ApplicationUser User { get; set; }
}
