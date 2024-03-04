using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiftShop.Domain.Commons.Extentions.Service;

public static class AuthenExtensions
{
    public static IServiceCollection AddAuthenticationConfig(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", policy =>
            {
                policy.AddAuthenticationSchemes("Bearer");
                policy.RequireAuthenticatedUser();
            });

            options.AddPolicy("cache-api:update", builder =>
            {
                builder.RequireClaim("permission", "cache-api:update");
            });
        });

        service.AddJwtAuthentication(configuration);

        return service;
    }
}
