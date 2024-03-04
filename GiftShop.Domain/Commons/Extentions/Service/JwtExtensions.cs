using GiftShop.Domain.Commons.Extentions.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GiftShop.Domain.Commons.Extentions.Service;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secret = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };

        services.AddSingleton(tokenValidationParams);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(tokenOptions =>
        {
            tokenOptions.SaveToken = true;
            tokenOptions.RequireHttpsMetadata = false;
            tokenOptions.TokenValidationParameters = tokenValidationParams;
        });
    }
}
