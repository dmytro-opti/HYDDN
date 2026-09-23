using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TravellerAI.Auth;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Services;
using TravellerAI.Infrastructure.Db.Mssql.Context;
using TravellerAI.Infrastructure.Db.Mssql.Identity;
using TravellerAI.Settings;
using static TravellerAI.Core.Constants;

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
        services.AddScoped(typeof(IBudgetService), typeof(BudgetService));
        services.AddScoped(typeof(ILocationService), typeof(LocationService));
        services.AddScoped(typeof(IAuthService), typeof(AuthService));
        services.AddScoped(typeof(INotificationService), typeof(NotificationService));
        services.AddScoped(typeof(IMapService), typeof(MapService));
        services.AddScoped(typeof(IValidationService), typeof(ValidationService));
        services.AddScoped(typeof(ICatalogService), typeof(CatalogService));

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

    /// <summary>
    /// Registers ASP.NET Core Identity (users, roles, password policy, lockout) and JWT bearer authentication.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">Application configuration holding the "Jwt" section.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        // fails on startup when the signing key is missing or too short
        var signingKey = JwtTokenService.CreateSigningKey(jwtSettings);

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = Security.MinPasswordLength;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = Security.MaxFailedAccessAttempts;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Security.LockoutMinutes);
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TravellerDbContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // keep short JWT claim names ("sub", "role")
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = JwtRegisteredClaimNames.Sub,
                    RoleClaimType = AuthClaims.Role
                };
            });

        services.AddAuthorization();

        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
