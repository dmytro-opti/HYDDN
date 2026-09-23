using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravellerAI.Core;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Identity;

namespace TravellerAI.Infrastructure.Db.Mssql.Context;

public class TravellerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public TravellerDbContext(DbContextOptions<TravellerDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserInfoEntity> UserInfos => Set<UserInfoEntity>();
    public DbSet<JourneyEntity> Journeys => Set<JourneyEntity>();
    public DbSet<TripEntity> Trips => Set<TripEntity>();
    public DbSet<TripStopEntity> TripStops => Set<TripStopEntity>();
    public DbSet<JourneyDayEntity> JourneyDays => Set<JourneyDayEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<BudgetEntity> Budgets => Set<BudgetEntity>();
    public DbSet<TransportEntity> Transports => Set<TransportEntity>();
    public DbSet<PlaceEntity> Places => Set<PlaceEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<LocationEntity> Locations => Set<LocationEntity>();
    public DbSet<CountryEntity> Countries => Set<CountryEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    // fixed ids: roles are created by migrations in every environment
    public static readonly Guid UserRoleId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");
    public static readonly Guid AdminRoleId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7");

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Identity tables (AspNetUsers, AspNetRoles, ...)
        base.OnModelCreating(modelBuilder);

        ConfigureIdentity(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureJourneys(modelBuilder);
        ConfigureTrips(modelBuilder);
        ConfigureBookings(modelBuilder);
        ConfigureTransports(modelBuilder);
        ConfigurePlaces(modelBuilder);
        ConfigureReviews(modelBuilder);
        ConfigureActivities(modelBuilder);
        ConfigureLocations(modelBuilder);
        ConfigureCountries(modelBuilder);
        ConfigureNotifications(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditInfo();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Sets Created / Updated for every tracked <see cref="BaseEntity"/> before it is saved.
    /// </summary>
    private void ApplyAuditInfo()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = now;
                    entry.Entity.Updated = now;
                    break;
                case EntityState.Modified:
                    entry.Property(e => e.Updated).CurrentValue = now;
                    entry.Property(e => e.Created).IsModified = false;
                    break;
                case EntityState.Unchanged when HasChangedOwnedEntities(entry):
                    // e.g. only the owned Period was changed - the owner itself stays Unchanged
                    entry.Property(e => e.Updated).CurrentValue = now;
                    break;
            }
        }
    }

    private static bool HasChangedOwnedEntities(EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null
            && r.TargetEntry.Metadata.IsOwned()
            && r.TargetEntry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);

    private static void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            // profile shares the primary key with the identity user and is removed with it
            builder.HasOne(u => u.Profile)
                .WithOne()
                .HasForeignKey<UserEntity>(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = UserRoleId, Name = Constants.Security.UserRole, NormalizedName = Constants.Security.UserRole.ToUpperInvariant(),
                ConcurrencyStamp = "3b6c3c3e-4a8e-4a51-9f1e-0d5f1f6d2a11"
            },
            new IdentityRole<Guid>
            {
                Id = AdminRoleId, Name = Constants.Security.AdminRole, NormalizedName = Constants.Security.AdminRole.ToUpperInvariant(),
                ConcurrencyStamp = "8e2f7a4b-2c1d-4f3e-9a6b-5d4c3b2a1f00"
            });

        modelBuilder.Entity<RefreshTokenEntity>(builder =>
        {
            builder.ToTable("RefreshTokens");
            builder.Property(t => t.TokenHash).HasMaxLength(64).IsRequired();
            builder.HasIndex(t => t.TokenHash).IsUnique();

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.ToTable("Users");
            builder.Property(u => u.Name).HasMaxLength(200);
            builder.Property(u => u.FirstName).HasMaxLength(200);
            builder.Property(u => u.LastName).HasMaxLength(200);
            builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasOne(u => u.UserInfo)
                .WithOne(i => i.User)
                .HasForeignKey<UserInfoEntity>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserInfoEntity>(builder =>
        {
            builder.ToTable("UserInfos");
            builder.Property(i => i.LookingFor).HasMaxLength(500);
            builder.HasIndex(i => i.UserId).IsUnique();

            builder.HasMany(i => i.ChosenActivities)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>("UserInfoChosenActivities",
                    right => right.HasOne<ActivityEntity>().WithMany().HasForeignKey("ActivityId").OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<UserInfoEntity>().WithMany().HasForeignKey("UserInfoId").OnDelete(DeleteBehavior.Cascade));

            // ClientCascade on the trip side: Users -> Trips -> links would be a second cascade path from Users
            builder.HasMany(i => i.ChosenTrips)
                .WithMany(t => t.ChosenBy)
                .UsingEntity<Dictionary<string, object>>("UserInfoChosenTrips",
                    right => right.HasOne<TripEntity>().WithMany().HasForeignKey("TripId").OnDelete(DeleteBehavior.ClientCascade),
                    left => left.HasOne<UserInfoEntity>().WithMany().HasForeignKey("UserInfoId").OnDelete(DeleteBehavior.Cascade));

            builder.HasMany(i => i.PreferredCountries)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>("UserInfoPreferredCountries",
                    right => right.HasOne<CountryEntity>().WithMany().HasForeignKey("CountryId").OnDelete(DeleteBehavior.Cascade),
                    left => left.HasOne<UserInfoEntity>().WithMany().HasForeignKey("UserInfoId").OnDelete(DeleteBehavior.Cascade));
        });
    }

    private static void ConfigureJourneys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JourneyEntity>(builder =>
        {
            builder.ToTable("Journeys");
            builder.Property(j => j.Title).HasMaxLength(200).IsRequired();
            builder.OwnsOne(j => j.Period);

            builder.HasOne(j => j.User)
                .WithMany(u => u.Journeys)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(j => j.Budget)
                .WithOne()
                .HasForeignKey<JourneyEntity>(j => j.BudgetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(j => j.Country)
                .WithMany()
                .HasForeignKey(j => j.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JourneyDayEntity>(builder =>
        {
            builder.ToTable("JourneyDays");
            builder.Property(d => d.Date).HasColumnType("date");
            builder.HasIndex(d => new { d.JourneyId, d.Date }).IsUnique();

            builder.HasOne(d => d.Journey)
                .WithMany(j => j.Days)
                .HasForeignKey(d => d.JourneyId)
                .OnDelete(DeleteBehavior.Cascade);

            // NoAction in DB: Users -> Trips -> days would be a second cascade path from Users;
            // trips scheduled in journeys cannot be deleted (TripService)
            builder.HasOne(d => d.Trip)
                .WithMany(t => t.Days)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    private static void ConfigureTrips(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripEntity>(builder =>
        {
            builder.ToTable("Trips");
            builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(500);
            builder.Property(t => t.City).HasMaxLength(100).IsRequired();
            builder.HasIndex(t => new { t.CountryId, t.City, t.IsPublic });

            // author
            builder.HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Country)
                .WithMany()
                .HasForeignKey(t => t.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TripStopEntity>(builder =>
        {
            builder.ToTable("TripStops");
            builder.HasIndex(s => new { s.TripId, s.Order }).IsUnique();

            builder.HasOne(s => s.Trip)
                .WithMany(t => t.Stops)
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Location)
                .WithMany()
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Activity)
                .WithMany()
                .HasForeignKey(s => s.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<BudgetEntity>(builder =>
        {
            builder.ToTable("Budgets");
            builder.Property(b => b.CurrencyCode).HasMaxLength(3);
        });
    }

    private static void ConfigureBookings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>(builder =>
        {
            builder.ToTable("Bookings");
            builder.Property(b => b.Currency).HasMaxLength(3);
            builder.Property(b => b.PaymentMethod).HasMaxLength(100);
            builder.OwnsOne(b => b.Period);
            builder.HasIndex(b => new { b.PropertyId, b.CheckInDate, b.CheckOutDate });
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Bookings_Dates", "[CheckInDate] <= [CheckOutDate]");
                t.HasCheckConstraint("CK_Bookings_Guests", "[Adults] >= 0 AND [Children] >= 0");
            });

            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Journey)
                .WithMany(j => j.Bookings)
                .HasForeignKey(b => b.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(b => b.Budget)
                .WithOne()
                .HasForeignKey<BookingEntity>(b => b.BudgetId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    private static void ConfigureTransports(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransportEntity>(builder =>
        {
            builder.ToTable("Transports");
            builder.Property(t => t.Company).HasMaxLength(200).IsRequired();
            // SQL "time" is limited to 24h, store duration as ticks instead
            builder.Property(t => t.Duration).HasConversion<long>();
            builder.ToTable(t => t.HasCheckConstraint("CK_Transports_SeatCount", "[SeatCount] >= 0"));
            builder.OwnsOne(t => t.Period);

            builder.HasOne(t => t.Journey)
                .WithMany(j => j.Transports)
                .HasForeignKey(t => t.JourneyId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurePlaces(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlaceEntity>(builder =>
        {
            builder.ToTable("Places");
            builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Type).HasMaxLength(100);
            builder.Property(p => p.Address).HasMaxLength(500);

            builder.HasOne(p => p.Owner)
                .WithMany(u => u.Places)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Location)
                .WithMany()
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    private static void ConfigureReviews(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewEntity>(builder =>
        {
            builder.ToTable("Reviews");
            builder.Property(r => r.Title).HasMaxLength(200);
            builder.ToTable(t => t.HasCheckConstraint("CK_Reviews_Target", "[PlaceId] IS NOT NULL OR [ActivityId] IS NOT NULL"));

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ClientCascade: a DB cascade would add a second path Users -> Places -> Reviews
            builder.HasOne(r => r.Place)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlaceId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(r => r.Activity)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureActivities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>(builder =>
        {
            builder.ToTable("Activities");
            builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
            builder.OwnsOne(a => a.Period);

            builder.HasOne(a => a.Location)
                .WithMany()
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    private static void ConfigureLocations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationEntity>(builder =>
        {
            builder.ToTable("Locations");
            builder.Property(l => l.Name).HasMaxLength(200);
            builder.Property(l => l.City).HasMaxLength(100);
            builder.HasIndex(l => new { l.CountryId, l.City });

            // locations are used by places and activities, a country with locations cannot be removed
            builder.HasOne(l => l.Country)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(l => l.Street).HasMaxLength(200);
            builder.Property(l => l.ZipCode).HasMaxLength(20);
        });
    }

    private static void ConfigureCountries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CountryEntity>(builder =>
        {
            builder.ToTable("Countries");
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(2).IsFixedLength().IsRequired();
            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasIndex(c => c.Code).IsUnique();
        });
    }

    private static void ConfigureNotifications(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationEntity>(builder =>
        {
            builder.ToTable("Notifications");
            builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
            builder.Property(n => n.Message).HasMaxLength(1000).IsRequired();
            builder.HasIndex(n => new { n.UserId, n.IsRead });

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
