using GiftShop.Application.Constrants.Responses;

namespace GiftShop.Application.Interfaces;

public interface IAuthenService
{
    Task<AuthenticationResponse> BasicLogin(string email, string password);
    Task<AuthenticationResponse> SocialNetWorkLogin(string email);
}
