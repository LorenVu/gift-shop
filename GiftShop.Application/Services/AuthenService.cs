using GiftShop.Application.Constrants.Requests;
using GiftShop.Application.Constrants.Responses;
using GiftShop.Application.Interfaces;
using GiftShop.Domain.Common.Message;
using GiftShop.Domain.Commons.Extentions.Security;
using GiftShop.Domain.Entities;
using GiftShop.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GiftShop.Application.Services;

public class AuthenService(
    //ApplicationDbContext _dbContext,
    UserManager<ApplicationUser> _userManager, 
    SignInManager<ApplicationUser> _signInManager,
    IHttpContextAccessor _httpContextAccessor,
    IConfiguration _configuration,
    TokenValidationParameters _tokenValidationParams,
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
                    existUserWithEmail.DeviceID = request.DeviceID;
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

                    response = await GetAuthenticationResultAsync(existUserWithEmail, false);
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
            }
            else
            {
                existUserWithEmail.DeviceID = deviceID;
                var resultUpdateUser = await _userManager.UpdateAsync(existUserWithEmail);

                if (!resultUpdateUser.Succeeded)
                {
                    return CreateErrorResponse(String.Join("\n", resultUpdateUser.Errors), EErrorCommon.IDENTITY_EXCEPTION);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return CreateErrorResponse(ex.Message, EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    public async Task<AuthenticationResponse> RegisterAccount(RegisterAccountRequest request)
    {
        var response = new AuthenticationResponse();

        try
        {
            if (String.IsNullOrEmpty(request.Email))
                return CreateErrorResponse(AuthenMessage.EMAIL_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);

            if (String.IsNullOrEmpty(request.Password) && String.IsNullOrEmpty(request.ReEnterPassword))
                return CreateErrorResponse(AuthenMessage.PASSWORD_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);

            if (String.IsNullOrEmpty(request.DeviceID))
                return CreateErrorResponse(AuthenMessage.DEVICE_ID_CANNOT_BLANK, EErrorCommon.INVALID_PARAMS);

            //if (string.Equals(request.Password, request.ReEnterPassword, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    return new AuthenticationResponse
            //    {
            //        Success = false,
            //        ErrorMessage = "Re-enter password is not the same",
            //        ErrorCode = (int)EErrorCommon.INVALID_PARAMS
            //    };
            //}

            if (request.Password != request.ReEnterPassword)
                return CreateErrorResponse(AuthenMessage.PASSWORD_NOT_MATCH, EErrorCommon.INVALID_PARAMS);

            var existUserWithEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existUserWithEmail is null)
            {
                var newUser = new ApplicationUser
                {
                    //Avatar = avatarUrl,
                    Avatar = "",
                    UserName = request.Email.Split("@")[0],
                    NickName = request.Email.Split("@")[0],
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    DeviceID = request.DeviceID,
                    PasswordHash = SecurityExtensions.MD5Hash(request.Password)
                };

                response = await CreateAccount(newUser);
            }
            else
                return CreateNotFoundResponse($"Exist account with email: {request.Email}");

        }
        catch (Exception ex)
        {
            response.Success = false;
            _logger.LogError(ex.Message, ex);
            return CreateErrorResponse("", EErrorCommon.API_EXCEPTION);
        }

        return response;
    }

    #region === Private Method ===
    private async Task<AuthenticationResponse> CreateAccount(ApplicationUser user)
    {
        var resultCreateNewUser = await _userManager.CreateAsync(user);
        if (!resultCreateNewUser.Succeeded)
        {
            return new AuthenticationResponse
            {
                Success = false,
                ErrorMessage = String.Join("\n", resultCreateNewUser.Errors),
                ErrorCode = (int)EErrorCommon.IDENTITY_EXCEPTION
            };
        }
        else
            return new AuthenticationResponse
            {
                Success = true,
                UserId = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                FirstLogin = true,
            };
    }

    async Task<AuthenticationResponse> GetAuthenticationResultAsync(ApplicationUser user, bool isFirstLogin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecret = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
        TimeSpan jwtTimeLife = TimeSpan.FromDays(int.Parse(_configuration["JwtConfig:TokenLifeTime"]));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim("nickName", user.NickName),
            new Claim("avatar", user.Avatar),
            new Claim("userID", user.Id.ToString()),
            new Claim("deviceID", user.DeviceID.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "ftech-ai",
            Audience = "portal-api",
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(jwtTimeLife),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtSecret), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

        // Gen refresh token
        var refreshToken = await CreateRefreshTokenInstance(user.Id, token.Id);

        return new AuthenticationResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Success = true,
            FirstLogin = isFirstLogin,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    private async Task<string> CreateRefreshTokenInstance(string userID, string tokenID)
    {
        try
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = GenerateRefreshToken(),
                UserId = userID,
                JwtId = tokenID,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(60),
            };

            RemoveAllRefreshTokens(userID);
            //await _dbContext.RefreshTokens.AddAsync(refreshToken);
            //await _dbContext.SaveChangesAsync();

            return refreshToken.Token;
        }
        catch (Exception ex)
        {
            _logger.LogError($"AuthenService|CreateRefreshTokenInstance|Error: {ex.Message}");
            return default;
        }
    }

    private void RemoveAllRefreshTokens(string userId)
    {
        //var refreshTokens = _dbContext.RefreshTokens.Where(rf => rf.UserId == userId);
        //if (refreshTokens != null && refreshTokens.Any())
        //{
        //    _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        //}
    }

    private static string GenerateRefreshToken()
    {
        var input = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Replace("-", ""));
        var key = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString().Replace("-", ""));

        using (HMACSHA256 hmac = new HMACSHA256(key))
        {
            byte[] output = hmac.ComputeHash(input);
            return Convert.ToBase64String(output);
        }
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
