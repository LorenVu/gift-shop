using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Common.Message;
using GiftShop.Domain.Commons.Extentions.Security;
using GiftShop.Domain.Entities;
using GiftShop.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GiftShop.Application.Services;

public class AuthenService(
    UserManager<ApplicationUser> _userManager, 
    SignInManager<ApplicationUser> _signInManager,
    IHttpContextAccessor _httpContextAccessor,
    ILogger<AuthenService> _logger
    ) : IAuthenService
{
    public async Task<AuthenticationResponse> BasicLogin(BasicLoginRequest request)
    {
        var response = new AuthenticationResponse();

        try
        {
            if (string.IsNullOrEmpty(request.Email))
                return CreateErrorResponse(AuthenMessage.EMAIL_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);

            if (!request.Email.Contains("@"))
                return CreateErrorResponse(AuthenMessage.EMAIL_ISVALID, EErrorCommon.INVALID_PARAMS);


            if (string.IsNullOrEmpty(request.Password))
                return CreateErrorResponse(AuthenMessage.PASSWORD_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);


            var existUserWithEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existUserWithEmail is null)
            {
                response.Success = false;
                response.ErrorCode = (int)EErrorCommon.NOTFOUND_ERROR;
                response.ErrorMessage = AuthenMessage.NOT_EXIST_ACCOUNT;
            }
            else
            {
                if (existUserWithEmail.PasswordHash.Equals(SecurityExtensions.MD5Hash(request.Password)))
                {
                    var resultUpdateUser = await _userManager.UpdateAsync(existUserWithEmail);

                    if (!resultUpdateUser.Succeeded)
                    {
                        return new AuthenticationResponse
                        {
                            Success = false,
                            ErrorMessage = String.Join("\n", resultUpdateUser.Errors),
                            ErrorCode = (int)EErrorCommon.IDENTITY_EXCEPTION
                        };
                    }

                    response.Claims = await GetAuthenticationResultAsync(existUserWithEmail);
                }
                else
                {

                    return CreateErrorResponse(AuthenMessage.PASSWORD_ISVALID, EErrorCommon.IDENTITY_EXCEPTION);
                }
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.ErrorCode = (int)EErrorCommon.API_EXCEPTION;
            _logger.LogError(ex.Message, ex);
        }

        return response;
    }

    public async Task<AuthenticationResponse> SocialNetWorkLogin(string email, string deviceID)
    {
        var response = new AuthenticationResponse();

        try
        {
            if (String.IsNullOrEmpty(deviceID))
                return CreateErrorResponse(AuthenMessage.DEVICE_ID_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);

            if (String.IsNullOrEmpty(email))
                return CreateErrorResponse(AuthenMessage.EMAIL_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);


            var existUserWithEmail = await _userManager.FindByEmailAsync(email);

            if (existUserWithEmail is null)
            {
                var newUser = new ApplicationUser
                {
                    Avatar = "",
                    UserName = email.Split("@")[0],
                    NickName = email.Split("@")[0],
                    Email = email,
                    DeviceID = deviceID
                };

                var resultCreateNewUser = await _userManager.CreateAsync(newUser);

                response.Claims = await GetAuthenticationResultAsync(newUser);
            }
            else
            {
                existUserWithEmail.DeviceID = deviceID;
                var resultUpdateUser = await _userManager.UpdateAsync(existUserWithEmail);

                if (!resultUpdateUser.Succeeded)
                {
                    return CreateErrorResponse(String.Join("\n", resultUpdateUser.Errors), EErrorCommon.IDENTITY_EXCEPTION);
                }

                response.Claims = await GetAuthenticationResultAsync(existUserWithEmail);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return CreateErrorResponse(ex.Message, EErrorCommon.API_EXCEPTION);
        }

        return response;
    }


    #region === Private Method ===
    async Task<ClaimsIdentity> GetAuthenticationResultAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim("NickName", user.NickName),
            new Claim("Avatar", user.Avatar),
            new Claim("UserID", user.Id.ToString()),
            new Claim("DeviceID", user.DeviceID.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Login");

        return claimsIdentity;
    }

    AuthenticationResponse CreateErrorResponse(string errorMessage, EErrorCommon errorCode = EErrorCommon.INVALID_PARAMS)
    {
        return new AuthenticationResponse
        {
            Success = false,
            ErrorMessage = errorMessage,
            ErrorCode = (int)errorCode
        };
    }

    AuthenticationResponse CreateNotFoundResponse(string errorMessage)
    {
        return new AuthenticationResponse
        {
            Success = false,
            ErrorCode = (int)EErrorCommon.NOTFOUND_ERROR,
            ErrorMessage = errorMessage
        };
    }
    #endregion === Private Method === 
}
