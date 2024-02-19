using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Constrants.Responses;

namespace GiftShop.Application.Interfaces;

public interface IAuthenService
{
    Task<AuthenticationResponse> BasicLogin(BasicLoginRequest request);
    Task<AuthenticationResponse> SocialNetWorkLogin(string email, string deviceID);
}
