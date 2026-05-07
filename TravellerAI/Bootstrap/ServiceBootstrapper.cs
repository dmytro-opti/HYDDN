using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Services;

namespace TravellerAI.Bootstrap;

public static class ServiceBootstrapper
{
    /// <summary>
    /// Registers application services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        // Register Logger Service
        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));

        return services;
    }
}
