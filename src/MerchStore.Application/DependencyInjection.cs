using Microsoft.Extensions.DependencyInjection;
using MerchStore.Application.Services.Implementations;
using MerchStore.Application.Services.Interfaces;

namespace MerchStore.Application;

/// <summary>
/// Contains extension methods for registering Application layer services with the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the DI container
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<ICatalogService, CatalogService>();

        return services;
    }
}