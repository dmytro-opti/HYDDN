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
        services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
        services.AddScoped(typeof(IUserService), typeof(UserService));
        services.AddScoped(typeof(IJourneyService), typeof(JourneyService));
        services.AddScoped(typeof(ITripService), typeof(TripService));

        return services;
    }
}
