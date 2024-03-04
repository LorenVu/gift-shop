using Microsoft.Extensions.DependencyInjection;

namespace GiftShop.Infastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.Scan(scan => scan
           .FromAssemblies(assembly)
           .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
           .AsImplementedInterfaces()
           .WithScopedLifetime());

        return services;
    }
}
