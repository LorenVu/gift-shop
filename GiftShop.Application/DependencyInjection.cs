using Microsoft.Extensions.DependencyInjection;

namespace GiftShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.Scan(scan => scan
           .FromAssemblies(assembly)
           .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
           .AsImplementedInterfaces()
           .WithScopedLifetime());

        return services;
    }
}
