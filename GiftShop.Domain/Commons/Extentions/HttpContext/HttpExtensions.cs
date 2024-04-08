namespace GiftShop.Domain.Commons.Extentions.HttpContext;

public static class HttpExtensions
{
    //public static long GetUserId(this HttpContext httpContext)
    //{
    //    if (httpContext.User == null)
    //    {
    //        throw new InvalidOperationException("Invalid Claims");
    //    }
    //    string idValue = httpContext.User.Claims.Single(x => x.Type == "UserID").Value;
    //    return long.TryParse(idValue, out long id) ? id : throw new InvalidOperationException("Invalid id Claim");
    //}

    //public static string GetClientIP(this HttpContext context)
    //{
    //    var xForward = context.Request.Headers["X-Forwarded-For"].ToString();
    //    return !string.IsNullOrWhiteSpace(xForward) ? xForward : context.Connection.RemoteIpAddress.ToString();
    //}
}
