using System.Security.Claims;

namespace GiftShop.Application.Constrants.Responses;

public class LoginCmsResponse
{
    public int StatusCode { get; set; }
    public int ErrorCode { get; set; }

    public string? Message { get; set; }

    public ClaimsIdentity Claims {  get; set; } = new ClaimsIdentity();
}
