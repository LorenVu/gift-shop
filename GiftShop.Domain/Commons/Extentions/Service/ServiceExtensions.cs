using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GiftShop.Domain.Commons.Extentions.Service;

public static class ServiceExtensions
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection service)
    {
        service.AddSwaggerGen();
        //service.AddSwaggerExamples();
        service.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Portal API",
                Version = "v1",
                Description = "Portal WebAPI.",
                Contact = new OpenApiContact
                {
                    Name = "FTECH",
                    Email = string.Empty,
                    Url = new Uri("https://ftech.ai/"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use license",
                    Url = new Uri("https://ftech.ai"),
                }
            });

            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


            //Add JWT Authentication to Header on Swagger
            // To avoid always write the keyword Bearer on the Swagger(a.k.a Swashbuckle) auth dialog, like: "bearer xT1..." -> use the code/config below
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });


            // Set the comments path for the Swagger JSON and UI.
            //  c.IncludeXmlComments(XmlCommentsFilePath);
            AddSwaggerXml(c);

        });

        return service;
    }

    static void AddSwaggerXml(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
    {
        var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
        foreach (var xmlFile in xmlFiles)
        {
            c.IncludeXmlComments(xmlFile);
        }
    }
}
