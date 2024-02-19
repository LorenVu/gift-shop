using System.ComponentModel.DataAnnotations;

namespace GiftShop.Application.Constrants.Requests;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}
