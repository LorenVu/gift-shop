namespace GiftShop.Domain.Entities;

public class BaseEntity
{
    public bool IsDeleted { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedUser { get; set; } = string.Empty;
    public DateTime ModifiedDate { get; set; }
    public string ModifiedUser { get; set; } = string.Empty;
}
