using System.Text.Json;
using backSendify.Couriers.Domain.Couriers.Entities;
using backSendify.Delivery.Domain.DeliveryPersons.Entities;
using backSendify.Shipments.Domain.Shipments.Entities;
using backSendify.Tracking.Domain.Tracking.Entities;
using backSendify.Users.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backSendify.Shared.Infrastructure.Persistence;

public class BackSendifyDbContext : DbContext
{
    public BackSendifyDbContext(DbContextOptions<BackSendifyDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<DeliveryPerson> DeliveryPersons => Set<DeliveryPerson>();
    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();
    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureShipments(modelBuilder);
        ConfigureDeliveryPersons(modelBuilder);
        ConfigureCouriers(modelBuilder);
        ConfigureUsers(modelBuilder);
        ConfigureTrackingEvents(modelBuilder);
    }

    private static void ConfigureShipments(ModelBuilder modelBuilder)
    {
        var shipment = modelBuilder.Entity<Shipment>();
        shipment.HasIndex(s => s.TrackingCode).IsUnique();
        shipment.Property(s => s.TrackingCode).HasMaxLength(32).IsRequired();
        shipment.Property(s => s.Weight).IsRequired();
        shipment.Property(s => s.Cost).HasPrecision(18, 2);

        shipment.OwnsOne(s => s.Sender, builder =>
        {
            builder.Property(p => p.Name).HasColumnName("SenderName");
            builder.Property(p => p.Email).HasColumnName("SenderEmail");
            builder.Property(p => p.Phone).HasColumnName("SenderPhone");
        });

        shipment.OwnsOne(s => s.Recipient, builder =>
        {
            builder.Property(p => p.Name).HasColumnName("RecipientName");
            builder.Property(p => p.Email).HasColumnName("RecipientEmail");
            builder.Property(p => p.Phone).HasColumnName("RecipientPhone");
        });

        shipment.OwnsOne(s => s.OriginAddress, builder =>
        {
            builder.Property(p => p.Street).HasColumnName("OriginStreet");
            builder.Property(p => p.City).HasColumnName("OriginCity");
            builder.Property(p => p.State).HasColumnName("OriginState");
            builder.Property(p => p.ZipCode).HasColumnName("OriginZipCode");
            builder.Property(p => p.Country).HasColumnName("OriginCountry");
        });

        shipment.OwnsOne(s => s.DestinationAddress, builder =>
        {
            builder.Property(p => p.Street).HasColumnName("DestinationStreet");
            builder.Property(p => p.City).HasColumnName("DestinationCity");
            builder.Property(p => p.State).HasColumnName("DestinationState");
            builder.Property(p => p.ZipCode).HasColumnName("DestinationZipCode");
            builder.Property(p => p.Country).HasColumnName("DestinationCountry");
        });
    }

    private static void ConfigureDeliveryPersons(ModelBuilder modelBuilder)
    {
        var delivery = modelBuilder.Entity<DeliveryPerson>();
        delivery.HasIndex(d => d.Code).IsUnique();

        var guidListConverter = new ValueConverter<List<Guid>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrEmpty(v)
                ? new List<Guid>()
                : JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>());

        var guidListComparer = new ValueComparer<List<Guid>>(
            (l1, l2) => l1 != null && l2 != null && l1.SequenceEqual(l2),
            l => l == null ? 0 : l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            l => l == null ? new List<Guid>() : l.ToList());

        delivery.Property(d => d.AssignedShipments)
            .HasConversion(guidListConverter)
            .Metadata.SetValueComparer(guidListComparer);
    }

    private static void ConfigureCouriers(ModelBuilder modelBuilder)
    {
        var courier = modelBuilder.Entity<Courier>();
        courier.Property(c => c.CostPerKg).HasPrecision(18, 2);
        courier.OwnsOne(c => c.Contact, builder =>
        {
            builder.Property(p => p.Email).HasColumnName("ContactEmail");
            builder.Property(p => p.Phone).HasColumnName("ContactPhone");
            builder.Property(p => p.Website).HasColumnName("ContactWebsite");
        });

        courier.OwnsOne(c => c.Pricing, builder =>
        {
            builder.Property(p => p.BaseCost).HasColumnName("PricingBase").HasPrecision(18, 2);
            builder.Property(p => p.PerKg).HasColumnName("PricingPerKg").HasPrecision(18, 2);
        });

        var stringListConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => string.IsNullOrEmpty(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

        var stringListComparer = new ValueComparer<List<string>>(
            (l1, l2) => l1 != null && l2 != null && l1.SequenceEqual(l2),
            l => l == null ? 0 : l.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            l => l == null ? new List<string>() : l.ToList());

        courier.Property(c => c.Services)
            .HasConversion(stringListConverter)
            .Metadata.SetValueComparer(stringListComparer);

        courier.Property(c => c.Coverage)
            .HasConversion(stringListConverter)
            .Metadata.SetValueComparer(stringListComparer);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<ApplicationUser>();
        user.HasIndex(u => u.Email).IsUnique();
    }

    private static void ConfigureTrackingEvents(ModelBuilder modelBuilder)
    {
        var tracking = modelBuilder.Entity<TrackingEvent>();
        tracking.HasIndex(t => new { t.ShipmentId, t.Timestamp });
    }
}
