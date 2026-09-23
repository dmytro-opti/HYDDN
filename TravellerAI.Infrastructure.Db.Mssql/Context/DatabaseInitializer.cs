using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TravellerAI.Infrastructure.Db.Mssql.Context;

/// <summary>
/// Creates / migrates the database and optionally fills it with test data (Scripts/SeedTestData.sql).
/// </summary>
public static class DatabaseInitializer
{
    private const string SeedScriptResource = "TravellerAI.Infrastructure.Db.Mssql.Scripts.SeedTestData.sql";

    public static async Task InitializeAsync(IServiceProvider services, bool seedTestData, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TravellerDbContext>();

        // creates the database when it does not exist and applies pending migrations
        await context.Database.MigrateAsync(cancellationToken);

        if (seedTestData)
        {
            // the script itself skips seeding when data already exists
            await context.Database.ExecuteSqlRawAsync(await ReadSeedScriptAsync(), cancellationToken);
        }
    }

    private static async Task<string> ReadSeedScriptAsync()
    {
        await using var stream = typeof(DatabaseInitializer).Assembly.GetManifestResourceStream(SeedScriptResource)
                                 ?? throw new InvalidOperationException($"Embedded resource {SeedScriptResource} not found");
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}
