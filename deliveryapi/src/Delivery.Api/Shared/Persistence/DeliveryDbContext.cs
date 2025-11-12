using Delivery.Api.DeliveryPersons.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Api.Shared.Persistence;

public class DeliveryDbContext : DbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
        : base(options)
    {
    }

    public DbSet<DeliveryPerson> DeliveryPersons => Set<DeliveryPerson>();
}