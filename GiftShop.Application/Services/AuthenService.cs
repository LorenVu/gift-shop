using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GiftShop.Application.Services;

public class AuthenService(
    UserManager<ApplicationUser> _userManager, 
    SignInManager<ApplicationUser> _signInManager,
    ILogger<AuthenService> _logger
    ) : IAuthenService
{
    public Task<AuthenticationResponse> BasicLogin(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticationResponse> SocialNetWorkLogin(string email)
    {
        throw new NotImplementedException();
    }
}
