using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace GiftShop.Domain.Commons.Helpers;

public class HttpHelpers
{
    private static IHttpContextAccessor _accessor;
    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _accessor = httpContextAccessor;
    }

    public static HttpContext HttpContext => _accessor.HttpContext;

    public static string UserID
    {
        get
        {
            try
            {
                string idValue = HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "userID")?.Value;
                return idValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public static string NickName
    {
        get
        {
            try
            {
                string idValue = HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "nickName")?.Value;
                return !string.IsNullOrEmpty(idValue) ? idValue : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public static string Avatar
    {
        get
        {
            try
            {
                string idValue = HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "avatar")?.Value;
                return !string.IsNullOrEmpty(idValue) ? idValue : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public static string UserName
    {
        get
        {
            try
            {
                string idValue = HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
                return !string.IsNullOrEmpty(idValue) ? idValue : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public static string UserEmail
    {
        get
        {
            try
            {
                string idValue = HttpContext?.User?.Claims?.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
                return !string.IsNullOrEmpty(idValue) ? idValue : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
