using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravellerAI.Domain.Entities;

namespace TravellerAI.Infrastructure.Db.Mssql.Context;

public class TravellerDbContext : DbContext
{
    public TravellerDbContext(DbContextOptions<TravellerDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<UserInfoEntity> UserInfos => Set<UserInfoEntity>();
    public DbSet<JourneyEntity> Journeys => Set<JourneyEntity>();
    public DbSet<TripEntity> Trips => Set<TripEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<BudgetEntity> Budgets => Set<BudgetEntity>();
    public DbSet<TransportEntity> Transports => Set<TransportEntity>();
    public DbSet<PlaceEntity> Places => Set<PlaceEntity>();
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<LocationEntity> Locations => Set<LocationEntity>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUsers(modelBuilder);
        ConfigureJourneys(modelBuilder);
        ConfigureTrips(modelBuilder);
        ConfigureBookings(modelBuilder);
        ConfigureTransports(modelBuilder);
        ConfigurePlaces(modelBuilder);
        ConfigureReviews(modelBuilder);
        ConfigureActivities(modelBuilder);
        ConfigureLocations(modelBuilder);
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
            builder.Property(i => i.TravelStyle).HasMaxLength(200);
            builder.Property(i => i.LookingFor).HasMaxLength(500);
            builder.Property(i => i.Destination).HasMaxLength(200);
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
        });
    }

    private static void ConfigureTrips(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripEntity>(builder =>
        {
            builder.ToTable("Trips");
            builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
            builder.OwnsOne(t => t.Period);

            builder.HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // NoAction in DB: SQL Server does not allow a second cascade path User -> Journey -> Trip
            builder.HasOne(t => t.Journey)
                .WithMany(j => j.Trips)
                .HasForeignKey(t => t.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(t => t.Budget)
                .WithOne()
                .HasForeignKey<TripEntity>(t => t.BudgetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(t => t.Booking)
                .WithOne(b => b.Trip)
                .HasForeignKey<TripEntity>(t => t.BookingId)
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
            builder.OwnsOne(t => t.Period);

            builder.HasOne(t => t.Trip)
                .WithMany(tr => tr.Transports)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Journey)
                .WithMany(j => j.Transports)
                .HasForeignKey(t => t.JourneyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Place)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

            builder.HasOne(a => a.Review)
                .WithMany()
                .HasForeignKey(a => a.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }

    private static void ConfigureLocations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationEntity>(builder =>
        {
            builder.ToTable("Locations");
            builder.Property(l => l.Country).HasMaxLength(100);
            builder.Property(l => l.City).HasMaxLength(100);
            builder.Property(l => l.Street).HasMaxLength(200);
            builder.Property(l => l.ZipCode).HasMaxLength(20);
        });
    }
}
