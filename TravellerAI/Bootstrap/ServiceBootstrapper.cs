using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Services;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Bootstrap;

public static class ServiceBootstrapper
{
    private const string ConnectionStringName = "TravellerDb";

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
        services.AddScoped(typeof(IBookingService), typeof(BookingService));
        services.AddScoped(typeof(ITransportService), typeof(TransportService));

        return services;
    }

    /// <summary>
    /// Registers MSSQL database context with lazy loading proxies.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration holding the connection string.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
                               ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is not configured");

        services.AddDbContext<TravellerDbContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlServer(connectionString, sql =>
                sql.MigrationsAssembly(typeof(TravellerDbContext).Assembly.FullName)));

        return services;
    }
}
