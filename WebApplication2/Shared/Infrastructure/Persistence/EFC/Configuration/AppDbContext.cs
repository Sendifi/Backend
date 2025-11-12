using Microsoft.EntityFrameworkCore;
using WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
// alias para la entidad
using CourierEntity = WebApplication2.Courier.Domain.Model.Aggregates.Courier;

namespace WebApplication2.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CourierEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            b.Property(x => x.Name).IsRequired().HasMaxLength(120);
            b.HasIndex(x => x.Name).IsUnique();
            b.Property(x => x.CostPerKg).HasColumnType("decimal(10,2)").IsRequired();
            b.Property(x => x.EstimatedDays).HasDefaultValue(2);
            b.Property(x => x.IsActive).HasDefaultValue(true);
        });

        builder.UseSnakeCaseNamingConvention();
    }

    public DbSet<CourierEntity> Couriers => Set<CourierEntity>();
}